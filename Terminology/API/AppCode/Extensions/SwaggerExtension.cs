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


﻿using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.Filters;
using Interneuron.Terminology.API.AppCode.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Extensions
{
    public static class SwaggerExtension
    {
        static IConfigurationSection SwaggerSection = null;

        public static void AddSwaggerToTerminologyApp(this IServiceCollection services, IConfiguration configuration)
        {
            SwaggerSection = configuration.GetSection("Swagger");

            var swaggerName = SwaggerSection.GetValue<string>("DocumentName");
            var swaggerVer = SwaggerSection.GetValue<string>("DocumentVersion");
            var swaggerAccessScopes = SwaggerSection.GetValue<string>("AccessScopes");

            //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>(); //to be commented later

            services.AddSwaggerGen(conf =>
            {
                conf.OperationFilter<SwaggerDefaultValues>();

                conf.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new System.Uri($"{SwaggerSection.GetValue<string>("AuthorizationAuthority")}/connect/authorize"),
                            TokenUrl = new System.Uri($"{SwaggerSection.GetValue<string>("AuthorizationAuthority")}/connect/token"),
                            Scopes = GetScopes(swaggerAccessScopes)
                        }
                    }
                });


                conf.OperationFilter<AuthorizeCheckOperationFilter>();

                conf.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                //conf.EnableAnnotations();

            });

            services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in

        }

        public static void UseSwaggerToTerminologyApp(this IApplicationBuilder app, IConfiguration configuration, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();

            var swaggerName = SwaggerSection.GetValue<string>("DocumentName");
            var oAuthClientId = SwaggerSection.GetValue<string>("OAuthClientId");
            var oAuthClientName = SwaggerSection.GetValue<string>("OAuthClientName");

            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint($"./{swaggerName}/swagger.json", $"CareRecordAPI {configuration["API_Version"]}");
                
                // create swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    //c.SwaggerEndpoint($"./swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

                    c.SwaggerEndpoint($"./{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                c.OAuthClientId(oAuthClientId);
                c.OAuthAppName(oAuthClientName);
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

    }
}
