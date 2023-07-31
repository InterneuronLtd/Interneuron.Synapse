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


﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Interneuron.CareRecord.Infrastructure.Tasks;
using Microsoft.Extensions.Configuration;
using Interneuron.Infrastructure.CustomExceptions;

namespace Interneuron.CareRecord.API.AppCode.Filters
{
    public class GlobalFilter : IAsyncActionFilter, IAsyncResultFilter, IAsyncExceptionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Should handle any request beginning task
            if (!IsSupportedHL7Type(context) && context.RouteData.Values.ContainsKey("action") && context.RouteData.Values["action"].ToString().EqualsIgnoreCase("fhir"))
            {
                throw new InterneuronBusinessException(500, "This HL7 resource type is not supported in Care Record API");
            }
            
            var actionContext = await next();
        }


        public Task OnExceptionAsync(ExceptionContext context)
        {
           ExecuteCleanupTask(context.HttpContext, context.Exception);
            return Task.CompletedTask;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultContext = await next();
            ExecuteCleanupTask(resultContext.HttpContext, null);
        }

        private void ExecuteStartupTask(HttpContext context, Exception ex)
        {
            var startupTasks = (IRequestStartupTask[])context.RequestServices.GetService(typeof(IRequestStartupTask[]));
            startupTasks?.Each(t => t.Execute());
        }

        private void ExecuteCleanupTask(HttpContext context, Exception ex)
        {
            var cleanupTasks = (IRequestCleanupTask[])context.RequestServices.GetService(typeof(IRequestCleanupTask[]));
            cleanupTasks?.Each(t => t.Execute(null));
        }

        private bool IsSupportedHL7Type(ActionExecutingContext context)
        {
            bool isSupportedHL7Type = false;

            if (context.RouteData.Values.ContainsKey("type"))
            {

                IConfiguration configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

                IConfigurationSection careRecordConfig = configuration.GetSection("CareRecordConfig");

                IEnumerable<KeyValuePair<string, string>> supportedHL7Types = careRecordConfig.GetSection("SupportedHL7Types").AsEnumerable();

                string hl7Type = context.RouteData.Values.GetValueOrDefault("type").ToString();

                isSupportedHL7Type = supportedHL7Types.Any(x => x.Value == hl7Type);

            }

            return isSupportedHL7Type;
        }
    }
}
