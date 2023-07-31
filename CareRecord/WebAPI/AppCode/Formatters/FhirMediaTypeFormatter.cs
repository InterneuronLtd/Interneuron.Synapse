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
﻿using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using Hl7.Fhir.Model;
using Interneuron.Infrastructure.CustomExceptions;
using System.Net;

namespace Interneuron.CareRecord.API.AppCode.Formatters
{
    public abstract class FhirMediaTypeFormatter : MediaTypeFormatter
    {
        protected HttpRequestMessage requestMessage;

        public FhirMediaTypeFormatter() : base()
        {
            SupportedEncodings.Clear();
            SupportedEncodings.Add(Encoding.UTF8);
        }
        
        public override bool CanReadType(Type type)
        {
            bool can = typeof(Resource).IsAssignableFrom(type);
            return can;
        }

        public override bool CanWriteType(Type type)
        {
            return typeof(Resource).IsAssignableFrom(type);
        }
        
        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            requestMessage = request;
            return base.GetPerRequestFormatterInstance(type, request, mediaType);
        }

        protected string ReadBodyFromStream(Stream readStream, HttpContent content)
        {
            var charset = content.Headers.ContentType.CharSet ?? Encoding.UTF8.HeaderName;
            var encoding = Encoding.GetEncoding(charset);

            if (encoding != Encoding.UTF8)
                //throw Error.BadRequest("FHIR supports UTF-8 encoding exclusively, not " + encoding.WebName);
                throw new InterneuronBusinessException((short)HttpStatusCode.BadRequest, "FHIR supports UTF-8 encoding exclusively, not " + encoding.WebName);


            StreamReader reader = new StreamReader(readStream, Encoding.UTF8, true);
            return reader.ReadToEnd();
        }
    }
}