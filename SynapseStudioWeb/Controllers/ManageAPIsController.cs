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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class ManageAPIsController : Controller
    {
        public IActionResult ApIManager()
        {
            ViewBag.APIresource = LoadAPIManager();
            return View();
        }
        protected DataTable LoadAPIManager()
        {
            string sql = "SELECT ir.\"Name\" , ir.\"DisplayName\" , ir.\"Description\", ir.\"Enabled\", ir.\"Id\" FROM \"ApiResources\" ir  ORDER BY \"Name\";";
            
            DataSet ds = DataServices.DataSetFromSQL(sql, null,"connectionString_SynapseIdentityStore");

            return ds.Tables[0];


        }
        public ActionResult CreateApIManager(ManageAPIsModel model)
        {
            string sql = "INSERT INTO \"ApiResources\" (\"Name\",\"DisplayName\",\"Enabled\",\"Description\") VALUES ( @name,@displayname,cast(@enabled as bool),@description);";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("name",model.APIName),
                new KeyValuePair<string, string>("displayname",model.DisplayName),
                 new KeyValuePair<string, string>("enabled",model.Enabled.ToString()),
                  new KeyValuePair<string, string>("description",model.Description)
                            };
            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("ApIManager");
        }

        public ActionResult toggleAPIResource(string id, string toggle)
        {
            string sql = "update  \"ApiResources\" set \"Enabled\" = cast(@status as bool) where \"Id\" = cast(@id as int);";

            var paramList = new List<KeyValuePair<string, string>>() {
                  new KeyValuePair<string, string>("status",toggle.ToLower() == "true"? "false":"true"),
                new KeyValuePair<string, string>("id", id),
                            };
            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("ApIManager");

        }

        public IActionResult ApIScopeManager(string id,string name)
        {
            APIscopeModel model = new APIscopeModel();
            model.Apiid = id;
            model.Apiname = name;
            ViewBag.APIScoperesource = LoadScopeAPI(id);
            return View(model);
        }
        protected DataTable LoadScopeAPI(string id)
        {
            string sql = "SELECT ir.\"Name\" , ir.\"DisplayName\" , ir.\"Description\", ir.\"Id\" FROM \"ApiScopes\" ir where \"ApiResourceId\" = cast('" + id.ToString() + "' as int)  ORDER BY \"Name\";";

            DataSet ds = DataServices.DataSetFromSQL(sql, null, "connectionString_SynapseIdentityStore");

            return ds.Tables[0];


        }
        public ActionResult CreateApIScope(APIscopeModel model)
        {
            string sql = "INSERT INTO \"ApiScopes\" (\"Name\",\"DisplayName\",\"Description\",\"Required\",\"Emphasize\",\"ShowInDiscoveryDocument\",\"ApiResourceId\") VALUES ( @name,@displayname,@description,false,false,true,cast(@apiid as int));";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("name",model.APIScopeName),
                new KeyValuePair<string, string>("displayname", model.DisplayName),
                                   new KeyValuePair<string, string>("description", model.Description),
                    new KeyValuePair<string, string>("apiid",model.Apiid)
                            };
            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("ApIScopeManager", "ManageAPIs", new { @id = model.Apiid,@name= model.Apiname});
        }
        public ActionResult RemoveAPIScope(string id,string name)
        {
            string sql = "delete from   \"ApiScopes\" where \"Id\" = cast(@id as int);";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("id", id)
                            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {
               
            }
            return RedirectToAction("ApIScopeManager", "ManageAPIs", new { @id =id, @name = name });

        }
    }
}