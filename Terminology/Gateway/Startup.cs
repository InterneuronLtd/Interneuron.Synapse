//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

//Copyright(C) 2021  Interneuron CIC

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
using System.Threading.Tasks;
using HealthChecks.UI.Client;
using Interneuron.Infrastructure.Web.Exceptions.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

namespace Interneuron.Terminology.API.Gateway
{
    public class Startup
    {
        private IConfiguration _configuration;
        private const string AuthenticationSchemeKeyName = "IdentityApiKey";
        private const string ClientTokenErrorMsg = "Error authenticating the client token";
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var hcConfigSection = _configuration.GetSection("HC");

            services.AddControllersWithViews();

            //Disabled health check for now
            //services.AddHealthChecks()
            //    .AddCheck("self", () => HealthCheckResult.Healthy())
            //    .AddUrlGroup(new Uri(hcConfigSection.GetValue<string>("TerminologydAPIURL")), name: "terminology-check", tags: new string[] { "terminologyapi" });

            HandleAuthentication(services);

            services.AddOcelot(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var pathBase = _configuration["PATH_BASE"];

            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseInterneuronExceptionHandler(options =>
            {
                options.OnExceptionHandlingComplete = (ex, errorId) =>
                {
                    LogException(ex, errorId);
                };
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Swagger}/{action=Index}");
            });

            //app.UseHealthChecks("/hc", new HealthCheckOptions()
            //{
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //});

            //app.UseHealthChecks("/liveness", new HealthCheckOptions
            //{
            //    Predicate = r => r.Name.Contains("self")
            //});


            app.UseOcelot().Wait();
        }

        private static void LogException(Exception ex, string errorId)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) || ex.Message.StartsWith("a network", StringComparison.InvariantCultureIgnoreCase))
                Log.Logger.ForContext("ErrorId", errorId).Fatal(ex, ex.Message);
            else
                Log.Logger.ForContext("ErrorId", errorId).Error(ex, ex.Message);
        }


        private void HandleAuthentication(IServiceCollection services)
        {
            var configSection = _configuration.GetSection("TerminologyConfig");

            var authorizationAuthority = configSection.GetValue<string>("AuthorizationAuthority");

            var authorizationAudience = configSection.GetValue<string>("AuthorizationAudience");

            services.AddAuthentication()
                .AddJwtBearer(AuthenticationSchemeKeyName, o =>
                {
                    o.Authority = authorizationAuthority;
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidAudiences = new[] { authorizationAudience }
                    };
                    o.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                    {
                        OnAuthenticationFailed = async ctx =>
                        {
                            if (ctx.Exception != null)
                                Log.Logger.Error(ctx.Exception, ClientTokenErrorMsg);
                            else
                                Log.Logger.Information(ClientTokenErrorMsg);
                        },
                        //OnTokenValidated = async ctx =>
                        //{
                        //},

                        //OnMessageReceived = async ctx =>
                        //{
                        //}
                    };
                });
        }
    }
}
