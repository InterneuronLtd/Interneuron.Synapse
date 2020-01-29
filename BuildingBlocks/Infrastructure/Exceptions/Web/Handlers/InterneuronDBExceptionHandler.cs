using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Interneuron.Infrastructure.Web.Exceptions.Handlers
{
    public class InterneuronDBExceptionHandler : IExceptionHandler
    {
        private InterneuronDBException dbEx;

        public InterneuronDBExceptionHandler(InterneuronDBException dbEx)
        {
            this.dbEx = dbEx;
        }
        public Task HandleExceptionAsync(HttpContext context, IntrneuronExceptionHandlerOptions options)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = this.dbEx.ErrorCode;
            var errorData = ExceptionError.GetExceptionErrorAsJSON(dbEx.ErrorId, dbEx.ErrorResponseMessage);
            var responseTask = context.Response.WriteAsync(errorData);
            if (options != null)
                options.OnExceptionHandlingComplete?.Invoke(this.dbEx, dbEx.ErrorId);
            return responseTask;
        }
    }
}
