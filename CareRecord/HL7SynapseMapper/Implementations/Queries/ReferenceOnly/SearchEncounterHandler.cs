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

//namespace Interneuron.CareRecord.HL7SynapseService.Implementations
//{
//    public class SearchEncounterHandler : IResourceSearchHandler
//    {
//        private IServiceProvider _provider;
//        private IReadOnlyRepository<entitystorematerialised_CorePerson> _matCorePersonRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> _matCorePersonIdentifierRepo;
//        private IReadOnlyRepository<entitystorematerialised_CoreEncounter> _matCoreEncounter;
//        private IMapper _mapper;
//        private Dictionary<string, Func<string, List<SynapseSearchTerm>>> _supportedSearchCriteria;
//        private string _defaultHospitalRefNo;

//        public SearchEncounterHandler(IServiceProvider provider, IMapper mapper, IReadOnlyRepository<entitystorematerialised_CorePerson> matCorePersonRepo, IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> matCorePersonIdentifierRepo,
//            IReadOnlyRepository<entitystorematerialised_CoreEncounter> matCoreEncounter)
//        {
//            this._provider = provider;
//            this._matCorePersonRepo = matCorePersonRepo;
//            this._matCorePersonIdentifierRepo = matCorePersonIdentifierRepo;
//            this._matCoreEncounter = matCoreEncounter;
//            this._mapper = mapper;

//            this._supportedSearchCriteria = new Dictionary<string, Func<string, List<SynapseSearchTerm>>>()
//            {
//                    { "Patient.Identifier", PatientIdSearchCriteria },
//                    { "Subject.Patient.Identifier", PatientIdSearchCriteria },
//                    { "_id", EncounterIdSearchCriteria },
//                    { "_lastUpdated", (paramVal)=>
//                        {
//                            return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"encounterData.{nameof(entitystorematerialised_CoreEncounter.Createddate)}", "ge", paramVal.Substring(2,paramVal.Length-2), new DateTimeSearchExpressionProvider()) };
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

//            var materializedCoreEncounters = searchResults.Item1;
//            var materializedCorePersonIdentifier = searchResults.Item2;

//            if (!materializedCoreEncounters.IsCollectionValid() || materializedCorePersonIdentifier.IsNull()) return searchResultData;

//            //var encounters = this._mapper.Map<List<Encounter>>(materializedCoreEncounters);

//            //encounters.Each(enc =>
//            //{
//            //    this._mapper.Map(materializedCorePersonIdentifier, enc, materializedCorePersonIdentifier.GetType(), typeof(Encounter));
//            //});

//            materializedCoreEncounters.Each((enc =>
//            {
//                var resourceData = new ResourceData(fhirParam);

//                var encounter = CreateEncounter(enc, materializedCorePersonIdentifier);

//                resourceData.Resource = encounter;

//                searchResultData.ResourcesData.Add(resourceData);

//            }));

//            return searchResultData;
//        }

//        public List createList(Type myType)
//        {
//            Type genericListType = typeof(List<>).MakeGenericType(myType);
//            return (List)Activator.CreateInstance(genericListType);
//        }
//        private Encounter CreateEncounter(entitystorematerialised_CoreEncounter encounter, entitystorematerialised_CorePersonidentifier personIdentifier)
//        {
//            var enc = encounter.GetEncounter();

//            AddIdentifier(encounter, enc, personIdentifier);

//            return enc;
//        }

//        private void AddIdentifier(entitystorematerialised_CoreEncounter encounter, Encounter enc, entitystorematerialised_CorePersonidentifier personIdentifier)
//        {
//            if (personIdentifier.IsNull()) return;

//            enc.Subject = new ResourceReference
//            {
//                Identifier = new Identifier(),

//                Reference = "Patient"
//            };

//            enc.Subject.Identifier.Value = personIdentifier.Idnumber;
//            enc.Subject.Identifier.System = personIdentifier.Idtypecode ?? _defaultHospitalRefNo;
//        }

//        private Tuple<List<entitystorematerialised_CoreEncounter>, entitystorematerialised_CorePersonidentifier> GetSearchResults(SearchParams searchParams)
//        {
//            var baseQuery = (from encounter in this._matCoreEncounter.ItemsAsReadOnly
//                             join person in this._matCorePersonRepo.ItemsAsReadOnly on
//                             encounter.PersonId equals person.PersonId
//                             join personId in this._matCorePersonIdentifierRepo.ItemsAsReadOnly on
//                             person.PersonId equals personId.PersonId
//                             select new
//                             {
//                                 encounterData = encounter,
//                                 personData = person,
//                                 personIdData = personId
//                             });

//            var searchOp = new HL7SynapseHandler.Service.Search.SearchOpProcessor();

//            //searchOp.GetSynapseSearchTerms(_supportedSearchCriteria, searchParams);

//            var withSearchClause = searchOp.Apply(_supportedSearchCriteria, searchParams, baseQuery);

//#if DEBUG 
//            var stringQuery = _matCorePersonIdentifierRepo.GetSql(withSearchClause);
//#endif
//            var matEncounters = withSearchClause
//                .OrderByDescending((entity) => entity.encounterData.Createdtimestamp)
//                .Select(entity => new
//                {
//                    encounter = entity.encounterData as entitystorematerialised_CoreEncounter,
//                    patientIdentifer = entity.personIdData as entitystorematerialised_CorePersonidentifier
//                })
//                .ToList();

//            if (matEncounters.IsCollectionValid())
//            {
//                var encounters = matEncounters.Select(m => m.encounter).ToList();
//                var patientIdentifier = matEncounters.Select(m => m.patientIdentifer).FirstOrDefault();

//                return new Tuple<List<entitystorematerialised_CoreEncounter>, entitystorematerialised_CorePersonidentifier>(encounters, patientIdentifier);
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

//        private List<SynapseSearchTerm> EncounterIdSearchCriteria(string paramVal)
//        {
//            return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"encounterData.{nameof(entitystorematerialised_CoreEncounter.EncounterId)}", "eq", paramVal, new StringSearchExpressionProvider())
//            };
//        }
//    }
//}


