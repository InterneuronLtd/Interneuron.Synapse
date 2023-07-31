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
﻿using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Interneuron.Common.Extensions;
using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Interneuron.CareRecord.API.AppCode.Core
{
    public class Validate
    {
        public static void Key(IKey key)
        {
            if (key.HasResourceId())
            {
                Validate.ResourceId(key.ResourceId);
            }
            if (key.HasVersionId())
            {
                Validate.VersionId(key.VersionId);
            }
            if (!string.IsNullOrEmpty(key.TypeName))
            {
                //Validate.TypeName(key.TypeName);
            }
        }

        private static void ResourceId(string resourceId)
        {
            if (resourceId.IsEmpty())
            {
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest,"Logical ID is empty");
            }
        }

        public static void VersionId(string versionId)
        {
            if (String.IsNullOrEmpty(versionId))
            {
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest, "Must pass history id in url.");
            }
        }

        //public static void TypeName(string name)
        //{
        //    if (ModelInfo.SupportedResources.Contains(name))
        //        return;

        //    //  Test for the most common mistake first: wrong casing of the resource name
        //    var correct = ModelInfo.SupportedResources.FirstOrDefault(s => s.ToUpperInvariant() == name.ToUpperInvariant());

        //    if (correct != null)
        //    {
        //        throw new InterneuronBusinessException(StatusCodes.Status404NotFound,"Wrong casing of collection name, try '{0}' instead", correct);
        //    }
        //    else
        //    {
        //        throw new InterneuronBusinessException(StatusCodes.Status404NotFound, "Unknown resource collection '{0}'", name);
        //    }
        //}

        public static void HasTypeName(IKey key)
        {
            if (string.IsNullOrEmpty(key.TypeName))
            {
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest, $"Resource type is missing: {key}");
            }
        }

        public static void HasResourceId(IKey key)
        {
            if (key.HasResourceId())
            {
                Validate.ResourceId(key.ResourceId);
            }
            else
            {
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest, "The request should have a resource id.");
            }
        }

        public static void HasResourceId(Resource resource)
        {
            if (string.IsNullOrEmpty(resource.Id))
            {
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest, "The resource MUST contain an Id.");
            }
        }

        public static void HasVersion(IKey key)
        {
            if (key.HasVersionId())
            {
                Validate.VersionId(key.VersionId);
            }
            else
            {
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest, "The request should contain a version id.");
            }
        }

        public static void HasNoVersion(IKey key)
        {
            if (key.HasVersionId())
            {
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest, "Resource should not contain a version.");
            }
        }

        public static void IsResourceIdEqual(IKey key, Resource resource)
        {
            if (key.ResourceId != resource.Id)
            {
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest, $"The Id in the request '{key.ResourceId}' is not the same is the Id in the resource '{resource.Id}'.");
            }
        }

        internal static void FhirParameterExists(Parameters parameters, string v)
        {
            
        }

        public static void ResourceType(IKey key, Resource resource)
        {
            if (resource == null)
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest, "Request did not contain a body");

            if (key.TypeName != resource.TypeName)
            {
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest,
                    "Received a body with a '{resource.TypeName}' resource, which does not match the indicated collection '{ key.TypeName}' in the url.");
            }

        }

        public static void HasSearchParameters(SearchParams searchParameters)
        {
            if (searchParameters.Parameters.Count < 1)
            {
                throw new InterneuronBusinessException(StatusCodes.Status400BadRequest, "At least one search parameter has to be present");
            }
        }
    }
}
