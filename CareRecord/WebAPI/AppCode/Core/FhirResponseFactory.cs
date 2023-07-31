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


using Hl7.Fhir.Model;
using Interneuron.CareRecord.HL7SynapseService.Models;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.CareRecord.API.AppCode.Core
{
    public interface IFhirResponseFactory
    {
        FhirResponse GetFhirResponse(ResourceData resourceData, IKey key = null, IEnumerable<object> parameters = null);
        FhirResponse GetFhirResponse(ResourceData resourceData, IKey key = null, params object[] parameters);
        FhirResponse GetFhirResponse(Bundle bundle);
    }

    public class FhirResponseFactory : IFhirResponseFactory
    {
        public FhirResponse GetFhirResponse(ResourceData resourceData, IKey key = null, IEnumerable<object> parameters = null)
        {
            if (resourceData == null || resourceData.Resource == null)
            {
                return (resourceData != null && resourceData.IsDeleted)
                    ? FhirResponseUtil.Gone(resourceData, key)
                    : FhirResponseUtil.NotFound(key);
            }


            //FhirResponse response = null;

            //return response ?? FhirResponseUtil.WithResource(resourceData);
            return FhirResponseUtil.WithResource(resourceData);
        }

        public FhirResponse GetFhirResponse(ResourceData resourceData, IKey key = null, params object[] parameters)
        {
            return GetFhirResponse(resourceData, key, parameters.ToList());
        }

        public FhirResponse GetFhirResponse(Bundle bundle)
        {
            return FhirResponseUtil.WithBundle(bundle);
        }
    }
}
