using Interneuron.Infrastructure.CustomExceptions;

namespace Interneuron.Infrastructure.Exceptions.Handlers
{
    public class InterneuronDBExceptionHandler : IExceptionHandler
    {
        private InterneuronDBException dbEx;

        public InterneuronDBExceptionHandler(InterneuronDBException dbEx)
        {
            this.dbEx = dbEx;
        }
        public void HandleExceptionAsync(IntrneuronExceptionHandlerOptions options)
        {
            if (options != null)
                options.OnException?.Invoke(this.dbEx, dbEx.ErrorId, dbEx.ErrorResponseMessage);

            if (options != null)
                options.OnExceptionHandlingComplete?.Invoke(this.dbEx, dbEx.ErrorId);
        }
    }
}
