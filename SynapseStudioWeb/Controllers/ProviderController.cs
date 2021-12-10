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


﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using NToastNotify;
using SynapseStudioWeb.AppCode.Filters;

namespace SynapseStudioWeb.Controllers
{
    public class ProviderController : Controller
    {
        private IToastNotification toastNotification;

        public ProviderController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
        }

        [StudioExceptionFilter()]
        public IActionResult ProviderList()
        {
            string token = HttpContext.Session.GetString("access_token");
            List<ProviderModel> providers = APIservice.GetList<ProviderModel>("synapsenamespace=core&synapseentityname=provider", token).OrderBy(p => p.firstname).ToList();
            return View(providers);
        }

        [StudioExceptionFilter()]
        public IActionResult NewProvider()
        {
            ProviderAddEditViewModel providerAddEditViewModel = new ProviderAddEditViewModel();

            providerAddEditViewModel.providerroles = GetProviderRoles();

            return View("AddEditProvider", providerAddEditViewModel);
        }

        [StudioExceptionFilter()]
        public ActionResult Edit(string id)
        {
            ProviderAddEditViewModel providerAddEditViewModel = new ProviderAddEditViewModel();
            string token = HttpContext.Session.GetString("access_token");

            List<ProviderModel> provider = APIservice.GetListById<ProviderModel>(id, "synapsenamespace=core&synapseentityname=provider&synapseattributename=provider_id&attributevalue=", token);
            providerAddEditViewModel.provider = provider.Select(w => w).First();

            providerAddEditViewModel.providerroles = GetProviderRoles();

            return View("AddEditProvider", providerAddEditViewModel);
        }

        [HttpPost]
        [StudioExceptionFilter()]
        public ActionResult SaveProvider(ProviderAddEditViewModel providerAddEditViewModel)
        {
            string token = HttpContext.Session.GetString("access_token");

            string sql = string.Empty;

            if (providerAddEditViewModel.provider.provider_id is null)
            {
                providerAddEditViewModel.provider.provider_id = System.Guid.NewGuid().ToString();
            }

            string result = APIservice.PostObject<ProviderModel>(providerAddEditViewModel.provider, "synapsenamespace=core&synapseentityname=provider", token);

            if (result == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Provider details is successfully saved.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while saving Provider details.");
            }

            providerAddEditViewModel.providerroles = GetProviderRoles();

            return View("AddEditProvider", providerAddEditViewModel);
        }

        [StudioExceptionFilter()]
        public ActionResult Delete(string id)
        {
            string token = HttpContext.Session.GetString("access_token");
            string result = APIservice.DeleteObject(id, "synapsenamespace=core&synapseentityname=provider&id=", token);

            if (result == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Provider details is successfully deleted.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while deleting the Provider details.");
            }

            return RedirectToAction("ProviderList");
        }

        private List<string> GetProviderRoles()
        {
            string token = HttpContext.Session.GetString("access_token");
            
            List<ProviderRole> providerRoles = APIservice.GetList<ProviderRole>("synapsenamespace=meta&synapseentityname=providerrole", token);

            return providerRoles.Select(p => p.role).ToList();
        }
    }
}
