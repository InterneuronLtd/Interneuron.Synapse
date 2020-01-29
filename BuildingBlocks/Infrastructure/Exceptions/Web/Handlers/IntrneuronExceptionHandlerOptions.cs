using System;

namespace Interneuron.Infrastructure.Web.Exceptions.Handlers
{
    public class IntrneuronExceptionHandlerOptions
    {
        public Action<Exception, string> OnExceptionHandlingComplete { get; set; }
    }
}
