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


//using Hl7.Fhir.Model;
//using Hl7.Fhir.Rest;
//using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
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
//    public class SearchResultHandler : IResourceSearchHandler
//    {
//        private IServiceProvider _provider;
//        private IReadOnlyRepository<entitystorematerialised_CoreResult1> _matCoreResultRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> _matCorePersonIdentifierRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePerson> _matCorePersonRepo;
//        private Dictionary<string, Func<string, List<SynapseSearchTerm>>> _supportedSearchCriteria;
//        private string _defaultHospitalRefNo;

//        public SearchResultHandler(IServiceProvider provider, IReadOnlyRepository<entitystorematerialised_CorePerson> matCorePersonRepo, IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> matCorePersonIdentifierRepo,
//            IReadOnlyRepository<entitystorematerialised_CoreResult1> matCoreResultRepo)
//        {
//            this._provider = provider;
//            this._matCoreResultRepo = matCoreResultRepo;
//            this._matCorePersonIdentifierRepo = matCorePersonIdentifierRepo;
//            this._matCorePersonRepo = matCorePersonRepo;

//            this._supportedSearchCriteria = new Dictionary<string, Func<string, List<SynapseSearchTerm>>>()
//            {
//                    { "Patient.Identifier", PatientIdSearchCriteria },
//                    { "Subject.Patient.Identifier", PatientIdSearchCriteria },
//                    { "_id", ResultIdSearchCriteria },
//                    { "_lastUpdated", (paramVal)=>
//                        {
//                            return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"resultData.{nameof(entitystorematerialised_CoreResult1.Createddate)}", "ge", paramVal.Substring(2,paramVal.Length-2), new DateTimeSearchExpressionProvider()) };
//                        }
//                    },
//                    { "Patient.DateOfBirth", PatientDOBCriteria
//                    },
//                    {"FamilyName", PatientSearchCriteria }
//            };

//            this._defaultHospitalRefNo = GetDefaultHospitalRefNo();
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

//            var materializedCoreResults = searchResults.Item1;
//            var materializedCorePersonIdentifier = searchResults.Item2;

//            if (!materializedCoreResults.IsCollectionValid() || materializedCorePersonIdentifier.IsNull()) return searchResultData;

//            materializedCoreResults.Each((res =>
//            {
//                var resourceData = new ResourceData(fhirParam);

//                var result = CreateResult(res, materializedCorePersonIdentifier);

//                resourceData.Resource = result;

//                searchResultData.ResourcesData.Add(resourceData);

//            }));

//            return searchResultData;
//        }

//        private Observation CreateResult(entitystorematerialised_CoreResult1 result, entitystorematerialised_CorePersonidentifier personIdentifier)
//        {
//            var res = result.GetResult();

//            AddIdentifier(result, res, personIdentifier);

//            return res;
//        }
        
//        private void AddIdentifier(entitystorematerialised_CoreResult1 result, Observation res, entitystorematerialised_CorePersonidentifier personIdentifier)
//        {
//            if (personIdentifier.IsNull()) return;

//            res.Subject = new ResourceReference
//            {
//                Identifier = new Identifier(),

//                Reference = "Patient"
//            };

//            res.Subject.Identifier.Value = personIdentifier.Idnumber;
//            res.Subject.Identifier.System = personIdentifier.Idtypecode ?? _defaultHospitalRefNo;
//        }

//        private Tuple<List<entitystorematerialised_CoreResult1>, entitystorematerialised_CorePersonidentifier> GetSearchResults(SearchParams searchParams)
//        {
//            var baseQuery = (from result in this._matCoreResultRepo.ItemsAsReadOnly
//                             join person in this._matCorePersonRepo.ItemsAsReadOnly on
//                             result.PersonId equals person.PersonId
//                             join personIden in this._matCorePersonIdentifierRepo.ItemsAsReadOnly on
//                             person.PersonId equals personIden.PersonId
//                             select new
//                             {
//                                 resultData = result,
//                                 personData = person,
//                                 personIdData = personIden
//                             });

//            var searchOp = new HL7SynapseHandler.Service.Search.SearchOpProcessor();

//            var withSearchClause = searchOp.Apply(_supportedSearchCriteria, searchParams, baseQuery);

//            //var stringQuery = _matCorePersonIdentifierRepo.GetSql(withSearchClause);

//            var matResults = withSearchClause
//                .OrderByDescending((entity) => entity.resultData.Createdtimestamp)
//                .Select(entity => new
//                {
//                    result = entity.resultData as entitystorematerialised_CoreResult1,
//                    patientIdentifer = entity.personIdData as entitystorematerialised_CorePersonidentifier
//                })
//                .ToList();

//            if (matResults.IsCollectionValid())
//            {
//                var results = matResults.Select(m => m.result).ToList();
//                var patientIdentifier = matResults.Select(m => m.patientIdentifer).FirstOrDefault();

//                return new Tuple<List<entitystorematerialised_CoreResult1>, entitystorematerialised_CorePersonidentifier>(results, patientIdentifier);
//            }

//            return null;
//        }

//        private string GetDefaultHospitalRefNo()
//        {
//            IConfiguration configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

//            IConfigurationSection careRecordConfig = configuration.GetSection("CareRecordConfig");

//            return careRecordConfig.GetValue<string>("HospitalNumberReference");
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

//        private List<SynapseSearchTerm> ResultIdSearchCriteria(string paramVal)
//        {
//            return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"resultData.{nameof(entitystorematerialised_CoreResult1.ResultId)}", "eq", paramVal, new StringSearchExpressionProvider())
//            };
//        }
//    }
//}
