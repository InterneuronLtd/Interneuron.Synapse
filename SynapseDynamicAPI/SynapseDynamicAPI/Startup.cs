 //Interneuron synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Interneuron.Infrastructure.Web.Exceptions.Handlers;
using Interneuron.Web.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SynapseDynamicAPI.Formatters;
using Swashbuckle.AspNetCore.Swagger;
using SynapseDynamicAPI.Infrastructure.Filters;
using Interneuron.Common.Extensions;
using Microsoft.OpenApi.Models;

namespace SynapseDynamicAPI
{
    public class Startup
    {
        IConfigurationSection SwaggerSection = null;
        IConfigurationSection SynapseCoreSection = null;
        IConfigurationSection SynapseCoreSettingsSection = null;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Environment.SetEnvironmentVariable("connectionString_SynapseDataStore", configuration["SynapseCore:ConnectionStrings:SynapseDataStore"]);
            Environment.SetEnvironmentVariable("connectionString_SynapseIdentityStore", configuration["SynapseCore:ConnectionStrings:SynapseIdentityStore"]);
            Environment.SetEnvironmentVariable("OutboundInterface_SendingApplicationName", configuration["OutboundInterface:SendingApplicationName"]);

            SwaggerSection = Configuration.GetSection("Swagger");

            SynapseCoreSection = Configuration.GetSection("SynapseCore");

            SynapseCoreSettingsSection = SynapseCoreSection.GetSection("Settings");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string showPII = Configuration["SynapseCore:Settings:ShowIdentitySeverPIIinLogs"];

            if (!string.IsNullOrWhiteSpace(showPII) && showPII.ToLower() == "true")
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            //services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
            //                                                        .AllowAnyMethod()
            //                                                         .AllowAnyHeader()))

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });


            services.AddMvc(options =>
            {
                options.InputFormatters.Insert(0, new RawJsonBodyInputFormatter());
            }).AddControllersAsServices();

            services.AddAuthentication("Bearer")
              .AddIdentityServerAuthentication(options =>
              {
                  options.Authority = Configuration["SynapseCore:Settings:AuthorizationAuthority"];
                  options.RequireHttpsMetadata = false;
                  options.ApiName = Configuration["SynapseCore:Settings:AuthorizationAudience"];
                  options.EnableCaching = false;
                  string byPassSSLValidation = Configuration["SynapseCore:Settings:IgnoreIdentitySeverSSLErrors"];

                  if (!string.IsNullOrWhiteSpace(byPassSSLValidation) && byPassSSLValidation.ToLower() == "true")
                      options.JwtBackChannelHandler = GetHandler();
              });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DynamicAPIWriters", builder =>
                {
                    builder.RequireClaim(Configuration["SynapseCore:Settings:SynapseRolesClaimType"], Configuration["SynapseCore:Settings:DynamicAPIWriteAccessRole"]);
                    builder.RequireScope(Configuration["SynapseCore:Settings:WriteAccessAPIScope"]);
                });
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DynamicAPIReaders", builder =>
                {
                    builder.RequireClaim(Configuration["SynapseCore:Settings:SynapseRolesClaimType"], Configuration["SynapseCore:Settings:DynamicAPIReadAccessRole"]);
                    builder.RequireScope(Configuration["SynapseCore:Settings:ReadAccessAPIScope"]);
                });
            });

            var swaggerName = SwaggerSection.GetValue<string>("DocumentName");
            var swaggerVer = SwaggerSection.GetValue<string>("DocumentVersion");
            var swaggerAccessScopes = SwaggerSection.GetValue<string>("AccessScopes");

            services.AddSwaggerGen(conf =>
            {
                //conf.DescribeAllEnumsAsStrings();

                conf.SwaggerDoc(swaggerName, new OpenApiInfo
                {
                    Title = "Synapse Dynamic HTTP API",
                    Version = swaggerVer,
                    Description = "The Dynamic Service HTTP API",
                    //TermsOfService = "The Synapse Dynamic Service HTTP API"
                });

                conf.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{SynapseCoreSettingsSection.GetValue<string>("AuthorizationAuthority")}/connect/authorize"),
                            TokenUrl = new Uri($"{SynapseCoreSettingsSection.GetValue<string>("AuthorizationAuthority")}/connect/token"),
                            Scopes = GetScopes(swaggerAccessScopes)
                        }
                    }
                });

                conf.OperationFilter<AuthorizeCheckOperationFilter>();
                conf.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                conf.DocInclusionPredicate((docName, apiDescription) =>
                {
                    apiDescription.RelativePath = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)apiDescription.ActionDescriptor).ActionName;
                    return true;
                });
            });
        }

        private static Dictionary<string, string> GetScopes(string swaggerAccessScopes)
        {
            var scopes = new Dictionary<string, string>();

            swaggerAccessScopes.Split(';')
                            .Each(scopeUnit =>
                            {
                                if (scopeUnit.IsNotEmpty())
                                {
                                    var scopeKV = scopeUnit.Split(':');
                                    if (scopeKV.IsCollectionValid())
                                    {
                                        scopes[scopeKV[0]] = scopeKV[1];
                                    }
                                }
                            });
            return scopes;
        }

        private static HttpClientHandler GetHandler()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            return handler;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseInterneuronExceptionHandler(options =>
            {
                options.OnExceptionHandlingComplete = (ex, errorId) =>
                {
                    LogException(ex, errorId);
                };
            });

            app.UseStaticFiles();


            app.UseRouting();
            app.UseCors("AllowAllHeaders");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware<InterneuronSerilogLoggingMiddleware>();

            //app.UseMvc();

            app.UseSwagger();

            var swaggerName = SwaggerSection.GetValue<string>("DocumentName");
            var oAuthClientId = SwaggerSection.GetValue<string>("OAuthClientId");
            var oAuthClientName = SwaggerSection.GetValue<string>("OAuthClientName");

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"./{swaggerName}/swagger.json", $"SynapseDynamicAPI {Configuration["API_Version"]}");

                c.OAuthClientId(oAuthClientId);
                c.OAuthAppName(oAuthClientName);
            });
        }

        private static void LogException(Exception ex, string errorId)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) || ex.Message.StartsWith("a network", StringComparison.InvariantCultureIgnoreCase))
                Log.Logger.ForContext("ErrorId", errorId).Fatal(ex, ex.Message);
            else
                Log.Logger.ForContext("ErrorId", errorId).Error(ex, ex.Message);
        }

    }


}
