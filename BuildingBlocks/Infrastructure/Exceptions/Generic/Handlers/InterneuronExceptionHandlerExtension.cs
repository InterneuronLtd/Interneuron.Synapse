using System;
using System.Threading.Tasks;

namespace Interneuron.Infrastructure.Exceptions.Handlers
{

    public static class InterneuronExceptionHandlerExtension
    {
        public static void HandleException(Exception exception, IntrneuronExceptionHandlerOptions options)
        {
                if (exception == null) return;

                IExceptionHandler exceptionHandler = InterneuronExceptionHandlerFactory.GetExceptionHandler(exception);

                Task.Run(()=> exceptionHandler.HandleExceptionAsync(options));
        }
    }
}
