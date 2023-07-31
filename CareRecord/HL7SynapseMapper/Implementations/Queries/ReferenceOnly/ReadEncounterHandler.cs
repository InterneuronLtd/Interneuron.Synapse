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


//using AutoMapper;
//using Hl7.Fhir.Model;
//using Interneuron.CareRecord.HL7SynapseService.Interfaces;
//using Interneuron.CareRecord.HL7SynapseService.Models;
//using Interneuron.CareRecord.Infrastructure.Domain;
//using Interneuron.CareRecord.Infrastructure.Search;
//using Interneuron.CareRecord.Model.DomainModels;
//using Interneuron.CareRecord.Repository;
//using Interneuron.Common.Extensions;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Interneuron.CareRecord.HL7SynapseService.Implementations
//{
//    public class ReadEncounterHandler : IResourceQueryHandler
//    {
//        private IServiceProvider _provider;
//        private IReadOnlyRepository<entitystore_CoreEncounter1> _coreEncounterRepo;
//        private IReadOnlyRepository<entitystorematerialised_CoreEncounter> _materialisedCoreEncounterRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> _materialisedCorePersonIdentifierRepo;
//        private IMapper _mapper;
//        private string _defaultHospitalRefNo;

//        public ReadEncounterHandler(IServiceProvider provider, IMapper mapper, IReadOnlyRepository<entitystore_CoreEncounter1> coreEncounterRepo, IReadOnlyRepository<entitystorematerialised_CoreEncounter> materialisedCoreEncounterRepo, IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> materialisedCorePersonIdentifierRepo)
//        {
//            this._provider = provider;
//            this._coreEncounterRepo = coreEncounterRepo;
//            this._materialisedCoreEncounterRepo = materialisedCoreEncounterRepo;
//            this._materialisedCorePersonIdentifierRepo = materialisedCorePersonIdentifierRepo;
//            this._mapper = mapper;
//        }

//        public ResourceData Handle(IFHIRParam fhirParam)
//        {
//            var resourceData = new ResourceData(fhirParam);

//            var materializedCoreEncounter = GetMaterializedEncounter(fhirParam);

//            if (materializedCoreEncounter == null || materializedCoreEncounter.EncounterId.IsEmpty())
//            {
//                var storeCoreEncounter = CheckInEntityStore(fhirParam);

//                if (storeCoreEncounter == null || storeCoreEncounter.EncounterId == null) return resourceData;

//                if (storeCoreEncounter.Recordstatus == 2) // Encounter in Deleted State
//                {
//                    resourceData.Resource = null;
//                    resourceData.DeletedDate = storeCoreEncounter.Createddate;
//                    resourceData.IsDeleted = true;

//                    return resourceData;
//                }

//                return resourceData;
//            }

//            var encounter = CreateEncounter(materializedCoreEncounter);

//            resourceData.Resource = encounter;

//            return resourceData;
//        }

//        private Encounter CreateEncounter(entitystorematerialised_CoreEncounter encounter)
//        {
//            IConfiguration configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

//            IConfigurationSection careRecordConfig = configuration.GetSection("CareRecordConfig");

//            string hospitalNumberReference = careRecordConfig.GetValue<string>("HospitalNumberReference");

//            //var enc = encounter.GetEncounter();
//            var enc = this._mapper.Map<Encounter>(encounter);

//            AddIdentifier(encounter, enc, hospitalNumberReference);

//            return enc;
//        }

//        private entitystore_CoreEncounter1 CheckInEntityStore(IFHIRParam fhirParam)
//        {
//            return _coreEncounterRepo.ItemsAsReadOnly.Where(e => e.EncounterId == fhirParam.ResourceId).OrderByDescending(x => x.Sequenceid).FirstOrDefault();

//        }

//        private entitystorematerialised_CoreEncounter GetMaterializedEncounter(IFHIRParam fhirParam)
//        {
//            return fhirParam.ResourceId.IsNotEmpty() ? _materialisedCoreEncounterRepo.ItemsAsReadOnly
//                 .Where(ce => ce.EncounterId == fhirParam.ResourceId)
//                 .OrderByDescending(ce => ce.Sequenceid)
//                 .FirstOrDefault() : _materialisedCoreEncounterRepo.ItemsAsReadOnly.FirstOrDefault();
//        }

//        private void AddIdentifier(entitystorematerialised_CoreEncounter encounter, Encounter enc, string hospitalNumberReference)
//        {
//            entitystorematerialised_CorePersonidentifier personIdentifier = _materialisedCorePersonIdentifierRepo.ItemsAsReadOnly.Where(x => x.PersonId == encounter.PersonId && x.Idtypecode == hospitalNumberReference).FirstOrDefault();

//            if (personIdentifier.IsNull()) return;

//            this._mapper.Map(personIdentifier, enc, typeof(entitystorematerialised_CorePersonidentifier), typeof(Encounter));


//            //enc.Subject = new ResourceReference();
//            //enc.Subject.Identifier = new Identifier();

//            //enc.Subject.Reference = "Patient";
//            //enc.Subject.Identifier.Value = personIdentifier.Idnumber;
//            //enc.Subject.Identifier.System = hospitalNumberReference;
//        }
//    }
//}
