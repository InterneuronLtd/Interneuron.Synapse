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


﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using Hl7.Fhir.Rest;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.CareRecord.API.AppCode.Extensions;
using Interneuron.Infrastructure.CustomExceptions;

namespace Interneuron.CareRecord.API.AppCode.Formatters
{
    public class JsonFhirFormatter : FhirMediaTypeFormatter
    {
        private readonly FhirJsonParser _parser;
        private readonly FhirJsonSerializer _serializer;

        public JsonFhirFormatter(FhirJsonParser parser, FhirJsonSerializer serializer) : base()
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            foreach (var mediaType in ContentType.JSON_CONTENT_HEADERS)
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaType));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = FhirMediaType.GetMediaTypeHeaderValue(type, ResourceFormat.Json);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            try
            {
                var body = base.ReadBodyFromStream(readStream, content);

                if (typeof(Resource).IsAssignableFrom(type))
                {
                    Resource resource = _parser.Parse<Resource>(body);
                    return System.Threading.Tasks.Task.FromResult<object>(resource);
                }
                else
                {
                    //throw Error.Internal("Cannot read unsupported type {0} from body", type.Name);
                    throw new InterneuronBusinessException((short)HttpStatusCode.InternalServerError, $"Cannot read unsupported type { type.Name} from body");
                }
            }
            catch (FormatException exception)
            {
                //throw Error.BadRequest("Body parsing failed: " + exception.Message);
                throw new InterneuronBusinessException(exception, (short)HttpStatusCode.BadRequest, "Body parsing failed: " + exception.Message);
            }
        }

        public override System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            using (StreamWriter streamwriter = new StreamWriter(writeStream))
            using (JsonWriter writer = new JsonTextWriter(streamwriter))
            {
                SummaryType summary = requestMessage.RequestSummary();

                if (type == typeof(OperationOutcome))
                {
                    Resource resource = (Resource)value;
                    _serializer.Serialize(resource, writer, summary);
                }
                else if (typeof(Resource).IsAssignableFrom(type))
                {
                    Resource resource = (Resource)value;
                    _serializer.Serialize(resource, writer, summary);
                }
                else if (typeof(FhirResponse).IsAssignableFrom(type))
                {
                    FhirResponse response = (value as FhirResponse);
                    if (response.HasBody)
                    {
                        _serializer.Serialize(response.Resource, writer, summary);
                    }
                }
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
