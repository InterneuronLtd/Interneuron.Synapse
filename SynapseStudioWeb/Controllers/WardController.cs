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
    public class WardController : Controller
    {
        private IToastNotification toastNotification;

        public WardController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
        }

        [StudioExceptionFilter()]
        public IActionResult WardList()
        {
            string token = HttpContext.Session.GetString("access_token");
            List<WardModel> wardList = APIservice.GetList<WardModel>("synapsenamespace=meta&synapseentityname=ward", token).OrderBy(w => w.wardcode).ToList();
            return View(wardList);
        }

        [StudioExceptionFilter()]
        public IActionResult NewWard()
        {
            return View("Edit");
        }

        [StudioExceptionFilter()]
        public ActionResult Edit(string id)
        {
            string token = HttpContext.Session.GetString("access_token");
            List<WardModel> ward = APIservice.GetListById<WardModel>(id, "synapsenamespace=meta&synapseentityname=ward&synapseattributename=ward_id&attributevalue=", token);

            return View(ward.Select(w => w).First());
        }

        [HttpPost]
        [StudioExceptionFilter()]
        public ActionResult AddEditWard(WardModel wardModel)
        {
            string token = HttpContext.Session.GetString("access_token");

            string context = string.Empty;
            string sql = string.Empty;

            if (wardModel.ward_id is null)
            {
                List<WardModel> ward = APIservice.GetListById<WardModel>(wardModel.wardcode,
                    "synapsenamespace=meta&synapseentityname=ward&synapseattributename=wardcode&attributevalue=", token);

                if (ward.Count > 0)
                {
                    this.toastNotification.AddErrorToastMessage("Ward Code already exists for the ward.");

                    wardModel = null;

                    return View("Edit", wardModel);
                }

                context = "ADD";
                wardModel.ward_id = System.Guid.NewGuid().ToString();
            }
            else
            {
                context = "UPDATE";
            }
            
            string result = APIservice.PostObject<WardModel>(wardModel, "synapsenamespace=meta&synapseentityname=ward", token);

            if (result == "OK")
            {
                if (context == "ADD")
                {
                    sql = "INSERT INTO entitystorematerialised.meta_personacontext(personacontext_id, displayname, contextname, persona_id, displayorder) ";
                    sql += "SELECT ward_id, warddisplay, wardcode, '4', ROW_NUMBER() OVER(ORDER BY warddisplay) FROM entitystorematerialised.meta_ward ";
                    sql += "WHERE wardcode = @wardcode;";

                    var paramList = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("wardcode", wardModel.wardcode)
                    };

                    DataServices.executeSQLStatement(sql, paramList);
                }
                else
                {
                    sql = "UPDATE entitystorematerialised.meta_personacontext SET displayname = @warddisplay WHERE contextname = @wardcode";

                    var paramList = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("warddisplay", wardModel.warddisplay),
                    new KeyValuePair<string, string>("wardcode", wardModel.wardcode)
                    };

                    DataServices.executeSQLStatement(sql, paramList);
                }                

                this.toastNotification.AddSuccessToastMessage("Ward is successfully saved.");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage("Error while saving ward details.");
            }

            return View("Edit", wardModel);
        }

        [StudioExceptionFilter()]
        public ActionResult Delete(string id)
        {
            string token = HttpContext.Session.GetString("access_token");
            string result = APIservice.DeleteObject(id, "synapsenamespace=meta&synapseentityname=ward&id=", token);

            if (result == "OK")
            {
                string sql = "DELETE FROM entitystorematerialised.meta_personacontext WHERE personacontext_id = @wardid";

                var paramList = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("wardid", id)
                    };

                DataServices.executeSQLStatement(sql, paramList);

                this.toastNotification.AddSuccessToastMessage("Ward is successfully deleted.");
            }

            return RedirectToAction("WardList");
        }
    }
}
