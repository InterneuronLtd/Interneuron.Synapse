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


using AutoMapper;
using Hl7.Fhir.Model;
using Interneuron.CareRecord.HL7SynapseService.Interfaces;
using Interneuron.CareRecord.HL7SynapseService.Models;
using Interneuron.CareRecord.Infrastructure.Domain;
using Interneuron.CareRecord.Infrastructure.Search;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.CareRecord.Repository;
using Interneuron.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.CareRecord.HL7SynapseService.Implementations
{
    public class ReadEncounterHandler : BaseReadQueryHandler
    {
        private const string SearchKeyIdentifier = "search_encounter";

        private IServiceProvider _provider;
        private IReadOnlyRepository<entitystorematerialised_CoreEncounter> _coreEncounterRepo;
        private IFHIRParam _fhirParam;

        public ReadEncounterHandler(IServiceProvider provider, IMapper mapper, IGenericSearchRepository genericSearchRepo, IReadOnlyRepository<entitystorematerialised_CoreEncounter> coreEncounterRepo) : base(provider, mapper, genericSearchRepo)
        {
            this._provider = provider;
            this._coreEncounterRepo = coreEncounterRepo;
        }

        public override ResourceData Handle(IFHIRParam fhirParam)
        {
            this._fhirParam = fhirParam;

            base.OnNoSearchResults = ()=> NoResultsHandler(fhirParam);

            return base.Handle(fhirParam);
        }

        private ResourceData NoResultsHandler(IFHIRParam fhirParam)
        {
            var resourceData = new ResourceData(fhirParam);

            var storeCoreEncounter = CheckInEntityStore(fhirParam);

            if (storeCoreEncounter == null || storeCoreEncounter.EncounterId == null) return resourceData;

            if (storeCoreEncounter.Recordstatus == 2) // Encounter in Deleted State
            {
                resourceData.Resource = null;
                resourceData.DeletedDate = storeCoreEncounter.Createddate;
                resourceData.IsDeleted = true;

                return resourceData;
            }

            return resourceData;
        }

        private entitystorematerialised_CoreEncounter CheckInEntityStore(IFHIRParam fhirParam)
        {
            return _coreEncounterRepo.ItemsAsReadOnly.Where(e => e.EncounterId == fhirParam.ResourceId).OrderByDescending(x => x.Sequenceid).FirstOrDefault();

        }

        public override List<SynapseSearchTerm> GetSynapseSearchTerms()
        {
            var searchTerms = new List<SynapseSearchTerm>
            {
                new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idtypecode)}", DefaultSearchExpressionProvider.EqualsOperator, this._defaultHospitalRefNo, new DefaultSearchExpressionProvider()),

                new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idnumber)}", DefaultSearchExpressionProvider.EqualsOperator, _fhirParam.ResourceId, new DefaultSearchExpressionProvider())
            };

            return searchTerms;
        }

        public override string GetSarchEntityIdentifier()
        {
            return SearchKeyIdentifier;
        }
    }
}
