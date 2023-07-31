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
﻿

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SynapseDynamicAPI.Models;
using SynapseDynamicAPI.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace SynapseDynamicAPI.Controllers
{
  
    [Authorize]
    public class BaseViewController : Controller
    {
        private const string AuthSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme + "," +
        JwtBearerDefaults.AuthenticationScheme;

        [HttpGet]
        [Route("")]
        [Route("[action]/{baseviewname?}/{orderby?}/{limit?}/{offset?}/{filter?}")]
        public string GetBaseViewList(string baseviewname, string orderby, string limit, string offset, string filter)
        {

            string limitString = "";
            if (!string.IsNullOrEmpty(limit))
            {
                limitString = " LIMIT " + limit;
            }
            string orderBySting = "";
            if (!string.IsNullOrEmpty(orderby))
            {
                orderBySting = " ORDER BY " + orderby;
            }

            string offsetString = "";
            if (!string.IsNullOrEmpty(offset))
            {
                offsetString = " OFFSET " + offset;
            }

          
            //string fieldList = SynapseEntityHelperServices.GetBaseViewAttributes(baseviewname);
            //if (string.IsNullOrEmpty(fieldList))
            //{
            //    fieldList = " * ";
            //}

            //Stub for later use
            string filterString = "";
            if (!string.IsNullOrEmpty(filter))
            {
                filterString = " AND " + filter;
            }

            string sql = "SELECT * FROM baseview." + baseviewname + " WHERE 1=1 " + filterString + orderBySting + limitString + offsetString + ";";
            var paramList = new List<KeyValuePair<string, object>>() { };

            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONString(dt);
            return json;
        }


        [HttpGet]
        [Route("")]
        [Route("[action]/{baseviewname?}/{synapseattributename?}/{attributevalue?}/{orderby?}/{limit?}/{offset?}/{filter?}")]
        public string GetBaseViewListByAttribute(string baseviewname, string synapseattributename, string attributevalue, string orderby, string limit, string offset, string filter)
        {

            string limitString = "";
            if (!string.IsNullOrEmpty(limit))
            {
                limitString = " LIMIT " + limit;
            }
            string orderBySting = "";
            if (!string.IsNullOrEmpty(orderby))
            {
                orderBySting = " ORDER BY " + orderby;
            }

            string offsetString = "";
            if (!string.IsNullOrEmpty(offset))
            {
                offsetString = " OFFSET " + offset;
            }


            //string fieldList = SynapseEntityHelperServices.GetBaseViewAttributes(baseviewname);
            //if (string.IsNullOrEmpty(fieldList))
            //{
            //    fieldList = " * ";
            //}

            //Stub for later use
            string filterString = "";
            if (!string.IsNullOrEmpty(filter))
            {
                filterString = " AND " + filter;
            }

            string sql = "SELECT * FROM baseview." + baseviewname + " WHERE " + synapseattributename + " = @p_keyAttributeValue"   + filterString + orderBySting + limitString + offsetString + "; ";
            var paramList = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("p_keyAttributeValue", attributevalue)
            };

            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONString(dt);
            return json;
            
        }


        [HttpGet]
        [Route("")]
        [Route("[action]/{baseviewname?}/{synapseattributename?}/{attributevalue?}/{filter?}")]
        public string GetBaseViewListObjectByAttribute(string baseviewname, string synapseattributename, string attributevalue, string filter)
        {
            
            //string fieldList = SynapseEntityHelperServices.GetBaseViewAttributes(baseviewname);
            //if (string.IsNullOrEmpty(fieldList))
            //{
            //    fieldList = " * ";
            //}

            //Stub for later use
            string filterString = "";
            if (!string.IsNullOrEmpty(filter))
            {
                filterString = " AND " + filter;
            }

            string sql = "SELECT * FROM baseview." + baseviewname + " WHERE " + synapseattributename + " = @p_keyAttributeValue" + filterString + " LIMIT 1; ";
            var paramList = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("p_keyAttributeValue", attributevalue)
            };

            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONObject(dt);
            return json;
             
        }



        [HttpPost]
        [Route("")]
        [Route("[action]/{baseviewname?}/{orderby?}/{limit?}/{offset?}/{filter?}")]
        public string GetBaseViewListByPost(string baseviewname, string orderby, string limit, string offset, string filter, [FromBody] string data)
        {
            dynamic results = JsonConvert.DeserializeObject<dynamic>(data);
            string filters = results[0].filters.ToString();
            
            StringBuilder filtersToApplySB = new StringBuilder();
            filtersToApplySB.Append(" WHERE 1 = 1 ");
            JArray obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(filters);
            foreach (var filterApplied in obj)
            {
                filtersToApplySB.Append(" AND (" + (string)filterApplied["filterClause"] + ")");
            }
            string filtersSQL = filtersToApplySB.ToString();

            string filterparams = results[1].filterparams.ToString();
            var paramListFromPost = new List<KeyValuePair<string, object>>();
            JArray objParams = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(filterparams);
            foreach (var paramApplied in objParams)
            {
                if (paramApplied["paramValue"].Type == JTokenType.Date)
                {
                    var temp = (DateTime)paramApplied["paramValue"];

                    var value = temp.ToString("yyyy-MM-ddTHH:mm:ss");

                    paramListFromPost.Insert(0, new KeyValuePair<string, object>((string)paramApplied["paramName"], value));
                }
                else
                {
                    paramListFromPost.Insert(0, new KeyValuePair<string, object>((string)paramApplied["paramName"], (string)paramApplied["paramValue"]));
                }
            }

            string selectstatement = results[2].selectstatement.ToString();

            string ordergroupbystatement = results[3].ordergroupbystatement.ToString();

            string limitString = "";
            if (!string.IsNullOrEmpty(limit))
            {
                limitString = " LIMIT " + limit;
            }
            string orderBySting = "";
            if (!string.IsNullOrEmpty(orderby))
            {
                orderBySting = " ORDER BY " + orderby;
            }

            string offsetString = "";
            if (!string.IsNullOrEmpty(offset))
            {
                offsetString = " OFFSET " + offset;
            }


            //string fieldList = SynapseEntityHelperServices.GetBaseViewAttributes(baseviewname);
            //if (string.IsNullOrEmpty(fieldList))
            //{
            //    fieldList = " * ";
            //}

            //Stub for later use
            string filterString = "";
            if (!string.IsNullOrEmpty(filter))
            {
                filterString = " AND " + filter;
            }

            string sql = selectstatement + " FROM (SELECT * FROM baseview." + baseviewname + " WHERE 1=1 " + filterString + orderBySting + limitString + offsetString + ") bv " + filtersSQL + " " + ordergroupbystatement + ";";
            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramListFromPost);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONString(dt);
            return json;
            
        }


        /// <summary>
        /// Parse a JSON object and return it as a dictionary of strings with keys showing the heirarchy.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="nodes"></param>
        /// <param name="parentLocation"></param>
        /// <returns></returns>
        public static bool ParseJson(JToken token, Dictionary<string, string> nodes, string parentLocation = "")
        {
            if (token.HasValues)
            {
                foreach (JToken child in token.Children())
                {
                    if (token.Type == JTokenType.Property)
                        parentLocation += "/" + ((JProperty)token).Name;
                    ParseJson(child, nodes, parentLocation);
                }

                // we are done parsing and this is a parent node
                return true;
            }
            else
            {
                // leaf of the tree
                if (nodes.ContainsKey(parentLocation))
                {
                    // this was an array
                    nodes[parentLocation] += "|" + token.ToString();
                }
                else
                {
                    // this was a single property
                    nodes.Add(parentLocation, token.ToString());
                }

                return false;
            }
        }
    }
}
