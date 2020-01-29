using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interneuron.Caching
{
    public static class CacheServiceExtension
    {
        public static IConfiguration Configuration;
        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
