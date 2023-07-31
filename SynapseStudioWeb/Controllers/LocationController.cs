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
    public class LocationController : Controller
    {
        private IToastNotification toastNotification;

        public LocationController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
        }

        [StudioExceptionFilter()]
        public IActionResult LocationList()
        {
            string token = HttpContext.Session.GetString("access_token");
            List<LocationModel> locations = APIservice.GetList<LocationModel>("synapsenamespace=core&synapseentityname=location", token).OrderBy(l => l.locationcode).ToList();

            return View(locations);
        }

        [StudioExceptionFilter()]
        public IActionResult NewSpecialty()
        {
            LocationAddEditViewModel locationAddEditViewModel = new LocationAddEditViewModel();
            locationAddEditViewModel.location = null;
            locationAddEditViewModel.locationTypes = GetLocationTypes();

            return View("AddEditLocation", locationAddEditViewModel);
        }

        [StudioExceptionFilter()]
        public ActionResult Edit(string id)
        {
            string token = HttpContext.Session.GetString("access_token");

            List<LocationModel> locations = APIservice.GetListById<LocationModel>(id, "synapsenamespace=core&synapseentityname=location&synapseattributename=location_id&attributevalue=", token);

            LocationAddEditViewModel locationAddEditViewModel = new LocationAddEditViewModel();
            locationAddEditViewModel.location = locations.Select(s => s).FirstOrDefault();
            locationAddEditViewModel.locationTypes = GetLocationTypes();

            return View("AddEditLocation", locationAddEditViewModel);
        }

        [HttpPost]
        [StudioExceptionFilter()]
        public ActionResult SaveLocation(LocationAddEditViewModel locationAddEditViewModel)
        {
            string token = HttpContext.Session.GetString("access_token");

            string sql = string.Empty;

            locationAddEditViewModel.location.locationtypetext = GetLocationTypes().Where(lt => lt.locationtypecode == 
                locationAddEditViewModel.location.locationtypecode).Select(lt => lt.locationtypetext).FirstOrDefault();

            if (locationAddEditViewModel.location.location_id is null)
            {
                List<LocationModel> locations = APIservice.GetListById<LocationModel>(locationAddEditViewModel.location.locationcode, "synapsenamespace=core&synapseentityname=location&synapseattributename=locationcode&attributevalue=", token);

                if (locations.Count > 0)
                {
                    this.toastNotification.AddErrorToastMessage("Location Code already exists.");

                    locationAddEditViewModel.location = null;
                    locationAddEditViewModel.locationTypes = GetLocationTypes();

                    return View("AddEditLocation", locationAddEditViewModel);
                }

                locationAddEditViewModel.location.location_id = System.Guid.NewGuid().ToString();
            }

            locationAddEditViewModel.location.statustext = locationAddEditViewModel.location.statuscode;

            string result = APIservice.PostObject<LocationModel>(locationAddEditViewModel.location, "synapsenamespace=core&synapseentityname=location", token);

            if (result == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Location is successfully saved.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while saving Location.");
            }

            locationAddEditViewModel.locationTypes = GetLocationTypes();

            return View("AddEditLocation", locationAddEditViewModel);
        }

        [StudioExceptionFilter()]
        public ActionResult Delete(string id)
        {
            string token = HttpContext.Session.GetString("access_token");
            string result = APIservice.DeleteObject(id, "synapsenamespace=core&synapseentityname=location&id=", token);

            if (result == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Location is successfully deleted.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while deleting the Location.");
            }

            return RedirectToAction("LocationList");
        }

        private List<LocationType> GetLocationTypes()
        {
            string token = HttpContext.Session.GetString("access_token");

            return APIservice.GetList<LocationType>("synapsenamespace=meta&synapseentityname=locationtype", token).OrderBy(l => l.locationtypetext).ToList();
        }
    }
}
