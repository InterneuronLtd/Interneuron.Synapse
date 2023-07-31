//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

//Copyright(C) 2021  Interneuron CIC

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


using Interneuron.Infrastructure.CustomExceptions;
using Npgsql;
using System;

namespace Interneuron.Infrastructure.Exceptions.Handlers
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
