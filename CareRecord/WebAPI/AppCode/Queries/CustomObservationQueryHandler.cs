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
﻿using System;
using Interneuron.CareRecord.HL7SynapseService.Interfaces;
using Interneuron.CareRecord.HL7SynapseService.Models;
using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Http;

namespace Interneuron.CareRecord.API.AppCode
{
    public class CustomObservationQueryHandler : ISynapseDataQueryHandler
    {
        private IServiceProvider _provider;

        public CustomObservationQueryHandler(IServiceProvider provider)
        {
            this._provider = provider;
        }
        public SynapseResourceData Handle(string personId) // Just for the observation, temporary implementation -- can be removed later
        {
            var modelFactory = this._provider.GetService(typeof(SynapseQueryHandlerFactory)) as SynapseQueryHandlerFactory;

            ISynapseQueryHandler synapseModelHandler = modelFactory.GetHandler("custom_observation");

            var synapseResource = synapseModelHandler.Handle(personId);

            if (synapseResource == null)
            {
                throw new InterneuronBusinessException(StatusCodes.Status500InternalServerError);
            }

            return synapseResource;
        }

    }
}

