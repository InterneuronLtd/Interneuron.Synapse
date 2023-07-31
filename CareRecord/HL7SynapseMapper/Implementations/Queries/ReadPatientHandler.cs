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


using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Interneuron.CareRecord.HL7SynapseService.Interfaces;
using Interneuron.CareRecord.HL7SynapseService.Models;
using Interneuron.CareRecord.Infrastructure.Domain;
using Interneuron.CareRecord.Infrastructure.Search;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.CareRecord.Repository;
using Interneuron.Common.Extensions;
using Microsoft.Extensions.Configuration;

namespace Interneuron.CareRecord.HL7SynapseService.Implementations
{
    public class ReadPatientHandler : BaseReadQueryHandler
    {
        private const string SearchKeyIdentifier = "search_patient";

        private IServiceProvider _provider;
        private IReadOnlyRepository<entitystorematerialised_CorePerson> _storeCorePersonRepo;
        private IFHIRParam _fhirParam;

        public ReadPatientHandler(IServiceProvider provider, IMapper mapper, IReadOnlyRepository<entitystorematerialised_CorePerson> storeCorePersonRepo, IGenericSearchRepository genericSearchRepo) : base(provider, mapper, genericSearchRepo)
        {
            this._provider = provider;
            this._storeCorePersonRepo = storeCorePersonRepo;
        }

        public override ResourceData Handle(IFHIRParam fhirParam)
        {
            this._fhirParam = fhirParam;

            base.OnNoSearchResults = () => NoResultsHandler(fhirParam);
            
            return base.Handle(fhirParam);
        }

        private ResourceData NoResultsHandler(IFHIRParam fhirParam)
        {
            var resourceData = new ResourceData(fhirParam);

            var storeCorePerson = CheckInEntityStore(fhirParam);

            if (storeCorePerson == null || storeCorePerson.PersonId == null) return resourceData;

            if (storeCorePerson.Recordstatus == 2) // Patient in Deleted State
            {
                resourceData.Resource = null;
                resourceData.DeletedDate = storeCorePerson.Createddate;
                resourceData.IsDeleted = true;

                return resourceData;
            }

            return resourceData;
        }

        private entitystorematerialised_CorePerson CheckInEntityStore(IFHIRParam fhirParam)
        {
            return _storeCorePersonRepo.ItemsAsReadOnly.Where(p => p.PersonId == fhirParam.ResourceId).OrderByDescending(x => x.Sequenceid).FirstOrDefault();
        }

        public override string GetSarchEntityIdentifier()
        {
            return SearchKeyIdentifier;
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
    }
}
