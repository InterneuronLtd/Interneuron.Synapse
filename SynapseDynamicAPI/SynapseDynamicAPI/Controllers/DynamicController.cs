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


﻿

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using SynapseDynamicAPI.Services;
using SynapseDynamicAPI.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Interneuron.Infrastructure.CustomExceptions;
using Interneuron.Common.Extensions;

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
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONString(dt);
            return json;


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
            ds = DataServices.DataSetFromSQL(sql, paramListFromPost);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONString(dt);
            return json;



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

            string keyAttribute = SynapseEntityHelperServices.GetEntityKeyAttribute(synapsenamespace, synapseentityname);


            if (string.IsNullOrWhiteSpace(keyAttribute))
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Invalid Parameters supplied - unable to retrieve key attribute column: " + keyAttribute, "Client Error");

            }



            string sql = "SELECT " + fieldList + " FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + keyAttribute + " = @p_keyAttributeValue" + " LIMIT 1;";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_keyAttributeValue", id)
                };



            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONObject(dt);
            return json;



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
                DataTable dt1 = dsCount.Tables[0];
                iCount = System.Convert.ToInt32(dt1.Rows[0]["entityRecords"].ToString());
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
                DataServices.executeSQLStatement(sb.ToString(), paramListInsert);
                DataServices.executeSQLStatement(sb_materialised.ToString(), paramListInsert_materialised);

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
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONObject(dt);
            return json;



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
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONString(dt);
            return json;



        }

        [HttpPost]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}")]
        public string PostObject(string synapsenamespace, string synapseentityname, [FromBody] string data)
        {
            var returnedData = PostObjectHandler(synapsenamespace, synapseentityname, data);
            return returnedData.Item1;
        }

        private Tuple<string, string> PostObjectHandler(string synapsenamespace, string synapseentityname, string data, dynamic connectionObj = null, dynamic transactionObj = null)
        {

            //27Feb2020-RK
            //RMF1 - Remove Feature- check if all relations have a key column value supplied in the post request

            //RMF1
            //DataTable dtRel = SynapseEntityHelperServices.GetEntityRelations(synapsenamespace, synapseentityname);

            string keyAttribute = SynapseEntityHelperServices.GetEntityKeyAttribute(synapsenamespace, synapseentityname);

            StringBuilder sb = new StringBuilder();

            //dynamic dObj = JObject.Parse(data);
            var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

            StringBuilder sbCols = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
            var paramList = new List<KeyValuePair<string, object>>();

            string keyValue = "";

            // int iRelationMatches = 0;
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
                //RMF1
                /*  foreach (DataRow row in dtRel.Rows)
                  {
                      sb.Append(row[0].ToString() + ",");
                      if (item.Key == row[0].ToString())
                      {
                          iRelationMatches++;
                      }
                  }*/

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
                        throw new InterneuronBusinessException(errorCode: 400, errorMessage: "No value supplied for Key Attribute: " + keyAttribute, "Client Error");

                    }

                    //Relations
                    //RMF1
                    /* foreach (DataRow row in dtRel.Rows)
                     {
                         if (item.Key == row[0].ToString())
                         {
                             throw new InterneuronBusinessException(errorCode: 400, errorMessage: "No values supplpied for relation: " + item.Key, "Client Error");

                         }
                     }*/


                }
            }

            //RMF1
            /*if (iRelationMatches < dtRel.Rows.Count)
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Not all relations have values specified.Please ensure that you have values are specified for all of the following fields: " + StringManipulationServices.TrimEnd(sb.ToString(), ","), "Client Error");
               
            }*/


            if (iKeyMatches < 1)
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "No value supplied for Key Attribute: " + keyAttribute, "Client Error");

            }

            string strCols = "INSERT INTO entitystore." + synapsenamespace + "_" + synapseentityname + "(" +
                              sbCols.ToString().TrimEnd(',') +
                              ") ";
            string strParams = " VALUES (" +
                               sbParams.ToString().TrimEnd(',') +
                               ") RETURNING _sequenceid;";
            var sql = strCols + strParams;

            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList, existingCon: connectionObj);
            DataTable dt = ds.Tables[0];
            int id = System.Convert.ToInt32(dt.Rows[0][0].ToString());

            //Delete all occurances from entitystorematerialised
            string sqlDelete = "DELETE FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + keyAttribute + " = @keyValue;";

            var paramListDelete = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyValue", keyValue)
                };

            DataServices.executeSQLStatement(sqlDelete, paramListDelete, existingCon: connectionObj);


            //Get all the entity's columns 
            string entityCols = "";
            string sqlEntityCols = "SELECT entitysettings.getentityattributestring(@synapsenamespace, @synapseentityname, 1)";
            var paramListEntityCols = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("synapsenamespace", synapsenamespace),
                     new KeyValuePair<string, object>("synapseentityname", synapseentityname)
                };

            DataSet dsEntityCols = new DataSet();
            dsEntityCols = DataServices.DataSetFromSQL(sqlEntityCols, paramListEntityCols, existingCon: connectionObj);
            DataTable dtEntityCols = dsEntityCols.Tables[0];
            entityCols = dtEntityCols.Rows[0][0].ToString();


            //Insert the newly inserted record into the materialised entity
            string sqlMaterialisedInsert = string.Empty;

            if (connectionObj == null)
            {
                sqlMaterialisedInsert = "INSERT INTO entitystorematerialised." + synapsenamespace + "_" + synapseentityname + "(" +
                              entityCols +
                              ") "
                               +
                              " SELECT " +
                              entityCols +
                             " FROM entityview." + synapsenamespace + "_" + synapseentityname
                              + " WHERE " + keyAttribute + " = @keyValue;";
            }
            else
            {
                sqlMaterialisedInsert = "INSERT INTO entitystorematerialised." + synapsenamespace + "_" + synapseentityname + "(" +
                             entityCols +
                             ") "
                              +
                             " SELECT " +
                             entityCols +
                            " FROM entityview." + synapsenamespace + "_" + synapseentityname
                             + " WHERE " + keyAttribute + " = @keyValue RETURNING *;";
            }


            var paramListMaterialisedInsert = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyValue", keyValue)
                };

            //DataServices.executeSQLStatement(sqlMaterialisedInsert, paramListMaterialisedInsert, existingCon: connectionObj);

            var insertedDataInMaterial = string.Empty;

            if (connectionObj == null)
            {
                DataServices.executeSQLStatement(sqlMaterialisedInsert, paramListMaterialisedInsert);
            }
            else
            {
                DataSet dsMaterilaized = DataServices.DataSetFromSQL(sqlMaterialisedInsert, paramListMaterialisedInsert, existingCon: connectionObj);
                insertedDataInMaterial = DataServices.ConvertDataTabletoJSONString(dsMaterilaized.Tables[0]);
            }


            //Get the return object
            //return GetReturnObjectByID(synapsenamespace, synapseentityname, id);

            string returnedData = string.Empty;

            if (connectionObj == null)
                returnedData = GetReturnObjectByID(synapsenamespace, synapseentityname, id);

            return new Tuple<string, string>(returnedData, insertedDataInMaterial);
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

        [HttpPost]
        [Route("[action]")]
        public string PostObjectsInTransaction([FromBody] string data)
        {
            StringBuilder returnValue = new StringBuilder();
            List<Tuple<KeyValuePair<string, List<string>>, string>> upsertedDataList = new List<Tuple<KeyValuePair<string, List<string>>, string>>();

            var entitiesToPersisit = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(data);

            //Start db operation
            var connWithTran = DataServices.BeginDbTransaction();

            try
            {
                entitiesToPersisit.Each(entityItems =>
                {
                    entityItems.Each(entity =>
                    {
                        var savedEntityWithIds = PersistMultipleOps(entity, connWithTran, returnValue);

                        if (savedEntityWithIds.Key != null && savedEntityWithIds.Key.IsNotEmpty()) // Do not allow without key
                            upsertedDataList.Add(new Tuple<KeyValuePair<string, List<string>>, string>(savedEntityWithIds, entity.Value.GetType().Name));
                    });
                });

                DataServices.CommitDbTransaction(connWithTran.Item1, connWithTran.Item2);

            }
            catch (Exception ex)
            {
                DataServices.RollbackDbTransaction(connWithTran.Item1, connWithTran.Item2);

                throw ex;
            }

            //Fetch upserted records from db - can be done once commit
            FetchUpsertedData(upsertedDataList, returnValue);

            return "[" + returnValue.ToString().TrimEnd(",") + "]";
        }

        private void FetchUpsertedData(List<Tuple<KeyValuePair<string, List<string>>, string>> upsertedDataList, StringBuilder returnValue)
        {
            var namespaceEntityDelimiter = this._configuration.GetValue<string>("SynapseCore:Settings:TransactionEndpointDelimiter");

            upsertedDataList.Each(item =>
            {
                StringBuilder entityBuilder = new StringBuilder();

                var entityItem = item.Item1;
                var entityItemType = item.Item2;

                var entityKey = entityItem.Key.Split(namespaceEntityDelimiter);
                var synapseNamespace = entityKey[0];
                var synapseEntityName = entityKey[1];

                var key = $"{synapseNamespace}{namespaceEntityDelimiter}{synapseEntityName}";

                if (entityItemType == "JObject")
                    entityBuilder.Append("{" + "\"" + key + "\"" + ":");
                else if (entityItemType == "JArray")
                    entityBuilder.Append("{" + "\"" + key + "\"" + ":[");

                StringBuilder itemBuilder = new StringBuilder();

                entityItem.Value.Each(postedData =>
                {
                    //Need refactoring here - should use 'in' in the sql stmt - current approach is not efficient way
                    //var postedData = GetReturnObjectByID(synapseNamespace, synapseEntityName, id);

                    if (postedData == null || postedData == "[]")
                        postedData = "{}";

                    itemBuilder.Append(postedData.TrimStart('[').TrimEnd(']') + ",");
                });

                entityBuilder.Append(itemBuilder.ToString().TrimEnd(","));

                if (entityItemType == "JObject")
                    entityBuilder.Append("},");
                else if (entityItemType == "JArray")
                    entityBuilder.Append("]},");

                returnValue.Append(entityBuilder.ToString());
            });

        }

        private KeyValuePair<string, List<string>> PersistMultipleOps(KeyValuePair<string, object> entity, dynamic connWithTran, StringBuilder returnValue)
        {
            var namespaceEntityDelimiter = this._configuration.GetValue<string>("SynapseCore:Settings:TransactionEndpointDelimiter");

            var action = "ins";
            string synapseNamespace, synapseEntityName = string.Empty;

            var actionDelimiter = entity.Key.Split(':');

            if (actionDelimiter.IsCollectionValid() && actionDelimiter.Length > 1)
            {
                action = actionDelimiter[1];

                var entityKey = actionDelimiter[0].Split(namespaceEntityDelimiter);
                synapseNamespace = entityKey[0];
                synapseEntityName = entityKey[1];
            }
            else
            {
                var entityKey = entity.Key.Split(namespaceEntityDelimiter);
                synapseNamespace = entityKey[0];
                synapseEntityName = entityKey[1];
            }

            KeyValuePair<string, List<string>> upsertedDataList = new KeyValuePair<string, List<string>>($"{synapseNamespace}{namespaceEntityDelimiter}{synapseEntityName}", new List<string>());

            if (action.EqualsIgnoreCase("ins"))
            {
                upsertedDataList = new KeyValuePair<string, List<string>>($"{synapseNamespace}{namespaceEntityDelimiter}{synapseEntityName}", new List<string>());
                HandlePostOp(entity, synapseNamespace, synapseEntityName, connWithTran, upsertedDataList);
            }
            else if (action.EqualsIgnoreCase("del"))
            {
                upsertedDataList = new KeyValuePair<string, List<string>>();
                HandleDeleteOp(entity, synapseNamespace, synapseEntityName, connWithTran, upsertedDataList);

                return new KeyValuePair<string, List<string>>();//No need to send back the deleted record data
            }


            return upsertedDataList;
        }

        private void HandleDeleteOp(KeyValuePair<string, object> entity, string synapseNamespace, string synapseEntityName, dynamic connWithTran, KeyValuePair<string, List<string>> upsertedDataList)
        {
            if (entity.Value.GetType().Name == "String")
            {
                this.DeleteByIdHandler(synapseNamespace, synapseEntityName, entity.Value.ToString(), connWithTran.Item1, connWithTran.Item2);
            }
            else if (entity.Value.GetType().Name == "JObject")
            {
                var entityData = JsonConvert.SerializeObject(entity.Value);

                var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(entityData);
                var firstData = dataDict.FirstOrDefault();

                this.DeleteObjectByAttributeHandler(synapseNamespace, synapseEntityName, firstData.Key, firstData.Value.ToString(), connWithTran.Item1, connWithTran.Item2);
            }
        }

        private void HandlePostOp(KeyValuePair<string, object> entity, string synapseNamespace, string synapseEntityName, dynamic connWithTran, KeyValuePair<string, List<string>> upsertedDataList)
        {
            var entityData = JsonConvert.SerializeObject(entity.Value);

            if (entity.Value.GetType().Name == "JArray")
            {
                JsonConvert.DeserializeObject<List<object>>(entityData).Each(item =>
                {
                    Tuple<string, string> postedData = PostObjectHandler(synapseNamespace, synapseEntityName, JsonConvert.SerializeObject(item), connWithTran.Item1, connWithTran.Item2);

                    upsertedDataList.Value.Add(postedData.Item2);
                });

            }
            else if (entity.Value.GetType().Name == "JObject")
            {
                Tuple<string, string> postedData = PostObjectHandler(synapseNamespace, synapseEntityName, entityData, connWithTran.Item1, connWithTran.Item2);

                upsertedDataList.Value.Add(postedData.Item2);
            }
        }

        [HttpDelete]
        [Route("")]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}/{synapseattributename?}/{attributevalue?}")]
        public string DeleteObjectByAttribute(string synapsenamespace, string synapseentityname, string synapseattributename, string attributevalue)
        {
            return DeleteObjectByAttributeHandler(synapsenamespace, synapseentityname, synapseattributename, attributevalue);
        }

        private string DeleteObjectByAttributeHandler(string synapsenamespace, string synapseentityname, string synapseattributename, string attributevalue, dynamic connectionObj = null, dynamic transactionObj = null)
        {
            string sqlDelete = "DELETE FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + synapseattributename + " = @keyValue;";

            var paramListDelete = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyValue", attributevalue)
                };

            DataServices.executeSQLStatement(sqlDelete, paramListDelete, existingCon: connectionObj);


            string sql = "UPDATE entitystore." + synapsenamespace + "_" + synapseentityname + " SET _recordstatus = 2 WHERE _recordstatus = 1 AND " + synapseattributename + " = @p_attributevalue";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_attributevalue", attributevalue)
                };

            DataServices.executeSQLStatement(sql, paramList, existingCon: connectionObj);

            KeyAttribute ka = new KeyAttribute();
            ka.Namespace = synapsenamespace;
            ka.EntityName = synapseentityname;
            ka.KeyAttributeName = synapseattributename;
            ka.Message = "Record(s) Deleted where " + synapseattributename + " = " + attributevalue;
            this.HttpContext.Response.StatusCode = 200;
            return JsonConvert.SerializeObject(ka, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpDelete]
        [Route("")]
        [Route("[action]/{synapsenamespace?}/{synapseentityname?}/{id?}")]
        public string DeleteObject(string synapsenamespace, string synapseentityname, string id)
        {
            return DeleteByIdHandler(synapsenamespace, synapseentityname, id);
        }

        private string DeleteByIdHandler(string synapsenamespace, string synapseentityname, string id, dynamic connectionObj = null, dynamic transactionObj = null)
        {
            string keyAttribute = SynapseEntityHelperServices.GetEntityKeyAttribute(synapsenamespace, synapseentityname);


            if (string.IsNullOrWhiteSpace(keyAttribute))
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Invalid Parameters supplied - key attribute column not supplied", "Client Error");

            }

            string sqlDelete = "DELETE FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE " + keyAttribute + " = @keyValue;";

            var paramListDelete = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("keyValue", id)
                };
            DataServices.executeSQLStatement(sqlDelete, paramListDelete, existingCon: connectionObj);



            string sql = "UPDATE entitystore." + synapsenamespace + "_" + synapseentityname + " SET _recordstatus = 2 WHERE " + keyAttribute + " = @p_keyAttributeValue;";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_keyAttributeValue", id)
                };
            DataServices.executeSQLStatement(sql, paramList, existingCon: connectionObj);
            KeyAttribute ka = new KeyAttribute();
            ka.Namespace = synapsenamespace;
            ka.EntityName = synapseentityname;
            ka.KeyAttributeName = keyAttribute;
            ka.Message = "Record Deleted where " + keyAttribute + " = " + id;
            this.HttpContext.Response.StatusCode = 200;
            return JsonConvert.SerializeObject(ka, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
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

            string keyAttribute = SynapseEntityHelperServices.GetEntityKeyAttribute(synapsenamespace, synapseentityname);


            if (string.IsNullOrWhiteSpace(keyAttribute))
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Invalid Parameters supplied - unable to retrieve key attribute column", "Client Error");

            }



            string sql = "SELECT " + fieldList + " FROM entitystore." + synapsenamespace + "_" + synapseentityname + " WHERE " + keyAttribute + " = @p_keyAttributeValue" + " ORDER BY _sequenceid;";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_keyAttributeValue", id)
                };



            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONString(dt);
            return json;



        }


        private string GetReturnObjectByID(string synapsenamespace, string synapseentityname, int sequenceid)
        {
            string sql = "SELECT * FROM entitystorematerialised." + synapsenamespace + "_" + synapseentityname + " WHERE _sequenceid = @p_sequenceid;";

            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_sequenceid", sequenceid)
                };

            DataSet ds = new DataSet();

            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONString(dt);
            return json;

        }

    }
}
