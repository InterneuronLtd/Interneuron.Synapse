using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;

namespace Interneuron.Infrastructure.Web.Exceptions.Handlers
{

    public static class InterneuronExceptionHandlerExtension
    {
        public static IApplicationBuilder UseInterneuronExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env)
        {
            var options = new IntrneuronExceptionHandlerOptions();
            return app.UseExceptionHandler((excpnOptions) => HandleException(app, options));
        }
        public static IApplicationBuilder UseInterneuronExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env, Action<IntrneuronExceptionHandlerOptions> configureOptions)
        {
            var options = new IntrneuronExceptionHandlerOptions();
            configureOptions(options);// it is set now
            return app.UseExceptionHandler((appbuilder) =>
            {
                HandleException(appbuilder, options);
            });
        }

        private static void HandleException(IApplicationBuilder app, IntrneuronExceptionHandlerOptions options)
        {
            app.Run(async context => {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                
                if (errorFeature == null) return;

                var exception = errorFeature.Error;

                if (exception == null) return;

                IExceptionHandler exceptionHandler = InterneuronExceptionHandlerFactory.GetExceptionHandler(exception);

                ////To be refactored to DI later
                //if (exception is PostgresException pgException)
                //{
                //    var dbExcpn = new SynapseDBException(npgEx: pgException);
                //    exceptionHandler = new SynapseDBExceptionHandler(dbExcpn);
                //}
                //else if (exception is SynapseDBException dBException)
                //{
                //    exceptionHandler = new SynapseDBExceptionHandler(dBException);
                //}
                //else if (exception is SynapseAPIBusinessException apiException)
                //{
                //    exceptionHandler = new SynapseAPIExceptionHandler(apiException);
                //}
                //else if (exception is Exception ex)
                //{
                //    exceptionHandler = new SynapseGenericExceptionHandler(ex);
                //}

                await Task.Run(() => exceptionHandler.HandleExceptionAsync(context, options));
            });
        }
    }
}
