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


using AutoMapper;
using Hl7.Fhir.Rest;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
using Interneuron.CareRecord.HL7SynapseService.Interfaces;
using Interneuron.CareRecord.Infrastructure.Domain;
using Interneuron.CareRecord.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Interneuron.CareRecord.Infrastructure.Search;
using Interneuron.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Search;
using Interneuron.CareRecord.Repository;
using Interneuron.CareRecord.HL7SynapseService.Models;
using Hl7.Fhir.Model;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Extensions;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Implementations.Queries.SearchTermsBuilder;

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.Implementations.Queries
{
    public class GenericSearchHandler : IResourceSearchHandler
    {
        private IGenericSearchRepository _genericSearchRepository;
        private IServiceProvider _provider;
        private IMapper _mapper;

        public GenericSearchHandler(IServiceProvider provider, IMapper mapper, IGenericSearchRepository genericSearchRepository)
        {
            this._provider = provider;
            this._mapper = mapper;
            this._genericSearchRepository = genericSearchRepository;
        }
        public SearchResultData Handle(FHIRParam fhirParam, SearchParams searchParams)
        {
            var searchEntityIdentifier = $"Search_{fhirParam.TypeName}";

            var resourcesData = new List<ResourceData>();

            var searchResultData = new SearchResultData
            {
                ResourcesData = resourcesData
            };

            var searchTermsBuilder = this._provider.GetService(typeof(SearchTermsBuilderFactory)) as SearchTermsBuilderFactory;

            var supportedSearchTerms = searchTermsBuilder.GetSearchTermsBuilder(searchEntityIdentifier).GetSearchTerms();

            var synapseSearchTerms = new SearchOpProcessor().GetSynapseSearchTerms(supportedSearchTerms, searchParams);

            var searchResults = this._genericSearchRepository.Search(searchEntityIdentifier, synapseSearchTerms);

            if (searchResults.IsNull()) return searchResultData;

            ProcessSearchResult(fhirParam, searchResultData, searchResults);

            return searchResultData;

            #region Oldcode
            /*
            var mainEntityInResult = searchResults[0];

            if (mainEntityInResult == null) return searchResultData;


            //var firstResType = ((System.Runtime.Remoting.ObjectHandle)firstRes).Unwrap().GetType();

            //var data = this._mapper.Map<List<Encounter>>(searchResults[0]);

            //Type dataType;

            //var x = firstRes.Get
            //if (firstRes is System.Collections.IEnumerable)
            //{
            //    dataType = typeof(List<>).MakeGenericType(fhirParam.TypeName.GetHL7ModelType().ConvertTo<List<>>());
            //}
            //else
            //{
            //    dataType = fhirParam.TypeName.GetHL7ModelType();
            //}

            var searchEntityType = fhirParam.TypeName.GetHL7ModelType();
            var dataType = Extensions.TypeExtensions.ConvertToListOrType(searchEntityType, mainEntityInResult);

            var data = this._mapper.Map(mainEntityInResult, mainEntityInResult.GetType(), dataType);// typeof(List<Encounter>));

            if (data is System.Collections.IEnumerable)
            {
                foreach (var dataItem in data)
                {
                    AppendWithSubsequentData(searchEntityType, dataItem, searchResults);

                    var resourceData = new ResourceData(fhirParam)
                    {
                        Resource = dataItem
                    };
                    searchResultData.ResourcesData.Add(resourceData);
                }
            }
            else
            {
                AppendWithSubsequentData(searchEntityType, data, searchResults);

                var resourceData = new ResourceData(fhirParam)
                {
                    Resource = data
                };

                searchResultData.ResourcesData.Add(resourceData);
            }

            return searchResultData;
            */
            #endregion
        }

        private void ProcessSearchResult(FHIRParam fhirParam, SearchResultData searchResultData, List<dynamic> searchResults)
        {
            var mainEntityInResult = searchResults[0];

            if (mainEntityInResult == null) return;

            var searchEntityType = fhirParam.TypeName.GetHL7ModelType();

            var dataType = Extensions.TypeExtensions.ConvertToListOrType(searchEntityType, mainEntityInResult);

            var data = this._mapper.Map(mainEntityInResult, mainEntityInResult.GetType(), dataType);

            if (data is System.Collections.IEnumerable)
            {
                foreach (var dataItem in data)
                {
                    AppendWithSubsequentData(searchEntityType, dataItem, searchResults);

                    var resourceData = new ResourceData(fhirParam)
                    {
                        Resource = dataItem
                    };
                    searchResultData.ResourcesData.Add(resourceData);
                }
            }
            else
            {
                AppendWithSubsequentData(searchEntityType, data, searchResults);

                var resourceData = new ResourceData(fhirParam)
                {
                    Resource = data
                };

                searchResultData.ResourcesData.Add(resourceData);
            }
        }

        private void AppendWithSubsequentData(Type searchEntityType, dynamic dataItem, List<dynamic> searchResults)
        {
            for (int resIndex = 1; resIndex < searchResults.Count; resIndex++)
            {
                var result = searchResults[resIndex];

                if (result != null)
                {
                    if (result is System.Collections.IEnumerable)
                    {
                        foreach (var resultData in result)
                        {
                            if (resultData != null)
                            {
                                var dataType = Extensions.TypeExtensions.ConvertToListOrType(searchEntityType, resultData);
                                this._mapper.Map(resultData, dataItem, resultData.GetType(), dataType);
                            }
                        }
                    }
                    else
                    {
                        var dataType = Extensions.TypeExtensions.ConvertToListOrType(searchEntityType, result);
                        this._mapper.Map(result, dataItem, result.GetType(), dataType);
                    }
                }
            }
        }
    }

    //public class SynapseQueryBuilder
    //{
    //    private IServiceProvider _provider;

    //    //public abstract IQueryable<dynamic> GetBaseQuery(string type);

    //    //public abstract Dictionary<string, Func<string, List<SynapseSearchTerm>>> GetSupportedSearchTerms(string type);

    //    //public abstract void BuildQuery(string type, SearchParams searchParams);
    //    public SynapseQueryBuilder(IServiceProvider provider)
    //    {
    //        this._provider = provider;
    //    }

    //    public void BuildQuery(string type, SearchParams searchParams)
    //    {
    //        var baseQuery = this.GetBaseQuery(type);

    //        var supportedSearchParams = GetSupportedSearchTerms(type);

    //        var searchProcessor = new SearchOpProcessor();
    //        var synapseSearchTerms = searchProcessor.GetSynapseSearchTerms(supportedSearchParams, searchParams);

    //        var withSearchClause = searchProcessor.Apply(baseQuery, synapseSearchTerms);


    //    }
    //}

    //public class SearchEncounterQueryRegistry
    //{
    //    private IServiceProvider _provider;
    //    private IReadOnlyRepository<entitystorematerialised_CorePerson> _matCorePersonRepo;
    //    private IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> _matCorePersonIdentifierRepo;
    //    private IReadOnlyRepository<entitystorematerialised_CoreEncounter> _matCoreEncounter;
    //    private IMapper _mapper;
    //    private string _defaultHospitalRefNo;

    //    public SearchEncounterQueryRegistry()
    //    {

    //    }
    //    public SearchEncounterQueryRegistry(IServiceProvider provider, IMapper mapper, IReadOnlyRepository<entitystorematerialised_CorePerson> matCorePersonRepo, IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> matCorePersonIdentifierRepo,
    //        IReadOnlyRepository<entitystorematerialised_CoreEncounter> matCoreEncounter)
    //    {
    //        this._provider = provider;
    //        this._matCorePersonRepo = matCorePersonRepo;
    //        this._matCorePersonIdentifierRepo = matCorePersonIdentifierRepo;
    //        this._matCoreEncounter = matCoreEncounter;
    //        this._mapper = mapper;
    //        this._defaultHospitalRefNo = GetDefaultHospitalRefNo();
    //    }

    //    public IQueryable<dynamic> GetBaseQuery(string type)
    //    {
    //        if (type.EqualsIgnoreCase("SearchEncounter"))
    //        {
    //            var query = (from encounter in this._matCoreEncounter.ItemsAsReadOnly
    //                         join person in this._matCorePersonRepo.ItemsAsReadOnly on
    //                         encounter.PersonId equals person.PersonId
    //                         join personId in this._matCorePersonIdentifierRepo.ItemsAsReadOnly on
    //                         person.PersonId equals personId.PersonId
    //                         select new
    //                         {
    //                             encounterData = encounter,
    //                             personData = person,
    //                             personIdData = personId
    //                         });
    //            return query;
    //        }

    //        return null;
    //    }

    //    public Dictionary<string, Func<string, List<SynapseSearchTerm>>> GetSupportedSearchTerms(string type)
    //    {
    //        var supportedSearchCriteria = new Dictionary<string, Func<string, List<SynapseSearchTerm>>>()
    //        {
    //                { "Patient.Identifier", PatientIdSearchCriteria },
    //                { "Subject.Patient.Identifier", PatientIdSearchCriteria },
    //                { "_lastUpdated", (paramVal)=>
    //                    {
    //                        return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"encounterData.{nameof(entitystorematerialised_CoreEncounter.Createddate)}", "ge", paramVal.Substring(2,paramVal.Length-2), new DateTimeSearchExpressionProvider()) };
    //                    }
    //                },
    //                { "Patient.DateOfBirth", PatientDOBCriteria
    //                },
    //                {"FamilyName", PatientSearchCriteria }
    //        };

    //        return supportedSearchCriteria;
    //    }

    //    private string GetDefaultHospitalRefNo()
    //    {
    //        IConfiguration configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

    //        IConfigurationSection careRecordConfig = configuration.GetSection("CareRecordConfig");

    //        return careRecordConfig.GetValue<string>("HospitalNumberReference");
    //    }
    //    private List<SynapseSearchTerm> PatientIdSearchCriteria(string paramVal)
    //    {
    //        var searchTerms = new List<SynapseSearchTerm>();

    //        if (paramVal.Contains("|"))
    //        {
    //            var paramVals = paramVal.Split("|");

    //            searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idtypecode)}", "eq", paramVals[0], new DefaultSearchExpressionProvider()));

    //            searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idnumber)}", "eq", paramVals[1], new DefaultSearchExpressionProvider()));

    //            return searchTerms;
    //        }
    //        searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idtypecode)}", "eq", this._defaultHospitalRefNo, new DefaultSearchExpressionProvider()));

    //        searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idnumber)}", "eq", paramVal, new DefaultSearchExpressionProvider()));

    //        return searchTerms;
    //    }

    //    private List<SynapseSearchTerm> PatientSearchCriteria(string paramVal)
    //    {
    //        var searchTerms = new List<SynapseSearchTerm>
    //        {
    //            new SynapseSearchTerm($"personData.{nameof(entitystorematerialised_CorePerson.Familyname)}", "eq", paramVal, new StringSearchExpressionProvider())
    //        };

    //        return searchTerms;
    //    }

    //    private List<SynapseSearchTerm> PatientDOBCriteria(string paramVal)
    //    {
    //        return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"personData.{nameof(entitystorematerialised_CorePerson.Dateofbirth)}", "eq", paramVal, new DateTimeSearchExpressionProvider())
    //        };
    //    }
    //}
}