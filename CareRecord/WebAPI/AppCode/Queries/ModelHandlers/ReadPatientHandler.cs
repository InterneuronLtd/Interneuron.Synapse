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
﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Hl7.Fhir.Model;
//using Interneuron.CareRecord.API.AppCode.Core;
//using Interneuron.CareRecord.API.AppCode.Extensions.ModelExtensions;
//using Interneuron.CareRecord.Model.DomainModels;
//using Interneuron.CareRecord.Service.Interfaces;
//using Interneuron.Common.Extensions;
//using static Hl7.Fhir.Model.Patient;

//namespace Interneuron.CareRecord.API.AppCode.Queries.ModelHandlers
//{
//    public class ReadPatientHandler : IResourceSynapseModelHandler
//    {
//        private IServiceProvider _provider;

//        public ReadPatientHandler(IServiceProvider provider)
//        {
//            this._provider = provider;
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
//            ISynapseResourceService<entitystore_CorePerson1> storePersonSvc = this._provider.GetService(typeof(ISynapseResourceService<entitystore_CorePerson1>)) as ISynapseResourceService<entitystore_CorePerson1>;

//            return storePersonSvc.GetQuerable().Where(p => p.PersonId == fhirParam.ResourceId).OrderByDescending(x => x.Sequenceid).FirstOrDefault();
//        }

//        private entitystorematerialised_CorePerson GetMaterializedPerson(IFHIRParam fhirParam)
//        {
//            var corePersonService = this._provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePerson>)) as ISynapseResourceService<entitystorematerialised_CorePerson>;

//            return fhirParam.ResourceId.IsNotEmpty() ? corePersonService.GetQuerable()
//                 .Where(cp => cp.PersonId == fhirParam.ResourceId)
//                 .OrderByDescending(cp => cp.Sequenceid)
//                 .FirstOrDefault() : corePersonService.GetQuerable().FirstOrDefault();
//        }

//        private Patient CreatePatientFromPerson(entitystorematerialised_CorePerson person)
//        {
//            //var corePersonService = this._provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePerson>)) as ISynapseResourceService<entitystorematerialised_CorePerson>;

//            //var person = fhirParam.ResourceId.IsNotEmpty() ? corePersonService.GetQuerable()
//            //     .Where(cp => cp.PersonId == fhirParam.ResourceId)
//            //     .OrderByDescending(cp => cp.Sequenceid)
//            //     .FirstOrDefault() : corePersonService.GetQuerable().FirstOrDefault();

//            // if (person == null || person.PersonId.IsEmpty()) return null;

//            var patient = person.GetPatientForPerson();

//            AddIdentifiers(person, patient);

//            AddPatientAddress(person, patient);

//            AddContact(person, patient);

//            AddKinContact(person, patient);

//            return patient;
//        }

//        private void AddIdentifiers(entitystorematerialised_CorePerson person, Patient patient)
//        {
//            ISynapseResourceService<entitystorematerialised_CorePersonidentifier> personIdentifier = this._provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersonidentifier>)) as ISynapseResourceService<entitystorematerialised_CorePersonidentifier>;

//            List<entitystorematerialised_CorePersonidentifier> personIdentifiers = personIdentifier.GetQuerable().Where(x => x.PersonId == person.PersonId).ToList();

//            patient.Identifier = new List<Identifier>();

//            personIdentifiers.GetIdentifiersForPerson().Each(ident => patient.Identifier.Add(ident));
//        }

//        private void AddPatientAddress(entitystorematerialised_CorePerson person, Patient patient)
//        {
//            ISynapseResourceService<entitystorematerialised_CorePersonaddress1> patientAddressSvc = this._provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersonaddress1>)) as ISynapseResourceService<entitystorematerialised_CorePersonaddress1>;

//            List<entitystorematerialised_CorePersonaddress1> personAddresses = patientAddressSvc.GetQuerable().Where(x => x.PersonId == person.PersonId).ToList();

//            patient.Address = new List<Address>();

//            personAddresses.GetAddressesForPerson().Each(addr => patient.Address.Add(addr));
//        }

//        private void AddContact(entitystorematerialised_CorePerson person, Patient patient)
//        {
//            ISynapseResourceService<entitystorematerialised_CorePersoncontactinfo1> contactSvc = this._provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersoncontactinfo1>)) as ISynapseResourceService<entitystorematerialised_CorePersoncontactinfo1>;

//            List<entitystorematerialised_CorePersoncontactinfo1> personContacts = contactSvc.GetQuerable().Where(x => x.PersonId == person.PersonId).ToList();

//            patient.Telecom = new List<ContactPoint>();

//            personContacts.GetContactInfosForPerson().Each(cnt => patient.Telecom.Add(cnt));
//        }

//        private void AddKinContact(entitystorematerialised_CorePerson person, Patient patient)
//        {
//            ISynapseResourceService<entitystorematerialised_CoreNextofkin1> kinContextSvc = this._provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CoreNextofkin1>)) as ISynapseResourceService<entitystorematerialised_CoreNextofkin1>;

//            List<entitystorematerialised_CoreNextofkin1> nextOfKins = kinContextSvc.GetQuerable().Where(x => x.PersonId == person.PersonId).ToList();

//            patient.Contact = new List<ContactComponent>();

//            nextOfKins.GetContactInfosForKin().Each(nk => patient.Contact.Add(nk));
//        }
//    }
//}
