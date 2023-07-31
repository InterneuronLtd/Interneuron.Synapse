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


﻿using AutoMapper;
using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using Interneuron.Terminology.Model.Search;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interneuron.Terminology.Model.History;
using Interneuron.Terminology.Model.Other;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public partial class FormularyQueries : IFormularyQueries
    {
        private IServiceProvider _provider;
        private IMapper _mapper;
        private IConfiguration _configuration;

        public FormularyQueries(IServiceProvider provider, IMapper mapper, IConfiguration configuration)
        {
            this._provider = provider;
            this._mapper = mapper;
            _configuration = configuration;
        }

        public List<FormularyDTO> GetLatestFormulariesBriefInfo()
        {
            var repo = this._provider.GetService(typeof(IReadOnlyRepository<FormularyHeader>)) as IReadOnlyRepository<FormularyHeader>;

            //Get Only non-archived and non-deleted latest records
            var formulariesHeaders = repo.ItemsAsReadOnly.Where(rec => rec.IsLatest == true && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED
            && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED).ToList();

            if (!formulariesHeaders.IsCollectionValid()) return null;

            var dtos = _mapper.Map<List<FormularyDTO>>(formulariesHeaders);

            return dtos;
        }

        public async Task<List<FormularySearchResultDTO>> GetLatestTopLevelFormulariesBasicInfo()
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyBasicSearchResultModel>)) as IFormularyRepository<FormularyBasicSearchResultModel>;

            var formulariesResultsFromDb = await repo.GetLatestTopLevelNodesWithBasicResults();

            if (!formulariesResultsFromDb.IsCollectionValid()) return null;

            var formulariesResults = formulariesResultsFromDb.ToList();

            if (!formulariesResults.IsCollectionValid()) return null;

            formulariesResults = formulariesResults.Where(rec => rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED).ToList();

            var resDTO = _mapper.Map<List<FormularySearchResultDTO>>(formulariesResults);

            return resDTO.OrderBy(rec => rec.Name).ToList();
        }

        
        public async Task<List<FormularySearchResultDTO>> GetFormularyDescendentForCodes(List<string> codes, bool onlyNonDeleted = true)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyBasicSearchResultModel>)) as IFormularyRepository<FormularyBasicSearchResultModel>;

            var formulariesResults = await repo.GetFormularyDescendentForCodes(codes.ToArray(), onlyNonDeleted);

            if (!formulariesResults.IsCollectionValid()) return null;

            var resDTO = _mapper.Map<List<FormularySearchResultDTO>>(formulariesResults);

            resDTO = resDTO.Where(rec => !codes.Contains(rec.Code)).ToList();

            return resDTO?.OrderBy(rec => rec.Name).ToList();
        }

        public async Task<List<FormularySearchResultDTO>> GetFormularyImmediateDescendentForCodes(List<string> codes, bool onlyNonDeleted = true)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyBasicSearchResultModel>)) as IFormularyRepository<FormularyBasicSearchResultModel>;

            var formulariesResults = await repo.GetFormularyImmediateDescendentForCodes(codes.ToArray(), onlyNonDeleted);

            if (!formulariesResults.IsCollectionValid()) return null;

            var resDTO = _mapper.Map<List<FormularySearchResultDTO>>(formulariesResults);

            //removing the parent record and retain only descendents of it
            resDTO = resDTO.Where(rec => !codes.Contains(rec.Code)).ToList();

            return resDTO?.OrderBy(rec => rec.Name).ToList();
        }

        public List<FormularyDTO> GetLatestFormulariesBriefInfoByNameOrCode(string nameOrCode, string productType = null, bool isExactMatch = false)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            //Get Only latest and non-deleted latest records
            var formulariesHeaders = repo.GetLatestFormulariesBriefInfoByNameOrCode(nameOrCode, productType, isExactMatch);

            if (!formulariesHeaders.IsCollectionValid()) return null;

            var dtos = _mapper.Map<List<FormularyDTO>>(formulariesHeaders);

            return dtos;
        }

        public async Task<List<FormularySearchResultDTO>> GetFormularyAsFlatList(FormularySearchFilterRequest filterCriteriaRequest)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyBasicSearchResultModel>)) as IFormularyRepository<FormularyBasicSearchResultModel>;

            //If search term is present -then call search repo function
            //If no search criteria persent then return bad request

            var results = new List<FormularyBasicSearchResultModel>();

            if (filterCriteriaRequest.SearchTerm.IsNotEmpty())
            {
                var searchResultsFromDb = await repo.SearchFormularyBySearchTerm(filterCriteriaRequest.SearchTerm);

                if (searchResultsFromDb.IsCollectionValid())
                {
                    //Consider only the AMPs from the results applying remaining filter criteria - The parent of these AMPs will be fetched in 'GetMissingAncestors'
                    results = searchResultsFromDb.ToList();
                    results = FilterAMPSearchRecordsByCriteria(filterCriteriaRequest, results);
                }
            }

            if (filterCriteriaRequest.RecStatusCds.IsCollectionValid() || filterCriteriaRequest.FormularyStatusCd.IsCollectionValid() || filterCriteriaRequest.HideArchived == true || filterCriteriaRequest.Flags.IsCollectionValid())
            {
                var ampResultsFromDbList = await SearchForAMPsByFilterCriteria(filterCriteriaRequest);

                if (ampResultsFromDbList.IsCollectionValid())
                {
                    results.AddRange(ampResultsFromDbList);
                }
            }

            if (!results.IsCollectionValid()) return null;

            var ancestorsList = await GetMissingAncestors(results);

            if (ancestorsList.IsCollectionValid())
            {
                results.AddRange(ancestorsList);
            }

            results = results.Distinct(r => r.FormularyVersionId).ToList();

            results = results.OrderBy(rec => rec.Name).ToList();

            var resultsDTO = _mapper.Map<List<FormularySearchResultDTO>>(results);
            return resultsDTO;
        }

        
        public async Task<FormularySearchResultsWithHierarchyDTO> GetFormularyHierarchyForSearchRequest(FormularySearchFilterRequest filterCriteriaRequest)
        {
            var resultsDTO = new FormularySearchResultsWithHierarchyDTO() { FilterCriteria = filterCriteriaRequest, Data = new List<FormularySearchResultWithTreeDTO>() };

            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyBasicSearchResultModel>)) as IFormularyRepository<FormularyBasicSearchResultModel>;

            //If search term is present -then call search repo function
            //If no search criteria persent then return bad request

            var results = new List<FormularyBasicSearchResultModel>();

            if (filterCriteriaRequest.SearchTerm.IsNotEmpty() && !filterCriteriaRequest.Flags.IsCollectionValid())
            {
                var searchResultsFromDb = await repo.SearchFormularyBySearchTerm(filterCriteriaRequest.SearchTerm);

                if (searchResultsFromDb.IsCollectionValid())
                {
                    //Consider only the AMPs from the results applying remaining filter criteria - The parent of these AMPs will be fetched in 'GetMissingAncestors'
                    results = searchResultsFromDb.ToList();
                    results = FilterAMPSearchRecordsByCriteria(filterCriteriaRequest, results);
                }
            }

            if (filterCriteriaRequest.RecStatusCds.IsCollectionValid() || filterCriteriaRequest.FormularyStatusCd.IsCollectionValid() || filterCriteriaRequest.Flags.IsCollectionValid() || filterCriteriaRequest.HideArchived == true)
            {
                var ampResultsFromDbList = await SearchForAMPsByFilterCriteria(filterCriteriaRequest);

                if (ampResultsFromDbList.IsCollectionValid())
                    results.AddRange(ampResultsFromDbList);
            }

            //Not a typical REST style - but to keep DTO consistent
            if (!results.IsCollectionValid()) return resultsDTO;

            var ancestorsList = await GetMissingAncestors(results);

            if (ancestorsList.IsCollectionValid())
            {
                results.AddRange(ancestorsList);
            }

            results = results.Distinct(r => r.FormularyVersionId).ToList();

            if (filterCriteriaRequest.HideArchived == true)
            {
                results = results.Where(x => x.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED).ToList();
            }

            var uniqueFormularyVersionIds = results.Select(r => r.FormularyVersionId).Distinct().ToArray();

            var treeOfAllNodes = BuildChildNodesLookup(results);

            //This comprises both logical and physical root nodes
            //Logical Root nodes: Records which will have the parentcode but not have that parentcode in the records fetched from db
            var rootNodes = GetRootFromAllNodes(results, treeOfAllNodes);

            //resultsFromDBList.OrderByDescending(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            //rootNodes.OrderBy(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            rootNodes.Distinct(r => r.FormularyVersionId).AsParallel().Each(res =>
            {
                var resDTO = GetNodeDetail(res, treeOfAllNodes);

                resultsDTO.Data.Add(resDTO);
            });

            if (resultsDTO.Data.IsCollectionValid())
            {
                var data = resultsDTO.Data.OrderBy(rec => rec.Name).ToList();
                resultsDTO.Data = data;
            }

            return resultsDTO;
        }

        private async Task<List<FormularyBasicSearchResultModel>> GetMissingAncestors(List<FormularyBasicSearchResultModel> results)
        {
            var uniqueParentCodes = results.Select(rec => rec.ParentCode)?.Distinct().ToHashSet();
            var uniqueCodes = results.Select(rec => rec.Code)?.Distinct().ToHashSet();

            var formularyDetailrepo = this._provider.GetService(typeof(IRepository<FormularyDetail>)) as IRepository<FormularyDetail>;

            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyBasicSearchResultModel>)) as IFormularyRepository<FormularyBasicSearchResultModel>;

            //Get the parent codes for which the record does not already exists in the results list
            var parentCodesWithoutRecord = new ConcurrentBag<string>();
            uniqueParentCodes.AsParallel().Each(rec =>
            {
                if (!uniqueCodes.Contains(rec)) parentCodesWithoutRecord.Add(rec);
            });

            if (!parentCodesWithoutRecord.IsCollectionValid()) return null;

            //Bring the parent or all ancestors for these
            var ancestors = await repo.GetFormularyAncestorForCodes(parentCodesWithoutRecord.ToArray());

            if (!ancestors.IsCollectionValid()) return null;

            var ancestorsIds = ancestors.Select(rec => rec.FormularyVersionId).Distinct().ToList();

            var details = formularyDetailrepo.ItemsAsReadOnly.Where(rec => ancestorsIds.Contains(rec.FormularyVersionId)).Distinct(rec => rec.FormularyVersionId).ToList();

            if (!details.IsCollectionValid()) ancestors.ToList();

            var detailForIds = new ConcurrentDictionary<string, FormularyDetail>();

            details.AsParallel().Each(det =>
        {
            detailForIds[det.FormularyVersionId] = det;
        });

            ancestors.Each(rec =>
            {
                if (detailForIds.ContainsKey(rec.FormularyVersionId))
                {
                    var detail = detailForIds[rec.FormularyVersionId];
                    rec.RnohFormularyStatuscd = detail.RnohFormularyStatuscd;
                    rec.Prescribable = (detail.Prescribable == true);
                }
            });

            return ancestors.ToList();
        }

        private List<FormularyBasicSearchResultModel> FilterAMPSearchRecordsByCriteria(FormularySearchFilterRequest filterCriteriaRequest, List<FormularyBasicSearchResultModel> results)
        {
            if (!results.IsCollectionValid()) return null;

            var formularyStatusCodes = new List<string>();
            var recStatusCodes = new List<string>();

            filterCriteriaRequest.FormularyStatusCd?.Each(rec => formularyStatusCodes.Add(rec));
            filterCriteriaRequest.RecStatusCds?.Each(rec => recStatusCodes.Add(rec));

            //if (filterCriteriaRequest.ShowOnlyArchived == true)
            //{
            //    recStatusCodes.Clear();
            //    recStatusCodes.Add(TerminologyConstants.RECORDSTATUS_ARCHIVED);
            //}

            //If Formulary Status has both formulary and non-formulary then make it null - since it includes both
            if (filterCriteriaRequest.FormularyStatusCd.IsCollectionValid() && filterCriteriaRequest.FormularyStatusCd.Contains(TerminologyConstants.FORMULARYSTATUS_FORMULARY) && filterCriteriaRequest.FormularyStatusCd.Contains(TerminologyConstants.FORMULARYSTATUS_NONFORMULARY))
            {
                formularyStatusCodes = null;
            }

            //Filter AMPs by criteria
            var amps = results.Where(rec => (string.Compare(rec.ProductType, "amp", true) == 0) 
            && (!recStatusCodes.IsCollectionValid() || recStatusCodes.Contains(rec.RecStatusCode))
            && (!formularyStatusCodes.IsCollectionValid() || formularyStatusCodes.Contains(rec.RnohFormularyStatuscd)))
                .ToList();

            if (filterCriteriaRequest.HideArchived == true)
                amps = amps.Where(r => r.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED).ToList();
            else
                amps = amps.Where(r => r.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED).ToList();

            if (filterCriteriaRequest.IncludeDeleted == false)
                amps = amps.Where(r => r.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED).ToList();

            return amps;
        }

        private async Task<List<FormularyBasicSearchResultModel>> SearchForAMPsByFilterCriteria(FormularySearchFilterRequest filterCriteriaRequest)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyBasicSearchResultModel>)) as IFormularyRepository<FormularyBasicSearchResultModel>;

            var formularyStatusCodes = new List<string>();
            var recStatusCodes = new List<string>();
            var flags = new List<string>();

            filterCriteriaRequest.FormularyStatusCd?.Each(rec => formularyStatusCodes.Add(rec));
            filterCriteriaRequest.RecStatusCds?.Each(rec => recStatusCodes.Add(rec));
            filterCriteriaRequest.Flags?.Each(rec => flags.Add(rec));

            //if (filterCriteriaRequest.ShowOnlyArchived == true)
            //{
            //    recStatusCodes.Clear();
            //    recStatusCodes.Add(TerminologyConstants.RECORDSTATUS_ARCHIVED);
            //}

            //If Formulary Status has both formulary and non-formulary then make it null - since it includes both
            if (filterCriteriaRequest.FormularyStatusCd.IsCollectionValid() && filterCriteriaRequest.FormularyStatusCd.Contains(TerminologyConstants.FORMULARYSTATUS_FORMULARY) && filterCriteriaRequest.FormularyStatusCd.Contains(TerminologyConstants.FORMULARYSTATUS_NONFORMULARY))
                formularyStatusCodes = null;

            var ampResultsFromDb = await repo.GetLatestAMPNodesWithBasicResultsForAttributes(filterCriteriaRequest.SearchTerm, recStatusCodes, formularyStatusCodes, flags);

            if (!ampResultsFromDb.IsCollectionValid()) return null;

            var ampResultsFromDbList = ampResultsFromDb.ToList();
            if (filterCriteriaRequest.HideArchived == true)
                ampResultsFromDbList = ampResultsFromDbList.Where(r => r.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED).ToList();
            else
                ampResultsFromDbList = ampResultsFromDbList.Where(r => r.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED).ToList();

            if (filterCriteriaRequest.IncludeDeleted == false)
                ampResultsFromDbList = ampResultsFromDbList.Where(r => r.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED).ToList();

            return ampResultsFromDbList;
        }

        

        private FormularySearchResultWithTreeDTO GetNodeDetail(FormularyBasicSearchResultModel res, ConcurrentDictionary<string, List<FormularyBasicSearchResultModel>> treeOfAllNodes)
        {
            var resDTO = _mapper.Map<FormularySearchResultWithTreeDTO>(res);

            
            //GetChildren Records -- IN Next level
            var chidrenNodes = GetChildren(treeOfAllNodes, res.Code);

            resDTO.Children = new List<FormularySearchResultWithTreeDTO>();

            if (chidrenNodes.IsCollectionValid())
                resDTO.Children.AddRange(chidrenNodes);

            return resDTO;
        }

        private List<FormularySearchResultWithTreeDTO> GetChildren(ConcurrentDictionary<string, List<FormularyBasicSearchResultModel>> treeOfAllNodes, string code, int? level = null)
        {
            var childRecords = new List<FormularySearchResultWithTreeDTO>();

            if (!treeOfAllNodes.IsCollectionValid() || !treeOfAllNodes.ContainsKey(code)) return childRecords;

            var childNodes = treeOfAllNodes[code];

            if (!childNodes.IsCollectionValid()) return new List<FormularySearchResultWithTreeDTO>();

            childNodes.AsParallel().Each(res =>
            {
                var resDTO = _mapper.Map<FormularySearchResultWithTreeDTO>(res);

                //GetChildren Records -- IN Next level
                var chidrenNodes = GetChildren(treeOfAllNodes, res.Code);//Not considering the level now

                resDTO.Children = new List<FormularySearchResultWithTreeDTO>();

                if (chidrenNodes.IsCollectionValid())
                    resDTO.Children.AddRange(chidrenNodes);

                childRecords.Add(resDTO);

            });

            return childRecords;
        }

        private List<FormularyBasicSearchResultModel> GetRootFromAllNodes(List<FormularyBasicSearchResultModel> resultsFromDBList, ConcurrentDictionary<string, List<FormularyBasicSearchResultModel>> parentWithChildNodes)
        {

            //If the record doesn't have its parentid null or not in the list of codes, then it is considered root
            List<FormularyBasicSearchResultModel> rootLevelRecords = new List<FormularyBasicSearchResultModel>();

            //If ParentId is null -- then it is root
            var initialRootLevelRecords = resultsFromDBList.Where(r => r.ParentCode == null);

            if (initialRootLevelRecords.IsCollectionValid())
            {
                rootLevelRecords.AddRange(initialRootLevelRecords);
            }

            if (!parentWithChildNodes.IsCollectionValid()) return rootLevelRecords;

            //hashset of all codes
            var allUniqueCodes = resultsFromDBList.Select(r => r.Code).Distinct().ToHashSet();

            parentWithChildNodes.Each(node =>
            {
                if (!allUniqueCodes.Contains(node.Key))//Each node.key is parent node code here
                {
                    rootLevelRecords.AddRange(node.Value);
                }
            });

            return rootLevelRecords;
        }

        /// <summary>
        /// Build Dictionary of Parent Nodes (as Keys) and its associated records
        /// </summary>
        /// <param name="resultsFromDB"></param>
        /// <returns></returns>
        private ConcurrentDictionary<string, List<FormularyBasicSearchResultModel>> BuildChildNodesLookup(List<FormularyBasicSearchResultModel> resultsFromDB)
        {
            var parentWithChildNodes = new ConcurrentDictionary<string, List<FormularyBasicSearchResultModel>>();

            var onlyChildRecords = resultsFromDB.Where(r => r.ParentCode != null);//first level will not have parentid returned from db

            if (onlyChildRecords.IsCollectionValid())
            {
                onlyChildRecords = onlyChildRecords.OrderBy(rec => rec.Name).ToList();

                onlyChildRecords.AsParallel().Each(r =>
                {
                    var key = r.ParentCode;

                    if (parentWithChildNodes.ContainsKey(key))
                    {
                        parentWithChildNodes[key].Add(r);
                    }
                    else
                    {
                        parentWithChildNodes[key] = new List<FormularyBasicSearchResultModel>() { r };
                    }
                });
            }

            return parentWithChildNodes;
        }


        public async Task<FormularyDTO> GetFormularyDetail(string id)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var resultsFromDB = repo.GetFormularyDetail(id);

            var resultObj = resultsFromDB.FirstOrDefault();

            if (resultObj == null) return null;

            var headerDTO = this._mapper.Map<FormularyDTO>(resultObj);

            var formularyDetailObj = resultObj.FormularyDetail.FirstOrDefault();

            if (formularyDetailObj.IsNotNull())
            {
                headerDTO.Detail = this._mapper.Map<FormularyDetailDTO>(formularyDetailObj);
            }

            if (resultObj.FormularyAdditionalCode.IsCollectionValid())
            {
                headerDTO.FormularyAdditionalCodes = this._mapper.Map<List<FormularyAdditionalCodeDTO>>(resultObj.FormularyAdditionalCode);
            }

            //if (resultObj.FormularyIndication.IsCollectionValid())
            //{
            //    headerDTO.FormularyIndications = this._mapper.Map<List<FormularyIndicationDTO>>(resultObj.FormularyIndication);
            //}

            if (resultObj.FormularyIngredient.IsCollectionValid())
            {
                headerDTO.FormularyIngredients = new List<FormularyIngredientDTO>();

                resultObj.FormularyIngredient.Each(fi =>
                {
                    headerDTO.FormularyIngredients.Add(_mapper.Map<FormularyIngredientDTO>(fi));
                });
                //headerDTO.FormularyIngredients = this._mapper.Map<List<FormularyIngredientDTO>>(resultObj.FormularyIngredient);
            }

            if (resultObj.FormularyRouteDetail.IsCollectionValid())
            {
                headerDTO.FormularyRouteDetails = new List<FormularyRouteDetailDTO>();

                resultObj.FormularyRouteDetail.Each(route =>
                {
                    headerDTO.FormularyRouteDetails.Add(_mapper.Map<FormularyRouteDetailDTO>(route));
                });

                //headerDTO.FormularyRouteDetails = this._mapper.Map<List<FormularyRouteDetailDTO>>(resultObj.FormularyRouteDetail);
            }

            if (resultObj.FormularyLocalRouteDetail.IsCollectionValid())
            {
                headerDTO.FormularyLocalRouteDetails = new List<FormularyLocalRouteDetailDTO>();

                resultObj.FormularyLocalRouteDetail.Each(route =>
                {
                    headerDTO.FormularyLocalRouteDetails.Add(_mapper.Map<FormularyLocalRouteDetailDTO>(route));
                });

                //headerDTO.FormularyRouteDetails = this._mapper.Map<List<FormularyRouteDetailDTO>>(resultObj.FormularyRouteDetail);
            }

            await HydrateWithLookupDetails(headerDTO);

            return headerDTO;
        }

        private async Task HydrateWithLookupDetails(FormularyDTO headerDTO)
        {
            //To be handled concurrent db call later
            //var uomsLkpTask = GetLookup<DmdLookupUomDTO, string, string>(LookupType.DMDUOM, (rec) => rec.Cd, rec => rec.Desc);
            //var doseFormsLkpTask = GetLookup<DmdLookupDrugformindDTO, string, string>(LookupType.DMDDoseForm, rec => rec.Cd.ToString(), rec => rec.Desc);
            //var formsLkpTask = GetLookup<DmdLookupFormDTO, string, string>(LookupType.DMDForm, rec => rec.Cd, rec => rec.Desc);
            //var strengthsTask = GetLookup<DmdLookupBasisofstrengthDTO, string, string>(LookupType.DMDPharamceuticalStrength, rec => rec.Cd.ToString(), rec => rec.Desc);
            //var ingredientsTask = GetLookup<DmdLookupIngredientDTO, string, string>(LookupType.DMDIngredient, rec => rec.Isid.ToString(), rec => rec.Nm);
            //var routesTask = GetLookup<DmdLookupRouteDTO, string, string>(LookupType.DMDRoute, rec => rec.Cd, rec => rec.Desc);

            //await Task.WhenAll(uomsLkpTask, doseFormsLkpTask, formsLkpTask, strengthsTask, ingredientsTask, routesTask);

            //var uomsLkp = await uomsLkpTask; var doseFormsLkp = await doseFormsLkpTask; var formsLkp = await formsLkpTask;
            //var strengthsLkp = await strengthsTask; var ingredientsLkp = await ingredientsTask; var routesLkp = await routesTask;

            var uomsLkp = await GetLookup<DmdLookupUomDTO, string, string>(LookupType.DMDUOM, (rec) => rec.Cd, rec => rec.Desc);
            var doseFormsLkp = await GetLookup<DmdLookupDrugformindDTO, string, string>(LookupType.DMDDoseForm, rec => rec.Cd.ToString(), rec => rec.Desc);
            var formsLkp = await GetLookup<DmdLookupFormDTO, string, string>(LookupType.DMDForm, rec => rec.Cd, rec => rec.Desc);
            var strengthsLkp = await GetLookup<DmdLookupBasisofstrengthDTO, string, string>(LookupType.DMDPharamceuticalStrength, rec => rec.Cd.ToString(), rec => rec.Desc);
            var ingredientsLkp = await GetLookup<DmdLookupIngredientDTO, string, string>(LookupType.DMDIngredient, rec => rec.Isid.ToString(), rec => rec.Nm);
            var routesLkp = await GetLookup<DmdLookupRouteDTO, string, string>(LookupType.DMDRoute, rec => rec.Cd, rec => rec.Desc);


            if (headerDTO.Detail != null)
            {
                if (headerDTO.Detail.DoseFormCd.IsNotEmpty() && doseFormsLkp.ContainsKey(headerDTO.Detail.DoseFormCd))
                {
                    headerDTO.Detail.DoseFormDesc = doseFormsLkp[headerDTO.Detail.DoseFormCd];
                }

                if (headerDTO.Detail.UnitDoseFormUnits.IsNotEmpty() && uomsLkp.ContainsKey(headerDTO.Detail.UnitDoseFormUnits))
                {
                    headerDTO.Detail.UnitDoseFormUnitsDesc = uomsLkp[headerDTO.Detail.UnitDoseFormUnits];
                }

                if (headerDTO.Detail.UnitDoseUnitOfMeasureCd.IsNotEmpty() && uomsLkp.ContainsKey(headerDTO.Detail.UnitDoseUnitOfMeasureCd))
                {
                    headerDTO.Detail.UnitDoseUnitOfMeasureDesc = uomsLkp[headerDTO.Detail.UnitDoseUnitOfMeasureCd];
                }

                if (headerDTO.Detail.FormCd.IsNotEmpty() && formsLkp.ContainsKey(headerDTO.Detail.FormCd))
                {
                    headerDTO.Detail.FormDesc = formsLkp[headerDTO.Detail.FormCd];
                }
            }

            if (headerDTO.FormularyIngredients.IsCollectionValid())
            {
                headerDTO.FormularyIngredients.Each(ing =>
                {
                    if (ing.BasisOfPharmaceuticalStrengthCd.IsNotEmpty() && strengthsLkp.ContainsKey(ing.BasisOfPharmaceuticalStrengthCd))
                    {
                        ing.BasisOfPharmaceuticalStrengthDesc = strengthsLkp[ing.BasisOfPharmaceuticalStrengthCd];
                    }

                    if (ing.IngredientCd.IsNotEmpty() && ing.IngredientName.IsEmpty() && ingredientsLkp.ContainsKey(ing.IngredientCd))
                    {
                        ing.IngredientName = ingredientsLkp.ContainsKey(ing.IngredientCd)? ingredientsLkp[ing.IngredientCd]: null;
                    }

                    if (ing.StrengthValueDenominatorUnitCd.IsNotEmpty() && uomsLkp.ContainsKey(ing.StrengthValueDenominatorUnitCd))
                    {
                        ing.StrengthValueDenominatorUnitDesc = uomsLkp[ing.StrengthValueDenominatorUnitCd];
                    }
                    if (ing.StrengthValueNumeratorUnitCd.IsNotEmpty() && uomsLkp.ContainsKey(ing.StrengthValueNumeratorUnitCd))
                    {
                        ing.StrengthValueNumeratorUnitDesc = uomsLkp[ing.StrengthValueNumeratorUnitCd];
                    }
                });
            }

            if (headerDTO.FormularyRouteDetails.IsCollectionValid())
            {
                var routeTypes = await GetLookup<FormularyLookupItemDTO>(LookupType.RouteFieldType);
                var routeTypesLkp = new Dictionary<string, string>();

                if (routesLkp.IsCollectionValid() && routeTypes.IsCollectionValid())
                {
                    routeTypesLkp = routeTypes.Where(d => d.Type == LookupType.RouteFieldType.GetTypeName())?
                                        .Select(rec => new
                                        {
                                            cd = rec.Cd,
                                            desc = rec.Desc
                                        })
                                        .ToDictionary(k => k.cd, d => d.desc);

                    headerDTO.FormularyRouteDetails.Each(route =>
                                        {
                                            if (route.RouteCd.IsNotEmpty() && routesLkp.ContainsKey(route.RouteCd) && routeTypesLkp.ContainsKey(route.RouteFieldTypeCd))
                                            {
                                                route.RouteDesc = routesLkp[route.RouteCd];
                                                route.RouteFieldTypeDesc = routeTypesLkp[route.RouteFieldTypeCd];
                                            }
                                        });
                }
            }

            if (headerDTO.FormularyLocalRouteDetails.IsCollectionValid())
            {
                var routeTypes = await GetLookup<FormularyLookupItemDTO>(LookupType.RouteFieldType);
                var routeTypesLkp = new Dictionary<string, string>();

                if (routesLkp.IsCollectionValid() && routeTypes.IsCollectionValid())
                {
                    routeTypesLkp = routeTypes.Where(d => d.Type == LookupType.RouteFieldType.GetTypeName())?
                                        .Select(rec => new
                                        {
                                            cd = rec.Cd,
                                            desc = rec.Desc
                                        })
                                        .ToDictionary(k => k.cd, d => d.desc);

                    headerDTO.FormularyLocalRouteDetails.Each(route =>
                    {
                        if (route.RouteCd.IsNotEmpty() && routesLkp.ContainsKey(route.RouteCd) && routeTypesLkp.ContainsKey(route.RouteFieldTypeCd))
                        {
                            route.RouteDesc = routesLkp[route.RouteCd];
                            route.RouteFieldTypeDesc = routeTypesLkp[route.RouteFieldTypeCd];
                        }
                    });
                }
            }
        }

        public async Task<List<FormularyLocalRouteDetailDTO>> GetFormulariesRoutes()
        {
            var repo = this._provider.GetService(typeof(IReadOnlyRepository<FormularyRouteDetail>)) as IReadOnlyRepository<FormularyRouteDetail>;

            var formulariesRoutes = repo.ItemsAsReadOnly.ToList();

            if (!formulariesRoutes.IsCollectionValid()) return null;

            var dtos = _mapper.Map<List<FormularyLocalRouteDetailDTO>>(formulariesRoutes);

            return dtos;
        }

        public async Task<List<FormularyHistoryDTO>> GetHistoryOfFormularies()
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyHistoryModel>)) as IFormularyRepository<FormularyHistoryModel>;

            var historyResults = await repo.GetHistoryOfFormularies();

            if (!historyResults.IsCollectionValid()) return null;

            var historyResultsList = _mapper.Map<List<FormularyHistoryDTO>>(historyResults);

            return historyResultsList.ToList();
        }


        public async Task<List<FormularyLocalLicensedUseDTO>> GetLocalLicensedUse(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyLocalLicensedUseModel>)) as IFormularyRepository<FormularyLocalLicensedUseModel>;

            var localLicensedUses = await repo.GetFormularyLocalLicensedUse(formularyVersionIds);

            if (!localLicensedUses.IsCollectionValid()) return null;

            var localLicensedUsesList = _mapper.Map<List<FormularyLocalLicensedUseDTO>>(localLicensedUses);

            return localLicensedUsesList.ToList();
        }

        public async Task<List<FormularyLocalUnlicensedUseDTO>> GetLocalUnlicensedUse(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyLocalUnlicensedUseModel>)) as IFormularyRepository<FormularyLocalUnlicensedUseModel>;

            var localUnlicensedUses = await repo.GetFormularyLocalUnlicensedUse(formularyVersionIds);

            if (!localUnlicensedUses.IsCollectionValid()) return null;

            var localUnlicensedUsesList = _mapper.Map<List<FormularyLocalUnlicensedUseDTO>>(localUnlicensedUses);

            return localUnlicensedUsesList.ToList();
        }

        public async Task<List<FormularyLocalLicensedRouteDTO>> GetLocalLicensedRoute(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyLocalLicensedRouteModel>)) as IFormularyRepository<FormularyLocalLicensedRouteModel>;

            var localLicensedRoutes = await repo.GetFormularyLocalLicensedRoute(formularyVersionIds);

            if (!localLicensedRoutes.IsCollectionValid()) return null;

            var localLicensedRoutesList = _mapper.Map<List<FormularyLocalLicensedRouteDTO>>(localLicensedRoutes);

            return localLicensedRoutesList.ToList();
        }

        public async Task<List<FormularyLocalUnlicensedRouteDTO>> GetLocalUnlicensedRoute(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyLocalUnlicensedRouteModel>)) as IFormularyRepository<FormularyLocalUnlicensedRouteModel>;

            var localUnlicensedRoutes = await repo.GetFormularyLocalUnlicensedRoute(formularyVersionIds);

            if (!localUnlicensedRoutes.IsCollectionValid()) return null;

            var localUnlicensedRoutesList = _mapper.Map<List<FormularyLocalUnlicensedRouteDTO>>(localUnlicensedRoutes);

            return localUnlicensedRoutesList.ToList();
        }

        public async Task<List<CustomWarningDTO>> GetCustomWarning(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<CustomWarningModel>)) as IFormularyRepository<CustomWarningModel>;

            var customWarnings = await repo.GetFormularyCustomWarning(formularyVersionIds);

            if (!customWarnings.IsCollectionValid()) return null;

            var customWarningsList = _mapper.Map<List<CustomWarningDTO>>(customWarnings);

            return customWarningsList.ToList();
        }

        public async Task<List<ReminderDTO>> GetReminder(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<ReminderModel>)) as IFormularyRepository<ReminderModel>;

            var reminders = await repo.GetFormularyReminder(formularyVersionIds);

            if (!reminders.IsCollectionValid()) return null;

            var remindersList = _mapper.Map<List<ReminderDTO>>(reminders);

            return remindersList.ToList();
        }

        public async Task<List<EndorsementDTO>> GetEndorsement(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<EndorsementModel>)) as IFormularyRepository<EndorsementModel>;

            var endorsements = await repo.GetFormularyEndorsement(formularyVersionIds);

            if (!endorsements.IsCollectionValid()) return null;

            var endorsementsList = _mapper.Map<List<EndorsementDTO>>(endorsements);

            return endorsementsList.ToList();
        }

        public async Task<List<MedusaPreparationInstructionDTO>> GetMedusaPreparationInstruction(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<MedusaPreparationInstructionModel>)) as IFormularyRepository<MedusaPreparationInstructionModel>;

            var medusapreparationinstructions = await repo.GetFormularyMedusaPreparationInstruction(formularyVersionIds);

            if (!medusapreparationinstructions.IsCollectionValid()) return null;

            var medusapreparationinstructionsList = _mapper.Map<List<MedusaPreparationInstructionDTO>>(medusapreparationinstructions);

            return medusapreparationinstructionsList.ToList();
        }

        public async Task<List<TitrationTypeDTO>> GetTitrationType(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<TitrationTypeModel>)) as IFormularyRepository<TitrationTypeModel>;

            var titrationTypes = await repo.GetFormularyTitrationType(formularyVersionIds);

            if (!titrationTypes.IsCollectionValid()) return null;

            var titrationTypesList = _mapper.Map<List<TitrationTypeDTO>>(titrationTypes);

            return titrationTypesList.ToList();
        }

        public async Task<List<RoundingFactorDTO>> GetRoundingFactor(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<RoundingFactorModel>)) as IFormularyRepository<RoundingFactorModel>;

            var roundingFactors = await repo.GetFormularyRoundingFactor(formularyVersionIds);

            if (!roundingFactors.IsCollectionValid()) return null;

            var roundingFactorsList = _mapper.Map<List<RoundingFactorDTO>>(roundingFactors);

            return roundingFactorsList.ToList();
        }

        public async Task<List<CompatibleDiluentDTO>> GetCompatibleDiluent(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<CompatibleDiluentModel>)) as IFormularyRepository<CompatibleDiluentModel>;

            var compatibleDiluents = await repo.GetFormularyCompatibleDiluent(formularyVersionIds);

            if (!compatibleDiluents.IsCollectionValid()) return null;

            var compatibleDiluentsList = _mapper.Map<List<CompatibleDiluentDTO>>(compatibleDiluents);

            return compatibleDiluentsList.ToList();
        }

        public async Task<List<ClinicalTrialMedicationDTO>> GetClinicalTrialMedication(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<ClinicalTrialMedicationModel>)) as IFormularyRepository<ClinicalTrialMedicationModel>;

            var clinicalTrialMedications = await repo.GetFormularyClinicalTrialMedication(formularyVersionIds);

            if (!clinicalTrialMedications.IsCollectionValid()) return null;

            var clinicalTrialMedicationsList = _mapper.Map<List<ClinicalTrialMedicationDTO>>(clinicalTrialMedications);

            return clinicalTrialMedicationsList.ToList();
        }

        public async Task<List<GastroResistantDTO>> GetGastroResistant(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<GastroResistantModel>)) as IFormularyRepository<GastroResistantModel>;

            var gastroResistants = await repo.GetFormularyGastroResistant(formularyVersionIds);

            if (!gastroResistants.IsCollectionValid()) return null;

            var gastroResistantsList = _mapper.Map<List<GastroResistantDTO>>(gastroResistants);

            return gastroResistantsList.ToList();
        }

        public async Task<List<CriticalDrugDTO>> GetCriticalDrug(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<CriticalDrugModel>)) as IFormularyRepository<CriticalDrugModel>;

            var criticalDrugs = await repo.GetFormularyCriticalDrug(formularyVersionIds);

            if (!criticalDrugs.IsCollectionValid()) return null;

            var criticalDrugsList = _mapper.Map<List<CriticalDrugDTO>>(criticalDrugs);

            return criticalDrugsList.ToList();
        }

        public async Task<List<ModifiedReleaseDTO>> GetModifiedRelease(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<ModifiedReleaseModel>)) as IFormularyRepository<ModifiedReleaseModel>;

            var modifiedReleases = await repo.GetFormularyModifiedRelease(formularyVersionIds);

            if (!modifiedReleases.IsCollectionValid()) return null;

            var modifiedReleasesList = _mapper.Map<List<ModifiedReleaseDTO>>(modifiedReleases);

            return modifiedReleasesList.ToList();
        }

        public async Task<List<ExpensiveMedicationDTO>> GetExpensiveMedication(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<ExpensiveMedicationModel>)) as IFormularyRepository<ExpensiveMedicationModel>;

            var expensiveMedications = await repo.GetFormularyExpensiveMedication(formularyVersionIds);

            if (!expensiveMedications.IsCollectionValid()) return null;

            var expensiveMedicationsList = _mapper.Map<List<ExpensiveMedicationDTO>>(expensiveMedications);

            return expensiveMedicationsList.ToList();
        }

        public async Task<List<HighAlertMedicationDTO>> GetHighAlertMedication(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<HighAlertMedicationModel>)) as IFormularyRepository<HighAlertMedicationModel>;

            var highAlertMedications = await repo.GetFormularyHighAlertMedication(formularyVersionIds);

            if (!highAlertMedications.IsCollectionValid()) return null;

            var highAlertMedicationsList = _mapper.Map<List<HighAlertMedicationDTO>>(highAlertMedications);

            return highAlertMedicationsList.ToList();
        }

        public async Task<List<IVToOralDTO>> GetIVToOral(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<IVToOralModel>)) as IFormularyRepository<IVToOralModel>;

            var ivToOrals = await repo.GetFormularyIVToOral(formularyVersionIds);

            if (!ivToOrals.IsCollectionValid()) return null;

            var ivToOralsList = _mapper.Map<List<IVToOralDTO>>(ivToOrals);

            return ivToOralsList.ToList();
        }

        public async Task<List<NotForPRNDTO>> GetNotForPRN(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<NotForPRNModel>)) as IFormularyRepository<NotForPRNModel>;

            var notForPRNs = await repo.GetFormularyNotForPRN(formularyVersionIds);

            if (!notForPRNs.IsCollectionValid()) return null;

            var notForPRNsList = _mapper.Map<List<NotForPRNDTO>>(notForPRNs);

            return notForPRNsList.ToList();
        }

        public async Task<List<BloodProductDTO>> GetBloodProduct(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<BloodProductModel>)) as IFormularyRepository<BloodProductModel>;

            var bloodProducts = await repo.GetFormularyBloodProduct(formularyVersionIds);

            if (!bloodProducts.IsCollectionValid()) return null;

            var bloodProductsList = _mapper.Map<List<BloodProductDTO>>(bloodProducts);

            return bloodProductsList.ToList();
        }

        public async Task<List<DiluentDTO>> GetDiluent(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<DiluentModel>)) as IFormularyRepository<DiluentModel>;

            var diluents = await repo.GetFormularyDiluent(formularyVersionIds);

            if (!diluents.IsCollectionValid()) return null;

            var diluentsList = _mapper.Map<List<DiluentDTO>>(diluents);

            return diluentsList.ToList();
        }

        public async Task<List<PrescribableDTO>> GetPrescribable(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<PrescribableModel>)) as IFormularyRepository<PrescribableModel>;

            var prescribables = await repo.GetFormularyPrescribable(formularyVersionIds);

            if (!prescribables.IsCollectionValid()) return null;

            var prescribablesList = _mapper.Map<List<PrescribableDTO>>(prescribables);

            return prescribablesList.ToList();
        }

        public async Task<List<OutpatientMedicationDTO>> GetOutpatientMedication(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<OutpatientMedicationModel>)) as IFormularyRepository<OutpatientMedicationModel>;

            var outpatientMedications = await repo.GetFormularyOutpatientMedication(formularyVersionIds);

            if (!outpatientMedications.IsCollectionValid()) return null;

            var outpatientMedicationsList = _mapper.Map<List<OutpatientMedicationDTO>>(outpatientMedications);

            return outpatientMedicationsList.ToList();
        }

        public async Task<List<IgnoreDuplicateWarningDTO>> GetIgnoreDuplicateWarning(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<IgnoreDuplicateWarningModel>)) as IFormularyRepository<IgnoreDuplicateWarningModel>;

            var ignoreDuplicateWarnings = await repo.GetFormularyIgnoreDuplicateWarning(formularyVersionIds);

            if (!ignoreDuplicateWarnings.IsCollectionValid()) return null;

            var ignoreDuplicateWarningsList = _mapper.Map<List<IgnoreDuplicateWarningDTO>>(ignoreDuplicateWarnings);

            return ignoreDuplicateWarningsList.ToList();
        }

        public async Task<List<ControlledDrugDTO>> GetControlledDrug(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<ControlledDrugModel>)) as IFormularyRepository<ControlledDrugModel>;

            var controlledDrugs = await repo.GetFormularyControlledDrug(formularyVersionIds);

            if (!controlledDrugs.IsCollectionValid()) return null;

            var controlledDrugsList = _mapper.Map<List<ControlledDrugDTO>>(controlledDrugs);

            return controlledDrugsList.ToList();
        }

        public async Task<List<PrescriptionPrintingRequiredDTO>> GetPrescriptionPrintingRequired(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<PrescriptionPrintingRequiredModel>)) as IFormularyRepository<PrescriptionPrintingRequiredModel>;

            var prescriptionPrintingRequireds = await repo.GetFormularyPrescriptionPrintingRequired(formularyVersionIds);

            if (!prescriptionPrintingRequireds.IsCollectionValid()) return null;

            var prescriptionPrintingRequiredsList = _mapper.Map<List<PrescriptionPrintingRequiredDTO>>(prescriptionPrintingRequireds);

            return prescriptionPrintingRequiredsList.ToList();
        }

        public async Task<List<IndicationMandatoryDTO>> GetIndicationMandatory(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<IndicationMandatoryModel>)) as IFormularyRepository<IndicationMandatoryModel>;

            var indicationMandatories = await repo.GetFormularyIndicationMandatory(formularyVersionIds);

            if (!indicationMandatories.IsCollectionValid()) return null;

            var indicationMandatoriesList = _mapper.Map<List<IndicationMandatoryDTO>>(indicationMandatories);

            return indicationMandatoriesList.ToList();
        }

        public async Task<List<WitnessingRequiredDTO>> GetWitnessingRequired(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<WitnessingRequiredModel>)) as IFormularyRepository<WitnessingRequiredModel>;

            var witnessingRequireds = await repo.GetFormularyWitnessingRequired(formularyVersionIds);

            if (!witnessingRequireds.IsCollectionValid()) return null;

            var witnessingRequiredsList = _mapper.Map<List<WitnessingRequiredDTO>>(witnessingRequireds);

            return witnessingRequiredsList.ToList();
        }

        public async Task<List<FormularyStatusDTO>> GetFormularyStatus(List<string> formularyVersionIds)
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyStatusModel>)) as IFormularyRepository<FormularyStatusModel>;

            var formularyStatuses = await repo.GetFormularyStatus(formularyVersionIds);

            if (!formularyStatuses.IsCollectionValid()) return null;

            var formularyStatusesList = _mapper.Map<List<FormularyStatusDTO>>(formularyStatuses);

            return formularyStatusesList.ToList();
        }
    }
}
