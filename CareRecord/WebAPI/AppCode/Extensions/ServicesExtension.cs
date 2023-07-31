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
﻿using AutoMapper;
using Interneuron.CareRecord.API.AppCode.Formatters;
using Interneuron.CareRecord.API.AppCode.Formatters.NetCore;
using Interneuron.CareRecord.API.AppCode.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Extensions
{
    public static class ServicesExtension
    {
        public static void AddAuthenticationToCareRecord(this IServiceCollection services, IConfiguration configuration)
        {
            var CareRecordConfigSection = configuration.GetSection("CareRecordConfig");

            services.AddAuthentication("Bearer")
              .AddIdentityServerAuthentication(options =>
              {
                  options.Authority = CareRecordConfigSection["AuthorizationAuthority"];

                  options.RequireHttpsMetadata = false;
                  
                  options.ApiName = CareRecordConfigSection["AuthorizationAudience"];
                  
                  options.EnableCaching = false;
                  
                  string byPassSSLValidation = CareRecordConfigSection["IgnoreIdentitySeverSSLErrors"];

                  if (!string.IsNullOrWhiteSpace(byPassSSLValidation) && byPassSSLValidation.ToLower() == "true")
                      options.JwtBackChannelHandler = GetHandler();
              });

            //To be uncommented for CBAC - Refer SynapseDynamicAPI
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CareRecordAPIWriters", builder =>
            //    {
            //        builder.RequireClaim(configuration["SynapseCore:Settings:SynapseRolesClaimType"], configuration["SynapseCore:Settings:DynamicAPIWriteAccessRole"]);
            //        builder.RequireScope(configuration["SynapseCore:Settings:WriteAccessAPIScope"]);
            //    });
            //});
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CareRecordAPIReaders", builder =>
            //    {
            //        builder.RequireClaim(configuration["SynapseCore:Settings:SynapseRolesClaimType"], configuration["SynapseCore:Settings:DynamicAPIReadAccessRole"]);
            //        builder.RequireScope(configuration["SynapseCore:Settings:ReadAccessAPIScope"]);
            //    });
            //});

        }

        private static HttpClientHandler GetHandler()
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            return handler;
        }

        public static void AddAutoMapperToCareRecord(this IServiceCollection services)
        {
            var assemblies = GetAssembliesFromBaseDirectory();

            services.AddAutoMapper(assemblies);
        }
        public static void AddFhir(this IServiceCollection services, CareRecordAPISettings settings, Action<MvcOptions> setupAction = null)
        {
            //AddFhirFormatters().
        }

        public static void AddFhirFormatters(this IServiceCollection services, Action<MvcOptions> setupAction = null)
        {
            services.AddMvc(options =>
            {
                options.InputFormatters.Add(new ResourceJsonInputFormatter());
                options.InputFormatters.Add(new SynapseResourceJsonInputFormatter());
                options.InputFormatters.Add(new GenericJsonInputFormatter());
                options.InputFormatters.Add(new ResourceXmlInputFormatter());
                options.InputFormatters.Add(new BinaryInputFormatter());
                options.OutputFormatters.Add(new ResourceJsonOutputFormatter());
                options.OutputFormatters.Add(new SynapseResourceJsonOutputFormatter());
                options.OutputFormatters.Add(new GenericJsonOutputFormatter());
                options.OutputFormatters.Add(new ResourceXmlOutputFormatter());
                options.OutputFormatters.Add(new BinaryOutputFormatter());

                options.RespectBrowserAcceptHeader = true;

                setupAction?.Invoke(options);
            })
            .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        private static Assembly[] GetAssembliesFromBaseDirectory()
        {
            //Load Assemblies
            //Get All assemblies.
            var refAssembyNames = Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies();

            if (refAssembyNames != null)
            {
                var refFilteredAssembyNames = refAssembyNames.Where(refAsm => refAsm.FullName.StartsWith("Interneuron.CareRecord"));

                //Load referenced assemblies
                foreach (var assemblyName in refFilteredAssembyNames)
                {
                    Assembly.Load(assemblyName);
                }
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies != null ? assemblies
                .Where(refAsm => refAsm.FullName.StartsWith("Interneuron.CareRecord")).ToArray() : null;

        }
    }
}
