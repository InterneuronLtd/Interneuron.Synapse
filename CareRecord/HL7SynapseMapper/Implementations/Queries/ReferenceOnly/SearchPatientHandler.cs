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


//using Hl7.Fhir.Model;
//using Hl7.Fhir.Rest;
//using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
//using Interneuron.CareRecord.HL7SynapseHandler.Service.Search;
//using Interneuron.CareRecord.HL7SynapseService.Implementations.ModelExtensions;
//using Interneuron.CareRecord.HL7SynapseService.Interfaces;
//using Interneuron.CareRecord.HL7SynapseService.Models;
//using Interneuron.CareRecord.Infrastructure.Domain;
//using Interneuron.CareRecord.Infrastructure.Search;
//using Interneuron.CareRecord.Model.DomainModels;
//using Interneuron.Common.Extensions;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using static Hl7.Fhir.Model.Patient;

//namespace Interneuron.CareRecord.HL7SynapseService.Implementations
//{
//    public class SearchPatientHandler : IResourceSearchHandler
//    {
//        private IServiceProvider _provider;
//        private IReadOnlyRepository<entitystorematerialised_CorePerson> _matCorePersonRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> _matCorePersonIdentifierRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePersonaddress1> _matCorePersonAddress;
//        private IReadOnlyRepository<entitystorematerialised_CorePersoncontactinfo1> _matCorePersonContact;
//        private IReadOnlyRepository<entitystorematerialised_CoreNextofkin1> _matCoreNextOfKin;
//        private Dictionary<string, Func<string, List<SynapseSearchTerm>>> _supportedSearchCriteria;
//        private string _defaultHospitalRefNo;

//        public SearchPatientHandler(IServiceProvider provider, IReadOnlyRepository<entitystorematerialised_CorePerson> matCorePersonRepo, IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> matCorePersonIdentifierRepo,
//            IReadOnlyRepository<entitystorematerialised_CorePersonaddress1> matCorePersonAddress, IReadOnlyRepository<entitystorematerialised_CorePersoncontactinfo1> matCorePersonContact, IReadOnlyRepository<entitystorematerialised_CoreNextofkin1> matCoreNextOfKin)
//        {
//            this._provider = provider;
//            this._matCorePersonRepo = matCorePersonRepo;
//            this._matCorePersonIdentifierRepo = matCorePersonIdentifierRepo;
//            this._matCorePersonAddress = matCorePersonAddress;
//            this._matCorePersonContact = matCorePersonContact;
//            this._matCoreNextOfKin = matCoreNextOfKin;

//            this._supportedSearchCriteria = new Dictionary<string, Func<string, List<SynapseSearchTerm>>>()
//            {
//                    { "Patient.Identifier", PatientIdSearchCriteria },
//                    { "Subject.Patient.Identifier", PatientIdSearchCriteria },
//                    { "_id", PersonIdSearchCriteria },
//                    { "_lastUpdated", (paramVal)=>
//                        {
//                            return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"personData.{nameof(entitystorematerialised_CorePerson.Createddate)}", "ge", paramVal.Substring(2,paramVal.Length-2), new DateTimeSearchExpressionProvider()) };
//                        }
//                    },
//                    { "Patient.DateOfBirth", PatientDOBCriteria
//                    },
//                    {"FamilyName", PatientSearchCriteria }
//            };

//            this._defaultHospitalRefNo = GetDefaultHospitalRefNo();
//        }

//        private string GetDefaultHospitalRefNo()
//        {
//            IConfiguration configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

//            IConfigurationSection careRecordConfig = configuration.GetSection("CareRecordConfig");

//            return careRecordConfig.GetValue<string>("HospitalNumberReference");
//        }

//        public SearchResultData Handle(FHIRParam fhirParam, SearchParams searchParams)
//        {
//            var resourcesData = new List<ResourceData>();

//            var searchResultData = new SearchResultData
//            {
//                ResourcesData = resourcesData
//            };

//            var searchResults = GetSearchResults(searchParams);

//            if (searchResults.IsNull()) return searchResultData;

