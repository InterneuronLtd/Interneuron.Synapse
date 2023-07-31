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
using Hl7.Fhir.Serialization;
using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Formatters
{
    public class ResourceXmlInputFormatter : TextInputFormatter
    {
        public ResourceXmlInputFormatter()
        {
            SupportedEncodings.Clear();
            SupportedEncodings.Add(Encoding.UTF8);

            SupportedMediaTypes.Add("application/xml");
            SupportedMediaTypes.Add("application/fhir+xml");
            SupportedMediaTypes.Add("application/xml+fhir");
            SupportedMediaTypes.Add("text/xml");
            SupportedMediaTypes.Add("text/xml+fhir");
        }

        protected override bool CanReadType(Type type)
        {
            return typeof(Resource).IsAssignableFrom(type);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            if (encoding != Encoding.UTF8)
                // throw Error.BadRequest("FHIR supports UTF-8 encoding exclusively, not " + encoding.WebName);
                // throw new ArgumentNullException(nameof(encoding));
                throw new InterneuronBusinessException((short)HttpStatusCode.BadRequest, $"FHIR supports UTF-8 encoding exclusively, not ${encoding.WebName}");

            try
            {
                using (TextReader reader = context.ReaderFactory(context.HttpContext.Request.Body, encoding))
                {
                    FhirXmlParser parser = context.HttpContext.RequestServices.GetRequiredService<FhirXmlParser>();
                    return await InputFormatterResult.SuccessAsync(parser.Parse(await reader.ReadToEndAsync()));
                }
            }
            catch (FormatException exception)
            {
                //throw Error.BadRequest($"Body parsing failed: {exception.Message}");
                throw new InterneuronBusinessException(exception, (short)HttpStatusCode.BadRequest, $"Body parsing failed: {exception.Message}");
            }
        }
    }
}