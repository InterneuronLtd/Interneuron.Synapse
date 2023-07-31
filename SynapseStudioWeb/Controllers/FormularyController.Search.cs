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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.DataService.APIModel;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models;
using SynapseStudioWeb.Models.MedicinalMgmt;
using SynapseStudioWeb.AppCode.Constants;

namespace SynapseStudioWeb.Controllers
{
    public partial class FormularyController : Controller
    {
        [HttpPost]
        public async Task<JsonResult> LoadFormularyList(FormularyListSearchCriteria searchCriteria)
        {
            var pageSize = 100;
            if (int.TryParse(_configuration["Settings:FormularyResultsPageSize"], out int configPageSize))
            {
                pageSize = configPageSize;
            }
            var paginatedVM = new FormularyTreePaginatedModel { PageSize = pageSize };

            string token = HttpContext.Session.GetString("access_token");

            HttpContext.Session.Remove(SynapseSession.FormularySearchResults);

            FormularyFilterCriteriaAPIModel filterCriteria = ConstructFormularyListFilterModel(searchCriteria);

            var recordStatusDictionary = HttpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.FormularyRecStatusLkpKey);

            var formularies = await InvokeSearchAPI(filterCriteria, token);

            if (!formularies.IsCollectionValid())
            {
                return Json(paginatedVM);
            }

            if (!recordStatusDictionary.IsCollectionValid())
            {
                var recordStatusLkp = await TerminologyAPIService.GetRecordStatusLookup(token);

                if (recordStatusLkp.IsCollectionValid())
                {
                    recordStatusDictionary = recordStatusLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    HttpContext.Session.SetObject(SynapseSession.FormularyRecStatusLkpKey, recordStatusDictionary);
                }
            }

            var formulariesAsTreeModel = ConvertToFormularyTreeModel(formularies, recordStatusDictionary);

            paginatedVM.TotalRecords = formulariesAsTreeModel.Count;

            HttpContext.Session.SetObject(SynapseSession.FormularySearchResults, formulariesAsTreeModel);

            paginatedVM.Results = formulariesAsTreeModel.Skip(0).Take(pageSize).ToList();

            return (Json(paginatedVM));
        }

        private async Task<List<FormularyListAPIModel>> InvokeSearchAPI(FormularyFilterCriteriaAPIModel filterCriteria, string token)
        {
            //If has any search criteria - hit search api
            if (filterCriteria.searchTerm.IsNotEmpty() || filterCriteria.formularyStatusCd.IsCollectionValid() || filterCriteria.IncludeDeleted || filterCriteria.recStatusCds.IsCollectionValid() || filterCriteria.Flags.IsCollectionValid() || filterCriteria.hideArchived)
            {
                var recStsCodes = new List<string>();

                if (filterCriteria.recStatusCds.IsCollectionValid())
                    recStsCodes = filterCriteria.recStatusCds;

                //if (filterCriteria.hideArchived)
                //{
                //    recStsCodes.Clear();
                //    recStsCodes.Add("004");
                //}

                if (recStsCodes.IsCollectionValid())
                    filterCriteria.recStatusCds = recStsCodes;

                var formulariesResponse = await TerminologyAPIService.SearchFormularies(filterCriteria, token);

                if (formulariesResponse.StatusCode != DataService.APIModel.StatusCode.Success)
                {
                    string errors = "Error getting the Formularies data.";

                    if (formulariesResponse.ErrorMessages.IsCollectionValid())
                        errors += string.Join('\n', formulariesResponse.ErrorMessages.ToArray());

                    _toastNotification.AddErrorToastMessage(errors);

                    return null;
                }
                return formulariesResponse.Data?.data;
            }
            else
            {
                var formulariesResponse = await TerminologyAPIService.GetLatestTopLevelFormulariesBasicInfo(token);
                if (formulariesResponse.StatusCode != DataService.APIModel.StatusCode.Success)
                {
                    string errors = "Error getting the Formularies data.";
                    if (formulariesResponse.ErrorMessages.IsCollectionValid())
                        errors += string.Join('\n', formulariesResponse.ErrorMessages.ToArray());

                    _toastNotification.AddErrorToastMessage(errors);
                    return null;
                }
                return formulariesResponse.Data;
            }
        }



        [HttpPost]
        public async Task<JsonResult> LoadChildrenFormularies(FormularyListSearchCriteria searchCriteria)
        {
            string token = HttpContext.Session.GetString("access_token");

            var apiRequest = new GetFormularyDescendentForCodesAPIRequest() { Codes = new List<string> { searchCriteria.FormularyCode }, OnlyNonDeleted = false };
            var formulariesResponse = await TerminologyAPIService.GetFormularyDescendentForCodes(apiRequest, token);
            var recordStatusDictionary = HttpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.FormularyRecStatusLkpKey);

            if (formulariesResponse.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                string errors = "Error getting the Formularies data.";

                if (formulariesResponse.ErrorMessages.IsCollectionValid())
                    errors = errors + string.Join('\n', formulariesResponse.ErrorMessages.ToArray());

                _toastNotification.AddErrorToastMessage(errors);

                return Json(null);
            }

