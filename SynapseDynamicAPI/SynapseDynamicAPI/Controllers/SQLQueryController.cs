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


﻿

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text;
using SynapseDynamicAPI.Services;
using SynapseDynamicAPI.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;

namespace SynapseDynamicAPI.Controllers
{

    [Authorize]
    public class SQLConnector : Controller
    {
        protected string dbStore { get; set; }

        [HttpPost]
        [Route("[action]")]
        public string ExecuteNonSQL([FromBody] string data)
        {
            var parameterList = new List<KeyValuePair<string, object>>();

            int index = 0;

            string returnValue = string.Empty;

            if (!string.IsNullOrEmpty(data))
            {
                var dataDict = JsonConvert.DeserializeObject<SQLData>(data);

                if (dataDict.parameters != null && dataDict.parameters.Count > 0)
                {
                    foreach (var item in dataDict.parameters)
                    {
                        parameterList.Insert(index, new KeyValuePair<string, object>(item.name, item.value));
                        index++;
                    }
                }

                try
                {
                    returnValue = DataServices.ExcecuteNonQuerySQL(dataDict.query, dataDict.returnColumn, parameterList, databaseName: dbStore);
                }
                catch (Exception ex)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    returnValue = "{ \"Status\": \"Failed\", \"Message\": \"" + ex.Message + ex.StackTrace + "\", \"Value\": \"\"}";
                }
            }

            return returnValue;
        }

        [HttpPost]
        [Route("[action]")]
        public string ExecuteSQL([FromBody] string data)
        {
            var parameterList = new List<KeyValuePair<string, object>>();

            int index = 0;

            string returnValue = string.Empty;

            if (!string.IsNullOrEmpty(data))
            {
                var dataDict = JsonConvert.DeserializeObject<SQLData>(data);

                if (dataDict.parameters != null && dataDict.parameters.Count > 0)
                {
                    foreach (var item in dataDict.parameters)
                    {
                        parameterList.Insert(index, new KeyValuePair<string, object>(item.name, item.value));
                        index++;
                    }
                }

                try
                {
                    DataSet ds = DataServices.DataSetFromSQL(dataDict.query, parameterList, databaseName: dbStore);
                    returnValue = DataServices.ConvertDataTabletoJSONString(ds.Tables[0]);
                }
                catch (Exception ex)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    returnValue = "{ \"Status\": \"Failed\", \"Message\": \"" + ex.Message + ex.StackTrace + "\", \"Value\": \"\"}";
                }
            }

            return returnValue;
        }

        [HttpGet]
        [Route("[action]")]
        public string test()
        { return "Test"; }

    }

    [Route("SQL/Identity/")]
    [Authorize]
    public class IdentityDataConnector : SQLConnector
    {
        public IdentityDataConnector()
        {
            dbStore = "connectionString_SynapseIdentityStore";
        }
    }

    [Route("SQL/")]
    [Route("SQL/Clinical/")]
    [Authorize]
    public class SynapseDataConnector : SQLConnector
    {
        public SynapseDataConnector()
        {
            dbStore = "connectionString_SynapseDataStore";
        }
    }

    public class SQLData
    {
        public string query { get; set; }
        public string returnColumn { get; set; }
        public List<ParametersData> parameters { get; set; }
    }

    public class ParametersData
    {
        public string name { get; set; }
        public object value { get; set; }
    }
}
