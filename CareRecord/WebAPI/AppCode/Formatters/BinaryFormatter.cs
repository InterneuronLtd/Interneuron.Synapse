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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Interneuron.CareRecord.API.AppCode.Core;
using System.Net.Http.Formatting;
using Interneuron.Infrastructure.CustomExceptions;
using System.Net;

namespace Interneuron.CareRecord.API.AppCode.Formatters
{
    public class BinaryFhirFormatter : FhirMediaTypeFormatter
    {
        public BinaryFhirFormatter() : base()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(FhirMediaType.OCTET_STREAM_CONTENT_HEADER));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(Resource);
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(Binary)  || type == typeof(FhirResponse);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var success = content.Headers.TryGetValues("X-Content-Type", out IEnumerable<string> contentHeaders);

            if (!success)
            {
                //throw Error.BadRequest("POST to binary must provide a Content-Type header");
                throw new InterneuronBusinessException((short)HttpStatusCode.BadRequest, "POST to binary must provide a Content-Type header");
            }

            string contentType = contentHeaders.FirstOrDefault();
            MemoryStream stream = new MemoryStream();
            readStream.CopyTo(stream);
            Binary binary = new Binary
            {
                Content = stream.ToArray(),
                ContentType = contentType
            };

            return System.Threading.Tasks.Task.FromResult((object)binary);
        }

        public override System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, System.Net.TransportContext transportContext)
        {
            Binary binary = (Binary)value;
            var stream = new MemoryStream(binary.Content);
            content.Headers.ContentType = new MediaTypeHeaderValue(binary.ContentType);
            stream.CopyTo(writeStream);
            stream.Flush();

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}