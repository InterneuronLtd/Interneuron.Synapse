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


﻿using System.Web.Http.Description;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using Swashbuckle.Swagger;
using InterneuronFDBAPI.Infrastructure;

namespace InterneuronFDBAPI.Configuration
{
    public class AssignOAuth2SecurityRequirements : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            //// Determine if the operation has the Authorize attribute
            //var authorizeAttributes = apiDescription
            //    .ActionDescriptor.GetCustomAttributes<FDBAuthorization>();

            //if (!authorizeAttributes.Any())
            //    return;

            //// Initialize the operation.security property
            //if (operation.security == null)
            //    operation.security = new List<IDictionary<string, IEnumerable<string>>>();

            //// Add the appropriate security definition to the operation
            //var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
            //    {
            //        { "oauth2", Enumerable.Empty<string>() }
            //    };

            //operation.security.Add(oAuthRequirements);

            var scopes = apiDescription.ActionDescriptor.GetFilterPipeline()
                .Select(filterInfo => filterInfo.Instance)
                .OfType<AuthorizeAttribute>()
                .SelectMany(attr => attr.Roles.Split(','))
                .Distinct();

            scopes = new List<string> { "terminologyapi.read", "terminologyapi.write" };

            if (scopes.Any())
            {
                if (operation.security == null)
                    operation.security = new List<IDictionary<string, IEnumerable<string>>>();

                var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", scopes }
                };

                operation.security.Add(oAuthRequirements);
            }
        }
    }
}