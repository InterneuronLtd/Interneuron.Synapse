using System;

namespace Interneuron.Infrastructure.Exceptions.Handlers
{
    public class InterneuronGenericExceptionHandler : IExceptionHandler
    {
        private Exception ex;

        public InterneuronGenericExceptionHandler(Exception ex)
        {
            this.ex = ex;
        }
        public void HandleExceptionAsync(IntrneuronExceptionHandlerOptions options)
        {
            var errorId = Guid.NewGuid().ToString();
            var errorResponseMessage = $"Error fetching data from database. Please check the error log with ID: {errorId} for more details";

          
            if (options != null)
                options.OnException?.Invoke(this.ex, errorId, errorResponseMessage);

            if (options != null)
                options.OnExceptionHandlingComplete?.Invoke(this.ex, errorId);
        }
    }
}
