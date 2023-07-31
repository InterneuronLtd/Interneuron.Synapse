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
﻿using DCSWebAPI.Services.SynapseDynamicAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DCSWebAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHttpContextAccessor();

            string SynapseDynamicAPIURI = Configuration.GetSection("DynamicAPISettings").GetSection("uri").Value;
            services.AddHttpClient();
            services.AddHttpClient<SynapseDynamicAPIClient>(clientConfig =>
            {
                clientConfig.BaseAddress = new Uri(SynapseDynamicAPIURI);
            });
            services.AddHttpClient<FormInstanceHelper>(clientConfig =>
            {
                clientConfig.BaseAddress = new Uri(SynapseDynamicAPIURI);
            });
            services.AddHttpClient<FormResponseHelper>(clientConfig =>
            {
                clientConfig.BaseAddress = new Uri(SynapseDynamicAPIURI);
            });

            //Authorization

            services.AddAuthentication("Bearer")
              .AddIdentityServerAuthentication(options =>
              {
                  options.Authority = Configuration["Settings:AuthorizationAuthority"];
                  options.RequireHttpsMetadata = false;
                  options.ApiName = Configuration["Settings:AuthorizationAudience"];
                  options.EnableCaching = false;

              });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DynamicAPIWriters", builder =>
                {
                    builder.RequireClaim(Configuration["Settings:SynapseRolesClaimType"], Configuration["Settings:DynamicAPIWriteAccessRole"]);
                    builder.RequireScope(Configuration["Settings:WriteAccessAPIScope"]);
                });
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DynamicAPIReaders", builder =>
                {
                    builder.RequireClaim(Configuration["Settings:SynapseRolesClaimType"], Configuration["Settings:DynamicAPIReadAccessRole"]);
                    builder.RequireScope(Configuration["Settings:ReadAccessAPIScope"]);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseCors("AllowAllHeaders");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
