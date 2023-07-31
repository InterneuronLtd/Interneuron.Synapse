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
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.CareRecord.API.AppCode.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Extensions
{
    public static class HttpFHIRExtensions
    {
        public static SummaryType RequestSummary(this HttpRequestMessage request)
        {
            string summary = request.GetParameter("_summary");
            return GetSummary(summary);
        }

        public static string GetParameter(this HttpRequestMessage request, string key)
        {
            NameValueCollection queryNameValuePairs = request.RequestUri.ParseQueryString();
            foreach (var currentKey in queryNameValuePairs.AllKeys)
            {
                if (currentKey == key) return queryNameValuePairs[currentKey];
            }
            return null;
        }

        public static SummaryType RequestSummary(this HttpRequest request)
        {
            request.Query.TryGetValue("_summary", out StringValues stringValues);
            return GetSummary(stringValues.FirstOrDefault());
        }

        private static SummaryType GetSummary(string summary)
        {
            SummaryType? summaryType;
            if (string.IsNullOrWhiteSpace(summary))
                summaryType = SummaryType.False;
            else
                summaryType = EnumUtility.ParseLiteral<SummaryType>(summary, true);

            return summaryType ?? SummaryType.False;
        }

        public static bool IsRawBinaryRequest(this HttpRequestMessage request, Type type)
        {
            if (type == typeof(Binary) || type == typeof(FhirResponse))
            {
                bool isFhirMediaType = false;
                if (request.Method == HttpMethod.Get)
                    isFhirMediaType = request.IsAcceptHeaderFhirMediaType();
                else if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
                    isFhirMediaType = request.Content.IsContentTypeHeaderFhirMediaType();

                var ub = new UriBuilder(request.RequestUri);
                // TODO: KM: Path matching is not optimal should be replaced by a more solid solution.
                return ub.Path.Contains("Binary")
                    && !isFhirMediaType;
            }
            else
                return false;
        }

        public static bool IsRawBinaryPostOrPutRequest(this HttpRequestMessage request)
        {
            var ub = new UriBuilder(request.RequestUri);
            // TODO: KM: Path matching is not optimal should be replaced by a more solid solution.
            return ub.Path.Contains("Binary")
                && !request.Content.IsContentTypeHeaderFhirMediaType()
                && (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put);
        }

        /// <summary>
        /// Returns true if the Accept header matches any of the FHIR supported Xml or Json MIME types, otherwise false.
        /// </summary>
        /// <param name="content">An instance of <see cref="HttpRequestMessage"/>.</param>
        /// <returns>Returns true if the Accept header matches any of the FHIR supported Xml or Json MIME types, otherwise false.</returns>
        public static bool IsAcceptHeaderFhirMediaType(this HttpRequestMessage request)
        {
            string accept = request.GetAcceptHeaderValue();
            return ContentType.XML_CONTENT_HEADERS.Contains(accept)
                || ContentType.JSON_CONTENT_HEADERS.Contains(accept);
        }

        public static IEnumerable<string> GetSegments(this IKey key)
        {
            if (key.Base != null) yield return key.Base;
            if (key.TypeName != null) yield return key.TypeName;
            if (key.ResourceId != null) yield return key.ResourceId;
            if (key.VersionId != null)
            {
                yield return "_history";
                yield return key.VersionId;
            }
        }

        public static string ToUriString(this IKey key)
        {
            var segments = key.GetSegments();
            return string.Join("/", segments);
        }
    }
}
