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


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AutoMapper;
//using Hl7.Fhir.Model;
//using Interneuron.CareRecord.HL7SynapseService.Interfaces;
//using Interneuron.CareRecord.HL7SynapseService.Models;
//using Interneuron.CareRecord.Infrastructure.Domain;
//using Interneuron.CareRecord.Model.DomainModels;
//using Interneuron.Common.Extensions;

//namespace Interneuron.CareRecord.HL7SynapseService.Implementations
//{
//    public class ReadPatientHandler : IResourceQueryHandler
//    {
//        private IServiceProvider _provider;
//        private IMapper _mapper;
//        private IReadOnlyRepository<entitystore_CorePerson1> _storeCorePersonRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePerson> _matCorePersonRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> _matCorePersonIdenRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePersonaddress1> _matCorePersonAddrRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePersoncontactinfo1> _matCorePersonContactInfoRepo;
//        private IReadOnlyRepository<entitystorematerialised_CoreNextofkin1> _matCoreNextToKinRepo;

//        public ReadPatientHandler(IServiceProvider provider, IMapper mapper, IReadOnlyRepository<entitystore_CorePerson1> storeCorePersonRepo, IReadOnlyRepository<entitystorematerialised_CorePerson> matCorePersonRepo, IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> materialisedCorePersonIdenRepo, IReadOnlyRepository<entitystorematerialised_CorePersonaddress1> matCorePersonAddrRepo, IReadOnlyRepository<entitystorematerialised_CorePersoncontactinfo1> matCorePersonContactInfoRepo, IReadOnlyRepository<entitystorematerialised_CoreNextofkin1> matCoreNextToKinRepo)
//        {
//            this._provider = provider;
//            this._mapper = mapper;
//            this._storeCorePersonRepo = storeCorePersonRepo;
//            this._matCorePersonRepo = matCorePersonRepo;
//            this._matCorePersonIdenRepo = materialisedCorePersonIdenRepo;
//            this._matCorePersonAddrRepo = matCorePersonAddrRepo;
//            this._matCorePersonContactInfoRepo = matCorePersonContactInfoRepo;
//            this._matCoreNextToKinRepo = matCoreNextToKinRepo;
//        }

//        public ResourceData Handle(IFHIRParam fhirParam)
//        {
//            //var hl7Type = fhirParam.TypeName.GetHl7ModelType();
//            //Startup.AutofacContainer.Resolve<ISynapseResourceService<CorePerson>>();

//            var resourceData = new ResourceData(fhirParam);

//            var materializedCorePerson = GetMaterializedPerson(fhirParam);

//            if (materializedCorePerson == null || materializedCorePerson.PersonId.IsEmpty())
//            {
//                var storeCorePerson = CheckInEntityStore(fhirParam);

//                if (storeCorePerson == null || storeCorePerson.PersonId == null) return resourceData;

//                if (storeCorePerson.Recordstatus == 2) // Patient in Deleted State
//                {
//                    resourceData.Resource = null;
//                    resourceData.DeletedDate = storeCorePerson.Createddate;
//                    resourceData.IsDeleted = true;

//                    return resourceData;
//                }

//                return resourceData;
//            }
//            fhirParam.ResourceId = materializedCorePerson.PersonId;

//            var patient = CreatePatientFromPerson(materializedCorePerson);

//            resourceData.Resource = patient;

//            return resourceData;
//        }

//        private entitystore_CorePerson1 CheckInEntityStore(IFHIRParam fhirParam)
//        {
//            return _storeCorePersonRepo.ItemsAsReadOnly.Where(p => p.PersonId == fhirParam.ResourceId).OrderByDescending(x => x.Sequenceid).FirstOrDefault();
//        }

//        private entitystorematerialised_CorePerson GetMaterializedPerson(IFHIRParam fhirParam)
//        {
//            return fhirParam.ResourceId.IsNotEmpty() ? _matCorePersonRepo.ItemsAsReadOnly
//                 .Where(cp => cp.PersonId == fhirParam.ResourceId)
//                 .OrderByDescending(cp => cp.Sequenceid)
//                 .FirstOrDefault() : _matCorePersonRepo.ItemsAsReadOnly.FirstOrDefault();
//        }

//        private Patient CreatePatientFromPerson(entitystorematerialised_CorePerson person)
//        {
//            var patient = this._mapper.Map<Patient>(person);// person.GetPatientForPerson();

//            AddIdentifiers(person, patient);

//            AddPatientAddress(person, patient);

//            AddContact(person, patient);

//            AddKinContact(person, patient);

//            return patient;
//        }

//        private void AddIdentifiers(entitystorematerialised_CorePerson person, Patient patient)
//        {
//            List<entitystorematerialised_CorePersonidentifier> personIdentifiers = _matCorePersonIdenRepo.ItemsAsReadOnly.Where(x => x.PersonId == person.PersonId).ToList();

//            //patient.Identifier = new List<Identifier>();

//            //personIdentifiers.GetIdentifiersForPerson().Each(ident => patient.Identifier.Add(ident));

//            personIdentifiers.Each(ident => this._mapper.Map(ident, patient, typeof(entitystorematerialised_CorePersonidentifier), typeof(Patient)));
//        }

//        private void AddPatientAddress(entitystorematerialised_CorePerson person, Patient patient)
//        {
//            List<entitystorematerialised_CorePersonaddress1> personAddresses = _matCorePersonAddrRepo.ItemsAsReadOnly.Where(x => x.PersonId == person.PersonId).ToList();

//            //patient.Address = new List<Address>();

//            //personAddresses.GetAddressesForPerson().Each(addr => patient.Address.Add(addr));

//            personAddresses.Each(addr => this._mapper.Map(addr, patient, typeof(entitystorematerialised_CorePersonaddress1), typeof(Patient)));
//        }


//        private void AddContact(entitystorematerialised_CorePerson person, Patient patient)
//        {
//            List<entitystorematerialised_CorePersoncontactinfo1> personContacts = _matCorePersonContactInfoRepo.ItemsAsReadOnly.Where(x => x.PersonId == person.PersonId).ToList();

//            //patient.Telecom = new List<ContactPoint>();

//            //personContacts.GetContactInfosForPerson().Each(cnt => patient.Telecom.Add(cnt));

//            personContacts.Each(cnt => this._mapper.Map(cnt, patient, typeof(entitystorematerialised_CorePersoncontactinfo1), typeof(Patient)));
//        }


//        private void AddKinContact(entitystorematerialised_CorePerson person, Patient patient)
//        {
//            List<entitystorematerialised_CoreNextofkin1> nextOfKins = _matCoreNextToKinRepo.ItemsAsReadOnly.Where(x => x.PersonId == person.PersonId).ToList();

//            //patient.Contact = new List<ContactComponent>();

//            //nextOfKins.GetContactInfosForKin().Each(nk => patient.Contact.Add(nk));

//            nextOfKins.Each(nk => this._mapper.Map(nk, patient, typeof(entitystorematerialised_CoreNextofkin1), typeof(Patient)));
//        }
//    }
//}
