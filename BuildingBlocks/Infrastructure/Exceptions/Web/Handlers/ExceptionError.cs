using Newtonsoft.Json;

namespace Interneuron.Infrastructure.Web.Exceptions.Handlers
{
    public class ExceptionError
    {
        public string ErrorId { get; set; }

        public string ErrorMessage { get; set; }

        public ExceptionError()
        {

        }

        public ExceptionError(string errorId, string errorMessage)
        {
            ErrorId = errorId;
            ErrorMessage = errorMessage;
        }

        public static string GetExceptionErrorAsJSON(string errorId, string errorMessage)
        {
            return JsonConvert.SerializeObject(new ExceptionError(errorId, errorMessage));
        }
    }
}
