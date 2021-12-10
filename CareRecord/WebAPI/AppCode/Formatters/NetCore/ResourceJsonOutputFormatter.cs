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


﻿using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.CareRecord.API.AppCode.Extensions;
using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Formatters
{
    public class ResourceJsonOutputFormatter : TextOutputFormatter
    {
        public ResourceJsonOutputFormatter()
        {
            SupportedEncodings.Clear();
            SupportedEncodings.Add(Encoding.UTF8);

            SupportedMediaTypes.Add("application/json");
            SupportedMediaTypes.Add("application/fhir+json");
            SupportedMediaTypes.Add("application/json+fhir");
            SupportedMediaTypes.Add("text/json");
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(Resource).IsAssignableFrom(type) || typeof(FhirResponse).IsAssignableFrom(type);
        }

        public override System.Threading.Tasks.Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (selectedEncoding == null) throw new ArgumentNullException(nameof(selectedEncoding));

            if (selectedEncoding != Encoding.UTF8)
                throw new InterneuronBusinessException((short)HttpStatusCode.BadRequest, $"FHIR supports UTF-8 encoding exclusively, not {selectedEncoding.WebName}");

            using (TextWriter writer = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            using (JsonWriter jsonWriter = new JsonTextWriter(writer))
            {
                if (!(context.HttpContext.RequestServices.GetService(typeof(FhirJsonSerializer)) is FhirJsonSerializer serializer))
                    throw new InterneuronBusinessException((short)HttpStatusCode.InternalServerError, $"Missing required dependency '{nameof(FhirJsonSerializer)}'");

                SummaryType summaryType = context.HttpContext.Request.RequestSummary();
                if (typeof(FhirResponse).IsAssignableFrom(context.ObjectType))
                {
                    FhirResponse response = context.Object as FhirResponse;
                    context.HttpContext.Response.StatusCode = (int)response.StatusCode;
                    if (response.Resource != null)
                        serializer.Serialize(response.Resource, jsonWriter, summaryType);
                }
                else if (context.ObjectType == typeof(OperationOutcome) || typeof(Resource).IsAssignableFrom(context.ObjectType))
                {
                    if (context.Object != null)
                        serializer.Serialize(context.Object as Resource, jsonWriter, summaryType);
                }
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }

    }
}