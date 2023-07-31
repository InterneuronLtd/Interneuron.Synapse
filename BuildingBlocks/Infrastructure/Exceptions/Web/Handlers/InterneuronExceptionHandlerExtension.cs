 //Interneuron synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;

namespace Interneuron.Infrastructure.Web.Exceptions.Handlers
{

    public static class InterneuronExceptionHandlerExtension
    {
        public static IApplicationBuilder UseInterneuronExceptionHandler(this IApplicationBuilder app)
        {
            var options = new IntrneuronExceptionHandlerOptions();
            return app.UseExceptionHandler((excpnOptions) => HandleException(app, options));
        }
        public static IApplicationBuilder UseInterneuronExceptionHandler(this IApplicationBuilder app, Action<IntrneuronExceptionHandlerOptions> configureOptions = null)
        {
            var options = new IntrneuronExceptionHandlerOptions();
            configureOptions?.Invoke(options);// it is set now
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
