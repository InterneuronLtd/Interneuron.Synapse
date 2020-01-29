using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Interneuron.Infrastructure.Web.Exceptions.Handlers
{
    public class InterneuronGenericExceptionHandler : IExceptionHandler
    {
        private Exception ex;

        public InterneuronGenericExceptionHandler(Exception ex)
        {
            this.ex = ex;
        }
        public Task HandleExceptionAsync(HttpContext context, IntrneuronExceptionHandlerOptions options)
        {
            var errorId = Guid.NewGuid().ToString();
            var errorResponseMessage = $"Error fetching data from database. Please check the error log with ID: {errorId} for more details";

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            
            var errorData = ExceptionError.GetExceptionErrorAsJSON(errorId, errorResponseMessage);

            var responseTask =  context.Response.WriteAsync(errorData);
            if (options != null)
                options.OnExceptionHandlingComplete?.Invoke(this.ex, errorId);
            return responseTask;
        }
    }
}
