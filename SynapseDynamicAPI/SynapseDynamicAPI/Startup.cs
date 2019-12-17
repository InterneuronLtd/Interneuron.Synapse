//Interneuron Synapse

//Copyright(C) 2019  Interneuron CIC

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
using System.Net.Http;
using System.Security.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SynapseDynamicAPI.Formatters;

namespace SynapseDynamicAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Environment.SetEnvironmentVariable("connectionString_SynapseDataStore", configuration["SynapseCore:ConnectionStrings:SynapseDataStore"]);
            Environment.SetEnvironmentVariable("connectionString_SynapseIdentityStore", configuration["SynapseCore:ConnectionStrings:SynapseIdentityStore"]);
            Environment.SetEnvironmentVariable("OutboundInterface_SendingApplicationName", configuration["OutboundInterface:SendingApplicationName"]);
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
            //                                                         .AllowAnyHeader()));

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
        }
        private static HttpClientHandler GetHandler()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            return handler;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseCors("AllowAllHeaders");



            app.UseMvc();
        }

    }
}
