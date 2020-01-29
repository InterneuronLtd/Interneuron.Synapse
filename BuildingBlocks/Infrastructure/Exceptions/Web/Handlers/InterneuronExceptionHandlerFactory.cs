using Interneuron.Infrastructure.CustomExceptions;
using Npgsql;
using System;

namespace Interneuron.Infrastructure.Web.Exceptions.Handlers
{
    public class InterneuronExceptionHandlerFactory
    {
        public static IExceptionHandler GetExceptionHandler(Exception exception)
        {
            if (exception is PostgresException pgException)
            {
                var dbExcpn = new InterneuronDBException(npgEx: pgException);
                return new InterneuronDBExceptionHandler(dbExcpn);
            }
            else if (exception is InterneuronDBException dBException)
            {
                return new InterneuronDBExceptionHandler(dBException);
            }
            else if (exception is InterneuronBusinessException apiException)
            {
                 return new InterneuronAPIExceptionHandler(apiException);
            }
            else if (exception is Exception ex)
            {
                return new InterneuronGenericExceptionHandler(ex);
            }
            return new InterneuronGenericExceptionHandler(exception);
        }
    }
}
