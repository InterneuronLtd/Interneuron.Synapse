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


﻿using Hl7.Fhir.Rest;
using Interneuron.CareRecord.API.AppCode.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Interneuron.CareRecord.API.AppCode.Formatters
{
    public class FhirContentNegotiator : DefaultContentNegotiator
    {
        public override ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            MediaTypeFormatter formatter;
            if(request.IsRawBinaryRequest(type))
            {
                formatter = formatters.Where(f => f is BinaryFhirFormatter).SingleOrDefault();
                if (formatter != null) return new ContentNegotiationResult(formatter.GetPerRequestFormatterInstance(type, request, null), null);
            }

            return base.Negotiate(type, request, formatters);
        }
    }
}