//            var materializedCorePersons = searchResults.Item1;
//            var materializedCorePersonIdentifiers = searchResults.Item2;
//            var materializedCorePersonAddresses = searchResults.Item3;
//            var materializedCorePersonContacts = searchResults.Item4;
//            var materializedCoreNextOfKins = searchResults.Item5;

//            if (!materializedCorePersons.IsCollectionValid() || !materializedCorePersonIdentifiers.IsCollectionValid()) return searchResultData;

//            materializedCorePersons.Each((per =>
//            {
//                var resourceData = new ResourceData(fhirParam);

//                var person = CreatePatient(per, materializedCorePersonIdentifiers, materializedCorePersonAddresses, materializedCorePersonContacts, materializedCoreNextOfKins);

//                resourceData.Resource = person;

//                searchResultData.ResourcesData.Add(resourceData);

//            }));

//            return searchResultData;
//        }

//        private Patient CreatePatient(entitystorematerialised_CorePerson person, List<entitystorematerialised_CorePersonidentifier> personIdentifiers, List<entitystorematerialised_CorePersonaddress1> personAddresses, 
//            List<entitystorematerialised_CorePersoncontactinfo1> personContacts, List<entitystorematerialised_CoreNextofkin1> personNextOfKins)
//        {
//            var per = person.GetPatientForPerson();

//            AddIdentifier(person, per, personIdentifiers);

//            AddPatientAddress(person, per, personAddresses);

//            AddContact(person, per, personContacts);

//            AddKinContact(person, per, personNextOfKins);

//            return per;
//        }

//        private void AddIdentifier(entitystorematerialised_CorePerson person, Patient per, List<entitystorematerialised_CorePersonidentifier> personIdentifiers)
//        {
//            if (personIdentifiers.IsNull()) return;

//            per.Identifier = new List<Identifier>();

//            personIdentifiers.GetIdentifiersForPerson().Each(ident => per.Identifier.Add(ident));
//        }

//        private void AddPatientAddress(entitystorematerialised_CorePerson person, Patient patient, List<entitystorematerialised_CorePersonaddress1> personAddresses)
//        {
//            if (personAddresses.IsNull()) return;

//            patient.Address = new List<Address>();

//            personAddresses.GetAddressesForPerson().Each(addr => patient.Address.Add(addr));
//        }

//        private void AddContact(entitystorematerialised_CorePerson person, Patient patient, List<entitystorematerialised_CorePersoncontactinfo1> personContacts)
//        {
//            if (personContacts.IsNull()) return;

//            patient.Telecom = new List<ContactPoint>();

//            personContacts.GetContactInfosForPerson().Each(cnt => patient.Telecom.Add(cnt));
//        }
        
//        private void AddKinContact(entitystorematerialised_CorePerson person, Patient patient, List<entitystorematerialised_CoreNextofkin1> nextOfKins)
//        {
//            if (nextOfKins.IsNull()) return;

//            patient.Contact = new List<ContactComponent>();

//            nextOfKins.GetContactInfosForKin().Each(nk => patient.Contact.Add(nk));
//        }

//        private Tuple<List<entitystorematerialised_CorePerson>, List<entitystorematerialised_CorePersonidentifier>, List<entitystorematerialised_CorePersonaddress1>, List<entitystorematerialised_CorePersoncontactinfo1>,
//                    List<entitystorematerialised_CoreNextofkin1>> GetSearchResults(SearchParams searchParams)
//        {
//            var baseQuery = (from person in this._matCorePersonRepo.ItemsAsReadOnly
//                             join personId in this._matCorePersonIdentifierRepo.ItemsAsReadOnly on
//                             person.PersonId equals personId.PersonId
//                             join personAddress in this._matCorePersonAddress.ItemsAsReadOnly on
//                             person.PersonId equals personAddress.PersonId into pa
//                             from address in pa.DefaultIfEmpty()
//                             join personContact in this._matCorePersonContact.ItemsAsReadOnly on
//                             person.PersonId equals personContact.PersonId into pc
//                             from contact in pc.DefaultIfEmpty()
//                             join personNextOfKin in this._matCoreNextOfKin.ItemsAsReadOnly on
//                             person.PersonId equals personNextOfKin.PersonId into pn
//                             from nextOfKin in pn.DefaultIfEmpty()
//                             select new
//                             {
//                                 personData = person,
//                                 personIdData = personId,
//                                 personAddressData = address,
//                                 personContactData = contact,
//                                 personNextOfKinData = nextOfKin
//                             });

