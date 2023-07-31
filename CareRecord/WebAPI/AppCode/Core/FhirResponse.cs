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
using Interneuron.CareRecord.API.AppCode.Extensions;
using System.Net;

namespace Interneuron.CareRecord.API.AppCode.Core
{
    public class FhirResponse
    {
        public HttpStatusCode StatusCode;
        public IKey Key;
        public Resource Resource;

        public FhirResponse(HttpStatusCode code, IKey key, Resource resource)
        {
            this.StatusCode = code;
            this.Key = key;
            this.Resource = resource;
        }

        public FhirResponse(HttpStatusCode code, Resource resource)
        {
            this.StatusCode = code;
            this.Key = null;
            this.Resource = resource;
        }

        public FhirResponse(HttpStatusCode code)
        {
            this.StatusCode = code;
            this.Key = null;
            this.Resource = null;
        }

        public bool IsValid
        {
            get
            {
                int code = (int)this.StatusCode;
                return code <= 300;
            }
        }

        public bool HasBody
        {
            get
            {
                return Resource != null;
            }
        }

        public override string ToString()
        {
            string details = (Resource != null) ? string.Format("({0})", Resource.TypeName) : null;
            string location = "key value"; //Key?.ToString();
            return string.Format("{0}: {1} {2} ({3})", (int)StatusCode, StatusCode.ToString(), details, location);
        }

        public static FhirResponse WithError(HttpStatusCode code, string message, params object[] args)
        {
            OperationOutcome outcome = new OperationOutcome();
            outcome.AddError(string.Format(message, args));
            return new FhirResponse(code, outcome);
        }
    }
}