            if (formulariesResponse == null || !formulariesResponse.Data.IsCollectionValid())
                return Json(new List<FormularyTreeModel>());

            var formularyData = formulariesResponse.Data;

            if (searchCriteria.HideArchived == true)
            {
                formularyData = formularyData.Where(rec => rec.RecStatusCode != TerminologyConstants.ARCHIEVED_STATUS_CD).ToList();
            }

            var vm = ConvertToFormularyTreeModel(formularyData, recordStatusDictionary);

            return Json(vm);
        }

        private List<FormularyTreeModel> ConvertToFormularyTreeModel(List<FormularyListAPIModel> apiModelList, Dictionary<string, string> recordStatusDictionary)
        {
            var treeList = new List<FormularyTreeModel>();

            if (!apiModelList.IsCollectionValid()) return treeList;

            foreach (FormularyListAPIModel result in apiModelList)
            {
                var formularyTree = new FormularyTreeModel();
                formularyTree.Data["Level"] = result.ProductType;
                formularyTree.Key = result.Code;
                formularyTree.Title = result.Name;
                formularyTree.FormularyVersionId = result.FormularyVersionId;
                formularyTree.Data["recordstatus"] = new FormaryLookupModel { IsDuplicate = result.IsDuplicate, Code = result.RecStatusCode, Description = result.RecStatusCode.IsNotEmpty() ? recordStatusDictionary[result.RecStatusCode] : null };
                formularyTree.Lazy = true;

                if (string.Compare(result.ProductType, "amp", true) == 0)
                    formularyTree.Children = new List<FormularyTreeModel>();
                else if (result.Children.IsCollectionValid())
                    formularyTree.Children = ConvertToFormularyTreeModel(result.Children, recordStatusDictionary);

                treeList.Add(formularyTree);
            }

            return treeList;
        }


        [HttpGet]
        public JsonResult GetFormulariesByPageNumber(int pageNumber)
        {
            var pageSize = 100;
            if (int.TryParse(_configuration["Settings:FormularyResultsPageSize"], out int configPageSize))
            {
                pageSize = configPageSize;
            }

            var formularies = HttpContext.Session.GetObject<List<FormularyTreeModel>>(SynapseSession.FormularySearchResults);

            if (!formularies.IsCollectionValid()) return Json(null);

            var viewModel = formularies.Skip(pageNumber == 0 ? 0 : ((pageNumber - 1) * pageSize)).Take(pageSize).ToList();

            return Json(viewModel);
        }


        private FormularyFilterCriteriaAPIModel ConstructFormularyListFilterModel(FormularyListSearchCriteria searchCriteria)
        {
            FormularyFilterCriteriaAPIModel filter = new FormularyFilterCriteriaAPIModel();

            filter.searchTerm = searchCriteria.SearchTerm ?? "";
            filter.hideArchived = searchCriteria.HideArchived ?? false;
            filter.recStatusCds = new List<string>();
            filter.formularyStatusCd = new List<string>();
            filter.Flags = new List<string>();

            if (searchCriteria.RecStatusCds != null)
            {
                AssignFilterByRecStatusCodes(filter, searchCriteria);
            }

            return filter;
        }

        private void AssignFilterByRecStatusCodes(FormularyFilterCriteriaAPIModel filter, FormularyListSearchCriteria searchCriteria)
        {
            if (searchCriteria.RecStatusCds.IndexOf(',') > -1)
            {
                foreach (string status in searchCriteria.RecStatusCds.Split(','))
                {
                    if (status == "Duplicate")
                    {
                        filter.showOnlyDuplicate = true;
                    }
                    else if (status.Split('|')[0] == "Rec")
                    {
                        filter.recStatusCds.Add(status.Split('|')[1]);
                    }
                    else if (status.Split('|')[0] == "Form")
                    {
                        filter.formularyStatusCd.Add(status.Split('|')[1]);
                    }
                    else if (status.Split('|')[0] == "Flags")
                    {
                        filter.Flags.Add(status.Split('|')[1]);
                    }
                }
            }
            else
            {
                if (searchCriteria.RecStatusCds == "Duplicate")
                {
                    filter.showOnlyDuplicate = true;
                }
                else if (searchCriteria.RecStatusCds.IndexOf('|') > -1 && searchCriteria.RecStatusCds.Split('|')[0] == "Rec")
                {
                    filter.recStatusCds.Add(searchCriteria.RecStatusCds.Split('|')[1]);
                }
                else if (searchCriteria.RecStatusCds.IndexOf('|') > -1 && searchCriteria.RecStatusCds.Split('|')[0] == "Form")
                {
                    filter.formularyStatusCd.Add(searchCriteria.RecStatusCds.Split('|')[1]);
                }
                else if (searchCriteria.RecStatusCds.IndexOf('|') > -1 && searchCriteria.RecStatusCds.Split('|')[0] == "Flags")
                {
                    filter.Flags.Add(searchCriteria.RecStatusCds.Split('|')[1]);
                }
            }
        }
    }
}
