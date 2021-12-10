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
using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Formatters
{
    public class BinaryInputFormatter : InputFormatter
    {
        public BinaryInputFormatter()
        {
            SupportedMediaTypes.Add(FhirMediaType.OCTET_STREAM_CONTENT_HEADER);
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(Resource);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue("X-Content-Type", out StringValues contentTypeHeaderValues))
                //throw Error.BadRequest("Binary POST must provide a Content-Type header.");
                throw new InterneuronBusinessException((short)HttpStatusCode.BadRequest, "Binary POST must provide a Content-Type header.");

            string contentType = contentTypeHeaderValues.FirstOrDefault();
            MemoryStream memoryStream = new MemoryStream((int) context.HttpContext.Request.Body.Length);
            await context.HttpContext.Request.Body.CopyToAsync(memoryStream);
            Binary binary = new Binary
            {
                ContentType = contentType,
                //Content = memoryStream.ToArray()
                Data = memoryStream.ToArray()
            };

            return await InputFormatterResult.SuccessAsync(binary);
        }
    }
}
