using System;

namespace Interneuron.Infrastructure.Exceptions.Handlers
{
    public class IntrneuronExceptionHandlerOptions
    {
        public Action<Exception, string, string> OnException { get; set; }

        public Action<Exception, string> OnExceptionHandlingComplete { get; set; }
    }
}
