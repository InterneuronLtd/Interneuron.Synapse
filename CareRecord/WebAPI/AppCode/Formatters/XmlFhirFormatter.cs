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


﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Text;
using Hl7.Fhir.Rest;
using Interneuron.CareRecord.API.AppCode.Formatters;
using Interneuron.CareRecord.API.AppCode.Extensions;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.Infrastructure.CustomExceptions;

namespace Interneuron.CareRecord.API.AppCode.Formatters
{
    public class XmlFhirFormatter : FhirMediaTypeFormatter
    {
        private readonly FhirXmlParser _parser;
        private readonly FhirXmlSerializer _serializer;

        public XmlFhirFormatter(FhirXmlParser parser, FhirXmlSerializer serializer) : base()
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            foreach (var mediaType in ContentType.XML_CONTENT_HEADERS)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaType));
            }
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = FhirMediaType.GetMediaTypeHeaderValue(type, ResourceFormat.Xml);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            try
            {
                var body = base.ReadBodyFromStream(readStream, content);

                if (type == typeof(Bundle))
                {
                    if (XmlSignatureHelper.IsSigned(body))
                    {
                        if (!XmlSignatureHelper.VerifySignature(body))
                            //throw Error.BadRequest("Digital signature in body failed verification");
                            throw new InterneuronBusinessException((short)HttpStatusCode.BadRequest, "Digital signature in body failed verification");
                    }
                }

                if (typeof(Resource).IsAssignableFrom(type))
                {
                    Resource resource = _parser.Parse<Resource>(body);
                    return System.Threading.Tasks.Task.FromResult<object>(resource);
                }
                else
                    //throw Error.Internal("The type {0} expected by the controller can not be deserialized", type.Name);
                    throw new InterneuronBusinessException((short)HttpStatusCode.InternalServerError, $"The type {type.Name} expected by the controller can not be deserialized");
            }
            catch (FormatException exc)
            {
                //throw Error.BadRequest("Body parsing failed: " + exc.Message);
                throw new InterneuronBusinessException(exc, (short)HttpStatusCode.BadRequest, "Body parsing failed: " + exc.Message);
            }
        }

        public override System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            XmlWriter writer = new XmlTextWriter(writeStream, new UTF8Encoding(false));
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
            else if (type == typeof(FhirResponse))
            {
                FhirResponse response = (value as FhirResponse);
                if (response.HasBody)
                    _serializer.Serialize(response.Resource, writer, summary);
            }

            writer.Flush();
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }

}


