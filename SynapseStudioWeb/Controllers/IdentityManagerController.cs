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
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using SynapseStudioWeb.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class IdentityManagerController : Controller
    {
        public IActionResult IdentityClaimsManager()
        {
            ViewBag.BaseviewNamespace = ToSelectList(IdentityResources(), "resourceid", "Name");
            ViewBag.Claims = LoadIdentityClaims();
            return View();
        }

        [HttpPost]
        public ActionResult CreatClaim(IFormCollection collection)
        {
            var Claimname = collection["Claimname"];
            var ResourceNameId = collection["ResourceNameId"];

            string sql = "INSERT INTO \"IdentityClaims\" (\"Type\", \"IdentityResourceId\") VALUES ( @type, cast(@resourceid as int));";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("type",Claimname),
                new KeyValuePair<string, string>("resourceid", ResourceNameId),
                            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");

                ViewBag.BaseviewNamespace = ToSelectList(IdentityResources(), "resourceid", "Name");
                ViewBag.Claims = LoadIdentityClaims();
            }
            catch (Exception ex)
            {
                //ShowError(ex.Message);
                //success = false;
            }

            return View("IdentityClaimsManager");
        }
      
        public ActionResult DeleteClaim(string id)
        {
            string sql = "delete from  \"IdentityClaims\" where \"Id\" = cast(@id as int);";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("id",id),
                            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {
                
            }

            return RedirectToAction("IdentityClaimsManager");
        }
        protected DataTable IdentityResources()
        {
            string sql = " Select ir.\"Name\" , ir.\"Id\" as resourceid FROM \"IdentityResources\" ir  where ir.\"Enabled\" = true order by ir.\"Name\";";


            DataSet ds = DataServices.DataSetFromSQL(sql, null, "connectionString_SynapseIdentityStore");
            return ds.Tables[0];


        }
        protected DataTable LoadIdentityClaims()
        {
            string sql = "SELECT ir.\"Name\" as resourcename, ic.\"Type\" as claimname ,ic.\"Id\" as claimid FROM \"IdentityResources\" ir INNER JOIN \"IdentityClaims\" ic on (ir.\"Id\" = ic.\"IdentityResourceId\")  ORDER BY \"Type\";";
            
            DataSet ds = DataServices.DataSetFromSQL(sql, null ,"connectionString_SynapseIdentityStore");
             return  ds.Tables[0];

            
        }
        [NonAction]
        public SelectList ToSelectList(DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }


        public IActionResult IdentityResourceManager()
        {
           
            ViewBag.resource = LoadIdentityResources();
            return View();
        }
        protected DataTable LoadIdentityResources()
        {
            string sql = "SELECT ir.\"Name\" , ir.\"DisplayName\" , ir.\"Description\", ir.\"Enabled\", ir.\"Id\" FROM \"IdentityResources\" ir  ORDER BY \"Name\";";
            
            DataSet ds = DataServices.DataSetFromSQL(sql, null, "connectionString_SynapseIdentityStore");
            return ds.Tables[0];      
            
        }

        public ActionResult CreatIdentityResources(IdentityResourceModel model)
        {
          
              
                string sql = "INSERT INTO \"IdentityResources\" (\"Name\",\"DisplayName\",\"Enabled\",\"Description\",\"Required\",\"Emphasize\",\"ShowInDiscoveryDocument\") VALUES ( @name,@displayname,cast(@enabled as bool),@description,false,false,true);";

                var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("name",model.ResourceName),
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
            return RedirectToAction("IdentityResourceManager");


        }

        public ActionResult toggleResource(string id , string toggle)
        {
            string sql = "update  \"IdentityResources\" set \"Enabled\" = cast(@status as bool) where \"Id\" = cast(@id as int);";

            var paramList = new List<KeyValuePair<string, string>>() {
                  new KeyValuePair<string, string>("status",toggle.ToString().ToLower() == "true"? "false":"true"),
                new KeyValuePair<string, string>("id", id),
                            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {
               
            }
            return RedirectToAction("IdentityResourceManager");

        }
    }
}