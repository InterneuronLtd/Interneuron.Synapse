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
using Interneuron.Terminology.API.AppCode.DTOs.DMD;
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
    public partial class DMDQueries : IDMDQueries
    {
        private IServiceProvider _provider;
        private IMapper _mapper;

        public DMDQueries(IServiceProvider provider, IMapper mapper)
        {
            this._provider = provider;
            this._mapper = mapper;
        }

        public async Task<DMDSearchResultsDTO> SearchDMD(string term)
        {
            var resultsDTO = new DMDSearchResultsDTO() { SearchText = term, Data = new List<DMDSearchResultDTO>() };

            var repo = this._provider.GetService(typeof(ITerminologyRepository<DMDSearchResultModel>)) as ITerminologyRepository<DMDSearchResultModel>;

            var resultsFromDB = repo.SearchDMDName(term).ToList();

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            resultsFromDB.Distinct(r => r.Code).Each(res =>
            {
                var resDTO = _mapper.Map<DMDSearchResultDTO>(res);

                if (res.ControlDrugCategory != null)
                    resDTO.ControlDrugCategory = _mapper.Map<DmdLookupControldrugcatDTO>(res.ControlDrugCategory);
                if (res.PrescribingStatus != null)
                    resDTO.PrescribingStatus = _mapper.Map<DmdLookupPrescribingstatusDTO>(res.PrescribingStatus);
                if (res.Routes.IsCollectionValid())
                    resDTO.Routes = _mapper.Map<List<DmdLookupRouteDTO>>(res.Routes);
                if (res.Form != null)
                    resDTO.Form = _mapper.Map<DmdLookupFormDTO>(res.Form);

                resultsDTO.Data.Add(resDTO);
            });

            return resultsDTO;
        }

        public async Task<DMDSearchResultsWithHierarchyDTO> SearchDMDNamesGetWithAllLevelNodes(string searchTerm)
        {
            var resultsDTO = new DMDSearchResultsWithHierarchyDTO() { SearchText = searchTerm, Data = new List<DMDSearchResultWithTreeDTO>() };

            if (searchTerm.IsEmpty()) return resultsDTO;

            var repo = this._provider.GetService(typeof(ITerminologyRepository<DMDSearchResultModel>)) as ITerminologyRepository<DMDSearchResultModel>;

            searchTerm = searchTerm.Trim();

            var resultsFromDB = await repo.SearchDMDGetWithAllDescendents(searchTerm);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var resultsFromDBList = resultsFromDB.ToList();

            var ancestorList = await GetMissingAncestors(resultsFromDBList);

            if (ancestorList.IsCollectionValid())
                resultsFromDBList.AddRange(ancestorList);

            var parentWithChildNodes = BuildChildNodesLookup(resultsFromDBList);

            //This comprises both logical and physical root nodes
            //Logical Root nodes: Records which will have the parentcode but not have that parentcode in the records fetched from db
            var rootNodes = GetRootFromChildLevelNodes(resultsFromDBList, parentWithChildNodes);

            //resultsFromDBList.OrderByDescending(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            rootNodes.OrderBy(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            {
                var resDTO = GetNodeDetail(res, parentWithChildNodes);

                resultsDTO.Data.Add(resDTO);
            });

            return resultsDTO;
        }

        public async Task<DMDSearchResultsWithHierarchyDTO> SearchDMDSyncLog(string searchTerm)
        {
            var resultsDTO = new DMDSearchResultsWithHierarchyDTO() { SearchText = searchTerm, Data = new List<DMDSearchResultWithTreeDTO>() };

            var syncLogRepo = this._provider.GetService(typeof(ITerminologyRepository<DmdSyncLog>)) as ITerminologyRepository<DmdSyncLog>;

            var pendingRecs = syncLogRepo.ItemsAsReadOnly
                .Where(rec => rec.IsFormularyUpdated == false || rec.IsFormularyUpdated == null)?
                .OrderBy(rec => rec.SerialNum).ToList();

            if (!pendingRecs.IsCollectionValid()) return resultsDTO;

            List<DmdNamesLookupAllMat> dmdNames = null;


            var pendingCodes = pendingRecs.Select(rec => rec.DmdId).ToList();

            var dmdNamesRepo = this._provider.GetService(typeof(ITerminologyRepository<DmdNamesLookupAllMat>)) as ITerminologyRepository<DmdNamesLookupAllMat>;

            dmdNames = dmdNamesRepo.ItemsAsReadOnly.Where(rec => pendingCodes.Contains(rec.Code)).ToList();

            if (searchTerm.IsNotEmpty())
            {
                dmdNames = dmdNames.Where(rec => rec.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!dmdNames.IsCollectionValid()) return resultsDTO;

            var dmdUniqueCodes = dmdNames.Select(rec => rec.Code).ToArray();

            var repo = this._provider.GetService(typeof(ITerminologyRepository<DMDSearchResultModel>)) as ITerminologyRepository<DMDSearchResultModel>;

            var resultsFromDBList = new List<DMDSearchResultModel>();

            var descendentsFromDB = await repo.GetDMDAllDescendentsForCodes(dmdUniqueCodes);
            if (descendentsFromDB.IsCollectionValid()) resultsFromDBList.AddRange(descendentsFromDB);

            var ancestorsFromDB = await repo.GetDMDAllAncestorsForCodes(dmdUniqueCodes);
            if (ancestorsFromDB.IsCollectionValid()) resultsFromDBList.AddRange(ancestorsFromDB);

            var parentWithChildNodes = BuildChildNodesLookup(resultsFromDBList);

            //This comprises both logical and physical root nodes
            //Logical Root nodes: Records which will have the parentcode but not have that parentcode in the records fetched from db
            var rootNodes = GetRootFromChildLevelNodes(resultsFromDBList, parentWithChildNodes);

            //resultsFromDBList.OrderByDescending(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            rootNodes.OrderBy(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            {
                var resDTO = GetNodeDetail(res, parentWithChildNodes);

                resultsDTO.Data.Add(resDTO);
            });

            return resultsDTO;



            /*

            var repo = this._provider.GetService(typeof(ITerminologyRepository<DMDSearchResultModel>)) as ITerminologyRepository<DMDSearchResultModel>;

            searchTerm = searchTerm.Trim();

            var resultsFromDB = await repo.SearchDMDGetWithAllDescendents(searchTerm);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var resultsFromDBList = resultsFromDB.ToList();

            var ancestorList = await GetMissingAncestors(resultsFromDBList);

            if (ancestorList.IsCollectionValid())
                resultsFromDBList.AddRange(ancestorList);

            var parentWithChildNodes = BuildChildNodesLookup(resultsFromDBList);

            //This comprises both logical and physical root nodes
            //Logical Root nodes: Records which will have the parentcode but not have that parentcode in the records fetched from db
            var rootNodes = GetRootFromChildLevelNodes(resultsFromDBList, parentWithChildNodes);

            //resultsFromDBList.OrderByDescending(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            rootNodes.OrderBy(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            {
                var resDTO = GetNodeDetail(res, parentWithChildNodes);

                resultsDTO.Data.Add(resDTO);
            });

            return resultsDTO;
            */
        }

        private async Task<List<DMDSearchResultModel>> GetMissingAncestors(List<DMDSearchResultModel> results)
        {
            var uniqueParentCodes = results.Select(rec => rec.ParentCode)?.Distinct().ToHashSet();
            var uniqueCodes = results.Select(rec => rec.Code)?.Distinct().ToHashSet();

            var repo = this._provider.GetService(typeof(ITerminologyRepository<DMDSearchResultModel>)) as ITerminologyRepository<DMDSearchResultModel>;

            //Get the parent codes for which the record does not already exists in the results list
            var parentCodesWithoutRecord = new ConcurrentBag<string>();
            uniqueParentCodes.AsParallel().Each(rec =>
            {
                if (!uniqueCodes.Contains(rec)) parentCodesWithoutRecord.Add(rec);
            });

            if (!parentCodesWithoutRecord.IsCollectionValid()) return null;

            //Bring the parent or all ancestors for these
            var ancestors = await repo.GetDMDAncestorForCodes(parentCodesWithoutRecord.ToArray());

            if (!ancestors.IsCollectionValid()) return null;

            return ancestors.ToList();
        }

        public async Task<DMDSearchResultsWithHierarchyDTO> SearchDMDNamesGetWithAllDescendents(string searchTerm)
        {
            var resultsDTO = new DMDSearchResultsWithHierarchyDTO() { SearchText = searchTerm, Data = new List<DMDSearchResultWithTreeDTO>() };

            if (searchTerm.IsEmpty()) return resultsDTO;

            var repo = this._provider.GetService(typeof(ITerminologyRepository<DMDSearchResultModel>)) as ITerminologyRepository<DMDSearchResultModel>;

            searchTerm = searchTerm.Trim();

            var resultsFromDB = await repo.SearchDMDGetWithAllDescendents(searchTerm);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var resultsFromDBList = resultsFromDB.ToList();

            var parentWithChildNodes = BuildChildNodesLookup(resultsFromDBList);

            //This comprises both logical and physical root nodes
            //Logical Root nodes: Records which will have the parentcode but not have that parentcode in the records fetched from db
            var rootNodes = GetRootFromChildLevelNodes(resultsFromDBList, parentWithChildNodes);

            //resultsFromDBList.OrderByDescending(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            rootNodes.OrderBy(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            {
                var resDTO = GetNodeDetail(res, parentWithChildNodes);

                resultsDTO.Data.Add(resDTO);
            });

            return resultsDTO;
        }

        public async Task<List<DMDSearchResultWithTreeDTO>> GetDMDDescendentForCodes(string[] codes)
        {
            var resultsDTO = new List<DMDSearchResultWithTreeDTO>();

            if (!codes.IsCollectionValid()) return resultsDTO;

            var repo = this._provider.GetService(typeof(ITerminologyRepository<DMDSearchResultModel>)) as ITerminologyRepository<DMDSearchResultModel>;

            var codesParam = codes.Select(c => c.Trim()).ToArray();

            var resultsFromDB = await repo.GetDMDDescendentForCodes(codesParam);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var resultsFromDBList = resultsFromDB.ToList();

            var parentWithChildNodes = BuildChildNodesLookup(resultsFromDBList);

            //This comprises both logical and physical root nodes
            //Logical Root nodes: Records which will have the parentcode but not have that parentcode in the records fetched from db
            var rootNodes = GetRootFromChildLevelNodes(resultsFromDBList, parentWithChildNodes);

            rootNodes.OrderBy(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            {
                var resDTO = GetNodeDetail(res, parentWithChildNodes);

                resultsDTO.Add(resDTO);
            });

            return resultsDTO;
        }

        public async Task<List<DMDSearchResultWithTreeDTO>> GetDMDAncestorForCodes(string[] codes)
        {
            var resultsDTO = new List<DMDSearchResultWithTreeDTO>();

            if (!codes.IsCollectionValid()) return resultsDTO;

            var repo = this._provider.GetService(typeof(ITerminologyRepository<DMDSearchResultModel>)) as ITerminologyRepository<DMDSearchResultModel>;

            var codesParam = codes.Select(c => c.Trim()).ToArray();

            var resultsFromDB = await repo.GetDMDAncestorForCodes(codesParam);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var resultsFromDBList = resultsFromDB.ToList();

            var nodesLookup = BuildAllNodesLookupWithCodeAsKey(resultsFromDBList);

            codesParam.Each(c =>
            {
                if (nodesLookup.ContainsKey(c))
                {
                    nodesLookup[c].AsParallel().Each(res =>
                    {
                        var resDTO = this._mapper.Map<DMDSearchResultWithTreeDTO>(res);

                        if (res.ControlDrugCategory != null)
                            resDTO.ControlDrugCategory = _mapper.Map<DmdLookupControldrugcatDTO>(res.ControlDrugCategory);
                        if (res.PrescribingStatus != null)
                            resDTO.PrescribingStatus = _mapper.Map<DmdLookupPrescribingstatusDTO>(res.PrescribingStatus);
                        if (res.Routes.IsCollectionValid())
                            resDTO.Routes = _mapper.Map<List<DmdLookupRouteDTO>>(res.Routes);
                        if (res.Form != null)
                            resDTO.Form = _mapper.Map<DmdLookupFormDTO>(res.Form);


                        //GetAncestor Records -- IN Next level
                        var ancestorNodes = GetAncestor(nodesLookup, res.ParentCode, res.LogicalLevel.GetValueOrDefault() + 1);//level = 2 -- next level

                        resDTO.Parents = new List<DMDSearchResultWithTreeDTO>();

                        if (ancestorNodes.IsCollectionValid())
                            resDTO.Parents.AddRange(ancestorNodes);

                        resultsDTO.Add(resDTO);
                    });
                }
            });

            return resultsDTO;
        }

        public async Task<DMDSearchResultsWithHierarchyDTO> SearchDMDNamesGetWithTopNodes(string searchTerm)
        {
            var resultsDTO = new DMDSearchResultsWithHierarchyDTO() { SearchText = searchTerm, Data = new List<DMDSearchResultWithTreeDTO>() };

            if (searchTerm.IsEmpty()) return resultsDTO;

            var repo = this._provider.GetService(typeof(ITerminologyRepository<DMDSearchResultModel>)) as ITerminologyRepository<DMDSearchResultModel>;

            searchTerm = searchTerm.Trim();

            var resultsFromDB = await repo.SearchDMDGetWithAllAncestors(searchTerm);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            var resultsFromDBList = resultsFromDB.ToList();

            var parentWithChildNodes = BuildChildNodesLookup(resultsFromDBList);

            //This comprises of physical root nodes i.e. logicallevel = 1, since the ancestors are fetched
            //var rootNodes = resultsFromDBList.Where(r => r.LogicalLevel == 1).OrderBy(r => r.LogicalLevel).Distinct(r => r.Code);
            var rootNodes = resultsFromDBList
                .Where(r => r.LogicalLevel == 1 || string.IsNullOrEmpty(r.ParentCode))
                .OrderBy(r => r.LogicalLevel).Distinct(r => r.Code);

            //resultsFromDBList.OrderByDescending(r => r.LogicalLevel).Distinct(r => r.Code).AsParallel().Each(res =>
            rootNodes.AsParallel().Each(res =>
            {
                var resDTO = GetNodeDetail(res, parentWithChildNodes);

                resultsDTO.Data.Add(resDTO);
            });

            return resultsDTO;
        }

        public async Task<List<T>> GetLookup<T>(LookupType lookupType) where T : ILookupItemDTO
        {
            var lookupFactory = this._provider.GetService(typeof(LookupFactory)) as LookupFactory;

            var results = await lookupFactory.GetLookupDTO<T>(lookupType);

            if (!results.IsCollectionValid()) return results;

            //return results.Where(res => res.Recordstatus == 1).ToList(); //not too many records in lookup -- otherwise  to be moved to db query
            return results;
        }

        public async Task<List<DMDDetailResultDTO>> GetDMDFullDataForCodes(string[] codes)
        {
            var resultsDTO = new List<DMDDetailResultDTO>();

            if (!codes.IsCollectionValid()) return resultsDTO;

            var repo = this._provider.GetService(typeof(ITerminologyRepository<DMDDetailResultModel>)) as ITerminologyRepository<DMDDetailResultModel>;

            var resultsFromDB = await repo.GetDMDFullDataForCodes(codes);

            if (!resultsFromDB.IsCollectionValid()) return resultsDTO;

            resultsFromDB.Distinct(r => r.Code).Each(res =>
            {
                var resDTO = _mapper.Map<DMDDetailResultDTO>(res);

                if (res.OntologyFormRoutes != null)
                    resDTO.OntologyFormRoutes = _mapper.Map<List<DmdLookupOntformrouteDTO>>(res.OntologyFormRoutes);

                if (res.AvailableRestriction != null)
                    resDTO.AvailableRestriction = _mapper.Map<DmdLookupAvailrestrictDTO>(res.AvailableRestriction);

                if (res.BasisOfName != null)
                    resDTO.BasisOfName = _mapper.Map<DmdLookupBasisofnameDTO>(res.BasisOfName);

                if (res.DoseForm != null)
                    resDTO.DoseForm = _mapper.Map<DmdLookupDrugformindDTO>(res.DoseForm);

                if (res.LicensingAuthority != null)
                    resDTO.LicensingAuthority = _mapper.Map<DmdLookupLicauthDTO>(res.LicensingAuthority);

                if (res.UnitDoseFormSizeUOM != null)
                    resDTO.UnitDoseFormSizeUOM = _mapper.Map<DmdLookupUomDTO>(res.UnitDoseFormSizeUOM);

                if (res.UnitDoseUOM != null)
                    resDTO.UnitDoseUOM = _mapper.Map<DmdLookupUomDTO>(res.UnitDoseUOM);

                if (res.ControlDrugCategory != null)
                    resDTO.ControlDrugCategory = _mapper.Map<DmdLookupControldrugcatDTO>(res.ControlDrugCategory);
                if (res.PrescribingStatus != null)
                    resDTO.PrescribingStatus = _mapper.Map<DmdLookupPrescribingstatusDTO>(res.PrescribingStatus);
                if (res.Routes.IsCollectionValid())
                    resDTO.Routes = _mapper.Map<List<DmdLookupRouteDTO>>(res.Routes);
                if (res.Form != null)
                    resDTO.Form = _mapper.Map<DmdLookupFormDTO>(res.Form);
                if (res.VMPIngredients.IsCollectionValid())
                    resDTO.VMPIngredients = _mapper.Map<List<DmdVmpIngredientDTO>>(res.VMPIngredients);

                resultsDTO.Add(resDTO);
            });

            return resultsDTO;

        }

        public List<string> GetAllDMDCodes()
        {
            var codes = new List<string>();

            var vtmRepo = this._provider.GetService(typeof(ITerminologyRepository<DmdVtm>)) as ITerminologyRepository<DmdVtm>;

            var vtms = vtmRepo.ItemsAsReadOnly.Where(rec => rec.Invalid.HasValue == false || (rec.Invalid.HasValue && rec.Invalid.Value != 1))
                 .Select(rec => rec.Vtmid1).ToList();

            if (vtms.IsCollectionValid())
            {
                codes.AddRange(vtms);
            }

            var vmpRepo = this._provider.GetService(typeof(ITerminologyRepository<DmdVmp>)) as ITerminologyRepository<DmdVmp>;

            var vmps = vmpRepo.ItemsAsReadOnly.Where(rec => rec.Invalid.HasValue == false || (rec.Invalid.HasValue && rec.Invalid.Value != 1))
                 .Select(rec => rec.Vpid).ToList();

            if (vmps.IsCollectionValid())
            {
                codes.AddRange(vmps);
            }

            var ampRepo = this._provider.GetService(typeof(ITerminologyRepository<DmdAmp>)) as ITerminologyRepository<DmdAmp>;

            var amps = ampRepo.ItemsAsReadOnly.Where(rec => rec.Invalid.HasValue == false || (rec.Invalid.HasValue && rec.Invalid.Value != 1))
                 .Select(rec => rec.Apid).ToList();

            if (amps.IsCollectionValid())
            {
                codes.AddRange(amps);
            }

            return codes?.Distinct()?.ToList();
        }

        public List<DmdAmpExcipientDTO> GetAMPExcipientsForCodes(List<string> dmdCodes)
        {
            var excipients = new List<DmdAmpExcipientDTO>();

            if (!dmdCodes.IsCollectionValid()) return null;

            var ampExcpientRepo = this._provider.GetService(typeof(ITerminologyRepository<DmdAmpExcipient>)) as ITerminologyRepository<DmdAmpExcipient>;

            var excipientsDAO = ampExcpientRepo.ItemsAsReadOnly.Where(rec => dmdCodes.Contains(rec.Apid)).ToList();

            if (!excipientsDAO.IsCollectionValid()) return excipients;

            excipients = _mapper.Map<List<DmdAmpExcipientDTO>>(excipientsDAO);

            return excipients;
        }

        public List<DmdAmpDrugrouteDTO> GetAMPDrugRoutesForCodes(List<string> dmdCodes)
        {
            var routes = new List<DmdAmpDrugrouteDTO>();

            if (!dmdCodes.IsCollectionValid()) return null;

            var ampDrugrouteRepo = this._provider.GetService(typeof(ITerminologyRepository<DmdAmpDrugroute>)) as ITerminologyRepository<DmdAmpDrugroute>;

            var excipientsDAO = ampDrugrouteRepo.ItemsAsReadOnly.Where(rec => dmdCodes.Contains(rec.Apid)).ToList();

            if (!excipientsDAO.IsCollectionValid()) return routes;

            routes = _mapper.Map<List<DmdAmpDrugrouteDTO>>(excipientsDAO);

            return routes;
        }

        public List<DmdVmpDrugrouteDTO> GetVMPDrugRoutesForCodes(List<string> dmdCodes)
        {
            var routes = new List<DmdVmpDrugrouteDTO>();

            if (!dmdCodes.IsCollectionValid()) return null;

            var vmpDrugrouteRepo = this._provider.GetService(typeof(ITerminologyRepository<DmdVmpDrugroute>)) as ITerminologyRepository<DmdVmpDrugroute>;

            var vmpRoutesDAO = vmpDrugrouteRepo.ItemsAsReadOnly.Where(rec => dmdCodes.Contains(rec.Vpid)).ToList();

            if (!vmpRoutesDAO.IsCollectionValid()) return routes;

            routes = _mapper.Map<List<DmdVmpDrugrouteDTO>>(vmpRoutesDAO);

            return routes;
        }

        public List<DmdSyncLog> GetDMDSyncLogs()
        {
            var repo = _provider.GetService(typeof(ITerminologyRepository<DmdSyncLog>)) as ITerminologyRepository<DmdSyncLog>;

            var logs = repo.ItemsAsReadOnly.ToList();

            if (!logs.IsCollectionValid()) return null;

            return logs;
        }

        public List<DmdSyncLog> GetDMDPendingSyncLogs()
        {
            var logs = this.GetDMDSyncLogs();
            if (!logs.IsCollectionValid()) return null;

            return logs.Where(rec => (rec.IsDmdUpdated.HasValue && rec.IsDmdUpdated == true) && rec.IsFormularyUpdated.HasValue == false || rec.IsFormularyUpdated == false).ToList();
        }

        public async Task<List<DmdATCCodeDTO>> GetAllATCCodesFromDMD()
        {
            var atcDTOs = new List<DmdATCCodeDTO>();

            atcDTOs = CacheService.Get<List<DmdATCCodeDTO>>(CacheKeys.DMD_ATC_Code);

            if (atcDTOs.IsCollectionValid()) return atcDTOs;

            var dmdATCRepo = this._provider.GetService(typeof(ITerminologyRepository<DmdAtc>)) as ITerminologyRepository<DmdAtc>;

            var atcDAO = dmdATCRepo.ItemsAsReadOnly.ToList();

            if (!atcDAO.IsCollectionValid()) return atcDTOs;

            atcDTOs = _mapper.Map<List<DmdATCCodeDTO>>(atcDAO);

            var atcLookupDtos = await this.GetLookup<AtcLookupDTO>(LookupType.ATCCode);

            if (!atcLookupDtos.IsCollectionValid()) return atcDTOs;

            var atcLookupDict = new Dictionary<string, string>();
            atcLookupDtos.Each(rec =>
            {
                if (rec.Cd.IsNotEmpty())
                    atcLookupDict[rec.Cd] = rec.Desc;
            });
            atcDTOs.Each(rec =>
            {
                if (rec.Cd.IsNotEmpty() && atcLookupDict.ContainsKey(rec.Cd))
                    rec.Desc = atcLookupDict[rec.Cd];
            });

            CacheService.Set(CacheKeys.DMD_ATC_Code, atcDTOs);

            return atcDTOs;
        }

        public async Task<List<DmdBNFCodeDTO>> GetAllBNFCodesFromDMD()
        {
            var bnfDTOs = new List<DmdBNFCodeDTO>();

            bnfDTOs = CacheService.Get<List<DmdBNFCodeDTO>>(CacheKeys.DMD_BNF_Code);

            if (bnfDTOs.IsCollectionValid()) return bnfDTOs;

            var dmdBNFRepo = this._provider.GetService(typeof(ITerminologyRepository<DmdBnf>)) as ITerminologyRepository<DmdBnf>;

            var bnfDAO = dmdBNFRepo.ItemsAsReadOnly.ToList();

            if (!bnfDAO.IsCollectionValid()) return bnfDTOs;

            bnfDTOs = _mapper.Map<List<DmdBNFCodeDTO>>(bnfDAO);

            var bnfLookupDtos = await this.GetLookup<DmdLookupBNFDTO>(LookupType.BNFCode);

            if (!bnfLookupDtos.IsCollectionValid()) return bnfDTOs;

            var bnfLookupDict = new Dictionary<string, string>();

            bnfLookupDtos.Each(rec =>
            {
                if (rec.Cd.IsNotEmpty())
                    bnfLookupDict[rec.Cd] = rec.Desc;
            });
            bnfDTOs.Each(rec =>
            {
                if (rec.Cd.IsNotEmpty() && bnfLookupDict.ContainsKey(rec.Cd))
                    rec.Desc = bnfLookupDict[rec.Cd];
            });
            CacheService.Set(CacheKeys.DMD_BNF_Code, bnfDTOs);
            return bnfDTOs;
        }

        private List<DMDSearchResultWithTreeDTO> GetAncestor(ConcurrentDictionary<string, List<DMDSearchResultModel>> nodesLookup, string code, int v)
        {
            var parentRecords = new List<DMDSearchResultWithTreeDTO>();

            if (!nodesLookup.IsCollectionValid() || !nodesLookup.ContainsKey(code)) return parentRecords;

            var parentNodesFromLkp = nodesLookup[code];// resultsFromDB.Where(r => r.Level == level && r.ParentId == conceptId);

            if (!parentNodesFromLkp.IsCollectionValid()) return new List<DMDSearchResultWithTreeDTO>();

            parentNodesFromLkp.Distinct(r => r.Code).AsParallel().Each(res =>
            {
                var resDTO = this._mapper.Map<DMDSearchResultWithTreeDTO>(res);

                if (res.ControlDrugCategory != null)
                    resDTO.ControlDrugCategory = _mapper.Map<DmdLookupControldrugcatDTO>(res.ControlDrugCategory);
                if (res.PrescribingStatus != null)
                    resDTO.PrescribingStatus = _mapper.Map<DmdLookupPrescribingstatusDTO>(res.PrescribingStatus);
                if (res.Routes.IsCollectionValid())
                    resDTO.Routes = _mapper.Map<List<DmdLookupRouteDTO>>(res.Routes);
                if (res.Form != null)
                    resDTO.Form = _mapper.Map<DmdLookupFormDTO>(res.Form);



                //var resDTO = new DMDSearchResultWithTreeDTO
                //{
                //    Code = res.Code,
                //    Name = res.Name,
                //    Form = res.Form,
                //    FormCode = res.FormCode,
                //    Route = res.Route,
                //    RouteCode = res.RouteCode,
                //    Supplier = res.Supplier,
                //    SupplierCode = res.SupplierCode,
                //    PrescribingStatus = res.PrescribingStatus,
                //    PrescribingStatusCode = res.PrescribingStatusCode,
                //    ControlDrugCategory = res.ControlDrugCategory,
                //    ControlDrugCategoryCode = res.ControlDrugCategoryCode,
                //    Level = res.LogicalLevel.GetDMDLevelCodeByLogicalLevel(),
                //    LogicalLevel = res.LogicalLevel,
                //    ParentCode = res.ParentCode
                //};

                //GetAncestor Records -- IN Next level
                var parentNodes = GetAncestor(nodesLookup, res.ParentCode, res.LogicalLevel.GetValueOrDefault() + 1);//Not considering the level now

                resDTO.Parents = new List<DMDSearchResultWithTreeDTO>();

                if (parentNodes.IsCollectionValid())
                    resDTO.Parents.AddRange(parentNodes);

                parentRecords.Add(resDTO);

            });

            return parentRecords;
        }

        private List<DMDSearchResultModel> GetRootFromChildLevelNodes(List<DMDSearchResultModel> resultsFromDBList, ConcurrentDictionary<string, List<DMDSearchResultModel>> parentWithChildNodes)
        {
            //If the record doesn't have its parentid null or not in the list of codes, then it is considered root
            List<DMDSearchResultModel> rootLevelRecords = new List<DMDSearchResultModel>();

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

        private ConcurrentDictionary<string, List<DMDSearchResultModel>> BuildAllNodesLookupWithCodeAsKey(List<DMDSearchResultModel> resultsFromDB)
        {
            var parentWithChildNodes = new ConcurrentDictionary<string, List<DMDSearchResultModel>>();

            if (resultsFromDB.IsCollectionValid())
            {
                resultsFromDB.AsParallel().Each(r =>
                {
                    var key = r.Code;

                    if (parentWithChildNodes.ContainsKey(key))
                    {
                        parentWithChildNodes[key].Add(r);
                    }
                    else
                    {
                        parentWithChildNodes[key] = new List<DMDSearchResultModel>() { r };
                    }
                });
            }

            return parentWithChildNodes;
        }

        /// <summary>
        /// Build Dictionary of Parent Nodes (as Keys) and its associated records
        /// </summary>
        /// <param name="resultsFromDB"></param>
        /// <returns></returns>
        private ConcurrentDictionary<string, List<DMDSearchResultModel>> BuildChildNodesLookup(List<DMDSearchResultModel> resultsFromDB)
        {
            var parentWithChildNodes = new ConcurrentDictionary<string, List<DMDSearchResultModel>>();

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
                        parentWithChildNodes[key] = new List<DMDSearchResultModel>() { r };
                    }
                });
            }

            return parentWithChildNodes;
        }

        private DMDSearchResultWithTreeDTO GetNodeDetail(DMDSearchResultModel res, ConcurrentDictionary<string, List<DMDSearchResultModel>> parentWithChildNodes)
        {
            var resDTO = _mapper.Map<DMDSearchResultWithTreeDTO>(res);

            if (res.ControlDrugCategory != null)
                resDTO.ControlDrugCategory = _mapper.Map<DmdLookupControldrugcatDTO>(res.ControlDrugCategory);
            if (res.PrescribingStatus != null)
                resDTO.PrescribingStatus = _mapper.Map<DmdLookupPrescribingstatusDTO>(res.PrescribingStatus);
            if (res.Routes.IsCollectionValid())
                resDTO.Routes = _mapper.Map<List<DmdLookupRouteDTO>>(res.Routes);
            if (res.Form != null)
                resDTO.Form = _mapper.Map<DmdLookupFormDTO>(res.Form);


            //var resDTO = new DMDSearchResultWithTreeDTO
            //{
            //    Code = res.Code,
            //    Name = res.Name,
            //    Form = res.Form,
            //    FormCode = res.FormCode,
            //    Route = res.Route,
            //    RouteCode = res.RouteCode,
            //    Supplier = res.Supplier,
            //    SupplierCode = res.SupplierCode,
            //    PrescribingStatus = res.PrescribingStatus,
            //    PrescribingStatusCode = res.PrescribingStatusCode,
            //    ControlDrugCategory = res.ControlDrugCategory,
            //    ControlDrugCategoryCode = res.ControlDrugCategoryCode,
            //    Level = res.LogicalLevel.GetDMDLevelCodeByLogicalLevel(),
            //    LogicalLevel = res.LogicalLevel,
            //    ParentCode = res.ParentCode
            //};

            //GetChildren Records -- IN Next level
            var chidrenNodes = GetChildren(parentWithChildNodes, res.Code, res.LogicalLevel.GetValueOrDefault() + 1);//level = 2 -- next level

            resDTO.Children = new List<DMDSearchResultWithTreeDTO>();

            if (chidrenNodes.IsCollectionValid())
                resDTO.Children.AddRange(chidrenNodes);

            return resDTO;
        }

        private List<DMDSearchResultWithTreeDTO> GetChildren(ConcurrentDictionary<string, List<DMDSearchResultModel>> parentWithChildNodes, string code, int level)
        {
            var childRecords = new List<DMDSearchResultWithTreeDTO>();

            if (!parentWithChildNodes.IsCollectionValid() || !parentWithChildNodes.ContainsKey(code)) return childRecords;

            var childNodes = parentWithChildNodes[code];// resultsFromDB.Where(r => r.Level == level && r.ParentId == conceptId);

            if (!childNodes.IsCollectionValid()) return new List<DMDSearchResultWithTreeDTO>();

            childNodes.Distinct(r => r.Code).AsParallel().Each(res =>
            {
                var resDTO = _mapper.Map<DMDSearchResultWithTreeDTO>(res);

                if (res.ControlDrugCategory != null)
                    resDTO.ControlDrugCategory = _mapper.Map<DmdLookupControldrugcatDTO>(res.ControlDrugCategory);
                if (res.PrescribingStatus != null)
                    resDTO.PrescribingStatus = _mapper.Map<DmdLookupPrescribingstatusDTO>(res.PrescribingStatus);
                if (res.Routes.IsCollectionValid())
                    resDTO.Routes = _mapper.Map<List<DmdLookupRouteDTO>>(res.Routes);
                if (res.Form != null)
                    resDTO.Form = _mapper.Map<DmdLookupFormDTO>(res.Form);

                //var resDTO = new DMDSearchResultWithTreeDTO
                //{
                //    Code = res.Code,
                //    Name = res.Name,
                //    Form = res.Form,
                //    FormCode = res.FormCode,
                //    Route = res.Route,
                //    RouteCode = res.RouteCode,
                //    Supplier = res.Supplier,
                //    SupplierCode = res.SupplierCode,
                //    PrescribingStatus = res.PrescribingStatus,
                //    PrescribingStatusCode = res.PrescribingStatusCode,
                //    ControlDrugCategory = res.ControlDrugCategory,
                //    ControlDrugCategoryCode = res.ControlDrugCategoryCode,
                //    Level = res.LogicalLevel.GetDMDLevelCodeByLogicalLevel(),
                //    LogicalLevel = res.LogicalLevel,
                //    ParentCode = res.ParentCode
                //};

                //GetChildren Records -- IN Next level
                var chidrenNodes = GetChildren(parentWithChildNodes, res.Code, res.LogicalLevel.GetValueOrDefault() + 1);//Not considering the level now

                resDTO.Children = new List<DMDSearchResultWithTreeDTO>();

                if (chidrenNodes.IsCollectionValid())
                    resDTO.Children.AddRange(chidrenNodes);

                childRecords.Add(resDTO);

            });

            return childRecords;
        }

        public DmdSnomedVersion GetDmdSnomedVersion()
        {
            var repo = _provider.GetService(typeof(ITerminologyRepository<DmdSnomedVersion>)) as ITerminologyRepository<DmdSnomedVersion>;

            var dmdSnomedVersion = repo.ItemsAsReadOnly.FirstOrDefault();

            if (dmdSnomedVersion.IsNull()) return null;

            return dmdSnomedVersion;
        }

        #region Not Necessary for now
        //public async Task<List<DMDLookupRouteDTO>> GetRouteLookup()
        //{
        //    var dtos = await GetLookupDetail<DmdLookupRoute, DMDLookupRouteDTO>();

        //    return dtos;

        //    //    new List<DMDLookupRouteDTO>();

        //    //var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdLookupRoute>)) as IReadOnlyRepository<DmdLookupRoute>;

        //    //var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    //if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    //dtos = _mapper.Map<List<DMDLookupRouteDTO>>(lookupFromDb);

        //    //return dtos;
        //}

        //private async Task<List<T2>> GetLookupDetail<T1, T2>() where T1 : EntityBase
        //{
        //    var dtos = new List<T2>();

        //    var repo = this._provider.GetService(typeof(IReadOnlyRepository<T1>)) as IReadOnlyRepository<T1>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    dtos = _mapper.Map<List<T2>>(lookupFromDb);

        //    return dtos;
        //}

        //public async Task<List<DMDLookupFormDTO>> GetFormLookup()
        //{
        //    var dtos = await GetLookupDetail<DmdLookupForm, DMDLookupFormDTO>();

        //    return dtos;

        //    //var dtos = new List<DMDLookupFormDTO>();

        //    //var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdLookupForm>)) as IReadOnlyRepository<DmdLookupForm>;

        //    //var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    //if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    //dtos = _mapper.Map<List<DMDLookupFormDTO>>(lookupFromDb);

        //    //return dtos;
        //}

        //public async Task<List<DMDLookupPrescribingStatusDTO>> GetPrescribingStatusLookup()
        //{
        //    var dtos = await GetLookupDetail<DmdLookupForm, DMDLookupPrescribingStatusDTO>();

        //    return dtos;

        //    //var dtos = new List<DMDLookupPrescribingStatusDTO>();

        //    //var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdLookupPrescribingstatus>)) as IReadOnlyRepository<DmdLookupPrescribingstatus>;

        //    //var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    //if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    //dtos = _mapper.Map<List<DMDLookupPrescribingStatusDTO>>(lookupFromDb);

        //    //return dtos;
        //}

        //public async Task<List<DMDLookupControlDrugCategoryDTO>> GetControlDrugCategoryLookup()
        //{
        //    var dtos = new List<DMDLookupControlDrugCategoryDTO>();

        //    var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdLookupControldrugcat>)) as IReadOnlyRepository<DmdLookupControldrugcat>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    dtos = _mapper.Map<List<DMDLookupControlDrugCategoryDTO>>(lookupFromDb);

        //    return dtos;
        //}

        //public async Task<List<DMDLookupSupplierDTO>> GetSupplierLookup()
        //{
        //    var dtos = new List<DMDLookupSupplierDTO>();

        //    var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdLookupSupplier>)) as IReadOnlyRepository<DmdLookupSupplier>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    dtos = _mapper.Map<List<DMDLookupSupplierDTO>>(lookupFromDb);

        //    return dtos;
        //}

        //public async Task<List<DMDLookupUOMDTO>> GetDMDUOMLookup()
        //{
        //    var dtos = new List<DMDLookupUOMDTO>();

        //    var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdVmpIngredient>)) as IReadOnlyRepository<DmdVmpIngredient>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    dtos = _mapper.Map<List<DMDLookupUOMDTO>>(lookupFromDb);

        //    return dtos;
        //}

        //public async Task<List<DMDLookupOntFormRouteDTO>> GetDMDOntFormRouteLookup()
        //{
        //    var dtos = new List<DMDLookupOntFormRouteDTO>();

        //    var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdVmpIngredient>)) as IReadOnlyRepository<DmdVmpIngredient>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    dtos = _mapper.Map<List<DMDLookupOntFormRouteDTO>>(lookupFromDb);

        //    return dtos;
        //}

        //public async Task<List<DMDLookupBasisOfNameDTO>> GetDMDBasisOfNameLookup()
        //{
        //    var dtos = new List<DMDLookupBasisOfNameDTO>();

        //    var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdVmpIngredient>)) as IReadOnlyRepository<DmdVmpIngredient>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    dtos = _mapper.Map<List<DMDLookupBasisOfNameDTO>>(lookupFromDb);

        //    return dtos;
        //}

        //public async Task<List<DMDLookupAvailRestrictionsDTO>> GetDMDAvailRestrictionsLookup()
        //{
        //    var dtos = new List<DMDLookupAvailRestrictionsDTO>();

        //    var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdVmpIngredient>)) as IReadOnlyRepository<DmdVmpIngredient>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    dtos = _mapper.Map<List<DMDLookupAvailRestrictionsDTO>>(lookupFromDb);

        //    return dtos;
        //}

        //public async Task<List<DMDLookupLicensingAuthorityDTO>> GetLicensingAuthorityLookup()
        //{
        //    var dtos = new List<DMDLookupLicensingAuthorityDTO>();

        //    var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdVmpIngredient>)) as IReadOnlyRepository<DmdVmpIngredient>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    dtos = _mapper.Map<List<DMDLookupLicensingAuthorityDTO>>(lookupFromDb);

        //    return dtos;
        //}


        //public async Task<List<FormularyLookupMedicationTypeDTO>> GetMedicationTypeLookup()
        //{
        //    var dtos = new List<FormularyLookupMedicationTypeDTO>();

        //    var repo = this._provider.GetService(typeof(IReadOnlyRepository<LookupMedicationType>)) as IReadOnlyRepository<LookupMedicationType>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    dtos = _mapper.Map<List<FormularyLookupMedicationTypeDTO>>(lookupFromDb);

        //    return dtos;
        //}

        //public async Task<List<FormularyLookupRulesDTO>> GetRulesLookup()
        //{
        //    var dtos = new List<FormularyLookupRulesDTO>();

        //    var repo = this._provider.GetService(typeof(IReadOnlyRepository<LookupRules>)) as IReadOnlyRepository<LookupRules>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    dtos = _mapper.Map<List<FormularyLookupRulesDTO>>(lookupFromDb);

        //    return dtos;
        //}
        #endregion Not Necessary for now
    }
}
