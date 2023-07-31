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


﻿using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Formatters.NetCore
{
    public class GenericJsonInputFormatter : TextInputFormatter
    {
        public GenericJsonInputFormatter()
        {
            SupportedEncodings.Clear();
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);

            SupportedMediaTypes.Add("application/json");
            SupportedMediaTypes.Add("text/json");
        }

        protected override bool CanReadType(Type type)
        {
            return !(typeof(SynapseResource).IsAssignableFrom(type) || typeof(List<SynapseResource>).IsAssignableFrom(type)) && !(typeof(Hl7.Fhir.Model.Resource).IsAssignableFrom(type));
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (encoding != Encoding.UTF8 && encoding != UTF16EncodingLittleEndian)
                throw new InterneuronBusinessException((short)HttpStatusCode.BadRequest, $"Care Record API supports UTF-8 or UTF-16 encoding exclusively, not ${encoding.WebName}");

            try
            {
                using (TextReader reader = context.ReaderFactory(context.HttpContext.Request.Body, encoding))
                {
                    if (typeof(string) == context.ModelType)//to support raw string
                    {
                        var body = await reader.ReadToEndAsync();

                        if (body.StartsWith('"'))//if already strigified and sent
                        {
                            var strAsObject = await InputFormatterResult.SuccessAsync(JsonConvert.DeserializeObject(body, context.ModelType));
                            return strAsObject;
                        }

                        var stringInput = await InputFormatterResult.SuccessAsync(body);
                        return stringInput;
                    }
                    var anyObject = await InputFormatterResult.SuccessAsync(JsonConvert.DeserializeObject(await reader.ReadToEndAsync(), context.ModelType));
                    return anyObject;
                }

            }
            catch (FormatException exception)
            {
                throw new InterneuronBusinessException(exception, (short)HttpStatusCode.BadRequest, $"Body parsing failed: {exception.Message}");
            }
        }
    }
}
