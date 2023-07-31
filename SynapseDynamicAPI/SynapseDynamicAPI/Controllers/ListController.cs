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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SynapseDynamicAPI.Services;

namespace SynapseDynamicAPI.Controllers
{

    [Authorize]
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
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONString(dt);
            return json;
            //try
            //{
            //    ds = DataServices.DataSetFromSQL(sql, paramList);
            //    DataTable dt = ds.Tables[0];
            //    var json = DataServices.ConvertDataTabletoJSONString(dt);
            //    return json;
            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Invalid Parameters supplied"
            //    };

            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}


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
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONObject(dt);
            return json;
            //try
            //{
            //    ds = DataServices.DataSetFromSQL(sql, paramList);
            //    DataTable dt = ds.Tables[0];
            //    var json = DataServices.ConvertDataTabletoJSONObject(dt);
            //    return json;
            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Invalid Parameters supplied"
            //    };

            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}


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

            string defaultContext = string.Empty;
            string defaultContextField = string.Empty;
            string matchedcontextfield = "";
            string rowcssfield = "";
            string snapshotTemplateLine1;
            string snapshotTemplateLine2;
            string snapshotTemplateBadge;

            string wardPersonaContextField;
            string clinicalUnitPersonaContextField;
            string teamPersonaContextField;
            string specialtyPersonaContextField;
            string listMasterSortColumn = "";
            string listmasterOrderby = "";
            string ordergroupbystatement = "";
            dsListDetails = DataServices.DataSetFromSQL(sqlListDetails, paramListDetails);
            DataTable dtListDetails = dsListDetails.Tables[0];
            try
            {
                baseViewID = dtListDetails.Rows[0]["baseview_id"].ToString();
            }
            catch { }

            try
            {
                defaultContext = dtListDetails.Rows[0]["defaultcontext"].ToString();
            }
            catch { }

            try
            {
                defaultContextField = dtListDetails.Rows[0]["defaultcontextfield"].ToString();
            }
            catch { }

            try
            {
                matchedcontextfield = dtListDetails.Rows[0]["matchedcontextfield"].ToString();
            }
            catch { }

            try
            {
                rowcssfield = dtListDetails.Rows[0]["rowcssfield"].ToString();
            }
            catch { }
            try
            {
                listMasterSortColumn = Convert.ToString(dtListDetails.Rows[0]["defaultsortcolumn"]);
                listmasterOrderby = Convert.ToString(dtListDetails.Rows[0]["defaultsortorder"]);
            }
            catch
            { }
            if (rowcssfield == "0")
            {
                rowcssfield = "''";
            }
            else if (String.IsNullOrWhiteSpace(rowcssfield))
            {
                rowcssfield = "''";
            }

            snapshotTemplateLine1 = Convert.ToString(dtListDetails.Rows[0]["snapshottemplateline1"]);
            snapshotTemplateLine2 = Convert.ToString(dtListDetails.Rows[0]["snapshottemplateline2"]);
            snapshotTemplateBadge = Convert.ToString(dtListDetails.Rows[0]["snapshottemplatebadge"]);

            wardPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["wardpersonacontextfield"]);
            clinicalUnitPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["clinicalunitpersonacontextfield"]);
            teamPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["teampersonacontextfield"]);
            specialtyPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["specialtypersonacontextfield"]);
            //try
            //{
            //    dsListDetails = DataServices.DataSetFromSQL(sqlListDetails, paramListDetails);
            //    DataTable dtListDetails = dsListDetails.Tables[0];
            //    try
            //    {
            //        baseViewID = dtListDetails.Rows[0]["baseview_id"].ToString();
            //    }
            //    catch { }

            //    try
            //    {
            //        matchedcontextfield = dtListDetails.Rows[0]["matchedcontextfield"].ToString();
            //    }
            //    catch { }

            //    try
            //    {
            //        rowcssfield = dtListDetails.Rows[0]["rowcssfield"].ToString();
            //    }
            //    catch { }
            //    try
            //    {
            //        listMasterSortColumn = Convert.ToString(dtListDetails.Rows[0]["defaultsortcolumn"]);
            //        listmasterOrderby = Convert.ToString(dtListDetails.Rows[0]["defaultsortorder"]);
            //    }
            //    catch
            //    { }
            //    if (rowcssfield == "0")
            //    {
            //        rowcssfield = "''";
            //    }
            //    else if (String.IsNullOrWhiteSpace(rowcssfield))
            //    {
            //        rowcssfield = "''";
            //    }

            //    snapshotTemplateLine1 = Convert.ToString(dtListDetails.Rows[0]["snapshottemplateline1"]);
            //    snapshotTemplateLine2 = Convert.ToString(dtListDetails.Rows[0]["snapshottemplateline2"]);
            //    snapshotTemplateBadge = Convert.ToString(dtListDetails.Rows[0]["snapshottemplatebadge"]);

            //    wardPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["wardpersonacontextfield"]);
            //    clinicalUnitPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["clinicalunitpersonacontextfield"]);
            //    teamPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["teampersonacontextfield"]);
            //    specialtyPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["specialtypersonacontextfield"]);
            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Unable to retrieve List:" + ex.ToString()
            //    };

            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}

            if (listMasterSortColumn.Trim() != "")
            {
                ordergroupbystatement = "ORDER BY " + listMasterSortColumn + " " + listmasterOrderby;
            }
            else
            {
                ordergroupbystatement = "";

            }

            if (string.IsNullOrWhiteSpace(baseViewID))
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Unable to retrieve baseview", "Client Error");
                //this.HttpContext.Response.StatusCode = 400;
                //var httpErr = new SynapseHTTPError
                //{
                //    ErrorCode = "HTTP.400",
                //    ErrorType = "Client Error",
                //    ErrorDescription = "Unable to retrieve baseview"
                //};

                //return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            //Get BaseView Details
            string sqlBV = "SELECT * FROM listsettings.baseviewmanager WHERE baseview_id = @baseview_id;";
            var paramListBV = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("baseview_id", baseViewID)
            };
            string baseViewName = "";
            string baseViewNameSpace = "";
            DataSet dsBV = new DataSet();
            dsBV = DataServices.DataSetFromSQL(sqlBV, paramListBV);
            DataTable dtBV = dsBV.Tables[0];
            try
            {
                baseViewName = dtBV.Rows[0]["baseviewname"].ToString();
                baseViewNameSpace = dtBV.Rows[0]["baseviewnamespace"].ToString();
            }
            catch { }
            //try
            //{
            //    dsBV = DataServices.DataSetFromSQL(sqlBV, paramListBV);
            //    DataTable dtBV = dsBV.Tables[0];
            //    try
            //    {
            //        baseViewName = dtBV.Rows[0]["baseviewname"].ToString();
            //        baseViewNameSpace = dtBV.Rows[0]["baseviewnamespace"].ToString();
            //    }
            //    catch { }
            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Unable to retrieve List"
            //    };

            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}


            if (string.IsNullOrWhiteSpace(baseViewName))
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Unable to retrieve baseview name", "Client Error");
                //this.HttpContext.Response.StatusCode = 400;
                //var httpErr = new SynapseHTTPError
                //{
                //    ErrorCode = "HTTP.400",
                //    ErrorType = "Client Error",
                //    ErrorDescription = "Unable to retrieve baseview name"
                //};

                //return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }



            //Get List and Attribute Details
            string sqlAttributes = "SELECT *, " +
                                   "@defaultcontext AS defaultcontext, " +
                                   "@defaultcontextfield AS defaultcontextfield, " +
                                   "@matchedcontextfield AS matchedcontext, " +
                                   "@snapshottemplateline1 AS snapshottemplateline1, " +
                                   "@snapshottemplateline2 AS snapshottemplateline2, " +
                                   "@snapshottemplatebadge AS snapshottemplatebadge, " +
                                   "@wardpersonacontextfield AS wardpersonacontextfield, " +
                                   "@clinicalunitpersonacontextfield AS clinicalunitpersonacontextfield, " +
                                   "@teampersonacontextfield AS teampersonacontextfield, " +
                                   "@specialtypersonacontextfield AS specialtypersonacontextfield " +
                                   "FROM listsettings.listattribute " +
                                   "WHERE list_id = @list_id AND isselected = true " +
                                   "ORDER BY ordinalposition;";
            var paramListAttributes = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId),
                new KeyValuePair<string, object>("defaultcontext", defaultContext),
                new KeyValuePair<string, object>("defaultcontextfield", defaultContextField),
                new KeyValuePair<string, object>("matchedcontextfield", matchedcontextfield),
                new KeyValuePair<string, object>("snapshottemplateline1", snapshotTemplateLine1),
                new KeyValuePair<string, object>("snapshottemplateline2", snapshotTemplateLine2),
                new KeyValuePair<string, object>("snapshottemplatebadge", snapshotTemplateBadge),
                new KeyValuePair<string, object>("wardpersonacontextfield", wardPersonaContextField),
                new KeyValuePair<string, object>("clinicalunitpersonacontextfield", clinicalUnitPersonaContextField),
                new KeyValuePair<string, object>("teampersonacontextfield", teamPersonaContextField),
                new KeyValuePair<string, object>("specialtypersonacontextfield", specialtyPersonaContextField)
            };

            DataSet dsAttributes = new DataSet();
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

                sb.AppendLine("'rowcssfield', " + rowcssfield + ",");

                sb.AppendLine("'defaultcontext', '" + row["defaultcontext"].ToString() + "', ");
                sb.AppendLine("'defaultcontextfield', '" + row["defaultcontextfield"].ToString() + "', ");

                sb.AppendLine("'matchedcontext', " + row["matchedcontext"].ToString() + ", ");
                sb.AppendLine("'snapshottemplateline1', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplateline1"])) ? "''" : row["snapshottemplateline1"].ToString()) + ", ");
                sb.AppendLine("'snapshottemplateline2', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplateline2"])) ? "''" : row["snapshottemplateline2"].ToString()) + ", ");
                sb.AppendLine("'snapshottemplatebadge', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplatebadge"])) ? "''" : row["snapshottemplatebadge"].ToString()) + ", ");
                sb.AppendLine("'wardpersonacontextfield', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["wardpersonacontextfield"])) ? "''" : row["wardpersonacontextfield"].ToString()) + ", ");
                sb.AppendLine("'clinicalunitpersonacontextfield', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["clinicalunitpersonacontextfield"])) ? "''" : row["clinicalunitpersonacontextfield"].ToString()) + ", ");
                sb.AppendLine("'teampersonacontextfield', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["teampersonacontextfield"])) ? "''" : row["teampersonacontextfield"].ToString()) + ", ");
                sb.AppendLine("'specialtypersonacontextfield', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["specialtypersonacontextfield"])) ? "''" : row["specialtypersonacontextfield"].ToString()));

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
            sb.AppendLine(" FROM (SELECT *, " + matchedcontextfield + " AS matchedcontext, " + rowcssfield + " AS rowcssfield, " +
                          (string.IsNullOrWhiteSpace(snapshotTemplateLine1) ? "''" : snapshotTemplateLine1) + " AS snapshottemplateline1, " +
                          (string.IsNullOrWhiteSpace(snapshotTemplateLine2) ? "''" : snapshotTemplateLine2) + " AS snapshottemplateline2, " +
                          (string.IsNullOrWhiteSpace(snapshotTemplateBadge) ? "''" : snapshotTemplateBadge) + " AS snapshottemplatebadge, " +
                          (string.IsNullOrWhiteSpace(wardPersonaContextField) ? "''" : wardPersonaContextField) + " AS wardpersonacontextfield, " +
                          (string.IsNullOrWhiteSpace(clinicalUnitPersonaContextField) ? "''" : clinicalUnitPersonaContextField) + " AS clinicalunitpersonacontextfield, " +
                          (string.IsNullOrWhiteSpace(teamPersonaContextField) ? "''" : teamPersonaContextField) + " AS teampersonacontextfield, " +
                          (string.IsNullOrWhiteSpace(specialtyPersonaContextField) ? "''" : specialtyPersonaContextField) + " AS specialtypersonacontextfield " +
                          "FROM baseview." + baseViewNameSpace + "_" + baseViewName + " " + ordergroupbystatement + ") bv;");

            string listSQL = sb.ToString();

            var paramList = new List<KeyValuePair<string, object>>()
            {
                //new KeyValuePair<string, object>("matchedcontextfield", matchedcontextfield),
            };

            DataSet dsList = new DataSet();
            dsList = DataServices.DataSetFromSQL(listSQL, paramList);
            DataTable dtList = dsList.Tables[0];
            return DataServices.ConvertDataTabletoJSONString(dtList);
            //try
            //{
            //    dsList = DataServices.DataSetFromSQL(listSQL, paramList);
            //    DataTable dtList = dsList.Tables[0];
            //    return DataServices.ConvertDataTabletoJSONString(dtList);
            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Invalid Parameters supplied"
            //    };



            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}



            //try
            //{
            //    dsAttributes = DataServices.DataSetFromSQL(sqlAttributes, paramListAttributes);
            //    DataTable dtAttributes = dsAttributes.Tables[0];

            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine("SELECT");
            //    //sb.AppendLine("encounter_id,");
            //    int iCount = 0;

            //    foreach (DataRow row in dtAttributes.Rows)
            //    {
            //        sb.AppendLine("json_build_object (");
            //        sb.AppendLine("'attributevalue', " + row["attributename"].ToString() + ",");
            //        sb.AppendLine("'attributename','" + row["attributename"].ToString() + "',");
            //        sb.AppendLine("'displayname','" + row["displayname"].ToString() + "',");
            //        //sb.AppendLine("'matchedcontext','" + row["matchedcontext"].ToString() + "',");
            //        sb.AppendLine("'defaultcssclassname','" + row["defaultcssclassname"].ToString() + "',");

            //        sb.AppendLine("'rowcssfield', " + rowcssfield + ",");

            //        sb.AppendLine("'matchedcontext', " + row["matchedcontext"].ToString() + ", ");
            //        sb.AppendLine("'snapshottemplateline1', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplateline1"])) ? "''" : row["snapshottemplateline1"].ToString()) + ", ");
            //        sb.AppendLine("'snapshottemplateline2', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplateline2"])) ? "''" : row["snapshottemplateline2"].ToString()) + ", ");
            //        sb.AppendLine("'snapshottemplatebadge', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplatebadge"])) ? "''" : row["snapshottemplatebadge"].ToString()) + ", ");
            //        sb.AppendLine("'wardpersonacontextfield', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["wardpersonacontextfield"])) ? "''" : row["wardpersonacontextfield"].ToString()) + ", ");
            //        sb.AppendLine("'clinicalunitpersonacontextfield', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["clinicalunitpersonacontextfield"])) ? "''" : row["clinicalunitpersonacontextfield"].ToString()) + ", ");
            //        sb.AppendLine("'teampersonacontextfield', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["teampersonacontextfield"])) ? "''" : row["teampersonacontextfield"].ToString()) + ", ");
            //        sb.AppendLine("'specialtypersonacontextfield', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["specialtypersonacontextfield"])) ? "''" : row["specialtypersonacontextfield"].ToString()));

            //        //sb.AppendLine("'ordinalposition', " + row["ordinalposition"].ToString() + "");

            //        if (iCount == dtAttributes.Rows.Count - 1)
            //        {
            //            sb.AppendLine(") as col_" + iCount.ToString());
            //        }
            //        else
            //        {
            //            sb.AppendLine(") as col_" + iCount.ToString() + ",");
            //        }


            //        //TextBox1.Text = row["ImagePath"].ToString();
            //        iCount++;
            //    }
            //    sb.AppendLine(" FROM (SELECT *, " + matchedcontextfield + " AS matchedcontext, " + rowcssfield + " AS rowcssfield, " +
            //                  (string.IsNullOrWhiteSpace(snapshotTemplateLine1) ? "''" : snapshotTemplateLine1) + " AS snapshottemplateline1, " +
            //                  (string.IsNullOrWhiteSpace(snapshotTemplateLine2) ? "''" : snapshotTemplateLine2) + " AS snapshottemplateline2, " +
            //                  (string.IsNullOrWhiteSpace(snapshotTemplateBadge) ? "''" : snapshotTemplateBadge) + " AS snapshottemplatebadge, " +
            //                  (string.IsNullOrWhiteSpace(wardPersonaContextField) ? "''" : wardPersonaContextField) + " AS wardpersonacontextfield, " +
            //                  (string.IsNullOrWhiteSpace(clinicalUnitPersonaContextField) ? "''" : clinicalUnitPersonaContextField) + " AS clinicalunitpersonacontextfield, " +
            //                  (string.IsNullOrWhiteSpace(teamPersonaContextField) ? "''" : teamPersonaContextField) + " AS teampersonacontextfield, " +
            //                  (string.IsNullOrWhiteSpace(specialtyPersonaContextField) ? "''" : specialtyPersonaContextField) + " AS specialtypersonacontextfield " +
            //                  "FROM baseview." + baseViewNameSpace + "_" + baseViewName + " " + ordergroupbystatement + ") bv;");

            //    string listSQL = sb.ToString();

            //    var paramList = new List<KeyValuePair<string, object>>()
            //    {
            //        //new KeyValuePair<string, object>("matchedcontextfield", matchedcontextfield),
            //    };

            //    DataSet dsList = new DataSet();
            //    try
            //    {
            //        dsList = DataServices.DataSetFromSQL(listSQL, paramList);
            //        DataTable dtList = dsList.Tables[0];
            //        return DataServices.ConvertDataTabletoJSONString(dtList);
            //    }
            //    catch (Exception ex)
            //    {
            //        this.HttpContext.Response.StatusCode = 400;
            //        var httpErr = new SynapseHTTPError
            //        {
            //            ErrorCode = "HTTP.400",
            //            ErrorType = "Client Error",
            //            ErrorDescription = "Invalid Parameters supplied"
            //        };



            //        return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //    }




            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Invalid Parameters supplied"
            //    };



            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}



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


            #endregion

            //Get List Details
            string sqlListDetails = "SELECT * FROM listsettings.listmanager WHERE list_id = @list_id;";
            var paramListDetails = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId)
            };

            DataSet dsListPersonaContexts = new DataSet();

            string baseViewID = "";
            DataSet dsListDetails = new DataSet();

            string defaultContext = string.Empty;
            string defaultContextField = string.Empty;

            string matchedcontextfield = "";
            string rowcssfield = "";

            string snapshotTemplateLine1;
            string snapshotTemplateLine2;
            string snapshotTemplateBadge;

            //string wardPersonaContextField;
            //string clinicalUnitPersonaContextField;
            //string teamPersonaContextField;
            //string specialtyPersonaContextField;
            string listMasterSortColumn = "";
            string listmasterOrderby = "";
            string ordergroupbystatement = "";

            dsListDetails = DataServices.DataSetFromSQL(sqlListDetails, paramListDetails);
            DataTable dtListDetails = dsListDetails.Tables[0];
            try
            {
                baseViewID = dtListDetails.Rows[0]["baseview_id"].ToString();
            }
            catch { }

            try
            {
                defaultContext = dtListDetails.Rows[0]["defaultcontext"].ToString();
            }
            catch { }

            try
            {
                defaultContextField = dtListDetails.Rows[0]["defaultcontextfield"].ToString();
            }
            catch { }

            try
            {
                matchedcontextfield = dtListDetails.Rows[0]["matchedcontextfield"].ToString();
            }
            catch { }

            try
            {
                rowcssfield = dtListDetails.Rows[0]["rowcssfield"].ToString();
            }
            catch { }
            try
            {
                listMasterSortColumn = Convert.ToString(dtListDetails.Rows[0]["defaultsortcolumn"]);
                listmasterOrderby = Convert.ToString(dtListDetails.Rows[0]["defaultsortorder"]);
            }
            catch
            { }
            snapshotTemplateLine1 = Convert.ToString(dtListDetails.Rows[0]["snapshottemplateline1"]);
            snapshotTemplateLine2 = Convert.ToString(dtListDetails.Rows[0]["snapshottemplateline2"]);
            snapshotTemplateBadge = Convert.ToString(dtListDetails.Rows[0]["snapshottemplatebadge"]);

            //wardPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["wardpersonacontextfield"]);
            //clinicalUnitPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["clinicalunitpersonacontextfield"]);
            //teamPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["teampersonacontextfield"]);
            //specialtyPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["specialtypersonacontextfield"]);

            //get all PersonaContexts defined for this list from list contexts table
            string sqlPersonaContextsForList = "select lc.persona_id, lc.field, ps.displayname, ps.personaname from entitystorematerialised.meta_listcontexts lc " +
                                                 "inner join entitystorematerialised.meta_persona ps on ps.persona_id = lc.persona_id " +
                                                    "where list_id = @list_id;";

            dsListPersonaContexts = DataServices.DataSetFromSQL(sqlPersonaContextsForList, paramListDetails);

            //try
            //{
            //    dsListDetails = DataServices.DataSetFromSQL(sqlListDetails, paramListDetails);
            //    DataTable dtListDetails = dsListDetails.Tables[0];
            //    try
            //    {
            //        baseViewID = dtListDetails.Rows[0]["baseview_id"].ToString();
            //    }
            //    catch { }

            //    try
            //    {
            //        matchedcontextfield = dtListDetails.Rows[0]["matchedcontextfield"].ToString();
            //    }
            //    catch { }

            //    try
            //    {
            //        rowcssfield = dtListDetails.Rows[0]["rowcssfield"].ToString();
            //    }
            //    catch { }
            //    try
            //    {
            //        listMasterSortColumn = Convert.ToString(dtListDetails.Rows[0]["defaultsortcolumn"]);
            //        listmasterOrderby = Convert.ToString(dtListDetails.Rows[0]["defaultsortorder"]);
            //    }
            //    catch
            //    { }
            //    snapshotTemplateLine1 = Convert.ToString(dtListDetails.Rows[0]["snapshottemplateline1"]);
            //    snapshotTemplateLine2 = Convert.ToString(dtListDetails.Rows[0]["snapshottemplateline2"]);
            //    snapshotTemplateBadge = Convert.ToString(dtListDetails.Rows[0]["snapshottemplatebadge"]);

            //    //wardPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["wardpersonacontextfield"]);
            //    //clinicalUnitPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["clinicalunitpersonacontextfield"]);
            //    //teamPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["teampersonacontextfield"]);
            //    //specialtyPersonaContextField = Convert.ToString(dtListDetails.Rows[0]["specialtypersonacontextfield"]);

            //    //get all PersonaContexts defined for this list from list contexts table
            //    string sqlPersonaContextsForList = "select lc.persona_id, lc.field, ps.displayname, ps.personaname from entitystorematerialised.meta_listcontexts lc " +
            //                                         "inner join entitystorematerialised.meta_persona ps on ps.persona_id = lc.persona_id " +
            //                                            "where list_id = @list_id;";

            //    dsListPersonaContexts = DataServices.DataSetFromSQL(sqlPersonaContextsForList, paramListDetails);


            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Unable to retrieve List"
            //    };

            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}

            if (results.Count > 3)
            {
                ordergroupbystatement = results[3].ordergroupbystatement.ToString();
            }
            else if (listMasterSortColumn.Trim() != "")
            {
                ordergroupbystatement = "ORDER BY " + listMasterSortColumn + " " + listmasterOrderby;
            }
            else
            {
                ordergroupbystatement = "";
            }



            if (string.IsNullOrWhiteSpace(baseViewID))
            {
                throw new InterneuronBusinessException(400, "Unable to retrieve baseview", "Client Error");

                //this.HttpContext.Response.StatusCode = 400;
                //var httpErr = new SynapseHTTPError
                //{
                //    ErrorCode = "HTTP.400",
                //    ErrorType = "Client Error",
                //    ErrorDescription = "Unable to retrieve baseview"
                //};

                //return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            //Get BaseView Details
            string sqlBV = "SELECT * FROM listsettings.baseviewmanager WHERE baseview_id = @baseview_id;";
            var paramListBV = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("baseview_id", baseViewID)
            };
            string baseViewName = "";
            string baseViewNameSpace = "";
            DataSet dsBV = new DataSet();

            dsBV = DataServices.DataSetFromSQL(sqlBV, paramListBV);
            DataTable dtBV = dsBV.Tables[0];
            try
            {
                baseViewName = dtBV.Rows[0]["baseviewname"].ToString();
                baseViewNameSpace = dtBV.Rows[0]["baseviewnamespace"].ToString();
            }
            catch { }

            //try
            //{
            //    dsBV = DataServices.DataSetFromSQL(sqlBV, paramListBV);
            //    DataTable dtBV = dsBV.Tables[0];
            //    try
            //    {
            //        baseViewName = dtBV.Rows[0]["baseviewname"].ToString();
            //        baseViewNameSpace = dtBV.Rows[0]["baseviewnamespace"].ToString();
            //    }
            //    catch { }

            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Unable to retrieve List"
            //    };

            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}


            if (string.IsNullOrWhiteSpace(baseViewName))
            {
                throw new InterneuronBusinessException(400, "Unable to retrieve baseview name", "Client Error");

                //this.HttpContext.Response.StatusCode = 400;
                //var httpErr = new SynapseHTTPError
                //{
                //    ErrorCode = "HTTP.400",
                //    ErrorType = "Client Error",
                //    ErrorDescription = "Unable to retrieve baseview name"
                //};

                //return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            //Get List and Attribute Details
            string sqlAttributes = "SELECT *, " +
                                   "@defaultcontext AS defaultcontext, " +
                                   "@defaultcontextfield AS defaultcontextfield, " +
                                   "@matchedcontextfield AS matchedcontext, " +
                                   "@snapshottemplateline1 AS snapshottemplateline1, " +
                                   "@snapshottemplateline2 AS snapshottemplateline2, " +
                                   "@snapshottemplatebadge AS snapshottemplatebadge " +
                                   "FROM listsettings.listattribute " +
                                   "WHERE list_id = @list_id AND isselected = true " +
                                   "ORDER BY ordinalposition;";

            var paramListAttributes = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId),
                new KeyValuePair<string, object>("defaultcontext", defaultContext),
                new KeyValuePair<string, object>("defaultcontextfield", defaultContextField),
                new KeyValuePair<string, object>("matchedcontextfield", matchedcontextfield),
                new KeyValuePair<string, object>("snapshottemplateline1", snapshotTemplateLine1),
                new KeyValuePair<string, object>("snapshottemplateline2", snapshotTemplateLine2),
                new KeyValuePair<string, object>("snapshottemplatebadge", snapshotTemplateBadge)
            };

            DataSet dsAttributes = new DataSet();

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

                sb.AppendLine("'rowcssfield', " + rowcssfield + ",");

                sb.AppendLine("'defaultcontext', '" + row["defaultcontext"].ToString() + "', ");
                sb.AppendLine("'defaultcontextfield', '" + row["defaultcontextfield"].ToString() + "', ");

                sb.AppendLine("'matchedcontext', " + row["matchedcontext"].ToString() + ", ");
                sb.AppendLine("'snapshottemplateline1', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplateline1"])) ? "''" : row["snapshottemplateline1"].ToString()) + ", ");
                sb.AppendLine("'snapshottemplateline2', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplateline2"])) ? "''" : row["snapshottemplateline2"].ToString()) + ", ");
                sb.AppendLine("'snapshottemplatebadge', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplatebadge"])) ? "''" : row["snapshottemplatebadge"].ToString()) + " ");

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

            sb.AppendLine(" FROM (SELECT *, " + matchedcontextfield + " AS matchedcontext, " + rowcssfield + " AS rowcssfield, " +
                          (string.IsNullOrWhiteSpace(snapshotTemplateLine1) ? "''" : snapshotTemplateLine1) + " AS snapshottemplateline1, " +
                          (string.IsNullOrWhiteSpace(snapshotTemplateLine2) ? "''" : snapshotTemplateLine2) + " AS snapshottemplateline2, " +
                          (string.IsNullOrWhiteSpace(snapshotTemplateBadge) ? "''" : snapshotTemplateBadge) + " AS snapshottemplatebadge");

            //append persona context filters
            foreach (DataRow row in dsListPersonaContexts.Tables[0].Rows)
            {
                sb.AppendLine(", " + row["field"] + " AS " + "\"" + row["persona_id"] + "\"");
            }

            sb.AppendLine("FROM baseview." + baseViewNameSpace + "_" + baseViewName + " " + ordergroupbystatement + ") AS View " + filtersToApplySB.ToString() + ";");
            string listSQL = sb.ToString();

            var paramList = new List<KeyValuePair<string, object>>()
            {
            };

            DataSet dsList = new DataSet();

            dsList = DataServices.DataSetFromSQL(listSQL, paramListFromPost);
            DataTable dtList = dsList.Tables[0];
            return DataServices.ConvertDataTabletoJSONString(dtList);

            //try
            //{
            //    dsList = DataServices.DataSetFromSQL(listSQL, paramListFromPost);
            //    DataTable dtList = dsList.Tables[0];
            //    return DataServices.ConvertDataTabletoJSONString(dtList);
            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Invalid Parameters supplied"
            //    };

            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}

            //try
            //{
            //    dsAttributes = DataServices.DataSetFromSQL(sqlAttributes, paramListAttributes);
            //    DataTable dtAttributes = dsAttributes.Tables[0];

            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine("SELECT");
            //    //sb.AppendLine("encounter_id,");
            //    int iCount = 0;
            //    foreach (DataRow row in dtAttributes.Rows)
            //    {


            //        sb.AppendLine("json_build_object (");
            //        sb.AppendLine("'attributevalue', " + row["attributename"].ToString() + ",");
            //        sb.AppendLine("'attributename','" + row["attributename"].ToString() + "',");
            //        sb.AppendLine("'displayname','" + row["displayname"].ToString() + "',");
            //        //sb.AppendLine("'matchedcontext','" + row["matchedcontext"].ToString() + "',");
            //        sb.AppendLine("'defaultcssclassname','" + row["defaultcssclassname"].ToString() + "',");

            //        sb.AppendLine("'rowcssfield', " + rowcssfield + ",");

            //        sb.AppendLine("'matchedcontext', " + row["matchedcontext"].ToString() + ", ");
            //        sb.AppendLine("'snapshottemplateline1', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplateline1"])) ? "''" : row["snapshottemplateline1"].ToString()) + ", ");
            //        sb.AppendLine("'snapshottemplateline2', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplateline2"])) ? "''" : row["snapshottemplateline2"].ToString()) + ", ");
            //        sb.AppendLine("'snapshottemplatebadge', " + (string.IsNullOrWhiteSpace(Convert.ToString(row["snapshottemplatebadge"])) ? "''" : row["snapshottemplatebadge"].ToString()) + " ");

            //        //sb.AppendLine("'ordinalposition', " + row["ordinalposition"].ToString() + "");


            //        if (iCount == dtAttributes.Rows.Count - 1)
            //        {
            //            sb.AppendLine(") as col_" + iCount.ToString());
            //        }
            //        else
            //        {
            //            sb.AppendLine(") as col_" + iCount.ToString() + ",");
            //        }

            //        //TextBox1.Text = row["ImagePath"].ToString();
            //        iCount++;
            //    }

            //    sb.AppendLine(" FROM (SELECT *, " + matchedcontextfield + " AS matchedcontext, " + rowcssfield + " AS rowcssfield, " +
            //                  (string.IsNullOrWhiteSpace(snapshotTemplateLine1) ? "''" : snapshotTemplateLine1) + " AS snapshottemplateline1, " +
            //                  (string.IsNullOrWhiteSpace(snapshotTemplateLine2) ? "''" : snapshotTemplateLine2) + " AS snapshottemplateline2, " +
            //                  (string.IsNullOrWhiteSpace(snapshotTemplateBadge) ? "''" : snapshotTemplateBadge) + " AS snapshottemplatebadge");

            //    //append persona context filters
            //    foreach (DataRow row in dsListPersonaContexts.Tables[0].Rows)
            //    {
            //        sb.AppendLine(", " + row["field"] + " AS " + "\"" + row["persona_id"] + "\"");
            //    }

            //    sb.AppendLine("FROM baseview." + baseViewNameSpace + "_" + baseViewName + " " + ordergroupbystatement + ") AS View " + filtersToApplySB.ToString() + ";");
            //    string listSQL = sb.ToString();

            //    var paramList = new List<KeyValuePair<string, object>>()
            //    {
            //    };

            //    DataSet dsList = new DataSet();
            //    try
            //    {
            //        dsList = DataServices.DataSetFromSQL(listSQL, paramListFromPost);
            //        DataTable dtList = dsList.Tables[0];
            //        return DataServices.ConvertDataTabletoJSONString(dtList);
            //    }
            //    catch (Exception ex)
            //    {
            //        this.HttpContext.Response.StatusCode = 400;
            //        var httpErr = new SynapseHTTPError
            //        {
            //            ErrorCode = "HTTP.400",
            //            ErrorType = "Client Error",
            //            ErrorDescription = "Invalid Parameters supplied"
            //        };

            //        return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //    }

            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Invalid Parameters supplied"
            //    };

            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}

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
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONObject(dt);
            return json;
            //try
            //{
            //    ds = DataServices.DataSetFromSQL(sql, paramList);
            //    DataTable dt = ds.Tables[0];
            //    var json = DataServices.ConvertDataTabletoJSONObject(dt);
            //    return json;
            //}
            //catch (Exception ex)
            //{
            //    this.HttpContext.Response.StatusCode = 400;
            //    var httpErr = new SynapseHTTPError
            //    {
            //        ErrorCode = "HTTP.400",
            //        ErrorType = "Client Error",
            //        ErrorDescription = "Invalid Parameters supplied"
            //    };

            //    return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}


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
                                   SELECT * FROM entitystorematerialised.core_listquestionvalue
                                   WHERE contextvalue = @contextvalue
                            ) lqv
                            --ON lm.list_id = lqv.list_id
                            --AND lq.listquestion_id = lqv.listquestion_id
                            --AND 
                            ON q.question_id = lqv.question_id
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
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONString(dt);
            return json;
             



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
             


            if (string.IsNullOrWhiteSpace(baseViewID))
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Unable to retrieve baseview", "Client Error");
               
            }


            //Get BaseView Details
            string sqlBV = "SELECT * FROM listsettings.baseviewmanager WHERE baseview_id = @baseview_id;";
            var paramListBV = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("baseview_id", baseViewID)
            };
            string baseViewName = "";
            string baseViewNameSpace = "";
            DataSet dsBV = new DataSet();
            dsBV = DataServices.DataSetFromSQL(sqlBV, paramListBV);
            DataTable dtBV = dsBV.Tables[0];
            try
            {
                baseViewName = dtBV.Rows[0]["baseviewname"].ToString();
                baseViewNameSpace = dtBV.Rows[0]["baseviewnamespace"].ToString();
            }
            catch { }
            


            if (string.IsNullOrWhiteSpace(baseViewName))
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Unable to retrieve baseview name", "Client Error");
                 
            }



            //Get List and Attribute Details
            string sqlAttributes = "SELECT " + patientbannerfield + " as patientbanner FROM baseview." + baseViewNameSpace + "_" + baseViewName + " WHERE " + matchedcontextfield + " = @contextvalue LIMIT 1;";
            var paramListAttributes = new List<KeyValuePair<string, object>>() {
                //new KeyValuePair<string, object>("patientbannerfield", patientbannerfield),
                //new KeyValuePair<string, object>("matchedcontextfield", matchedcontextfield),
                new KeyValuePair<string, object>("contextvalue", contextvalue),
            };

            DataSet dsAttributes = new DataSet();
            dsAttributes = DataServices.DataSetFromSQL(sqlAttributes, paramListAttributes);
            DataTable dtAttributes = dsAttributes.Tables[0];
            var json = DataServices.ConvertDataTabletoJSONObject(dtAttributes);
            return json;
            
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
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            //var json = DataServices.ConvertDataTabletoJSONString(dt);
            //return json;
            return dt.Rows[0][0].ToString();
             

        }

        [HttpGet]
        [Route("[action]/{listId?}")]
        public string GetListSnapshot(string listId)
        {
            //Get List Details

            string sqlListDetails = "SELECT * FROM listsettings.listmanager WHERE list_id = @list_id;";
            var paramListDetails = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("list_id", listId)
            };

            string baseViewID = "";
            DataSet dsListDetails = new DataSet();

            string defaultContext = string.Empty;
            string defaultContextField = string.Empty;

            string matchedcontextfield = "";
            string snapshotTemplateLine1 = "";
            string snapshotTemplateLine2 = "";
            string snapshotTemplateBadge = "";
            dsListDetails = DataServices.DataSetFromSQL(sqlListDetails, paramListDetails);
            DataTable dtListDetails = dsListDetails.Tables[0];
            try
            {
                baseViewID = dtListDetails.Rows[0]["baseview_id"].ToString();
            }
            catch { }

            try
            {
                defaultContext = dtListDetails.Rows[0]["defaultcontext"].ToString();
            }
            catch { }

            try
            {
                defaultContextField = dtListDetails.Rows[0]["defaultcontextfield"].ToString();
            }
            catch { }

            try
            {
                matchedcontextfield = dtListDetails.Rows[0]["matchedcontextfield"].ToString();
            }
            catch { }

            snapshotTemplateLine1 = Convert.ToString(dtListDetails.Rows[0]["snapshotTemplateLine1"]);
            snapshotTemplateLine2 = Convert.ToString(dtListDetails.Rows[0]["snapshotTemplateLine2"]);
            snapshotTemplateBadge = Convert.ToString(dtListDetails.Rows[0]["snapshotTemplateBadge"]);
             

            if (string.IsNullOrWhiteSpace(baseViewID))
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Unable to retrieve baseview", "Client Error");
                
            }

            //Get BaseView Details
            string sqlBV = "SELECT * FROM listsettings.baseviewmanager WHERE baseview_id = @baseview_id;";
            var paramListBV = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("baseview_id", baseViewID)
            };
            string baseViewName = "";
            string baseViewNameSpace = "";
            DataSet dsBV = new DataSet();
            dsBV = DataServices.DataSetFromSQL(sqlBV, paramListBV);
            DataTable dtBV = dsBV.Tables[0];
            try
            {
                baseViewName = dtBV.Rows[0]["baseviewname"].ToString();
                baseViewNameSpace = dtBV.Rows[0]["baseviewnamespace"].ToString();
            }
            catch { }
             
            if (string.IsNullOrWhiteSpace(baseViewName))
            {
                throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Unable to retrieve baseview name", "Client Error");
                
            }

            DataSet dsAttributes = new DataSet();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT");
            //sb.AppendLine("encounter_id,");
            int iCount = 0;

            string colSnapshotTemplateLine1 = string.IsNullOrWhiteSpace(snapshotTemplateLine1) ? "''" : snapshotTemplateLine1;
            string colSnapshotTemplateLine2 = string.IsNullOrWhiteSpace(snapshotTemplateLine2) ? "''" : snapshotTemplateLine2;
            string colSnapshotTemplateBadge = string.IsNullOrWhiteSpace(snapshotTemplateBadge) ? "''" : snapshotTemplateBadge;

            sb.AppendLine("json_build_object (");
            sb.AppendLine("'defaultcontext', '" + defaultContext + "',");
            sb.AppendLine("'defaultcontextfield', '" + defaultContextField + "',");
            sb.AppendLine("'matchedcontext', " + matchedcontextfield + ",");
            sb.AppendLine("'snapshottemplateline1', " + colSnapshotTemplateLine1 + ",");
            sb.AppendLine("'snapshottemplateline2', " + colSnapshotTemplateLine2 + ",");
            sb.AppendLine("'snapshottemplatebadge', " + colSnapshotTemplateBadge);

            sb.AppendLine(") as snapshot ");

            sb.AppendLine("FROM (SELECT *, " +
                matchedcontextfield + " AS matchedcontext, " +
                colSnapshotTemplateLine1 + " AS snapshottemplateline1, " +
                colSnapshotTemplateLine2 + " AS snapshottemplateline2, " +
                colSnapshotTemplateBadge + " AS snapshottemplatebadge " +
                "FROM baseview." + baseViewNameSpace + "_" + baseViewName + ") bv;");

            string listSQL = sb.ToString();

            var paramList = new List<KeyValuePair<string, object>>()
            {
                //new KeyValuePair<string, object>("matchedcontextfield", matchedcontextfield),
            };

            DataSet dsList = new DataSet();
            dsList = DataServices.DataSetFromSQL(listSQL, paramList);
            DataTable dtList = dsList.Tables[0];
            return DataServices.ConvertDataTabletoJSONString(dtList);
            
        }

        [HttpGet]
        [Route("[action]/{defaultcontext?}/{defaultcontextfield?}/{value?}")]
        public string GetContext(string defaultcontext, string defaultcontextfield, string value)
        {
            string entityDetailsSQL = "SELECT synapsenamespacename, entityname FROM entitysettings.entitymanager WHERE entityid = @entityid";

            var parameters = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("entityid", defaultcontext)
            };

            DataSet dsEntityDetails = new DataSet();
            
            dsEntityDetails = DataServices.DataSetFromSQL(entityDetailsSQL, parameters);
            DataTable dtEntityDetail = dsEntityDetails.Tables[0];

            string synapseNamespaceName = Convert.ToString(dtEntityDetail.Rows[0]["synapsenamespacename"]);
            string entityName = Convert.ToString(dtEntityDetail.Rows[0]["entityname"]);


            string cteSQL = "with recursive cte_relation as" +
                            " ( " +
                            " select synapsenamespacename, entityname, parentsynapsenamespacename, parententityname, parentattributename" +
                            " from entitysettings.entityrelation " +
                            " where entityname = @entityname" +
                            " and synapsenamespacename = @synapseNamespaceName" +
                            " and parentsynapsenamespacename not in ('meta')" +
                            " union" +
                            " select er.synapsenamespacename, er.entityname, er.parentsynapsenamespacename, er.parententityname, er.parentattributename" +
                            " from entitysettings.entityrelation er" +
                            " inner join cte_relation cte on er.synapsenamespacename = cte.parentsynapsenamespacename and er.entityname = cte.parententityname" +
                            " where er.parententityname in ('person', 'encounter')" +
                            " and er.entityname not in ('person', 'encounter')" +
                            " )" +
                            " select* from cte_relation";

            var paras = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("entityname", entityName),
                new KeyValuePair<string, object>("synapseNamespaceName", synapseNamespaceName)
            };

            DataSet dsEntityRelation = new DataSet();

            dsEntityRelation = DataServices.DataSetFromSQL(cteSQL, paras);
            DataTable dtEntityRelation = dsEntityRelation.Tables[0];

            int aliasA = 0, aliasB = 0;

            Dictionary<string, string> entities = new Dictionary<string, string>();

            foreach (DataRow row in dtEntityRelation.Rows)
            {
                string entityKey = Convert.ToString(row["synapsenamespacename"]) + "_" + Convert.ToString(row["entityname"]);
                string entityValue = "a" + Convert.ToString(aliasA);

                if (!entities.ContainsKey(entityKey))
                {
                    entities.Add(entityKey, entityValue);
                }

                aliasA++;

                string parentEntityKey = Convert.ToString(row["parentsynapsenamespacename"]) + "_" + Convert.ToString(row["parententityname"]);
                string parentEntityValue = "b" + Convert.ToString(aliasB);

                if (!entities.ContainsKey(parentEntityKey))
                {
                    entities.Add(parentEntityKey, parentEntityValue);
                }

                aliasB++;
            }

            if (entities.Count == 0) return "[]";

            StringBuilder contextQuery = new StringBuilder();

            string initSelect = "select " + entities.First().Value + "." +  defaultcontextfield + ", ";

            foreach (DataRow row in dtEntityRelation.Rows)
            {
                string parentEntity = Convert.ToString(row["parentsynapsenamespacename"]) + "_" + Convert.ToString(row["parententityname"]);

                if (!contextQuery.ToString().Contains($"{entities.GetValueOrDefault(parentEntity)}.{Convert.ToString(row["parentattributename"])}"))
                {
                    contextQuery.Append(entities.GetValueOrDefault(parentEntity) + "." + Convert.ToString(row["parentattributename"]) + ", ");
                }               
                
            }

            string selectParameters = contextQuery.ToString().TrimEnd(", ");

            contextQuery.Clear();

            contextQuery.Append(selectParameters);

            contextQuery.Append(" from ");


            foreach (DataRow row in dtEntityRelation.Rows)
            {
                string entity = Convert.ToString(row["synapsenamespacename"]) + "_" + Convert.ToString(row["entityname"]);

                if(!contextQuery.ToString().Contains(entity))
                {
                    contextQuery.Append(" entitystorematerialised." + entity + " as " + entities.GetValueOrDefault(entity));
                }


                string parentEntity = Convert.ToString(row["parentsynapsenamespacename"]) + "_" + Convert.ToString(row["parententityname"]);

                if (!contextQuery.ToString().Contains($"left join entitystorematerialised.{parentEntity} as {entities.GetValueOrDefault(parentEntity)}"))
                {
                    contextQuery.Append(" left join entitystorematerialised." + parentEntity + " as " + entities.GetValueOrDefault(parentEntity));

                    contextQuery.Append(" on " + entities.GetValueOrDefault(entity) + "." + Convert.ToString(row["parentattributename"]) + " = " + entities.GetValueOrDefault(parentEntity) + "." + Convert.ToString(row["parentattributename"]));                
                }
            }

            contextQuery.Append(" where " + entities.First().Value + "." + defaultcontextfield + " = @defaultcontextfield");

            string finalContextQuery = initSelect + contextQuery.ToString();

            var pars = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("defaultcontextfield", value)
            };

            DataSet dsContext = new DataSet();
            dsContext = DataServices.DataSetFromSQL(finalContextQuery, pars);
            DataTable dtContext = dsContext.Tables[0];
            return DataServices.ConvertDataTabletoJSONString(dtContext);

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

   
}