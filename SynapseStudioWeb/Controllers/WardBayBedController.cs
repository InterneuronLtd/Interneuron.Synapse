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


﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SynapseStudioWeb.AppCode.Filters;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace SynapseStudioWeb.Controllers
{
    public class WardBayBedController : Controller
    {
        private IToastNotification toastNotification;

        public WardBayBedController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
        }

        [StudioExceptionFilter()]
        public IActionResult WardBayBedList()
        {
            string token = HttpContext.Session.GetString("access_token");

            WardBayBedIndexViewModel wardBayBedIndexViewModel = new WardBayBedIndexViewModel();
            wardBayBedIndexViewModel.Wards = GetWards();
            wardBayBedIndexViewModel.WardBays = new List<WardBayModel>();

            wardBayBedIndexViewModel.WardBayBeds = APIservice.GetList<WardBayBedModel>("synapsenamespace=meta&synapseentityname=wardbaybed", token).OrderBy(w => w.wardcode).ThenBy(w => w.baycode).ThenBy(w => w.bedcode).ToList();

            return View(wardBayBedIndexViewModel);
        }

        [HttpPost]
        [StudioExceptionFilter()]
        public IActionResult SearchWardBayBeds(WardBayBedIndexViewModel wardBayBedIndexView)
        {
            WardBayBedIndexViewModel wardBayBedIndexViewModel = new WardBayBedIndexViewModel();

            wardBayBedIndexViewModel.Wards = GetWards();
            wardBayBedIndexViewModel.WardBays = GetWardBays(wardBayBedIndexView.selectedWardCode);
            wardBayBedIndexViewModel.WardBayBeds = GetWardBayBeds(wardBayBedIndexView.selectedWardCode, wardBayBedIndexView.selectedBayCode);

            return View("WardBayBedList", wardBayBedIndexViewModel);
        }

        [StudioExceptionFilter()]
        public IActionResult AddWardBayBed()
        {
            WardBayBedAddEditViewModel addWardBayBed = new WardBayBedAddEditViewModel();
            addWardBayBed.Wards = GetWards();
            addWardBayBed.WardBays = new List<WardBayModel>();
            //addWardBayBed.WardBayBed = new WardBayBedModel();

            return View("AddEditWardBayBed", addWardBayBed);
        }

        [StudioExceptionFilter()]
        public IActionResult Edit(string id)
        {
            string token = HttpContext.Session.GetString("access_token");

            WardBayBedAddEditViewModel editWardBayBed = new WardBayBedAddEditViewModel();
            editWardBayBed.Wards = GetWards();

            editWardBayBed.WardBayBed = APIservice.GetListById<WardBayBedModel>(id,
                "synapsenamespace=meta&synapseentityname=wardbaybed&synapseattributename=wardbaybed_id&attributevalue=", token).FirstOrDefault();

            editWardBayBed.WardBays = GetWardBays(editWardBayBed.WardBayBed.wardcode);

            return View("AddEditWardBayBed", editWardBayBed);
        }

        [StudioExceptionFilter()]
        public IActionResult Delete(string id)
        {
            string token = HttpContext.Session.GetString("access_token");

            string result = APIservice.DeleteObject(id, "synapsenamespace=meta&synapseentityname=wardbaybed&id=", token);

            if (result == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Ward Bay Bed is deleted successfully.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while deleting the Ward Bay Bed");
            }

            return RedirectToAction("WardBayBedList");
        }

        [HttpPost]
        [StudioExceptionFilter()]
        public IActionResult SaveWardBayBed(WardBayBedAddEditViewModel wardBayBedAddEditViewModel)
        {
            string token = HttpContext.Session.GetString("access_token");

            if (wardBayBedAddEditViewModel.WardBayBed.wardbaybed_id is null)
            {
                List<WardBayBedModel> wardBayBeds = APIservice.GetListById<WardBayBedModel>(wardBayBedAddEditViewModel.WardBayBed.wardcode,
                    "synapsenamespace=meta&synapseentityname=wardbaybed&synapseattributename=wardcode&attributevalue=", token);

                if (wardBayBeds.Where(w => w.baycode == wardBayBedAddEditViewModel.WardBayBed.baycode
                         && w.bedcode == wardBayBedAddEditViewModel.WardBayBed.bedcode).ToList().Count > 0)
                {
                    this.toastNotification.AddErrorToastMessage("Bed Code already exists for the ward and bay.");

                    wardBayBedAddEditViewModel.Wards = GetWards();
                    wardBayBedAddEditViewModel.WardBays = new List<WardBayModel>();
                    wardBayBedAddEditViewModel.WardBayBed = null;

                    return View("AddEditWardBayBed", wardBayBedAddEditViewModel);
                }
                else
                {
                    wardBayBedAddEditViewModel.WardBayBed.wardbaybed_id = System.Guid.NewGuid().ToString();
                }

                List<WardModel> wards = APIservice.GetListById<WardModel>(wardBayBedAddEditViewModel.WardBayBed.wardcode,
                                "synapsenamespace=meta&synapseentityname=ward&synapseattributename=wardcode&attributevalue=", token);

                wardBayBedAddEditViewModel.WardBayBed.warddisplay = wards.Select(w => w.warddisplay).FirstOrDefault();

                List<WardBayModel> wardBays = APIservice.GetListById<WardBayModel>(wardBayBedAddEditViewModel.WardBayBed.baycode,
                                "synapsenamespace=meta&synapseentityname=wardbay&synapseattributename=baycode&attributevalue=", token);

                wardBayBedAddEditViewModel.WardBayBed.baydisplay = wardBays.Select(w => w.baydisplay).FirstOrDefault();
            }

            string result = APIservice.PostObject<WardBayBedModel>(wardBayBedAddEditViewModel.WardBayBed, "synapsenamespace=meta&synapseentityname=wardbaybed", token);

            if (result == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Ward Bay Bed details saved successfully.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while saving Ward Bay Bed details");
            }

            wardBayBedAddEditViewModel.WardBays = GetWardBays(wardBayBedAddEditViewModel.WardBayBed.wardcode);

            return View("AddEditWardBayBed", wardBayBedAddEditViewModel);
        }

        [NonAction]
        private List<WardModel> GetWards()
        {
            string token = HttpContext.Session.GetString("access_token");

            return APIservice.GetList<WardModel>("synapsenamespace=meta&synapseentityname=ward", token).OrderBy(w=>w.warddisplay).ToList();
        }

        [NonAction]
        private List<WardBayModel> GetWardBays(string wardCode)
        {
            List<WardBayModel> wardBays = new List<WardBayModel>();

            string token = HttpContext.Session.GetString("access_token");

            if (wardCode != null)
            {
                wardBays = APIservice.GetListById<WardBayModel>(wardCode,
                                "synapsenamespace=meta&synapseentityname=wardbay&synapseattributename=wardcode&attributevalue=", token).OrderBy(w=>w.baydisplay).ToList();
            }

            return wardBays;
        }

        [NonAction]
        private List<WardBayBedModel> GetWardBayBeds(string wardCode, string bayCode)
        {
            string token = HttpContext.Session.GetString("access_token");

            List<WardBayBedModel> wardBayBeds = new List<WardBayBedModel>();

            if (wardCode is null)
            {
                wardBayBeds = APIservice.GetList<WardBayBedModel>("synapsenamespace=meta&synapseentityname=wardbaybed", token);
            }
            else
            {
                wardBayBeds = APIservice.GetListById<WardBayBedModel>(wardCode,
                    "synapsenamespace=meta&synapseentityname=wardbaybed&synapseattributename=wardcode&attributevalue=", token);
            }

            if (bayCode is null)
            {
                return wardBayBeds.OrderBy(w => w.wardcode).ThenBy(w => w.baycode).ThenBy(w => w.bedcode).ToList();
            }
            else
            {
                return wardBayBeds.Where(w => w.baycode == bayCode).OrderBy(w => w.wardcode).ThenBy(w => w.baycode).ThenBy(w => w.bedcode).ToList();
            }
        }

        public JsonResult LoadWardBays(string wardCode)
        {
            List<WardBayModel> wardBays = new List<WardBayModel>();
            WardBayBedAddEditViewModel addWardBayBed = new WardBayBedAddEditViewModel();
            addWardBayBed.Wards = GetWards();
            addWardBayBed.WardBays = new List<WardBayModel>();

            string token = HttpContext.Session.GetString("access_token");

            if (wardCode != null && wardCode != "")
            {
                wardBays = APIservice.GetListById<WardBayModel>(wardCode,
                                "synapsenamespace=meta&synapseentityname=wardbay&synapseattributename=wardcode&attributevalue=", token);
            }

            return (Json(wardBays));
        }
    }
}
