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
using Interneuron.Caching;
using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.SNOMED;
using Interneuron.Terminology.API.AppCode.Infrastructure.Caching;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using Interneuron.Terminology.Model.Search;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public partial class SnomedCTQueries : ISnomedCTQueries
    {
        private IServiceProvider _provider;
        private IMapper _mapper;

        public SnomedCTQueries(IServiceProvider provider, IMapper mapper)
        {
            this._provider = provider;
            this._mapper = mapper;
        }


        public SnomedSearchResultsDTO SearchSnomedTermBySemanticTag(string searchTerm, string semanticTag)
        {
            var resultsDTO = new SnomedSearchResultsDTO() { SearchText = searchTerm, SemanticTag = semanticTag, Data = new List<SnomedSearchResultDTO>() };

            if (searchTerm.IsEmpty() || semanticTag.IsEmpty()) return resultsDTO;

            searchTerm = searchTerm.Trim();

            semanticTag = semanticTag.Trim();

            var repo = this._provider.GetService(typeof(ITerminologyRepository<SNOMEDCTSearchResultModel>)) as ITerminologyRepository<SNOMEDCTSearchResultModel>;

            var resultsFromDB = repo.SearchSnomedTermBySemanticTag(searchTerm, semanticTag).ToList();

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var allRecordsDictionary = new ConcurrentDictionary<string, SnomedSearchResultDTO>();
            var searchTermMatchingRecordsDictionary = new ConcurrentDictionary<string, SnomedSearchResultDTO>();
            var searchTermExactMatchingRecordsDictionary = new ConcurrentDictionary<string, SnomedSearchResultDTO>();


            resultsFromDB.Distinct(r => r.ConceptId)
                .OrderBy(r => r.PreferredTerm)
                .AsParallel().Each(res =>
              {
                  var resDTO = _mapper.Map<SnomedSearchResultDTO>(res);

                  if (!allRecordsDictionary.ContainsKey(resDTO.Code))
                      allRecordsDictionary[resDTO.Code] = resDTO;

                  if (!searchTermExactMatchingRecordsDictionary.ContainsKey(resDTO.Code) && string.Compare(resDTO.Term, searchTerm, true) == 0)
                      searchTermExactMatchingRecordsDictionary[resDTO.Code] = resDTO;
                  else if (!searchTermMatchingRecordsDictionary.ContainsKey(resDTO.Code) && resDTO.Term.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                      searchTermMatchingRecordsDictionary[resDTO.Code] = resDTO;

              });

            if (searchTermMatchingRecordsDictionary.IsCollectionValid() || searchTermExactMatchingRecordsDictionary.IsCollectionValid())
            {
                searchTermExactMatchingRecordsDictionary?.AsParallel().Each(all =>
                {
                    resultsDTO.Data.Add(all.Value);
                });

                searchTermMatchingRecordsDictionary?.AsParallel().Each(all =>
                {
                    resultsDTO.Data.Add(all.Value);
                });

                //Get other non-matching records
                var nonMatchingRecs = allRecordsDictionary
                    .Where(all => (!searchTermMatchingRecordsDictionary.IsCollectionValid() || (searchTermMatchingRecordsDictionary.IsCollectionValid() && !searchTermMatchingRecordsDictionary.ContainsKey(all.Key))) && (!searchTermExactMatchingRecordsDictionary.IsCollectionValid() ||(searchTermExactMatchingRecordsDictionary.IsCollectionValid() && !searchTermExactMatchingRecordsDictionary.ContainsKey(all.Key))))
                    .Select(all => all.Value);

                if (nonMatchingRecs.IsCollectionValid())
                {
                    var orderedNonMatchingRecs = nonMatchingRecs.OrderBy(t => t.Term);
                    resultsDTO.Data.AddRange(orderedNonMatchingRecs);
                }
            }
            else
            {
                allRecordsDictionary.Each(all =>
                {
                    resultsDTO.Data.Add(all.Value);
                });
            }

            return resultsDTO;
        }

        public async Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedTermsGetWithAllNodes(string searchTerm, string semanticTag)
        {
            var resultsDTO = new SnomedSearchResultsWithHierarchyDTO() { SearchText = searchTerm, SemanticTag = semanticTag, Data = new List<SnomedSearchResultWithTreeDTO>() };

            if (searchTerm.IsEmpty() || semanticTag.IsEmpty()) return resultsDTO;

            searchTerm = searchTerm.Trim();
            semanticTag = semanticTag.Trim();

            var repoAncestor = this._provider.GetService(typeof(ITerminologyRepository<SNOMEDCTSearchResultModel>)) as ITerminologyRepository<SNOMEDCTSearchResultModel>;
            var repoChild = this._provider.GetService(typeof(ITerminologyRepository<SNOMEDCTSearchResultModel>)) as ITerminologyRepository<SNOMEDCTSearchResultModel>;

            var dbTasks = new List<Task<IEnumerable<SNOMEDCTSearchResultModel>>>();

            var ancestorDbTask = repoAncestor.SearchSnomedTermsGetWithAllAncestors(searchTerm, semanticTag);

            var childrenDbTask = repoChild.SearchSnomedTermsGetWithAllDescendents(searchTerm, semanticTag);

            dbTasks.Add(ancestorDbTask);

            dbTasks.Add(childrenDbTask);

            var dbData = await Task.WhenAll(dbTasks);

            if (!dbData.IsCollectionValid()) return resultsDTO;

            var ancestorResultsFromDB = dbData[0];
            var childResultsFromDB = dbData[1];

            if (!ancestorResultsFromDB.IsCollectionValid()) return resultsDTO;
            if (!childResultsFromDB.IsCollectionValid()) return resultsDTO;

            var ancestorResultsFromDBList = ancestorResultsFromDB.ToList();
            var childResultsFromDBList = childResultsFromDB.ToList();

            var nodeWithAncestorNodes = BuildAncestorLookup(ancestorResultsFromDBList);
            var nodeWithChildrenNodes = BuildChildNodesLookup(childResultsFromDBList);

            var resDTOTasks = new List<Task<SnomedSearchResultWithTreeDTO>>();

            //Can be results from ancestor or child query --both should have the same matches at-least
            ancestorResultsFromDBList.Where(r => r.Level == 1).Distinct(r => r.ConceptId)
                .AsParallel()
                .Each((res) =>
            {
                resDTOTasks.Add(GetNodeWithChildrenAndAncestor(res, nodeWithAncestorNodes, nodeWithChildrenNodes));
                //resultsDTO.Data.Add(resDTO);
            });
            var resDTOs = await Task.WhenAll(resDTOTasks);
            resultsDTO.Data.AddRange(resDTOs.ToList());

            if (resultsDTO.Data.IsCollectionValid())
            {
                var orderedData = resultsDTO.Data.OrderBy(r => r.Term).ToList();
                resultsDTO.Data = orderedData;
            }

            return resultsDTO;
        }

        public async Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedTermsGetWithAllAncestors(string searchTerm, string semanticTag)
        {
            var resultsDTO = new SnomedSearchResultsWithHierarchyDTO() { SearchText = searchTerm, SemanticTag = semanticTag, Data = new List<SnomedSearchResultWithTreeDTO>() };

            if (searchTerm.IsEmpty() || semanticTag.IsEmpty()) return resultsDTO;

            searchTerm = searchTerm.Trim();
            semanticTag = semanticTag.Trim();

            var repo = this._provider.GetService(typeof(ITerminologyRepository<SNOMEDCTSearchResultModel>)) as ITerminologyRepository<SNOMEDCTSearchResultModel>;

            var resultsFromDB = await repo.SearchSnomedTermsGetWithAllAncestors(searchTerm, semanticTag);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var resultsFromDBList = resultsFromDB.ToList();

            var childWithAncestorNodes = BuildAncestorLookup(resultsFromDBList);

            resultsFromDBList.Where(r => r.Level == 1).Distinct(r => r.ConceptId).AsParallel().Each(res =>
            {
                var resDTO = GetAncestorNode(res, childWithAncestorNodes);
                resultsDTO.Data.Add(resDTO);
            });

            if (resultsDTO.Data.IsCollectionValid())
            {
                var orderedData = resultsDTO.Data.OrderBy(r => r.Term).ToList();
                resultsDTO.Data = orderedData;
            }

            return resultsDTO;
        }

        public async Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedTermsGetWithAllDescendents(string searchTerm, string semanticTag)
        {
            var resultsDTO = new SnomedSearchResultsWithHierarchyDTO() { SearchText = searchTerm, SemanticTag = semanticTag, Data = new List<SnomedSearchResultWithTreeDTO>() };

            if (searchTerm.IsEmpty() || semanticTag.IsEmpty()) return resultsDTO;

            searchTerm = searchTerm.Trim();
            semanticTag = semanticTag.Trim();

            var repo = this._provider.GetService(typeof(ITerminologyRepository<SNOMEDCTSearchResultModel>)) as ITerminologyRepository<SNOMEDCTSearchResultModel>;

            var resultsFromDB = await repo.SearchSnomedTermsGetWithAllDescendents(searchTerm, semanticTag);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var resultsFromDBList = resultsFromDB.ToList();

            var parentWithChildNodes = BuildChildNodesLookup(resultsFromDBList);

            resultsFromDBList.Where(r => r.Level == 1).Distinct(r => r.ConceptId).AsParallel().Each(res =>
            {
                var resDTO = GetChildNode(res, parentWithChildNodes);

                resultsDTO.Data.Add(resDTO);
            });

            if (resultsDTO.Data.IsCollectionValid())
            {
                var orderedData = resultsDTO.Data.OrderBy(r => r.Term).ToList();
                resultsDTO.Data = orderedData;
            }

            return resultsDTO;
        }

        public async Task<List<SnomedSearchResultWithTreeDTO>> GetSnomedAncestorForConceptIds(string[] conceptIds)
        {
            var resultsDTO = new List<SnomedSearchResultWithTreeDTO>();

            if (!conceptIds.IsCollectionValid()) return resultsDTO;

            var repo = this._provider.GetService(typeof(ITerminologyRepository<SNOMEDCTSearchResultModel>)) as ITerminologyRepository<SNOMEDCTSearchResultModel>;

            var conceptIdsParam = conceptIds.Select(c => c.Trim()).ToArray();

            var resultsFromDB = await repo.GetSnomedAncestorForConceptIds(conceptIdsParam);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var resultsFromDBList = resultsFromDB.ToList();

            var childWithAncestorNodes = BuildAncestorLookup(resultsFromDBList);

            resultsFromDBList.Where(r => r.Level == 1).Distinct(r => r.ConceptId).AsParallel().Each(res =>
            {
                var resDTO = GetAncestorNode(res, childWithAncestorNodes);
                resultsDTO.Add(resDTO);
            });

            if (resultsDTO.IsCollectionValid())
            {
                var orderedData = resultsDTO.OrderBy(r => r.Term).ToList();
                resultsDTO = orderedData;
            }

            return resultsDTO;
        }

        public async Task<List<SnomedSearchResultWithTreeDTO>> GetSnomedDescendentForConceptIds(string[] conceptIds)
        {
            var resultsDTO = new List<SnomedSearchResultWithTreeDTO>();

            if (!conceptIds.IsCollectionValid()) return resultsDTO;

            var repo = this._provider.GetService(typeof(ITerminologyRepository<SNOMEDCTSearchResultModel>)) as ITerminologyRepository<SNOMEDCTSearchResultModel>;

            var conceptIdsParam = conceptIds.Select(c => c.Trim()).ToArray();

            var resultsFromDB = await repo.GetSnomedDescendentForConceptIds(conceptIdsParam);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var resultsFromDBList = resultsFromDB.ToList();

            var parentWithChildNodes = BuildChildNodesLookup(resultsFromDBList);

            resultsFromDBList.Where(r => r.Level == 1).Distinct(r => r.ConceptId).AsParallel().Each(res =>
            {
                var resDTO = GetChildNode(res, parentWithChildNodes);

                resultsDTO.Add(resDTO);
            });

            if (resultsDTO.IsCollectionValid())
            {
                var orderedData = resultsDTO.OrderBy(r => r.Term).ToList();
                resultsDTO = orderedData;
            }

            return resultsDTO;
        }

        public List<SnomedTradeFamiliesDTO> GetTradeFamilyForConceptIds(List<string> conceptIds)
        {
            var resultsDTO = new List<SnomedTradeFamiliesDTO>();

            resultsDTO = CacheService.Get<List<SnomedTradeFamiliesDTO>>(CacheKeys.SNOMED_TRADE_FAMILIES);

            if (!resultsDTO.IsCollectionValid())
            {
                var repo = this._provider.GetService(typeof(ITerminologyRepository<SnomedctTradefamiliesMat>)) as ITerminologyRepository<SnomedctTradefamiliesMat>;

                var resultsFromDB = repo.ItemsAsListReadOnly.ToList();

                if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

                resultsDTO = _mapper.Map<List<SnomedTradeFamiliesDTO>>(resultsFromDB);

                CacheService.Set(CacheKeys.SNOMED_TRADE_FAMILIES, resultsDTO);
            }

            if (conceptIds.IsCollectionValid())
                resultsDTO = resultsDTO.Where(rec => conceptIds.Contains(rec.BrandedDrugId)).ToList();

            return resultsDTO;
        }


        public List<SnomedModifiedReleaseDTO> GetModifiedReleaseForConceptIds(List<string> conceptIds)
        {
            var resultsDTO = new List<SnomedModifiedReleaseDTO>();

            resultsDTO = CacheService.Get<List<SnomedModifiedReleaseDTO>>(CacheKeys.SNOMED_MODIFIED_RELEASE);

            if (!resultsDTO.IsCollectionValid())
            {
                var repo = this._provider.GetService(typeof(ITerminologyRepository<SnomedctModifiedReleaseMat>)) as ITerminologyRepository<SnomedctModifiedReleaseMat>;

                var resultsFromDB = repo.ItemsAsListReadOnly.ToList();

                if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

                resultsDTO = _mapper.Map<List<SnomedModifiedReleaseDTO>>(resultsFromDB);

                CacheService.Set(CacheKeys.SNOMED_MODIFIED_RELEASE, resultsDTO);
            }

            if (conceptIds.IsCollectionValid())
                resultsDTO = resultsDTO.Where(rec => conceptIds.Contains(rec.DrugId)).ToList();

            return resultsDTO;
        }

        private async Task<SnomedSearchResultWithTreeDTO> GetNodeWithChildrenAndAncestor(SNOMEDCTSearchResultModel res, ConcurrentDictionary<string, List<SNOMEDCTSearchResultModel>> nodeWithAncestorNodes, ConcurrentDictionary<string, List<SNOMEDCTSearchResultModel>> nodeWithChildrenNodes)
        {
            var resDTO = _mapper.Map<SnomedSearchResultWithTreeDTO>(res);

            var nodeTasks = new List<Task<List<SnomedSearchResultWithTreeDTO>>>();
            //GetAncestor Records -- IN Next level
            var ancestorNodesTask = new TaskFactory().StartNew(() =>
            {
                return GetAncestors(nodeWithAncestorNodes, res.ParentId, res.Level.GetValueOrDefault() + 1);//level = 2 -- next level
            });

            //GetChildren Records -- IN Next level
            var chidrenNodesTask = new TaskFactory().StartNew(() =>
            {
                return GetChildren(nodeWithChildrenNodes, res.ConceptId, res.Level.GetValueOrDefault() + 1);//level = 2 -- next level
            });

            nodeTasks.Add(ancestorNodesTask);
            nodeTasks.Add(chidrenNodesTask);

            var nodesData = await Task.WhenAll(nodeTasks);

            var ancestorNodes = nodesData[0];
            var chidrenNodes = nodesData[1];

            resDTO.Parents = new List<SnomedSearchResultWithTreeDTO>();

            if (ancestorNodes.IsCollectionValid())
                resDTO.Parents.AddRange(ancestorNodes);


            resDTO.Children = new List<SnomedSearchResultWithTreeDTO>();

            if (chidrenNodes.IsCollectionValid())
                resDTO.Children.AddRange(chidrenNodes);

            return resDTO;
        }

        private SnomedSearchResultWithTreeDTO GetChildNode(SNOMEDCTSearchResultModel res, ConcurrentDictionary<string, List<SNOMEDCTSearchResultModel>> parentWithChildNodes)
        {
            var resDTO = _mapper.Map<SnomedSearchResultWithTreeDTO>(res);

            //GetChildren Records -- IN Next level
            var chidrenNodes = GetChildren(parentWithChildNodes, res.ConceptId, res.Level.GetValueOrDefault() + 1);//level = 2 -- next level

            resDTO.Children = new List<SnomedSearchResultWithTreeDTO>();

            if (chidrenNodes.IsCollectionValid())
                resDTO.Children.AddRange(chidrenNodes);

            return resDTO;
        }

        private ConcurrentDictionary<string, List<SNOMEDCTSearchResultModel>> BuildChildNodesLookup(List<SNOMEDCTSearchResultModel> resultsFromDB)
        {
            var parentWithChildNodes = new ConcurrentDictionary<string, List<SNOMEDCTSearchResultModel>>();

            var onlyChildRecords = resultsFromDB.Where(r => r.Level != 1);// resultsFromDB.Where(r => r.ParentId != null);//first level will not have parentid returned from fn

            if (onlyChildRecords.IsCollectionValid())
            {
                onlyChildRecords.AsParallel().Each(r =>
                {
                    var key = r.ParentId;// $"{r.ParentId }|{r.Level}";

                    if (parentWithChildNodes.ContainsKey(key))
                    {
                        parentWithChildNodes[key].Add(r);
                    }
                    else
                    {
                        parentWithChildNodes[key] = new List<SNOMEDCTSearchResultModel>() { r };
                    }
                });
            }

            return parentWithChildNodes;
        }

        private SnomedSearchResultWithTreeDTO GetAncestorNode(SNOMEDCTSearchResultModel res, ConcurrentDictionary<string, List<SNOMEDCTSearchResultModel>> childWithAncestorNodes)
        {
            var resDTO = _mapper.Map<SnomedSearchResultWithTreeDTO>(res);

            //GetAncestor Records -- IN Next level
            var ancestorNodes = GetAncestors(childWithAncestorNodes, res.ParentId, res.Level.GetValueOrDefault() + 1);//level = 2 -- next level

            resDTO.Parents = new List<SnomedSearchResultWithTreeDTO>();

            if (ancestorNodes.IsCollectionValid())
                resDTO.Parents.AddRange(ancestorNodes);

            return resDTO;
        }

        private ConcurrentDictionary<string, List<SNOMEDCTSearchResultModel>> BuildAncestorLookup(List<SNOMEDCTSearchResultModel> resultsFromDB)
        {
            var childWithAncestorNodes = new ConcurrentDictionary<string, List<SNOMEDCTSearchResultModel>>();

            var onlyParentRecords = resultsFromDB.Where(r => r.Level != 1);//first level will not have parentid returned from fn

            if (onlyParentRecords.IsCollectionValid())
            {
                onlyParentRecords.AsParallel().Each(r =>
                {
                    if (childWithAncestorNodes.ContainsKey(r.ConceptId))
                    {
                        childWithAncestorNodes[r.ConceptId].Add(r);
                    }
                    else
                    {
                        childWithAncestorNodes[r.ConceptId] = new List<SNOMEDCTSearchResultModel>() { r };
                    }
                });
            }

            return childWithAncestorNodes;
        }

        private List<SnomedSearchResultWithTreeDTO> GetAncestors(ConcurrentDictionary<string, List<SNOMEDCTSearchResultModel>> childWithAncestorNodes, string parentId, int level)
        {
            List<SnomedSearchResultWithTreeDTO> parentRecords = new List<SnomedSearchResultWithTreeDTO>();

            if (!childWithAncestorNodes.IsCollectionValid() || !childWithAncestorNodes.ContainsKey(parentId)) return parentRecords;

            var parentNodes = childWithAncestorNodes[parentId];// resultsFromDB.Where(r => r.Level == level && r.ParentId == conceptId);

            if (!parentNodes.IsCollectionValid()) return new List<SnomedSearchResultWithTreeDTO>();

            parentNodes.Distinct(r => r.ConceptId).AsParallel().Each(res =>
            {
                var resDTO = _mapper.Map<SnomedSearchResultWithTreeDTO>(res);

                resDTO.Level = level;

                //GetAncestors Records -- IN Next level
                var ancestorNodes = GetAncestors(childWithAncestorNodes, res.ParentId, level + 1);

                resDTO.Parents = new List<SnomedSearchResultWithTreeDTO>();

                if (ancestorNodes.IsCollectionValid())
                    resDTO.Parents.AddRange(ancestorNodes);

                parentRecords.Add(resDTO);

            });

            return parentRecords;
        }


        private List<SnomedSearchResultWithTreeDTO> GetChildren(ConcurrentDictionary<string, List<SNOMEDCTSearchResultModel>> parentWithChildNodes, string conceptId, int level)
        {
            List<SnomedSearchResultWithTreeDTO> childRecords = new List<SnomedSearchResultWithTreeDTO>();

            var key = conceptId;// $"{conceptId }|{level}";//not considering the level - child can be in any level

            if (!parentWithChildNodes.IsCollectionValid() || !parentWithChildNodes.ContainsKey(key)) return childRecords;

            var childNodes = parentWithChildNodes[key];// resultsFromDB.Where(r => r.Level == level && r.ParentId == conceptId);

            if (!childNodes.IsCollectionValid()) return new List<SnomedSearchResultWithTreeDTO>();

            childNodes.Distinct(r => r.ConceptId).AsParallel().Each(res =>
            {
                var resDTO = _mapper.Map<SnomedSearchResultWithTreeDTO>(res);

                resDTO.Level = level;

                //GetChildren Records -- IN Next level
                var chidrenNodes = GetChildren(parentWithChildNodes, res.ConceptId, level + 1);//Not considering the level

                resDTO.Children = new List<SnomedSearchResultWithTreeDTO>();

                if (chidrenNodes.IsCollectionValid())
                    resDTO.Children.AddRange(chidrenNodes);

                childRecords.Add(resDTO);

            });

            return childRecords;
        }
    }
}
