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


﻿using Hl7.Fhir.Model;
using Interneuron.CareRecord.API.AppCode;
using Interneuron.CareRecord.API.AppCode.Commands;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
using Interneuron.CareRecord.Infrastructure.Domain;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.Common.Extensions;
using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Route("fhirplayground/v{version:apiVersion}")]
    public class FhirPlaygroundController : ControllerBase
    {
        private ILogger<FhirGenericController> _logger;
        private IServiceProvider _provider;

        public FhirPlaygroundController(ILogger<FhirGenericController> logger, IServiceProvider provider)
        {
            this._logger = logger;
            this._provider = provider;
        }


        [HttpGet, Route("Observation/{personId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SynapseResource>>> GetObservation(string personId)
        {
            ConditionalHeaderParameters parameters = new ConditionalHeaderParameters(Request);

            var queryHandlerFactory = this._provider.GetService(typeof(SynapseDataQueryHandlerFactory)) as SynapseDataQueryHandlerFactory;

            var handler = queryHandlerFactory.GetHandler("custom_observation_read");

            var handlerTask = new TaskFactory().StartNew(() =>
            {
                var synResourceData = handler.Handle(personId);
                return synResourceData.SynapseResources;

                //var obsRoot = new List<ObservationRoot>() {
                //    new ObservationRoot {
                //        ObservationEvent = new ObservationEventDTO { Encounter_Id = "121212121" },
                //        Observations = new List<ObservationDTO> {new ObservationDTO { Observationevent_Id="1112121213"} }
                //    },
                //     new ObservationRoot {
                //        ObservationEvent = new ObservationEventDTO { Encounter_Id = "333333" },
                //        Observations = new List<ObservationDTO> {new ObservationDTO { Observationevent_Id="3333333344"} }
                //    }

                //};
                //var sn = new List<SynapseResource>();
                //obsRoot.Each(o => sn.Add(o));
                //return sn;
                
                //return new ObservationRoot
                //{
                //    ObservationEvent = new ObservationEventDTO { Encounter_Id = "121212121" },
                //    Observations = new List<ObservationDTO> { new ObservationDTO { Observationevent_Id = "1112121213" } }
                //};
            });

            return await handlerTask;
        }

        [HttpGet, Route("{type}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<FhirResponse> ReadAsync(string type, string id)
        {
            ConditionalHeaderParameters parameters = new ConditionalHeaderParameters(Request);

            var fhirParams = Key.Create(type, id);

            var queryHandlerFactory = this._provider.GetService(typeof(QueryHandlerFactory)) as QueryHandlerFactory;

            var handler = queryHandlerFactory.GetHandler("Read");

            return handler.Handle(fhirParams);

            //var handlerTask = new TaskFactory().StartNew(() => handler.Handle(fhirParams));

            //return await handlerTask;
        }

        [HttpGet, Route("{type}/{id}"), MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<FhirResponse> ReadAsyncV2(string type, string id)
        {
            ConditionalHeaderParameters parameters = new ConditionalHeaderParameters(Request);

            var fhirParams = Key.Create(type, id);

            var queryHandlerFactory = this._provider.GetService(typeof(QueryHandlerFactory)) as QueryHandlerFactory;

            var handler = queryHandlerFactory.GetHandler("Read");

            return handler.Handle(fhirParams);

            //var handlerTask = new TaskFactory().StartNew(() => handler.Handle(fhirParams));

            //return await handlerTask;
        }

        //This is just a temporary implementation
        [HttpPost]
        [Route("observation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ObservationRoot>> CreateObservation([FromBody]ObservationRoot observation)
        {
            //if (observation.IsNull() || observation.ObservationEvent.IsNull())
            //{
            //    throw new InterneuronBusinessException(StatusCodes.Status400BadRequest, "Observation should have event.");
            //}

            var commandHandlerFactory = this._provider.GetService(typeof(CommandHandlerFactory)) as CommandHandlerFactory;

            var handler = commandHandlerFactory.GetHandler(nameof(CreateObservation));

            var key = Key.Create("observation");
            var handlerTask = new TaskFactory().StartNew(() =>
            {
                return (ObservationRoot)handler.Handle(key, observation);
            });

            return await handlerTask;
        }

    }

}