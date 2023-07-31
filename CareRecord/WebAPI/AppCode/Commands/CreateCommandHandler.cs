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
﻿using Hl7.Fhir.Model;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.CareRecord.API.AppCode.Extensions;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
using Interneuron.CareRecord.HL7SynapseService.Interfaces;
using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.API.AppCode.Commands
{
    public class CreateCommandHandler : ICommandHandler
    {
        private IServiceProvider _provider;

        public CreateCommandHandler(IServiceProvider provider)
        {
            this._provider = provider;
        }

        public FhirResponse Handle(IKey key, Resource resource)
        {
            throw new NotImplementedException();
        }

        public FhirResponse Handle(IKey key, Resource resource, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public FhirResponse Handle(IKey key, Resource resource, IEnumerable<object> parameters = null)
        {
            throw new NotImplementedException();
        }

       
        public SynapseResource Handle(IKey key, SynapseResource resource) // Just for the observation, temporary implementation -- can be removed later
        {
            var hl7Type = key.TypeName.GetHl7ModelType();

            var modelFactory = this._provider.GetService(typeof(ResourceCommandHandlerFactory)) as ResourceCommandHandlerFactory;

            IResourceCommandHandler commandHandler = modelFactory.GetHandler(hl7Type);

            var fhirParam = FHIRParam.Create(key.TypeName, key.ResourceId, key.VersionId);

            var synapseResource = commandHandler.Handle(resource);

            if (synapseResource == null)
            {
                throw new InterneuronBusinessException(StatusCodes.Status500InternalServerError);
            }

            return synapseResource;
        }

    }
}
