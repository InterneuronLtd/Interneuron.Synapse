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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using SynapseStudioWeb.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class DevOpsController : Controller
    {
        public IActionResult DevOpsDashboard()
        {            
            return View();
        }
        public IActionResult DevOpsActivity()
        {
            string dbActivitySQL = "SELECT * FROM pg_stat_activity WHERE datname='SYNAPSE_DATABASE_NAME';";
            var paramList = new List<KeyValuePair<string, string>>();

            DataSet dsActivity = DataServices.DataSetFromSQL(dbActivitySQL, paramList);            
            List<DatabaseActivityDto> DatabaseActivityDto = dsActivity.Tables[0].ToList<DatabaseActivityDto>();
            
            return View("_DevOpsActivity", DatabaseActivityDto);
        }
        public IActionResult DevOpsReplStatus()
        {
            //string replStatusSQL = "select pid, usename, client_addr, client_port, backend_start, sent_location, write_location, flush_location, replay_location from pg_stat_replication;";
            string replStatusSQL = "select pid, usename, client_addr, client_port, backend_start, sent_lsn as sent_location, " +
                "write_lsn as write_location, flush_lsn as flush_location, replay_lsn as replay_location from pg_stat_replication;";

            var paramList = new List<KeyValuePair<string, string>>();

            DataSet dsActivity = DataServices.DataSetFromSQL(replStatusSQL, paramList);
            List<DatabaseReplStatusDto> DatabaseActivityDto = dsActivity.Tables[0].ToList<DatabaseReplStatusDto>();

            return View("_DevOpsReplStatus", DatabaseActivityDto);
        }        
    }
}
