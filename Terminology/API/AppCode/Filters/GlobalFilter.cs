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
﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Interneuron.Terminology.Infrastructure.Tasks;
using Microsoft.Extensions.Configuration;
using Interneuron.Infrastructure.CustomExceptions;

namespace Interneuron.Terminology.API.AppCode.Filters
{
    public class GlobalFilter : IAsyncActionFilter, IAsyncResultFilter, IAsyncExceptionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
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

    }
}
