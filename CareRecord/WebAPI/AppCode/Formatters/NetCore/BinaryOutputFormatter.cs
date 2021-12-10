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
using Interneuron.CareRecord.API.AppCode.Core;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;

namespace Interneuron.CareRecord.API.AppCode.Formatters
{
    public class BinaryOutputFormatter : OutputFormatter
    {
        public BinaryOutputFormatter()
        {
            SupportedMediaTypes.Add(FhirMediaType.OCTET_STREAM_CONTENT_HEADER);
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof(Binary) || type == typeof(FhirResponse);
        }

        public override async System.Threading.Tasks.Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (typeof(Binary).IsAssignableFrom(context.ObjectType))
            {
                Binary binary = (Binary)context.Object;
                //var stream = new MemoryStream(binary.Content);
                var stream = new MemoryStream(binary.Data);
                context.HttpContext.Response.Headers.Add("Content-Type", binary.ContentType);
                await stream.CopyToAsync(context.HttpContext.Response.Body);
            }
        }
    }
}
