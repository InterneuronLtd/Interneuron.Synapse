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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Interneuron.CareRecord.API.AppCode;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.CareRecord.API.AppCode.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Interneuron.CareRecord.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("fhir")]
    public class FhirGenericController : ControllerBase
    {
        private ILogger<FhirGenericController> _logger;
        private IServiceProvider _provider;
        private readonly IConfiguration _configuration;

        public FhirGenericController(ILogger<FhirGenericController> logger, IServiceProvider provider, IConfiguration configuration)
        {
            this._logger = logger;
            this._provider = provider;
            this._configuration = configuration;
        }

        [HttpGet, Route("{type}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FhirResponse>> Read(string type, string id)
        {
            ConditionalHeaderParameters parameters = new ConditionalHeaderParameters(Request);

            var fhirParams = Key.Create(type, id);

            var queryHandlerFactory = this._provider.GetService(typeof(QueryHandlerFactory)) as QueryHandlerFactory;

            var handler = queryHandlerFactory.GetHandler(nameof(Read));

            return await new TaskFactory().StartNew(()=> handler.Handle(fhirParams));
         }

        [HttpGet, Route("{type}/{id}/{versionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<FhirResponse> VRead(string type, string id, string versionId)
        {
            ConditionalHeaderParameters parameters = new ConditionalHeaderParameters(Request);

            var fhirParams = Key.Create(type, id, versionId);

            var queryHandlerFactory = this._provider.GetService(typeof(QueryHandlerFactory)) as QueryHandlerFactory;

            var handler = queryHandlerFactory.GetHandler(nameof(Read));

            return handler.Handle(fhirParams);
        }

        [HttpGet, Route("{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FhirResponse>> Search(string type)
        {
            var searchparams = Request.GetSearchParams();

            var searchHandlerFactory = this._provider.GetService(typeof(SearchHandlerFactory)) as SearchHandlerFactory;

            var handler = searchHandlerFactory.GetHandler(nameof(Search));

            var key = Key.Create(type);

            return await new TaskFactory().StartNew(() => handler.Handle(key, searchparams));
        }

        [HttpPost, Route("$GetStructuredRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FhirResponse>> GetStructuredRecord([FromBody] Parameters parameters)
        {
            var includeDemographicsOnlyParam = parameters.Parameter.FirstOrDefault(x => x.Name == "demographicsOnly");
            if (includeDemographicsOnlyParam == null)
            {
                return FhirResponse.WithError(HttpStatusCode.BadRequest, "Missing demographicsOnly parameter.");
            }

            if (includeDemographicsOnlyParam.Part == null)
            {
                return FhirResponse.WithError(HttpStatusCode.BadRequest, "Missing Part in demographicsOnly parameter.");
            }

            var includeDemographicsOnlyPart = includeDemographicsOnlyParam.Part.FirstOrDefault(x => x.Name == "includeDemographicsOnly");

            if (includeDemographicsOnlyPart == null)
            {
                return FhirResponse.WithError(HttpStatusCode.BadRequest, "Missing includeDemographicsOnly Part in demographicsOnly parameter.");
            }

            var nhsNumberParameter = parameters.Parameter.FirstOrDefault(x => x.Name == "patientNHSNumber");

            var nhsNumber = nhsNumberParameter.Value as Identifier;


            if (includeDemographicsOnlyPart.Value.Matches(new FhirBoolean(true)))
            {
                var patientResource = Read("Patient", nhsNumber.Value).Result.Value;

                Bundle bundle = new Bundle();
                bundle.Entry = new List<Bundle.EntryComponent>();

                Bundle.EntryComponent bundleEntry = new Bundle.EntryComponent();
                bundleEntry.Resource = patientResource.Resource;
                bundle.Type = Bundle.BundleType.Collection;
                bundle.Entry.Add(bundleEntry);

                return new FhirResponse(System.Net.HttpStatusCode.OK, bundle);
            }
            else
            {
                List<IKey> fhirParams = new List<IKey>();

                foreach (var type in _configuration.GetSection("CareRecordConfig").GetSection("SupportedHL7Types").Get<string[]>())
                {
                    fhirParams.Add(Key.Create(type, nhsNumber.Value));
                }

                var queryHandlerFactory = this._provider.GetService(typeof(QueryHandlerFactory)) as QueryHandlerFactory;

                var handler = queryHandlerFactory.GetHandler(nameof(Read));

                return await new TaskFactory().StartNew(() => handler.Handle(fhirParams));
            }
        }
    }
}