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
﻿using Hl7.Fhir.Rest;
using System;
using System.Linq;
using System.Net.Http;

namespace Interneuron.CareRecord.API.AppCode.Extensions
{
    public static class HttpHeadersExtension
    {
        public static bool IsContentTypeHeaderFhirMediaType(this HttpContent content)
        {
            string contentType = content.Headers.ContentType?.MediaType;
            return ContentType.XML_CONTENT_HEADERS.Contains(contentType)
                || ContentType.JSON_CONTENT_HEADERS.Contains(contentType);
        }
    }
}
