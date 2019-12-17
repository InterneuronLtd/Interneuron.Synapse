//Interneuron Synapse

//Copyright(C) 2019  Interneuron CIC

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

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Linq;
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
    public class DynamicController : Controller
    {

        private const string AuthSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme + "," +
        JwtBearerDefaults.AuthenticationScheme;
        private IConfiguration _configuration { get; }

        public DynamicController(IConfiguration Configration)
        {
            _configuration = Configration;
        }

        [HttpGet]
        [Route("")]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}/{returnsystemattributes?}/{orderby?}/{limit?}/{offset?}/{filter?}")]
        public string GetList(string synapsenamespace, string synapseentityname, string returnsystemattributes, string orderby, string limit, string offset, string filter)
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

            if (string.IsNullOrWhiteSpace(returnsystemattributes))
            {
                returnsystemattributes = "0";
            }

            string fieldList = SynapseEntityHelperServices.GetEntityAttributes(synapsenamespace, synapseentityname, returnsystemattributes);
            if (string.IsNullOrEmpty(fieldList))
            {
                fieldList = " * ";
            }

            //Stub for later use
            string filterString = "";
            if (!string.IsNullOrEmpty(filter))
            {
                filterString = " AND " + filter;
            }

            string sql = "SELECT " + fieldList + " FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE 1=1 " + filterString + orderBySting + limitString + offsetString + ";";
            var paramList = new List<KeyValuePair<string, object>>() { };

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                var json = DataServices.ConvertDataTabletoJSONString(dt);
                return json;
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied";

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }

        [HttpPost]
        [Route("")]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}/{returnsystemattributes?}/{orderby?}/{limit?}/{offset?}/{filter?}")]
        public string GetListByPost(string synapsenamespace, string synapseentityname, string returnsystemattributes, string orderby, string limit, string offset, string filter, [FromBody] string data)
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
                paramListFromPost.Insert(0, new KeyValuePair<string, object>((string)paramApplied["paramName"], (string)paramApplied["paramValue"]));
            }



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

            if (string.IsNullOrWhiteSpace(returnsystemattributes))
            {
                returnsystemattributes = "0";
            }

            string fieldList = SynapseEntityHelperServices.GetEntityAttributes(synapsenamespace, synapseentityname, returnsystemattributes);
            if (string.IsNullOrEmpty(fieldList))
            {
                fieldList = " * ";
            }

            //Stub for later use
            string filterString = "";
            if (!string.IsNullOrEmpty(filter))
            {
                filterString = " AND " + filter;
            }

            string sql = "SELECT * FROM (SELECT " + fieldList + " FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE 1=1 " + filterString + orderBySting + limitString + offsetString + ") entview " + filtersSQL + ";";

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramListFromPost);
                DataTable dt = ds.Tables[0];
                var json = DataServices.ConvertDataTabletoJSONString(dt);
                return json;
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied";

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }

        [HttpGet]
        [Route("")]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}/{id?}/{returnsystemattributes?}")]
        public string GetObject(string synapsenamespace, string synapseentityname, string id, string returnsystemattributes)
        {

            if (string.IsNullOrWhiteSpace(returnsystemattributes))
            {
                returnsystemattributes = "0";
            }

            string fieldList = SynapseEntityHelperServices.GetEntityAttributes(synapsenamespace, synapseentityname, returnsystemattributes);
            if (string.IsNullOrEmpty(fieldList))
            {
                fieldList = " * ";
            }

            string keyAttribute = "";
            try
            {
                keyAttribute = SynapseEntityHelperServices.GetEntityKeyAttribute(synapsenamespace, synapseentityname);
            }
            catch
            {
                this.HttpContext.Response.StatusCode = 500;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.500";
                httpErr.ErrorType = "System Error";
                httpErr.ErrorDescription = "Unable to retrieve key attribute column";
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            if (string.IsNullOrWhiteSpace(keyAttribute))
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied - unable to retrieve key attribute column";
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }



            string sql = "SELECT " + fieldList + " FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + keyAttribute + " = @p_keyAttributeValue" + " LIMIT 1;";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_keyAttributeValue", id)
                };



            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                var json = DataServices.ConvertDataTabletoJSONObject(dt);
                return json;
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied";

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }


        [HttpGet]
        [Route("")]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}/{synapseattributename?}/{attributevalue?}/{keyvalue?}/{returnsystemattributes?}")]
        public string GetObjectWithInsert(string synapsenamespace, string synapseentityname, string synapseattributename, string attributevalue, string keyvalue, string returnsystemattributes)
        {

            if (string.IsNullOrWhiteSpace(returnsystemattributes))
            {
                returnsystemattributes = "0";
            }


            if (string.IsNullOrWhiteSpace(keyvalue))
            {
                keyvalue = System.Guid.NewGuid().ToString();
            }


            string sqlCount = "SELECT COUNT(*) AS entityRecords FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + synapseattributename + " = @p_keyAttributeValue;";
            var paramListCount = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_keyAttributeValue", attributevalue)
                };


            int iCount = 0;

            DataSet dsCount = new DataSet();
            try
            {
                dsCount = DataServices.DataSetFromSQL(sqlCount, paramListCount);
                DataTable dt = dsCount.Tables[0];
                iCount = System.Convert.ToInt32(dt.Rows[0]["entityRecords"].ToString());
            }
            catch (Exception ex)
            {
                iCount = 0;
            }

            if (iCount == 0)
            {
                //insert to return row
                string keyAttribute = SynapseEntityHelperServices.GetEntityKeyAttribute(synapsenamespace, synapseentityname);

                StringBuilder sb = new StringBuilder();
                StringBuilder sb_materialised = new StringBuilder();

                var paramListInsert = new List<KeyValuePair<string, object>>();
                var paramListInsert_materialised = new List<KeyValuePair<string, object>>();

                if (keyAttribute == synapseattributename)  //Only insert attributevalue
                {
                    sb.Append("INSERT INTO entitystore." + synapsenamespace + "_" + synapseentityname);
                    sb.Append(" (" + keyAttribute + ")");
                    sb.Append(" VALUES (@keyvalue)");
                    paramListInsert = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyvalue", attributevalue)
                    };

                    sb_materialised.Append("INSERT INTO entitystorematerialised." + synapsenamespace + "_" + synapseentityname);
                    sb_materialised.Append(" (" + keyAttribute + ")");
                    sb_materialised.Append(" VALUES (@keyvalue)");
                    paramListInsert_materialised = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyvalue", attributevalue)
                    };
                }
                else
                {
                    sb.Append("INSERT INTO entitystore." + synapsenamespace + "_" + synapseentityname);
                    sb.Append(" (" + keyAttribute + "," + synapseattributename + ")");
                    sb.Append(" VALUES (@keyvalue, @synapseattributevalue)");
                    paramListInsert = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyvalue", keyvalue),
                     new KeyValuePair<string, object>("synapseattributevalue", attributevalue)
                    };


                    sb_materialised.Append("INSERT INTO entitystorematerialised." + synapsenamespace + "_" + synapseentityname);
                    sb_materialised.Append(" (" + keyAttribute + "," + synapseattributename + ")");
                    sb_materialised.Append(" VALUES (@keyvalue, @synapseattributevalue)");
                    paramListInsert_materialised = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyvalue", keyvalue),
                     new KeyValuePair<string, object>("synapseattributevalue", attributevalue)
                    };
                }

                try
                {
                    DataServices.executeSQLStatement(sb.ToString(), paramListInsert);
                    DataServices.executeSQLStatement(sb_materialised.ToString(), paramListInsert_materialised);
                }
                catch
                {
                    this.HttpContext.Response.StatusCode = 400;
                    var httpErr = new SynapseHTTPError();
                    httpErr.ErrorCode = "HTTP.400";
                    httpErr.ErrorType = "Client Error";
                    httpErr.ErrorDescription = "Invalid paramaters supplied";
                    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }
            }

            string fieldList = SynapseEntityHelperServices.GetEntityAttributes(synapsenamespace, synapseentityname, returnsystemattributes);
            if (string.IsNullOrEmpty(fieldList))
            {
                fieldList = " * ";
            }


            string sql = "SELECT " + fieldList + " FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + synapseattributename + " = @p_keyAttributeValue LIMIT 1;";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_keyAttributeValue", attributevalue)
                };



            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                var json = DataServices.ConvertDataTabletoJSONObject(dt);
                return json;
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied";

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }

        [HttpGet]
        [Route("")]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}/{synapseattributename?}/{attributevalue?}/{returnsystemattributes?}/{orderby?}/{limit?}/{offset?}/{filter?}")]
        public string GetListByAttribute(string synapsenamespace, string synapseentityname, string synapseattributename, string attributevalue, string returnsystemattributes, string orderby, string limit, string offset, string filter)
        {

            if (string.IsNullOrWhiteSpace(returnsystemattributes))
            {
                returnsystemattributes = "0";
            }

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

            //Stub for later use
            string filterString = "";
            if (!string.IsNullOrEmpty(filter))
            {
                filterString = " AND " + filter;
            }

            string fieldList = SynapseEntityHelperServices.GetEntityAttributes(synapsenamespace, synapseentityname, returnsystemattributes);
            if (string.IsNullOrEmpty(fieldList))
            {
                fieldList = " * ";
            }


            string sql = "SELECT " + fieldList + " FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + synapseattributename + " = @p_keyAttributeValue" + filterString + orderBySting + limitString + offsetString + ";";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_keyAttributeValue", attributevalue)
                };



            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                var json = DataServices.ConvertDataTabletoJSONString(dt);
                return json;
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied";

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }

        [HttpPost]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}")]
        public string PostObject(string synapsenamespace, string synapseentityname, [FromBody] string data)
        {

            DataTable dtRel = SynapseEntityHelperServices.GetEntityRelations(synapsenamespace, synapseentityname);

            string keyAttribute = SynapseEntityHelperServices.GetEntityKeyAttribute(synapsenamespace, synapseentityname);

            StringBuilder sb = new StringBuilder();

            //dynamic dObj = JObject.Parse(data);
            var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

            StringBuilder sbCols = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
            var paramList = new List<KeyValuePair<string, object>>();

            string keyValue = "";

            int iRelationMatches = 0;
            int iKeyMatches = 0;

            var count = dataDict.Count();

            //RK: 08012019 set_createdby : userid from access token  
            #region set_createdby         
            var useridClaim = User.FindFirst(_configuration["SynapseCore:Settings:TokenUserIdClaimType"]);
            var userid = "unknown";

            if (useridClaim != null)
            {
                userid = useridClaim.Value;
            }
            else if (dataDict.ContainsKey("_createdby"))
            {
                userid = Convert.ToString(dataDict["_createdby"]);
            }

            //check if dataDict has _createdby key
            if (dataDict.ContainsKey("_createdby"))
            {
                //update they key value to userid from toke               
                dataDict["_createdby"] = userid;
            }
            else
            {
                //add _createdby key and set value to userid from token 
                dataDict.Add("_createdby", userid);
            }
            #endregion

            foreach (KeyValuePair<string, object> item in dataDict)
            {
                //Count if Key Attribute matches item.Key
                var a = item.Key;
                var b = a;

                if (keyAttribute == item.Key)
                {
                    keyValue = item.Value.ToString();
                    iKeyMatches++;
                }
                //Count all Key Columns
                foreach (DataRow row in dtRel.Rows)
                {
                    sb.Append(row[0].ToString() + ",");
                    if (item.Key == row[0].ToString())
                    {
                        iRelationMatches++;
                    }
                }

                if (item.Value != null)
                {

                    sbCols.Append(item.Key + ",");
                    sbParams.Append("@p_" + item.Key + ",");
                    paramList.Insert(0, new KeyValuePair<string, object>("p_" + item.Key, item.Value));
                }
                else //Check it is not a key or relation column that is null
                {

                    if (keyAttribute == item.Key)
                    {
                        this.HttpContext.Response.StatusCode = 400;
                        var httpErr = new SynapseHTTPError();
                        httpErr.ErrorCode = "HTTP.400";
                        httpErr.ErrorType = "Client Error";
                        httpErr.ErrorDescription = "No value supplied for Key Attribute: " + keyAttribute;
                        return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    }

                    //Relations
                    foreach (DataRow row in dtRel.Rows)
                    {
                        if (item.Key == row[0].ToString())
                        {
                            this.HttpContext.Response.StatusCode = 400;
                            var httpErr = new SynapseHTTPError();
                            httpErr.ErrorCode = "HTTP.400";
                            httpErr.ErrorType = "Client Error";
                            httpErr.ErrorDescription = "No values supplpied for relation: " + item.Key;
                            return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        }
                    }


                }
            }

            if (iRelationMatches < dtRel.Rows.Count)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Not all relations have values specified. Please ensure that you have values are specified for all of the following fields: " + StringManipulationServices.TrimEnd(sb.ToString(), ",");
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            if (iKeyMatches < 1)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "No value supplied for Key Attribute: " + keyAttribute;
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            string strCols = "INSERT INTO entitystore." + synapsenamespace + "_" + synapseentityname + "(" +
                              sbCols.ToString().TrimEnd(',') +
                              ") ";
            string strParams = " VALUES (" +
                               sbParams.ToString().TrimEnd(',') +
                               ") RETURNING _sequenceid;";
            var sql = strCols + strParams;

            try
            {
                DataSet ds = new DataSet();
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                int id = System.Convert.ToInt32(dt.Rows[0][0].ToString());

                //Delete all occurances from entitystorematerialised
                string sqlDelete = "DELETE FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + keyAttribute + " = @keyValue;";

                var paramListDelete = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyValue", keyValue)
                };

                try
                {
                    DataServices.executeSQLStatement(sqlDelete, paramListDelete);
                }
                catch
                {
                    this.HttpContext.Response.StatusCode = 400;
                    var httpErr = new SynapseHTTPError();
                    httpErr.ErrorCode = "HTTP.500";
                    httpErr.ErrorType = "Server Error";
                    httpErr.ErrorDescription = "Unable to delete materialised entity history";
                    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }


                //Get all the entity's columns 
                string entityCols = "";
                string sqlEntityCols = "SELECT entitysettings.getentityattributestring(@synapsenamespace, @synapseentityname, 1)";
                var paramListEntityCols = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("synapsenamespace", synapsenamespace),
                     new KeyValuePair<string, object>("synapseentityname", synapseentityname)
                };

                try
                {
                    DataSet dsEntityCols = new DataSet();
                    dsEntityCols = DataServices.DataSetFromSQL(sqlEntityCols, paramListEntityCols);
                    DataTable dtEntityCols = dsEntityCols.Tables[0];
                    entityCols = dtEntityCols.Rows[0][0].ToString();
                }
                catch
                {
                    this.HttpContext.Response.StatusCode = 400;
                    var httpErr = new SynapseHTTPError();
                    httpErr.ErrorCode = "HTTP.500";
                    httpErr.ErrorType = "Server Error";
                    httpErr.ErrorDescription = "Unable to get all entity columns";
                    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }


                //Insert the newly inserted record into the materialised entity
                string sqlMaterialisedInsert = "INSERT INTO entitystorematerialised." + synapsenamespace + "_" + synapseentityname + "(" +
                              entityCols +
                              ") "
                               +
                              " SELECT " +
                              entityCols +
                             " FROM entityview." + synapsenamespace + "_" + synapseentityname
                              + " WHERE " + keyAttribute + " = @keyValue;";


                var paramListMaterialisedInsert = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyValue", keyValue)
                };

                try
                {
                    DataServices.executeSQLStatement(sqlMaterialisedInsert, paramListMaterialisedInsert);
                }
                catch
                {
                    this.HttpContext.Response.StatusCode = 400;
                    var httpErr = new SynapseHTTPError();
                    httpErr.ErrorCode = "HTTP.500";
                    httpErr.ErrorType = "Server Error";
                    httpErr.ErrorDescription = "Unable to insert into materialised entity";
                    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }


                //Get the return obext
                return GetReturnObjectByID(synapsenamespace, synapseentityname, id);

            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid paramaters supplied";
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

        }

        [HttpPost]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}")]
        public string PostObjectArray(string synapsenamespace, string synapseentityname, [FromBody] string data)
        {
            StringBuilder returnValue = new StringBuilder();

            var dataToPost = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(data);

            foreach (var item in dataToPost)
            {
                string postedData = PostObject(synapsenamespace, synapseentityname, JsonConvert.SerializeObject(item));

                returnValue.Append(postedData.TrimStart('[').TrimEnd(']') + ",");
            }

            return "[" + returnValue.ToString().TrimEnd(",") + "]";
        }

        [HttpDelete]
        [Route("")]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}/{synapseattributename?}/{attributevalue?}")]
        public string DeleteObjectByAttribute(string synapsenamespace, string synapseentityname, string synapseattributename, string attributevalue)
        {
            string sqlDelete = "DELETE FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + synapseattributename + " = @keyValue;";

            var paramListDelete = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyValue", attributevalue)
                };

            try
            {
                DataServices.executeSQLStatement(sqlDelete, paramListDelete);
            }
            catch
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.500";
                httpErr.ErrorType = "Server Error";
                httpErr.ErrorDescription = "Unable to delete materialised entity history";
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            string sql = "UPDATE entitystore." + synapsenamespace + "_" + synapseentityname + " SET _recordstatus = 2 WHERE _recordstatus = 1 AND " + synapseattributename + " = @p_attributevalue";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_attributevalue", attributevalue)
                };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
                KeyAttribute ka = new KeyAttribute();
                ka.Namespace = synapsenamespace;
                ka.EntityName = synapseentityname;
                ka.KeyAttributeName = synapseattributename;
                ka.Message = "Record(s) Deleted where " + synapseattributename + " = " + attributevalue;
                this.HttpContext.Response.StatusCode = 200;
                return JsonConvert.SerializeObject(ka, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied";

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }

        [HttpDelete]
        [Route("")]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}/{id?}")]
        public string DeleteObject(string synapsenamespace, string synapseentityname, string id)
        {
            string keyAttribute = "";
            try
            {
                keyAttribute = SynapseEntityHelperServices.GetEntityKeyAttribute(synapsenamespace, synapseentityname);
            }
            catch
            {
                this.HttpContext.Response.StatusCode = 500;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.500";
                httpErr.ErrorType = "System Error";
                httpErr.ErrorDescription = "Unable to retrieve key attribute column - please ensure you have provided the correct namespace and entity";
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            if (string.IsNullOrWhiteSpace(keyAttribute))
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied - key attribute column not supplied";
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            string sqlDelete = "DELETE FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + keyAttribute + " = @keyValue;";

            var paramListDelete = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyValue", id)
                };

            try
            {
                DataServices.executeSQLStatement(sqlDelete, paramListDelete);
            }
            catch
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.500";
                httpErr.ErrorType = "Server Error";
                httpErr.ErrorDescription = "Unable to delete materialised entity history";
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }



            string sql = "UPDATE entitystore." + synapsenamespace + "_" + synapseentityname + " SET _recordstatus = 2 WHERE " + keyAttribute + " = @p_keyAttributeValue;";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_keyAttributeValue", id)
                };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
                KeyAttribute ka = new KeyAttribute();
                ka.Namespace = synapsenamespace;
                ka.EntityName = synapseentityname;
                ka.KeyAttributeName = keyAttribute;
                ka.Message = "Record Deleted where " + keyAttribute + " = " + id;
                this.HttpContext.Response.StatusCode = 200;
                return JsonConvert.SerializeObject(ka, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied";

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }

        [HttpGet]
        [Route("")]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}/{id?}")]
        public string GetObjectHistory(string synapsenamespace, string synapseentityname, string id)
        {


            string fieldList = SynapseEntityHelperServices.GetEntityAttributes(synapsenamespace, synapseentityname, "1");
            if (string.IsNullOrEmpty(fieldList))
            {
                fieldList = " * ";
            }

            string keyAttribute = "";
            try
            {
                keyAttribute = SynapseEntityHelperServices.GetEntityKeyAttribute(synapsenamespace, synapseentityname);
            }
            catch
            {
                this.HttpContext.Response.StatusCode = 500;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.500";
                httpErr.ErrorType = "System Error";
                httpErr.ErrorDescription = "Unable to retrieve key attribute column";
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            if (string.IsNullOrWhiteSpace(keyAttribute))
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied - unable to retrieve key attribute column";
                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }



            string sql = "SELECT " + fieldList + " FROM entitystore." + synapsenamespace + "_" + synapseentityname + " WHERE " + keyAttribute + " = @p_keyAttributeValue" + " ORDER BY _sequenceid;";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_keyAttributeValue", id)
                };



            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                var json = DataServices.ConvertDataTabletoJSONString(dt);
                return json;
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = "Invalid Parameters supplied";

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }


        private string GetReturnObjectByID(string synapsenamespace, string synapseentityname, int sequenceid)
        {
            string sql = "SELECT * FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE _sequenceid = @p_sequenceid;";

            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_sequenceid", sequenceid)
                };

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                var json = DataServices.ConvertDataTabletoJSONString(dt);
                return json;
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 500;
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.500";
                httpErr.ErrorType = "System Error";
                httpErr.ErrorDescription = "Insert successful but unable to return newly inserted record";

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
        }

    }
}
