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
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SynapseStudioWeb.AppCode.Filters;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SynapseStudioWeb.Controllers
{
    public class SpecialtyController : Controller
    {
        private IToastNotification toastNotification;

        public SpecialtyController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
        }

        [StudioExceptionFilter()]
        public IActionResult SpecialtyList()
        {
            string token = HttpContext.Session.GetString("access_token");
            List<SpecialtyModel> specialties = APIservice.GetList<SpecialtyModel>("synapsenamespace=core&synapseentityname=specialty", token).OrderBy(s => s.specialtycode).ToList();

            return View(specialties);
        }

        [StudioExceptionFilter()]
        public IActionResult NewSpecialty()
        {
            return View("AddEditSpecialty");
        }

        [StudioExceptionFilter()]
        public ActionResult Edit(string id)
        {
            string token = HttpContext.Session.GetString("access_token");

            List<SpecialtyModel> specialties = APIservice.GetListById<SpecialtyModel>(id, "synapsenamespace=core&synapseentityname=specialty&synapseattributename=specialty_id&attributevalue=", token);

            SpecialtyModel specialty = specialties.Select(s => s).FirstOrDefault();

            return View("AddEditSpecialty", specialty);
        }

        [HttpPost]
        [StudioExceptionFilter()]
        public ActionResult SaveSpecialty(SpecialtyModel specialtyModel)
        {
            string token = HttpContext.Session.GetString("access_token");

            string sql = string.Empty;

            if (specialtyModel.specialty_id is null)
            {
                List<SpecialtyModel> specialties = APIservice.GetListById<SpecialtyModel>(specialtyModel.specialtycode, "synapsenamespace=core&synapseentityname=specialty&synapseattributename=specialtycode&attributevalue=", token);

                if (specialties.Count > 0)
                {
                    this.toastNotification.AddErrorToastMessage("Specialty Code already exists.");

                    specialtyModel = null;

                    return View("AddEditSpecialty", specialtyModel);
                }

                specialtyModel.specialty_id = System.Guid.NewGuid().ToString();
            }

            specialtyModel.statustext = specialtyModel.statuscode;

            string result = APIservice.PostObject<SpecialtyModel>(specialtyModel, "synapsenamespace=core&synapseentityname=specialty", token);

            if (result == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Specialty details is successfully saved.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while saving Specialty details.");
            }

            return View("AddEditSpecialty", specialtyModel);
        }

        [StudioExceptionFilter()]
        public ActionResult Delete(string id)
        {
            string token = HttpContext.Session.GetString("access_token");
            string result = APIservice.DeleteObject(id, "synapsenamespace=core&synapseentityname=specialty&id=", token);

            if (result == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Specialty details is successfully deleted.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while deleting the Specialty details.");
            }

            return RedirectToAction("SpecialtyList");
        }
    }
}
