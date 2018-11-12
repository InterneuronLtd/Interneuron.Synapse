//Interneuron Synapse

//Copyright(C) 2018  Interneuron CIC

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
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SynapseDynamicAPI.Models;
using SynapseDynamicAPI.Services;

namespace SynapseDynamicAPI.Controllers
{
    //[Produces("application/json")]
    //[Authorize(AuthenticationSchemes = AuthSchemes)]
    [Route("List/")]
    public class ListController : Controller
    {

        private const string AuthSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme + "," +
        JwtBearerDefaults.AuthenticationScheme;


        [HttpGet]
        [Route("")]
        [Route("[action]/{listId?}")]
        public string GetListColumns(string listId)
        {

            string sql = "SELECT COALESCE(displayname, attributename) AS displayname FROM listsettings.listattribute WHERE list_id = @list_id AND isselected = true ORDER BY ordinalposition; ";
            var paramList = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId)
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
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Invalid Parameters supplied"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }

        [HttpGet]
        [Route("")]
        [Route("[action]/{listId?}")]
        public string GetListDetails(string listId)
        {

            string sql = "SELECT * FROM listsettings.listmanager WHERE list_id = @list_id;";
            var paramList = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId)
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
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Invalid Parameters supplied"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }


        [HttpGet]
        [Route("")]
        [Route("[action]/{listId?}")]
        public string GetListData(string listId)
        {


            //Get List Details
            string sqlListDetails = "SELECT * FROM listsettings.listmanager WHERE list_id = @list_id;";
            var paramListDetails = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId)
            };

            string baseViewID = "";
            DataSet dsListDetails = new DataSet();

