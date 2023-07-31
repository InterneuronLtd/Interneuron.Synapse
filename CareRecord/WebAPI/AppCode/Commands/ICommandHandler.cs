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
using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
using System.Collections.Generic;

namespace Interneuron.CareRecord.API.AppCode.Commands
{
    public interface ICommandHandler
    {
        FhirResponse Handle(IKey key, Resource resource);

        FhirResponse Handle(IKey key, Resource resource, params object[] parameters);

        FhirResponse Handle(IKey key, Resource resource, IEnumerable<object> parameters = null);

        SynapseResource Handle(IKey key, SynapseResource resource); // Just for the observation
    }
}
