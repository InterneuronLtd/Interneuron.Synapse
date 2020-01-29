using System.Threading.Tasks;

namespace Interneuron.Infrastructure.Exceptions.Handlers
{
    public interface IExceptionHandler
    {
        void HandleExceptionAsync(IntrneuronExceptionHandlerOptions options);
    }
}
