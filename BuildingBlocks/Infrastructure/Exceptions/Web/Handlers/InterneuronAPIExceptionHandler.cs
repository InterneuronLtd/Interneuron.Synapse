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
