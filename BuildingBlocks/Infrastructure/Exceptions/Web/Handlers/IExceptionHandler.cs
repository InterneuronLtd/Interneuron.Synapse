using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Interneuron.Infrastructure.Web.Exceptions.Handlers
{
    public interface IExceptionHandler
    {
        Task HandleExceptionAsync(HttpContext context, IntrneuronExceptionHandlerOptions options);
    }
}
