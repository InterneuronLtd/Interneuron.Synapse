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


﻿using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using NToastNotify;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using SynapseStudioWeb.AppCode.Filters;
using SynapseStudioWeb.Helpers;
using Microsoft.AspNetCore.Http.Features;

namespace SynapseStudioWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Environment.SetEnvironmentVariable("connectionString_ServiceBaseURL", configuration["SynapseCore:ConnectionStrings:ServiceBaseURL"]);
            Environment.SetEnvironmentVariable("connectionString_SynapseDataStore", configuration["SynapseCore:ConnectionStrings:SynapseDataStore"]);
            Environment.SetEnvironmentVariable("connectionString_SynapseIdentityStore", configuration["SynapseCore:ConnectionStrings:SynapseIdentityStore"]);
            Environment.SetEnvironmentVariable("settings_ShowIdentitySeverPIIinLogs", configuration["SynapseCore:Settings:ShowIdentitySeverPIIinLogs"]);
            Environment.SetEnvironmentVariable("connectionString_TerminologyServiceBaseURL", configuration["SynapseCore:ConnectionStrings:TerminologyServiceBaseURL"]);
            Configuration = configuration;
            StaticConfiguration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static IConfiguration StaticConfiguration { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.Configure<CookiePolicyOptions>(options =>
                        {
                            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                            options.CheckConsentNeeded = context => false;
                            options.MinimumSameSitePolicy = SameSiteMode.None;
                        });

            string showPII = Environment.GetEnvironmentVariable("settings_ShowIdentitySeverPIIinLogs");
            if (!string.IsNullOrWhiteSpace(showPII) && showPII.ToLower() == "true")
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

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

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                var sessionIdleTimeInMins = Configuration.GetValue<int>("SynapseCore:SessionTimeOutInMins");
                options.IdleTimeout = TimeSpan.FromMinutes(sessionIdleTimeInMins);//You can set Time   
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddMvc((options) =>
            {
                options.Filters.Add<StudioSessionCheckFilter>();
            })
             .AddNToastNotifyToastr(new ToastrOptions()
             {
                 ProgressBar = false,
                 PositionClass = ToastPositions.TopRight
             });

            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromHours(24);
            //    options.Cookie.IsEssential = true;
            //});

            services.AddControllersWithViews();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
            })
             .AddCookie("cookie", options =>
             {
                 //options.SlidingExpiration = true;
                 //options.ExpireTimeSpan = TimeSpan.FromHours(24);

                 options.Events.OnSigningOut = async e =>
                 {
                     // automatically revoke refresh token at signout time
                     await e.HttpContext.RevokeUserRefreshTokenAsync();
                 };
             })
             .AddOpenIdConnect("oidc", options =>
             {
                 options.Authority = Configuration["SynapseCore:Settings:AuthorizationAuthority"];
                 options.ClientId = Configuration["SynapseCore:Settings:ClientId"];
                 options.SignInScheme = "cookie";
                 options.SignOutScheme = "cookie";
                 options.SignedOutRedirectUri = Configuration["SynapseCore:Settings:SignedOutRedirectUri"];
                 options.CallbackPath = Configuration["SynapseCore:Settings:CallbackPath"];
                 options.Scope.Add(Configuration["SynapseCore:Settings:OpenIdAPIScope"]);
                 options.Scope.Add(Configuration["SynapseCore:Settings:ReadAccessAPIScope"]);
                 options.Scope.Add(Configuration["SynapseCore:Settings:OfflineAccess"]);
                 options.GetClaimsFromUserInfoEndpoint = true;
                 options.ResponseType = Configuration["SynapseCore:Settings:ResponseType"];
                 options.RequireHttpsMetadata = false;
                 options.SaveTokens = true;

                 //options.Events.OnTicketReceived = async (context) =>
                 //{
                 //    context.Properties.ExpiresUtc = DateTime.UtcNow.AddHours(2);
                 //};
             });

            // add automatic token management
            services.AddAccessTokenManagement();


            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddAutoMapper(typeof(Startup));
        }
        private static HttpClientHandler GetHandler()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            return handler;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseInterneuronExceptionHandler(options =>
            //{
            //    options.OnExceptionHandlingComplete = (ex, errorId) =>
            //    {
            //        LogException(ex, errorId);

            //        if (env.IsDevelopment())
            //        {
            //            app.UseDeveloperExceptionPage();
            //        }
            //        else
            //        {
            //            //app.UseExceptionHandler("/Home/Error");
            //            //app.UseHsts();
            //        }
            //    };
            //});

            if (env.IsDevelopment())
            {
                //Un-Comment this only during the coding phase
                app.UseDeveloperExceptionPage();
                //Comment this only during the coding phase
                //app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                //Option 1
                app.UseExceptionHandler("/Home/Error");

                //Option 2
                //app.UseExceptionHandler(new ExceptionHandlerOptions()
                //{
                //    ExceptionHandlingPath = new PathString("/Home/Error"),
                //    ExceptionHandler = async (context) =>
                //    {
                //        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                //        if (errorFeature != null)
                //        {
                //            var exception = errorFeature.Error;

                //            if (exception != null)
                //                LogException(exception, Guid.NewGuid().ToString());
                //        }
                //    }
                //});
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            //Not required in MVC applns - since the context is set in the error page
            //app.UseMiddleware<InterneuronSerilogLoggingMiddleware>();



            app.UseCors("AllowAllHeaders");

            app.UseNToastNotify();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Account}/{action=Index}/{id?}");
            //});

            //This is required when hosted in IIS as in-proc - requestimeout in web.config does not work
            //Reference: https://www.seeleycoder.com/blog/asp-net-core-request-timeout-iis-in-process-mode
            app.UseMaximumRequestTimeout();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Index}/{id?}");
            });
        }

        private static void LogException(Exception ex, string errorId)
        {
            //Log.Error(ex.Message);
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) || ex.Message.StartsWith("a network", StringComparison.InvariantCultureIgnoreCase))
                Log.Logger.ForContext("ErrorId", errorId).Fatal(ex, ex.Message);
            else
                Log.Logger.ForContext("ErrorId", errorId).Error(ex, ex.Message);

        }
    }
}
