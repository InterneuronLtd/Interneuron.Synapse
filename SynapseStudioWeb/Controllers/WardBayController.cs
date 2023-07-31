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
    public class WardBayController : Controller
    {
        private IToastNotification toastNotification;

        public WardBayController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
        }

        [StudioExceptionFilter()]
        public IActionResult WardBayList()
        {
            string token = HttpContext.Session.GetString("access_token");

            List<WardModel> wardList = GetWards();
            List<WardBayModel> wardBayList = APIservice.GetList<WardBayModel>("synapsenamespace=meta&synapseentityname=wardbay", token);

            WardBayViewModel wardBayViewModel = new WardBayViewModel();
            wardBayViewModel.Wards = wardList;
            wardBayViewModel.WardBays = wardBayList.OrderBy(w => w.wardcode).ThenBy(w => w.warddisplay).ThenBy(w => w.baycode).ToList();

            return View(wardBayViewModel);
        }

        [HttpPost]
        [StudioExceptionFilter()]
        public IActionResult GetWardBaysForWard(WardBayViewModel wardBay)
        {
            string token = HttpContext.Session.GetString("access_token");

            List<WardModel> wardList = GetWards();
            List<WardBayModel> wardBayList = new List<WardBayModel>();

            if (wardBay.SelectedWardCode is null)
            {
                wardBayList = APIservice.GetList<WardBayModel>("synapsenamespace=meta&synapseentityname=wardbay", token);
            }
            else
            {
                wardBayList = APIservice.GetListById<WardBayModel>(wardBay.SelectedWardCode, "synapsenamespace=meta&synapseentityname=wardbay&synapseattributename=wardcode&attributevalue=", token);
            }

            WardBayViewModel wardBayViewModel = new WardBayViewModel();
            wardBayViewModel.Wards = wardList;
            wardBayViewModel.WardBays = wardBayList.OrderBy(w => w.wardcode).ThenBy(w => w.warddisplay).ThenBy(w => w.baycode).ToList();

            return View("WardBayList", wardBayViewModel);
        }

        [StudioExceptionFilter()]
        public IActionResult AddWardBay()
        {
            AddEditWardBayViewModel addEditWardBay = new AddEditWardBayViewModel();
            addEditWardBay.Wards = GetWards();

            return View("AddEditWardBay", addEditWardBay);
        }

        [HttpPost]
        [StudioExceptionFilter()]
        public IActionResult SaveWardBay(AddEditWardBayViewModel addEditWardBayViewModel)
        {
            string token = HttpContext.Session.GetString("access_token");

            if (addEditWardBayViewModel.WardBay.wardbay_id is null)
            {
                List<WardBayModel> wardBays = APIservice.GetListById<WardBayModel>(addEditWardBayViewModel.WardBay.wardcode,
                            "synapsenamespace=meta&synapseentityname=wardbay&synapseattributename=wardcode&attributevalue=", token);

                if(wardBays.Where(w => w.baycode == addEditWardBayViewModel.WardBay.baycode.Trim()).ToList().Count > 0)
                {
                    AddEditWardBayViewModel addEditWardBay = new AddEditWardBayViewModel();
                    addEditWardBay.Wards = GetWards();
                    addEditWardBay.WardBay = null;

                    this.toastNotification.AddErrorToastMessage("Bay Code already exists for the ward.");

                    addEditWardBay.WardBay = null;

                    return View("AddEditWardBay", addEditWardBay);
                }
                else
                {
                    addEditWardBayViewModel.WardBay.wardbay_id = System.Guid.NewGuid().ToString();
                }
            }

            List<WardModel> wards = APIservice.GetListById<WardModel>(addEditWardBayViewModel.WardBay.wardcode,
                            "synapsenamespace=meta&synapseentityname=ward&synapseattributename=wardcode&attributevalue=", token);

            addEditWardBayViewModel.WardBay.warddisplay = wards.Select(w => w.warddisplay).FirstOrDefault();

            string result = APIservice.PostObject<WardBayModel>(addEditWardBayViewModel.WardBay, "synapsenamespace=meta&synapseentityname=wardbay", token);

            if (result == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Ward Bay details saved successfully.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while saving Ward Bay details");
            }

            addEditWardBayViewModel.Wards = GetWards();

            return View("AddEditWardBay", addEditWardBayViewModel);
        }

        [StudioExceptionFilter()]
        public IActionResult Edit(string id)
        {
            string token = HttpContext.Session.GetString("access_token");

            AddEditWardBayViewModel addEditWardBay = new AddEditWardBayViewModel();
            addEditWardBay.Wards = GetWards();
            addEditWardBay.WardBay = APIservice.GetListById<WardBayModel>(id,
                "synapsenamespace=meta&synapseentityname=wardbay&synapseattributename=wardbay_id&attributevalue=", token).FirstOrDefault();

            return View("AddEditWardBay", addEditWardBay);
        }

        [StudioExceptionFilter()]
        public IActionResult Delete(string id)
        {
            string token = HttpContext.Session.GetString("access_token");

            string result = APIservice.DeleteObject(id, "synapsenamespace=meta&synapseentityname=wardbay&id=", token);

            if (result == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Ward Bay is deleted successfully.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while deleting the Ward Bay");
            }

            return RedirectToAction("WardBayList");
        }

        [NonAction]
        private List<WardModel> GetWards()
        {
            string token = HttpContext.Session.GetString("access_token");

            return APIservice.GetList<WardModel>("synapsenamespace=meta&synapseentityname=ward", token).OrderBy(w => w.warddisplay).ToList();
        }
    }
}
