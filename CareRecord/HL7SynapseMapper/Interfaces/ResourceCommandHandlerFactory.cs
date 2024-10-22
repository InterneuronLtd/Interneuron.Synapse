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
using Hl7.Fhir.Model;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Implementations;
using System;

namespace Interneuron.CareRecord.HL7SynapseService.Interfaces
{
    public class ResourceCommandHandlerFactory
    {
        private IServiceProvider _provider;

        public ResourceCommandHandlerFactory(IServiceProvider provider)
        {
            this._provider = provider;
        }
        public IResourceCommandHandler GetHandler(Type fhirResourceType)
        {
            if (fhirResourceType == typeof(Observation))
            {
                var createHandler = this._provider.GetService(typeof(CreateObservationHandler)) as CreateObservationHandler;

                return createHandler; // new CreateObservationHandler(this._provider);
            }
            return null;
        }
    }
}
