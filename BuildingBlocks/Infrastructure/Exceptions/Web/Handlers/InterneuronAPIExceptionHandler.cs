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


using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Interneuron.Infrastructure.Web.Exceptions.Handlers
{
    public class InterneuronAPIExceptionHandler : IExceptionHandler
    {
        private InterneuronBusinessException businessEx;

        public InterneuronAPIExceptionHandler(InterneuronBusinessException businessEx)
        {
            this.businessEx = businessEx;
        }
        public Task HandleExceptionAsync(HttpContext context, IntrneuronExceptionHandlerOptions options)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = businessEx.ErrorCode;
            var errorData = ExceptionError.GetExceptionErrorAsJSON(businessEx.ErrorId, businessEx.ErrorResponseMessage);

            var responseTask = context.Response.WriteAsync(errorData);

            if (options != null)
                options.OnExceptionHandlingComplete?.Invoke(this.businessEx, businessEx.ErrorId);
            
            return responseTask;
        }
    }
}
