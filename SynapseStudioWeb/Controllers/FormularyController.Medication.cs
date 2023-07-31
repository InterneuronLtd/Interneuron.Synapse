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


﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.DataService.APIModel;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models;

namespace SynapseStudioWeb.Controllers
{
    public partial class FormularyController : Controller
    {
        private const string SUCCESS_IMPORT_MSG = "Successfully Imported the records.";

        [HttpPost]
        [Route("Formulary/LoadDMDList")]
        public async Task<JsonResult> LoadDMDList(string searchTxt)
        {
            string token = HttpContext.Session.GetString("access_token");

            var dmdResponseTask = TerminologyAPIService.SearchDMDNamesGetWithAllLevelNodes(searchTxt, token);// TerminologyAPIService.SearchDMDWithAllDescendents(searchTxt, token);

            var dmdResponse = await dmdResponseTask;

            if (dmdResponse.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                var errors = "Error getting the DMD data.";

                if (dmdResponse.ErrorMessages.IsCollectionValid())
                    errors = errors + string.Join('\n', dmdResponse.ErrorMessages);

                _toastNotification.AddErrorToastMessage(errors);

                return Json(null);
            }

            var dmds = dmdResponse.Data;

            if (!dmds.Data.IsCollectionValid()) return Json(null);

            var orderedDataList = dmds.Data.OrderBy(rec => rec.Name).ToList();

            var formularyList = FillFormularyTreeModel(orderedDataList);

            return Json(formularyList);
        }

        [HttpPost]
        [Route("Formulary/LoadSyncPendingDMDList")]
        public async Task<JsonResult> LoadSyncPendingDMDList(string searchTxt)
        {
            string token = HttpContext.Session.GetString("access_token");

            var dmdResponseTask = TerminologyAPIService.SearchDMDSyncLog(searchTxt, token);

            var dmdResponse = await dmdResponseTask;

            if (dmdResponse.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                var errors = "Error getting the DMD data.";

                if (dmdResponse.ErrorMessages.IsCollectionValid())
                    errors = errors + string.Join('\n', dmdResponse.ErrorMessages);

                _toastNotification.AddErrorToastMessage(errors);

                return Json(null);
            }

            var dmds = dmdResponse.Data;

            if (!dmds.Data.IsCollectionValid()) return Json(null);

            var orderedDataList = dmds.Data.OrderBy(rec => rec.Name).ToList();

            var formularyList = FillFormularyTreeModel(orderedDataList);

            return Json(formularyList);
        }

        private List<FormularyTreeModel> FillFormularyTreeModel(List<DMDSearchResultWithTree> nodes)
        {
            var formularyNodes = new List<FormularyTreeModel>();

            foreach (DMDSearchResultWithTree node in nodes)
            {
                var formularyNode = new FormularyTreeModel();
                formularyNode.Data["Level"] = node.Level;
                formularyNode.Key = node.Code;
                formularyNode.Title = node.Name;

                if (node.Children.IsCollectionValid())
                    formularyNode.Children = FillFormularyTreeModel(node.Children);

                formularyNodes.Add(formularyNode);
            }
            return formularyNodes;
        }

        [HttpPost]
        [Route("Formulary/ImportMeds")]
        public async Task<IActionResult> ImportMeds(List<string> meds)
        {
            string token = HttpContext.Session.GetString("access_token");

            var resultResponse = await TerminologyAPIService.ImportMeds(meds, token);

            if (resultResponse == null)
            {
                _toastNotification.AddErrorToastMessage(UNKNOWN_SAVE_STATUS_MSG);
                return Json(null);
            }

            if (resultResponse.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                string errors = "Error Importing the data.";

                if (resultResponse.ErrorMessages.IsCollectionValid())
                    errors += string.Join('\n', resultResponse.ErrorMessages);
                _toastNotification.AddErrorToastMessage(errors);
                return Json(null);
            }

            if (resultResponse.StatusCode == DataService.APIModel.StatusCode.Success)
            {
                if (resultResponse.Data.Status != null && resultResponse.Data.Status.ErrorMessages.IsCollectionValid())
                {
                    var errors = string.Join('\n', resultResponse.Data.Status.ErrorMessages);
                    _toastNotification.AddWarningToastMessage(errors);
                    return Json(null);
                }
                else
                {
                    var postProcessImportResponse = await TerminologyAPIService.InvokePostImportProcess(token, meds);

                    if (postProcessImportResponse.StatusCode != DataService.APIModel.StatusCode.Success)
                    {
                        string errors = "Error Completing post import process of the data.";

                        if (postProcessImportResponse.ErrorMessages.IsCollectionValid())
                            errors += string.Join('\n', postProcessImportResponse.ErrorMessages);
                        _toastNotification.AddErrorToastMessage(errors);
                        return Json(null);
                    }
                }
            }

            _toastNotification.AddSuccessToastMessage(SUCCESS_IMPORT_MSG);

            return Json(new List<string> { "Success" });//Just to indicate the client
        }

        [HttpGet]
        [Route("Formulary/GetLatestFormulariesHeaderOnly")]
        public async Task<JsonResult> GetLatestFormulariesHeaderOnly()
        {
            string token = HttpContext.Session.GetString("access_token");

            var response = await TerminologyAPIService.GetLatestFormulariesHeaderOnly(token);

            if (response.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                var errors = "Error getting the DMD data.";

                if (response.ErrorMessages.IsCollectionValid())
                    errors = errors + string.Join('\n', response.ErrorMessages);

                _toastNotification.AddErrorToastMessage(errors);

                return Json(null);
            }

            var dmds = response.Data;

            if (dmds.IsCollectionValid())
            {
                var result = dmds.Distinct(x => x.code).ToHashSet();

                return (Json(result));
            }
            else
            {
                return Json(null);
            }
        }

        [HttpGet]
        [Route("Formulary/GetDMDVersion")]
        public async Task<JsonResult> GetDMDVersion()
        {
            string token = HttpContext.Session.GetString("access_token");

            var response = await TerminologyAPIService.GetDmdSnomedVersion(token);

            if (response.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                var errors = "Error while getting the DMD SNOMED version data.";

                if (response.ErrorMessages.IsCollectionValid())
                    errors = errors + string.Join('\n', response.ErrorMessages);

                _toastNotification.AddErrorToastMessage(errors);

                return Json(null);
            }

            var dmdSnomedVersion = response.Data;

            if (dmdSnomedVersion.IsNotNull())
            {
                var result = dmdSnomedVersion.DmdVersion;

                return (Json(result));
            }
            else
            {
                return Json(null);
            }
        }

        [HttpGet]
        [Route("Formulary/GetHistoryOfFormularies")]
        public async Task<JsonResult> GetHistoryOfFormularies()
        {
            string token = HttpContext.Session.GetString("access_token");

            var response = await TerminologyAPIService.GetHistoryOfFormularies(token);

            if (response.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                var errors = "Error while getting the history of formularies data.";

                if (response.ErrorMessages.IsCollectionValid())
                    errors = errors + string.Join('\n', response.ErrorMessages);

                _toastNotification.AddErrorToastMessage(errors);

                return Json(null);
            }

            var formulariesHistory = response.Data;

            if (!formulariesHistory.IsCollectionValid()) return Json(null);

            var result = formulariesHistory;

            return (Json(result));
        }
    }
}
