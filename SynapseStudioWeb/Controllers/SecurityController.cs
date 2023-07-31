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

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {
        public IActionResult SecurityManager()
        {
            ViewBag.Clients = LoadClients();
            ViewBag.IdentityClaims = LoadIdentityClaims();
            ViewBag.APIResources = LoadAPIResources();
            ViewBag.Roles = LoadRoles();
            return View();
        }
        protected DataTable LoadClients()
        {
            string sql = "SELECT \"Id\", \"ClientId\", case when \"Enabled\" = 'True' then 'Yes' else 'No' end as enabled FROM \"Clients\" ORDER BY \"ClientId\";";
            var paramList = new List<KeyValuePair<string, string>>();

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");
            return  ds.Tables[0];

        }

        protected DataTable LoadIdentityClaims()
        {
            string sql = "SELECT ir.\"Name\" as resourcename, ic.\"Type\" as claimname FROM \"IdentityResources\" ir INNER JOIN \"IdentityClaims\" ic on (ir.\"Id\" = ic.\"IdentityResourceId\")  ORDER BY claimname;";
            var paramList = new List<KeyValuePair<string, string>>();

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");
          
            return ds.Tables[0];
        }

        protected DataTable LoadAPIResources()
        {
            string sql = "SELECT ar.\"Name\" as resourcename, aps.\"Name\" as scopename, aps.\"Description\" as scopedescription FROM \"ApiResources\" ar INNER JOIN \"ApiScopes\" aps on (ar.\"Id\" = aps.\"ApiResourceId\")  ORDER BY scopename;";
            var paramList = new List<KeyValuePair<string, string>>();

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");
            return ds.Tables[0];

        }

        protected DataTable LoadRoles()
        {
            string sql = "SELECT \"Name\" as rolename, \"Id\" as id FROM \"AspNetRoles\" ORDER BY rolename;";
            var paramList = new List<KeyValuePair<string, string>>();

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");
            return ds.Tables[0];

        }
    }
}