            string matchedcontextfield = "";
            try
            {
                dsListDetails = DataServices.DataSetFromSQL(sqlListDetails, paramListDetails);
                DataTable dtListDetails = dsListDetails.Tables[0];
                try
                {
                    baseViewID = dtListDetails.Rows[0]["baseview_id"].ToString();
                }
                catch { }

                try
                {
                    matchedcontextfield = dtListDetails.Rows[0]["matchedcontextfield"].ToString();
                }
                catch { }

            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve List"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            if (string.IsNullOrWhiteSpace(baseViewID))
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve baseview"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            //Get BaseView Details
            string sqlBV = "SELECT * FROM listsettings.baseviewmanager WHERE baseview_id = @baseview_id;";
            var paramListBV = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("baseview_id", baseViewID)
            };
            string baseViewName = "";
            string baseViewNameSpace = "";
            DataSet dsBV = new DataSet();
            try
            {
                dsBV = DataServices.DataSetFromSQL(sqlBV, paramListBV);
                DataTable dtBV = dsBV.Tables[0];
                try
                {
                    baseViewName = dtBV.Rows[0]["baseviewname"].ToString();
                    baseViewNameSpace = dtBV.Rows[0]["baseviewnamespace"].ToString();
                }
                catch { }
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve List"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            if (string.IsNullOrWhiteSpace(baseViewName))
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve baseview name"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }



            //Get List and Attribute Details
            string sqlAttributes = "SELECT *,  @matchedcontextfield AS matchedcontext FROM listsettings.listattribute WHERE list_id = @list_id AND isselected = true ORDER BY ordinalposition;";
            var paramListAttributes = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId),
                new KeyValuePair<string, object>("matchedcontextfield", matchedcontextfield),
            };

            DataSet dsAttributes = new DataSet();
            try
            {
                dsAttributes = DataServices.DataSetFromSQL(sqlAttributes, paramListAttributes);
                DataTable dtAttributes = dsAttributes.Tables[0];

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT");
                //sb.AppendLine("encounter_id,");
                int iCount = 0;
                foreach (DataRow row in dtAttributes.Rows)
                {


                    sb.AppendLine("json_build_object (");
                    sb.AppendLine("'attributevalue', " + row["attributename"].ToString() + ",");
                    sb.AppendLine("'attributename','" + row["attributename"].ToString() + "',");
                    sb.AppendLine("'displayname','" + row["displayname"].ToString() + "',");
                    //sb.AppendLine("'matchedcontext','" + row["matchedcontext"].ToString() + "',");
                    sb.AppendLine("'defaultcssclassname','" + row["defaultcssclassname"].ToString() + "',");
                    sb.AppendLine("'matchedcontext', " + row["matchedcontext"].ToString());
                    //sb.AppendLine("'ordinalposition', " + row["ordinalposition"].ToString() + "");

                    if (iCount == dtAttributes.Rows.Count - 1)
                    {
                        sb.AppendLine(") as col_" + iCount.ToString());
                    }
                    else
                    {
                        sb.AppendLine(") as col_" + iCount.ToString() + ",");
                    }


                    //TextBox1.Text = row["ImagePath"].ToString();
                    iCount++;
                }
                sb.AppendLine(" FROM (SELECT *, " + matchedcontextfield + " AS matchedcontext FROM baseview." + baseViewNameSpace + "_" + baseViewName + ") bv;");

                string listSQL = sb.ToString();

                var paramList = new List<KeyValuePair<string, object>>()
                {
                    //new KeyValuePair<string, object>("matchedcontextfield", matchedcontextfield),
                };

                DataSet dsList = new DataSet();
                try
                {
                    dsList = DataServices.DataSetFromSQL(listSQL, paramList);
                    DataTable dtList = dsList.Tables[0];
                    return DataServices.ConvertDataTabletoJSONString(dtList);
                }
                catch (Exception ex)
                {
                    this.HttpContext.Response.StatusCode = 400;
                    var httpErr = new SynapseHTTPError
                    {
                        ErrorCode = "HTTP.400",
                        ErrorType = "Client Error",
                        ErrorDescription = "Invalid Parameters supplied"
                    };



                    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }




            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Invalid Parameters supplied"
                };



                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }



        }


        [HttpPost]
        [Route("")]
        [Route("[action]/{listId?}")]
        public string GetListDataByPost(string listId, [FromBody] string data)
        {
            #region GettingParamsFromPost
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


            string selectstatement = results[2].selectstatement.ToString();

            string ordergroupbystatement = results[3].ordergroupbystatement.ToString();
            #endregion


            //Get List Details
            string sqlListDetails = "SELECT * FROM listsettings.listmanager WHERE list_id = @list_id;";
            var paramListDetails = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId)
            };

            string baseViewID = "";
            DataSet dsListDetails = new DataSet();
            string matchedcontextfield = "";
            try
            {
                dsListDetails = DataServices.DataSetFromSQL(sqlListDetails, paramListDetails);
                DataTable dtListDetails = dsListDetails.Tables[0];
                try
                {
                    baseViewID = dtListDetails.Rows[0]["baseview_id"].ToString();
                }
                catch { }

                try
                {
                    matchedcontextfield = dtListDetails.Rows[0]["matchedcontextfield"].ToString();
                }
                catch { }
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve List"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            if (string.IsNullOrWhiteSpace(baseViewID))
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve baseview"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            //Get BaseView Details
            string sqlBV = "SELECT * FROM listsettings.baseviewmanager WHERE baseview_id = @baseview_id;";
            var paramListBV = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("baseview_id", baseViewID)
            };
            string baseViewName = "";
            string baseViewNameSpace = "";
            DataSet dsBV = new DataSet();
            try
            {
                dsBV = DataServices.DataSetFromSQL(sqlBV, paramListBV);
                DataTable dtBV = dsBV.Tables[0];
                try
                {
                    baseViewName = dtBV.Rows[0]["baseviewname"].ToString();
                    baseViewNameSpace = dtBV.Rows[0]["baseviewnamespace"].ToString();
                }
                catch { }

            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve List"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            if (string.IsNullOrWhiteSpace(baseViewName))
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve baseview name"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }



            //Get List and Attribute Details
            string sqlAttributes = "SELECT *,  @matchedcontextfield AS matchedcontext FROM listsettings.listattribute WHERE list_id = @list_id AND isselected = true ORDER BY ordinalposition;";
            var paramListAttributes = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId),
                new KeyValuePair<string, object>("matchedcontextfield", matchedcontextfield),
            };

            DataSet dsAttributes = new DataSet();
            try
            {
                dsAttributes = DataServices.DataSetFromSQL(sqlAttributes, paramListAttributes);
                DataTable dtAttributes = dsAttributes.Tables[0];

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT");
                //sb.AppendLine("encounter_id,");
                int iCount = 0;
                foreach (DataRow row in dtAttributes.Rows)
                {


                    sb.AppendLine("json_build_object (");
                    sb.AppendLine("'attributevalue', " + row["attributename"].ToString() + ",");
                    sb.AppendLine("'attributename','" + row["attributename"].ToString() + "',");
                    sb.AppendLine("'displayname','" + row["displayname"].ToString() + "',");
                    //sb.AppendLine("'matchedcontext','" + row["matchedcontext"].ToString() + "',");
                    sb.AppendLine("'defaultcssclassname','" + row["defaultcssclassname"].ToString() + "',");
                    sb.AppendLine("'matchedcontext', " + row["matchedcontext"].ToString());
                    //sb.AppendLine("'ordinalposition', " + row["ordinalposition"].ToString() + "");

                    if (iCount == dtAttributes.Rows.Count - 1)
                    {
                        sb.AppendLine(") as col_" + iCount.ToString());
                    }
                    else
                    {
                        sb.AppendLine(") as col_" + iCount.ToString() + ",");
                    }


                    //TextBox1.Text = row["ImagePath"].ToString();
                    iCount++;
                }
                sb.AppendLine(" FROM (SELECT *, " + matchedcontextfield + " AS matchedcontext FROM baseview." + baseViewNameSpace + "_" + baseViewName  + " " + filtersToApplySB.ToString() + " " + ordergroupbystatement + ") bv;");

                string listSQL = sb.ToString();

                var paramList = new List<KeyValuePair<string, object>>()
                {
                };

                DataSet dsList = new DataSet();
                try
                {
                    dsList = DataServices.DataSetFromSQL(listSQL, paramListFromPost);
                    DataTable dtList = dsList.Tables[0];
                    return DataServices.ConvertDataTabletoJSONString(dtList);
                }
                catch (Exception ex)
                {
                    this.HttpContext.Response.StatusCode = 400;
                    var httpErr = new SynapseHTTPError
                    {
                        ErrorCode = "HTTP.400",
                        ErrorType = "Client Error",
                        ErrorDescription = "Invalid Parameters supplied"
                    };



                    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }




            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Invalid Parameters supplied"
                };



                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

        }



        [HttpGet]
        [Route("")]
        [Route("[action]/{locatorboard_id?}")]
        public string GetListByLocatorBoardID(string locatorboard_id)
        {

            string sql = "SELECT * FROM eboards.v_locatorboardlist WHERE locatorboard_id = @locatorboard_id;";
            var paramList = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("locatorboard_id", locatorboard_id)
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
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Invalid Parameters supplied"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }


        [HttpGet]
        [Route("")]
        [Route("[action]/{listId?}/{contextvalue?}")]
        public string GetListQuestions(string listId, string contextvalue)
        {
            string listapiurl = "";
            string sqlList = "SELECT listapiurl FROM listsettings.setting LIMIT 1;";
            DataSet dsList = new DataSet();
            try
            {
                dsList = DataServices.DataSetFromSQL(sqlList, null);
                DataTable dtList = dsList.Tables[0];
                listapiurl = dtList.Rows[0]["listapiurl"].ToString();

            }
            catch (Exception ex)
            {
                var a = "";
            }



            string sql = @"SELECT
                            lm.list_id,
                            lm.defaultcontext,
                            lm.defaultcontextfield,
                            lq.listquestion_id,
                            q.question_id,
                            q.labeltext,
                            q.questionquickname,
                            q.questiontype_id,
                            q.questiontypetext,
                            CASE 
	                            WHEN q.questiontype_id IN ('3d236e17-e40e-472d-95a5-5e45c5e02faf','fc1f2643-b491-4889-8d1a-910619b65722', 'ca1f1b24-b490-4e57-8921-9f680819e47c', '71490eff-a54b-455a-86b1-a4d5ab676f32') THEN ge.opts
	                            WHEN q.questiontype_id IN ('4f31c02d-fa36-4033-8977-8f25bef33d52') THEN '" + listapiurl + "' || 'GetQuestionOptionCollection/' || " + "q.questionoptioncollection_id " +
                            @"else null
                            end AS questionoptions,
                            CASE 
	                            WHEN q.questiontype_id = 'feb547a3-3b84-40c7-8007-547c9fe267e9' THEN defaultvaluetext --textarea
	                            WHEN q.questiontype_id = '6c166d07-53d0-4cd3-80f4-801cadcc88eb' THEN CAST(defaultvaluedatetime as text) --calendar control
                                WHEN q.questiontype_id = '164c31d5-d32e-4c97-91d6-a0d01822b9b6' THEN CAST(null as text) --Single Checkbox
                                WHEN q.questiontype_id = '221ca4a0-3a39-42ff-a0f4-885ffde0f0bd' THEN CAST(null as text) --Checkbox Image (Binary)
                                WHEN q.questiontype_id = '3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf' THEN CAST(COALESCE(q.questioncustomhtml, null) as text) --HTML Tag (Label, Custom HTML)
	                            ELSE CAST(defaultvaluetext as text)
                            END AS defaultvalue,

                            CASE 
	                            WHEN q.questiontype_id = 'feb547a3-3b84-40c7-8007-547c9fe267e9' THEN COALESCE(lqv.valuelongtext, defaultvaluetext) --textarea
	                            WHEN q.questiontype_id = '6c166d07-53d0-4cd3-80f4-801cadcc88eb' THEN CAST(COALESCE(lqv.valuedate, defaultvaluedatetime) as text) --calendar control
                                WHEN q.questiontype_id = '164c31d5-d32e-4c97-91d6-a0d01822b9b6' THEN CAST(COALESCE(lqv.valueboolean, null) as text) --Single Checkbox (Binary)
                                WHEN q.questiontype_id = '221ca4a0-3a39-42ff-a0f4-885ffde0f0bd' THEN CAST(COALESCE(lqv.valueboolean, null) as text) --Checkbox Image (Binary)   
                                WHEN q.questiontype_id = '3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf' THEN CAST(COALESCE(q.questioncustomhtml, null) as text) --HTML Tag (Label, Custom HTML)
	                            ELSE CAST(COALESCE(lqv.valueshorttext,defaultvaluetext) as text)
                            END AS displayvalue,

                            CASE
                                WHEN q.questiontype_id = '3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf' THEN CAST(COALESCE(q.questioncustomhtml, null) as text) --HTML Tag (Label, Custom HTML)
                                WHEN q.questiontype_id = '164c31d5-d32e-4c97-91d6-a0d01822b9b6' THEN CAST(COALESCE(q.questioncustomhtml, null) as text) --Single Checkbox (Binary)
                                WHEN q.questiontype_id = '221ca4a0-3a39-42ff-a0f4-885ffde0f0bd' THEN CAST(COALESCE(q.questioncustomhtml, null) as text) --SCheckbox Image (Binary)   
                                ELSE ''
                            END AS htmlsnippet,

                            CASE                                
                                WHEN q.questiontype_id = '164c31d5-d32e-4c97-91d6-a0d01822b9b6' THEN CAST(COALESCE(q.questioncustomhtmlalt, null) as text) --Single Checkbox (Binary)
                                WHEN q.questiontype_id = '221ca4a0-3a39-42ff-a0f4-885ffde0f0bd' THEN CAST(COALESCE(q.questioncustomhtmlalt, null) as text) --SCheckbox Image (Binary)   
                                ELSE ''
                            END AS htmlsnippetalt

                            FROM listsettings.listmanager lm
                            INNER JOIN listsettings.listquestion lq
                            ON lm.list_id = lq.list_id
                            INNER JOIN listsettings.question q
                            ON lq.question_id = q.question_id
                            LEFT OUTER JOIN
                            (
	                            SELECT * FROM entityview.core_listquestionvalue
	                            WHERE contextvalue = @contextvalue
                            ) lqv
                            ON lm.list_id = lqv.list_id
                            AND lq.listquestion_id = lqv.listquestion_id
                            AND q.question_id = lqv.question_id
                            LEFT JOIN LATERAL listsettings.getoptionsasjson(q.questionoptioncollection_id) ge(opts)
							ON q.questiontype_id IN ('3d236e17-e40e-472d-95a5-5e45c5e02faf','fc1f2643-b491-4889-8d1a-910619b65722', 'ca1f1b24-b490-4e57-8921-9f680819e47c', '71490eff-a54b-455a-86b1-a4d5ab676f32') 
                            WHERE lq.list_id = @list_id
                            AND COALESCE(isselected, false) = true
                            ORDER BY lq.ordinalposition;";

            var paramList = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId),
                new KeyValuePair<string, object>("contextvalue", contextvalue)
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
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Invalid Parameters supplied"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }



        }


        [HttpGet]
        [Route("")]
        [Route("[action]/{listId?}/{contextvalue?}")]
        public string GetListPersonBanner(string listId, string contextvalue)
        {


            //Get List Details
            string sqlListDetails = "SELECT * FROM listsettings.listmanager WHERE list_id = @list_id;";
            var paramListDetails = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId)
            };

            string baseViewID = "";
            DataSet dsListDetails = new DataSet();

            string matchedcontextfield = "";


            string patientbannerfield = "";

            try
            {
                dsListDetails = DataServices.DataSetFromSQL(sqlListDetails, paramListDetails);
                DataTable dtListDetails = dsListDetails.Tables[0];
                try
                {
                    baseViewID = dtListDetails.Rows[0]["baseview_id"].ToString();
                }
                catch { }

                try
                {
                    matchedcontextfield = dtListDetails.Rows[0]["matchedcontextfield"].ToString();
                }
                catch { }

                try
                {
                    patientbannerfield = dtListDetails.Rows[0]["patientbannerfield"].ToString();
                }
                catch { }
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve List"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            if (string.IsNullOrWhiteSpace(baseViewID))
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve baseview"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            //Get BaseView Details
            string sqlBV = "SELECT * FROM listsettings.baseviewmanager WHERE baseview_id = @baseview_id;";
            var paramListBV = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("baseview_id", baseViewID)
            };
            string baseViewName = "";
            string baseViewNameSpace = "";
            DataSet dsBV = new DataSet();
            try
            {
                dsBV = DataServices.DataSetFromSQL(sqlBV, paramListBV);
                DataTable dtBV = dsBV.Tables[0];
                try
                {
                    baseViewName = dtBV.Rows[0]["baseviewname"].ToString();
                    baseViewNameSpace = dtBV.Rows[0]["baseviewnamespace"].ToString();
                }
                catch { }
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve List"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            if (string.IsNullOrWhiteSpace(baseViewName))
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Unable to retrieve baseview name"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }



            //Get List and Attribute Details
            string sqlAttributes = "SELECT " + patientbannerfield + " as patientbanner FROM baseview." + baseViewNameSpace + "_" + baseViewName + " WHERE " + matchedcontextfield + " = @contextvalue LIMIT 1;";
            var paramListAttributes = new List<KeyValuePair<string, object>>() {
                //new KeyValuePair<string, object>("patientbannerfield", patientbannerfield),
                //new KeyValuePair<string, object>("matchedcontextfield", matchedcontextfield),
                new KeyValuePair<string, object>("contextvalue", contextvalue),
            };

            DataSet dsAttributes = new DataSet();
            try
            {
                dsAttributes = DataServices.DataSetFromSQL(sqlAttributes, paramListAttributes);
                DataTable dtAttributes = dsAttributes.Tables[0];
                var json = DataServices.ConvertDataTabletoJSONObject(dtAttributes);
                return json;
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Invalid Parameters supplied"
                };



                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }



        }




        [HttpGet]
        [Route("")]
        [Route("[action]/{questionoptioncollection_id?}")]
        public string GetQuestionOptionCollection(string questionoptioncollection_id)
        {


            string sql = @"SELECT listsettings.getoptionsasjson('" + questionoptioncollection_id + "');";
            var paramList = new List<KeyValuePair<string, object>>()
            {
                //new KeyValuePair<string, object>("questionoptioncollection_id", questionoptioncollection_id)
            };

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                //var json = DataServices.ConvertDataTabletoJSONString(dt);
                //return json;
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Invalid Parameters supplied"
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


        }
        [HttpGet]
        [Route("")]
        [Route("[action]/{encounterId?}")]
        public string GenerateInpatientTransferMessage(string encounterId)
        {
            string messageControlId = null;

            string sql = "select ebenc.wardcode, beds.baycode, beds.bedcode, to_char(ebenc.edd, 'YYYYMMDDHH24MISS') as edd, enc.visitnumber, " +
                         "enc.consultingdoctortext, exenc.consultingdoctorgmccode, exenc.consultingdoctorpasid, exenc.specialtycode, " +
                         "enc.person_id, enc.encounter_id, enc.patientclasscode, mrnpid.idnumber as mrn, nhspid.idnumber as empi " +
                         "from entityview.local_eboards_encounter ebenc " +
                         "inner join entityview.core_encounter enc on enc.encounter_id = ebenc.encounter_id " +
                         "left outer join entityview.extended_encounter exenc on enc.encounter_id = exenc.encounter_id " +
                         "left outer join entityview.meta_wardbaybed beds on beds.wardbaybed_id = ebenc.bedcode " +
                         "left outer join entityview.core_personidentifier mrnpid on enc.person_id = mrnpid.person_id and mrnpid.idtypecode = 'RNOH' " +
                         "left outer join entityview.core_personidentifier nhspid on enc.person_id = nhspid.person_id and nhspid.idtypecode = 'NHS' " +
                         "where enc.encounter_id = @encounter_id";

            var selectParamList = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("encounter_id", encounterId)
            };

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, selectParamList);
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    InpatientTransferMessageModel messageData = new InpatientTransferMessageModel()
                    {
                        BayCode = Convert.ToString(dt.Rows[0]["baycode"]),
                        BedCode = Convert.ToString(dt.Rows[0]["bedcode"]),
                        ConsultingDoctorGMCCode = Convert.ToString(dt.Rows[0]["consultingdoctorgmccode"]),
                        ConsultingDoctorName = Convert.ToString(dt.Rows[0]["consultingdoctortext"]),
                        ConsultingDoctorPASId = Convert.ToString(dt.Rows[0]["consultingdoctorpasid"]),
                        EMPI = Convert.ToString(dt.Rows[0]["empi"]),
                        Encounter_id = Convert.ToString(dt.Rows[0]["encounter_id"]),
                        ExpectedDischargeDate = Convert.ToString(dt.Rows[0]["edd"]),
                        MRN = Convert.ToString(dt.Rows[0]["mrn"]),
                        PatinetClassCode = Convert.ToString(dt.Rows[0]["patientclasscode"]),
                        Person_Id = Convert.ToString(dt.Rows[0]["person_id"]),
                        SpecialtyCode = Convert.ToString(dt.Rows[0]["specialtycode"]),
                        VisitNumber = Convert.ToString(dt.Rows[0]["visitnumber"]),
                        WardCode = Convert.ToString(dt.Rows[0]["wardcode"])
                    };
                    var hl7Message = HL7MessageServices.GenerateInpatientTransferMessage(messageData, out messageControlId);

                    OutboundMessageStore store = new OutboundMessageStore()
                    {
                        EMPINumber = Convert.ToString(dt.Rows[0]["empi"]),
                        Encounter_Id = Convert.ToString(dt.Rows[0]["encounter_id"]),
                        HospitalNumber = Convert.ToString(dt.Rows[0]["mrn"]),
                        Message = hl7Message,
                        Message_Id = Guid.NewGuid().ToString(),
                        OutboundMessageStore_Id = Guid.NewGuid().ToString(),
                        Person_Id = Convert.ToString(dt.Rows[0]["person_id"]),
                        SendStatus = 0
                    };

                    InsertOutboundMessage(store);
                }
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 400;
                var httpErr = new SynapseHTTPError
                {
                    ErrorCode = "HTTP.400",
                    ErrorType = "Client Error",
                    ErrorDescription = "Error processing request."
                };

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            return messageControlId;
        }

        private void InsertOutboundMessage(OutboundMessageStore store)
        {
            string sql = "select outbounddestination_id, destinationip, destinationport from interop.outbounddestination";

            DataSet ds = DataServices.DataSetFromSQL(sql, new List<KeyValuePair<string, object>>());

            string insertSQL = "INSERT INTO interop.outboundmessagestore( " +
                               "_createdsource, _createdby, outboundmessagestore_id, message_id, message, destinationip, destinationport, " +
                               "sendstatus, person_id, encounter_id, hospitalnumber, empinumber) " +
                               "VALUES(@createdsource, @createdby, @outboundmessagestore_id, @message_id, @message, @destinationip, @destinationport, " +
                               "@sendstatus, @person_id, @encounter_id, @hospitalnumber, @empinumber) ";

            var insertParamList = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("createdsource", "SynapseDynamicAPI"),
                    new KeyValuePair<string, object>("createdby", "SynapseDynamicAPI"),
                    new KeyValuePair<string, object>("outboundmessagestore_id", store.OutboundMessageStore_Id),
                    new KeyValuePair<string, object>("message_id", store.Message_Id),
                    new KeyValuePair<string, object>("message", store.Message),
                    new KeyValuePair<string, object>("destinationip", Convert.ToString(ds.Tables[0].Rows[0]["destinationip"])),
                    new KeyValuePair<string, object>("destinationport", Convert.ToString(ds.Tables[0].Rows[0]["destinationport"])),
                    new KeyValuePair<string, object>("sendstatus", store.SendStatus),
                    new KeyValuePair<string, object>("person_id", store.Person_Id),
                    new KeyValuePair<string, object>("encounter_id", store.Encounter_Id),
                    new KeyValuePair<string, object>("hospitalnumber", store.HospitalNumber),
                    new KeyValuePair<string, object>("empinumber", store.EMPINumber)
                };

            DataServices.ExcecuteNonQueryFromSQL(insertSQL, insertParamList);
        }
    }
}


public class ListAttribute
{
    public string listattribute_id { get; set; }
    public string list_id { get; set; }
    public string baseviewattribute_id { get; set; }
    public string attributename { get; set; }
    public string datatype { get; set; }
    public string displayname { get; set; }
    public int ordinalposition { get; set; }
    public string defaultcssclassname { get; set; }

    //List<ListAttribute> la = new List<ListAttribute>();
    //la = (from DataRow row in dt.Rows

    //select new ListAttribute
    //{
    //    listattribute_id = row["listattribute_id"].ToString(),
    //    list_id = row["list_id"].ToString(),
    //    baseviewattribute_id = row["baseviewattribute_id"].ToString(),
    //    attributename = row["attributename"].ToString(),
    //    datatype = row["datatype"].ToString(),
    //    displayname = row["displayname"].ToString(),
    //    ordinalposition = Convert.ToInt32(row["ordinalposition"].ToString()),
    //    defaultcssclassname = row["defaultcssclassname"].ToString()

    //}).ToList();
}


