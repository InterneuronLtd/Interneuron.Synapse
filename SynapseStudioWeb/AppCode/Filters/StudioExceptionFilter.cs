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
﻿using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using NToastNotify;
using Serilog;
using System;

namespace SynapseStudioWeb.AppCode.Filters
{
    public class StudioExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        const string GENERIC_MESSAGE = "Error performing the operation. Please check the error log with Id:";
        private string customMessage = null;
        private bool noToast = false;
        private bool showExceptionMsg;

        public StudioExceptionFilter(bool showExceptionMsg = true, string customMessage = null, bool noToast = false)
        {
            this.customMessage = customMessage;
            this.noToast = noToast;
            this.showExceptionMsg = showExceptionMsg;
        }
        
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var toast = context.HttpContext.RequestServices.GetService<IToastNotification>();
            var excpnMsgToConsider = this.showExceptionMsg ? context.Exception.Message : "";

            var errMsg = customMessage.IsEmpty() ? $"{GENERIC_MESSAGE} {Guid.NewGuid()}. {excpnMsgToConsider}" : $"{GENERIC_MESSAGE} {Guid.NewGuid()}. {excpnMsgToConsider} {customMessage}";

            if (!this.noToast)
                toast.AddErrorToastMessage(errMsg);

            Log.Logger.ForContext("ErrorId", Guid.NewGuid().ToString()).Error(context.Exception, context.Exception.Message);

            //context.HttpContext.Response.StatusCode = 500;
            //context.HttpContext.Response.WriteAsync(errMsg);

            context.ExceptionHandled = true;
        }
    }

}