//            var searchOp = new HL7SynapseHandler.Service.Search.SearchOpProcessor();

//            var withSearchClause = searchOp.Apply(_supportedSearchCriteria, searchParams, baseQuery);

//            //var stringQuery = _matCorePersonIdentifierRepo.GetSql(withSearchClause);

//            var matPersons = withSearchClause
//                .OrderByDescending((entity) => entity.personData.Createdtimestamp)
//                .Select(entity => new
//                {
//                    person = entity.personData as entitystorematerialised_CorePerson,
//                    patientIdentifer = entity.personIdData as entitystorematerialised_CorePersonidentifier,
//                    personAddress = entity.personAddressData as entitystorematerialised_CorePersonaddress1,
//                    personContact = entity.personContactData as entitystorematerialised_CorePersoncontactinfo1,
//                    personNextOfKin = entity.personNextOfKinData as entitystorematerialised_CoreNextofkin1
//                })
//                .ToList();

//            if (matPersons.IsCollectionValid())
//            {
//                var persons = matPersons.Select(m => m.person).ToList();
//                var patientIdentifiers = matPersons.Select(m => m.patientIdentifer).ToList();
//                var personAddresses = matPersons.Select(m => m.personAddress).ToList();
//                var personContacts = matPersons.Select(m => m.personContact).ToList();
//                var personNextOfKins = matPersons.Select(m => m.personNextOfKin).ToList();

//                return new Tuple<List<entitystorematerialised_CorePerson>, List<entitystorematerialised_CorePersonidentifier>, List<entitystorematerialised_CorePersonaddress1>, List<entitystorematerialised_CorePersoncontactinfo1>,
//                    List<entitystorematerialised_CoreNextofkin1>>(persons, patientIdentifiers, personAddresses, personContacts, personNextOfKins);
//            }

//            return null;
//        }

//        private List<SynapseSearchTerm> PatientIdSearchCriteria(string paramVal)
//        {
//            var searchTerms = new List<SynapseSearchTerm>();

//            if (paramVal.Contains("|"))
//            {
//                var paramVals = paramVal.Split("|");

//                searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idtypecode)}", "eq", paramVals[0], new DefaultSearchExpressionProvider()));

//                searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idnumber)}", "eq", paramVals[1], new DefaultSearchExpressionProvider()));

//                return searchTerms;
//            }
//            searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idtypecode)}", "eq", this._defaultHospitalRefNo, new DefaultSearchExpressionProvider()));

//            searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idnumber)}", "eq", paramVal, new DefaultSearchExpressionProvider()));

//            return searchTerms;
//        }

//        private List<SynapseSearchTerm> PatientSearchCriteria(string paramVal)
//        {
//            var searchTerms = new List<SynapseSearchTerm>
//            {
//                new SynapseSearchTerm($"personData.{nameof(entitystorematerialised_CorePerson.Familyname)}", "eq", paramVal, new StringSearchExpressionProvider())
//            };

//            return searchTerms;
//        }

//        private List<SynapseSearchTerm> PatientDOBCriteria(string paramVal)
//        {
//            return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"personData.{nameof(entitystorematerialised_CorePerson.Dateofbirth)}", "eq", paramVal, new DateTimeSearchExpressionProvider())
//            };
//        }

//        private List<SynapseSearchTerm> PersonIdSearchCriteria(string paramVal)
//        {
//            return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"personData.{nameof(entitystorematerialised_CorePerson.PersonId)}", "eq", paramVal, new StringSearchExpressionProvider())
//            };
//        }
//    }
//}


