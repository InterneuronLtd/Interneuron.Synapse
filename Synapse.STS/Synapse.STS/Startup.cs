//Interneuron Synapse

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


﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Synapse.STS.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Synapse.STS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using IdentityServer4.EntityFramework.DbContexts;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.Mappers;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.HttpOverrides;

namespace Synapse.STS
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        private readonly IConfiguration _configuration;


        public Startup(IConfiguration configuration, IHostingEnvironment environment, IServiceProvider serviceProvider)
        {
            Configuration = configuration;
            Environment = environment;
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _configuration["Settings:ConnectionString"];//Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

     //       X509Store store = new X509Store(StoreLocation.CurrentUser);
              X509Certificate2 cer = new X509Certificate2();
     //       try
     //       {
     //           string tb = _configuration["Settings:SigningCredentialsTP"];
         //       store.Open(OpenFlags.ReadOnly);
         //       X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint,
         //         tb, false);
         //       if (col == null || col.Count == 0)
     //           {

     //           }
         //       else { cer = col[0]; }

            //}
            //finally
            //{
         //       store = null;
     //       }


            services.AddDbContext<SynapseDbContext>(options =>
                options.UseNpgsql(connectionString));


            services.AddIdentity<SynapseUser, IdentityRole>()
                .AddEntityFrameworkStores<SynapseDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<SynapseRoleBasedClaimsContext>(options =>
                options.UseNpgsql(connectionString));



            services.AddMvc();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                // options.Authentication.CookieLifetime = TimeSpan.FromMinutes(1);
            })
                .AddAspNetIdentity<SynapseUser>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseNpgsql(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseNpgsql(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;

                    // options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                });

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(_configuration["Settings:SigningCredentialsTP"]))
                    builder.AddDeveloperSigningCredential();
                else
                    builder.AddSigningCredential(cer);
            }



            services.AddAuthentication().AddWsFederation(_configuration["Settings:ADFSSchemeName"], _configuration["Settings:ADFSDisplayName"], options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                //options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                // MetadataAddress represents the Active Directory instance used to authenticate users.
                options.MetadataAddress = _configuration["Settings:ADFSMetaAddress"];

                // Wtrealm is the app's identifier in the Active Directory instance.
                // For ADFS, use the relying party's identifier, its WS-Federation Passive protocol URL:
                // For AAD, use the App ID URI from the app registration's Properties blade:

                options.Events.OnRemoteFailure = context => HandleRemoteFailure(context);
                options.Wtrealm = _configuration["Settings:ADFSWtrealm"];
                options.Events.OnTicketReceived += OnTicketReceived;
                options.RequireHttpsMetadata = false;
                options.SaveTokens = false;
                options.RemoteAuthenticationTimeout = TimeSpan.FromSeconds(double.Parse(_configuration["Settings:RemoteAuthTimeoutSecs"]));


            }).AddOpenIdConnect(_configuration["Settings:AADSchemeName"], _configuration["Settings:AADDisplayName"], options =>
            {

                options.ClientId = _configuration["Settings:AzureClientId"];
                options.Authority = string.Format(CultureInfo.InvariantCulture, "https://login.microsoft.com/{0}", _configuration["Settings:AzureTenantId"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false
                };

                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Events = new OpenIdConnectEvents
                {
                    OnRemoteFailure = context => HandleRemoteFailure(context)
                };
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.CallbackPath = "/signin-oidc";
                options.RemoteAuthenticationTimeout = TimeSpan.FromSeconds(double.Parse(_configuration["Settings:RemoteAuthTimeoutSecs"]));
                //options.NonceCookie.Expiration = new TimeSpan(2, 0, 0, 0); 
                options.ProtocolValidator.NonceLifetime = TimeSpan.FromSeconds(double.Parse(_configuration["Settings:RemoteAuthTimeoutSecs"]));

            });

            builder.Services.AddTransient<IProfileService, CustomClaims>();

            services.ConfigureApplicationCookie(options =>
            {

                options.ExpireTimeSpan = TimeSpan.FromMinutes(double.Parse(_configuration["Settings:CookieExpirationTimeout"]));
                options.SlidingExpiration = bool.Parse(_configuration["Settings:SlidingExpiration"]);

            });
        }

        private Task HandleRemoteFailure(RemoteFailureContext context)
        {
            if (context.Failure.Message.ToLower().Contains("correlation"))
            {
                context.HandleResponse();
                context.Response.Redirect("/Home/CorrelationError");
                return Task.FromResult(0);
            }
            else
                throw context.Failure;
        }

        private async Task OnTicketReceived(TicketReceivedContext ticketReceivedContext)
        {
            // Only one identity supported by the current implementation of IdentityServer4
            if (ticketReceivedContext.Principal.Identities.Count() != 1) throw new InvalidOperationException("only a single identity supported");

            var oldIdentity = ticketReceivedContext.Principal.Identity as ClaimsIdentity;
            var oldPrincipal = ticketReceivedContext.Principal;
            oldPrincipal.Clone();
            var claims = new List<Claim>();
            claims.AddRange(oldPrincipal.Claims);
            claims.Add(new Claim("sub", ticketReceivedContext.Principal.FindFirstValue(_configuration["Settings:WindowsAccountNameClaimtype"])));
            var newIdentity = new ClaimsIdentity(claims, oldIdentity.AuthenticationType, oldIdentity.NameClaimType, oldIdentity.RoleClaimType);
            newIdentity.Actor = oldIdentity.Actor;
            ticketReceivedContext.Principal = new ClaimsPrincipal(newIdentity);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            // ref: https://github.com/aspnet/Docs/issues/2384
            app.UseForwardedHeaders(forwardOptions);

            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();

        }
    }
}
