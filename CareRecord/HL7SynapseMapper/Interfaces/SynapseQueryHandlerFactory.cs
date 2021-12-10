//Interneuron Synapse

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


using Interneuron.CareRecord.HL7SynapseService.Implementations;
using Interneuron.Common.Extensions;
using System;

namespace Interneuron.CareRecord.HL7SynapseService.Interfaces
{
    public class SynapseQueryHandlerFactory
    {
        private IServiceProvider _provider;

        public SynapseQueryHandlerFactory(IServiceProvider provider)
        {
            this._provider = provider;
        }
        public ISynapseQueryHandler GetHandler(string typeName)
        {
            if (typeName.EqualsIgnoreCase("custom_observation"))
            {
                var readHandler = this._provider.GetService(typeof(ReadObservationHandler)) as ReadObservationHandler;
                return readHandler;
            }
            return null;
        }
    }
}
