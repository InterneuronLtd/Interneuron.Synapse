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


﻿using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private IConfiguration _configuration;

        public AuthorizeCheckOperationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var SwaggerSection = _configuration.GetSection("Swagger");

            var swaggerAccessScopes = SwaggerSection.GetValue<string>("AccessScopes");

            // Policy names map to scopes
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct().ToList();

            // Check for authorize attribute
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                               context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!hasAuthorize) return;

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            if (requiredScopes.IsCollectionValid())
                requiredScopes.AddRange(GetScopes(swaggerAccessScopes));
            else
                requiredScopes = GetScopes(swaggerAccessScopes);

            if (requiredScopes.Any())
            {
                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ oAuthScheme ] = requiredScopes.ToList()
                    }
                };
            }

            //operation.Security = new List<OpenApiSecurityRequirement>
            //{
            //    new OpenApiSecurityRequirement(){
            //    {
            //        new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            //            },
            //            new[] { "carerecordapi.read", "carerecordapi.write" }
            //    }
            //    }
            //};
        }

        private static List<string> GetScopes(string swaggerAccessScopes)
        {
            var scopes = new List<string>();

            swaggerAccessScopes.Split(';')
                            .Each(scopeUnit =>
                            {
                                if (scopeUnit.IsNotEmpty())
                                {
                                    var scopeKV = scopeUnit.Split(':');
                                    if (scopeKV.IsCollectionValid())
                                    {
                                        scopes.Add(scopeKV[0]);
                                    }
                                }
                            });
            return scopes;
        }
    }
}
