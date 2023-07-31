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
﻿using Interneuron.Infrastructure.CustomExceptions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Formatters.NetCore
{
    public class TerminologyResourceJsonOutputFormatter : TextOutputFormatter
    {
        public TerminologyResourceJsonOutputFormatter()
        {
            SupportedEncodings.Clear();
            SupportedEncodings.Add(Encoding.UTF8);

            SupportedMediaTypes.Add("application/json");
            SupportedMediaTypes.Add("text/json");
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(List<TerminologyResource>).IsAssignableFrom(type) || typeof(TerminologyResource).IsAssignableFrom(type) || typeof(TerminologyResponse).IsAssignableFrom(type);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (selectedEncoding == null) throw new ArgumentNullException(nameof(selectedEncoding));

            if (selectedEncoding != Encoding.UTF8)
                throw new InterneuronBusinessException((short)StatusCodes.Status400BadRequest, $"API supports UTF-8 encoding exclusively, not {selectedEncoding.WebName}");

            if (typeof(TerminologyResponse).IsAssignableFrom(context.ObjectType))
            {
                var response = context.Object as TerminologyResponse;
                context.HttpContext.Response.StatusCode = (int)response.StatusCode; //StatusCodes.Status200OK;

                if (response.Resource != null)
                {
                    var responseString = JsonConvert.SerializeObject(response.Resource);
                    return context.HttpContext.Response.WriteAsync(responseString);
                }
            }
            else if (typeof(TerminologyResource).IsAssignableFrom(context.ObjectType))
            {
                var response = context.Object as TerminologyResource;
                context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;

                if (response != null)
                {
                    var responseString = JsonConvert.SerializeObject(response);
                    return context.HttpContext.Response.WriteAsync(responseString);
                }
            }
            else if (typeof(List<TerminologyResource>).IsAssignableFrom(context.ObjectType))
            {
                var synapseResponseList = context.Object as List<TerminologyResource>;
                context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;// (int)response.StatusCode;

                if (synapseResponseList != null)
                {
                    var responseString = JsonConvert.SerializeObject(synapseResponseList);
                    return context.HttpContext.Response.WriteAsync(responseString);
                }
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
