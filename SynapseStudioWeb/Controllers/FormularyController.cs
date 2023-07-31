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
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.DataService.APIModel;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.Rendering;
using Interneuron.Common.Extensions;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models.MedicinalMgmt;
using AutoMapper;
using SynapseStudioWeb.AppCode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SynapseStudioWeb.Controllers
{
    public partial class FormularyController : Controller
    {
        private const string UNKNOWN_SAVE_STATUS_MSG = "Unknown error saving the status.";
        private const string STATUS_SUCCESS_MSG = "Successfully changed the record status to {0} in the system.";
        private const string UNKNOWN_GET_DATA_MSG = "Unknown error getting the data from the data store.";
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _provider;
        private readonly IHttpContextAccessor _contextAccessor;
        private FormularyLookupLoaderService _formularyLookupService;

        public FormularyController(IMapper mapper, IToastNotification toastNotification, IServiceProvider provider, IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _toastNotification = toastNotification;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

            _provider = provider;
            _contextAccessor = _provider.GetService<IHttpContextAccessor>();

            _formularyLookupService = new FormularyLookupLoaderService(_contextAccessor);
        }

        private async Task LoadLookupData()
        {
            var isLookupExists = _contextAccessor.HttpContext.Session.GetObject<bool>(SynapseSession.IsFormularyLookupExists);

            if (!isLookupExists)
                await _formularyLookupService.LoadLookUps();
        }


        public async Task<IActionResult> FormularyList()
        {
            await LoadLookupData();

            string token = HttpContext.Session.GetString("access_token");

            var recordStatusDictionary = HttpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.FormularyRecStatusLkpKey);
            var formularyStatusDictionary = HttpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.FormularyStatusLkpKey);

            if (!formularyStatusDictionary.IsCollectionValid() || !recordStatusDictionary.IsCollectionValid())
            {
                var recordStatusLkpTask = TerminologyAPIService.GetRecordStatusLookup(token);

                var formularyStatusLkpTask = TerminologyAPIService.GetFormularyStatusLookup(token);

                await Task.WhenAll(recordStatusLkpTask, formularyStatusLkpTask);

                var recordStatusLkp = await recordStatusLkpTask;

                if (recordStatusLkp.IsCollectionValid())
                {
                    recordStatusDictionary = recordStatusLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    HttpContext.Session.SetObject(SynapseSession.FormularyRecStatusLkpKey, recordStatusDictionary);
                }

                var formularyStatusLkp = await formularyStatusLkpTask;

                if (formularyStatusLkp.IsCollectionValid())
                {
                    formularyStatusDictionary = formularyStatusLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    HttpContext.Session.SetObject(SynapseSession.FormularyStatusLkpKey, formularyStatusDictionary);
                }
            }

            var vm = new FormularyListFilterModel();

            var statusItems = new List<SelectListItem>();
            if (recordStatusDictionary != null)
            {
                recordStatusDictionary.Keys.Each(K =>
                {
                    statusItems.Add(new SelectListItem() { Value = $"Rec|{K}", Text = recordStatusDictionary[K], Group = new SelectListGroup() { Name = "RecordStatus" } });
                });
            }
            if (formularyStatusDictionary != null)
            {
                formularyStatusDictionary.Keys.Each(K =>
                {
                    statusItems.Add(new SelectListItem() { Value = $"Form|{K}", Text = formularyStatusDictionary[K], Group = new SelectListGroup() { Name = "FormularyStatus" } });
                });
            }
            var possibleFlags = FormularyListSearchCriteria.GetPossibleFlags();

            if (possibleFlags.IsCollectionValid())
            {
                possibleFlags.Keys.Each(K =>
                {
                    statusItems.Add(new SelectListItem() { Value = $"Flags|{K}", Text = possibleFlags[K], Group = new SelectListGroup() { Name = "Flags" } });
                });
            }
            vm.FilterStatuses = statusItems.FindAll(i => i.Value == "Rec|001" || i.Value == "Rec|002" || i.Value == "Rec|003" || i.Value == "Form|001" || i.Value == "Form|002" || (i.Group != null && i.Group.Name == "Flags"));

            vm.SelectedFilterStatuses = new List<string>();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFormularyStatus(string formularyVersionId, string reason, string status)
        {
            string token = HttpContext.Session.GetString("access_token");

            UpdateFormularyStatusAPIRequest request = new UpdateFormularyStatusAPIRequest();

            request.RequestData = new List<UpdateFormularyStatusAPRequestData>()
            {
                new UpdateFormularyStatusAPRequestData
                {
                    FormularyVersionId = formularyVersionId,
                    RecordStatusCode =status,
                    RecordStatusCodeChangeMsg = reason
                }
            };

            var resultResponse = await TerminologyAPIService.UpdateFormularyStatus(request, token);

            if (resultResponse == null)
            {
                _toastNotification.AddErrorToastMessage(UNKNOWN_SAVE_STATUS_MSG);
                return (Json(null));
            }

            if (resultResponse.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                string errors = "Error updating the status.";

                if (resultResponse.ErrorMessages.IsCollectionValid())
                    errors += string.Join('\n', resultResponse.ErrorMessages.ToArray());

                _toastNotification.AddErrorToastMessage(errors);

                return Json(null);
            }

            if (resultResponse.StatusCode == DataService.APIModel.StatusCode.Success)
            {
                if (resultResponse.Data.Status != null && resultResponse.Data.Status.ErrorMessages.IsCollectionValid())
                {
                    var errors = string.Join('\n', resultResponse.Data.Status.ErrorMessages);

                    _toastNotification.AddInfoToastMessage(errors);
                    return Json(null);
                }
            }

            var recordStatuses = await _formularyLookupService.GetRecordStatusLookup();//Let it throw the error if not exist

            var statusName =  recordStatuses?[status];
            _toastNotification.AddSuccessToastMessage(STATUS_SUCCESS_MSG.ToFormat(statusName));

            return Json(new List<string> { "success" });
        }

        [HttpPost]
        public async Task<IActionResult> BulkUpdateFormularyStatus(UpdateFormularyStatusAPIRequest request)
        {
            string token = HttpContext.Session.GetString("access_token");

            var resultResponse = await TerminologyAPIService.BulkUpdateFormularyStatus(request, token);

            if (resultResponse == null)
            {
                //_toastNotification.AddErrorToastMessage(UNKNOWN_SAVE_STATUS_MSG);
                return (Json(null));
            }

            if (resultResponse.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                string errors = "";

                if (resultResponse.ErrorMessages.IsCollectionValid())
                    errors += string.Join('\n', resultResponse.ErrorMessages.ToArray());

                //_toastNotification.AddErrorToastMessage(errors);
                
                return Json(errors);
            }

            if (resultResponse.StatusCode == DataService.APIModel.StatusCode.Success)
            {
                if (resultResponse.Data.Status != null && resultResponse.Data.Status.ErrorMessages.IsCollectionValid())
                {
                    var errors = string.Join('\n', resultResponse.Data.Status.ErrorMessages);

                    //_toastNotification.AddInfoToastMessage(errors);
                    return Json(errors);
                }
            }

            return Json(new List<string> { "success" });
        }
    }
}
