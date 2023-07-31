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


﻿using Hl7.Fhir.Model;
using Interneuron.CareRecord.API.AppCode.Extensions;
using Interneuron.CareRecord.HL7SynapseService.Models;
using System;
using System.Net;

namespace Interneuron.CareRecord.API.AppCode.Core
{
    public class FhirResponseUtil
    {
        public static FhirResponse NotFound(IKey fhirParam)
        {
            if (fhirParam.VersionId == null)
            {
                return WithError(HttpStatusCode.NotFound, $"No {fhirParam.TypeName} resource with id {fhirParam.ResourceId} was found.");
            }
            else
            {
                return WithError(HttpStatusCode.NotFound, $"There is no {fhirParam.TypeName} resource with id {fhirParam.ResourceId}, or there is no version {fhirParam.VersionId}");
            }
        }

        public static FhirResponse WithError(HttpStatusCode statucCode, string message, params object[] args)
        {
            OperationOutcome outcome = new OperationOutcome();
            outcome.AddError(string.Format(message, args));
            return new FhirResponse(statucCode, outcome);
        }

        public static FhirResponse Gone(ResourceData resourceData, IKey fhirPram)
        {
            var message = String.Format(
                  "A {0} resource with id {1} existed, but was deleted on {2}.",// (version {3}).",
                  fhirPram.TypeName,
                  fhirPram.ResourceId,
                  resourceData.DeletedDate);
            //fhirPram.ToRelativeUri());

            return WithError(HttpStatusCode.Gone, message);
        }

        public static FhirResponse WithBundle(Bundle bundle)
        {
            return new FhirResponse(HttpStatusCode.OK, bundle);
        }

        public static FhirResponse WithResource(ResourceData resourceData)
        {
            var resourceDataFHIRPara = resourceData.fHIRParam; // This can be different

            IKey key = Key.Create(resourceDataFHIRPara.TypeName, resourceDataFHIRPara.ResourceId, resourceDataFHIRPara.VersionId);

            return new FhirResponse(HttpStatusCode.OK, key, resourceData.Resource);

        }
    }
}
