using Interneuron.Infrastructure.CustomExceptions;

namespace Interneuron.Infrastructure.Exceptions.Handlers
{
    public class InterneuronAPIExceptionHandler : IExceptionHandler
    {
        private InterneuronBusinessException businessEx;

        public InterneuronAPIExceptionHandler(InterneuronBusinessException businessEx)
        {
            this.businessEx = businessEx;
        }
        public void HandleExceptionAsync(IntrneuronExceptionHandlerOptions options)
        {
            if (options != null)
                options.OnException?.Invoke(this.businessEx, businessEx.ErrorId, businessEx.ErrorResponseMessage);

            if (options != null)
                options.OnExceptionHandlingComplete?.Invoke(this.businessEx, businessEx.ErrorId);
            
        }
    }
}
