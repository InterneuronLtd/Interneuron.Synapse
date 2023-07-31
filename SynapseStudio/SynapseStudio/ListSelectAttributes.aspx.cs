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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class ListSelectAttributes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = "";
                try
                {
                    id = Request.QueryString["id"].ToString();
                }
                catch
                {
                    Response.Redirect("Error.aspx");
                }


                if (String.IsNullOrEmpty(id))
                {
                    Response.Redirect("Error.aspx");
                }

                this.hdnListID.Value = id;

                try
                {

                }
                catch { }

                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }


                //this.lblSummaryType.Text = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
                this.lblSummaryType.Text = SynapseHelpers.GetListNameFromID(id);



                string uri = SynapseHelpers.GetEBoardURL() + "ListPreview.aspx?id=" + this.hdnListID.Value;

                this.hlPreview.NavigateUrl = uri;
                this.hlPreview.Target = "_blank";
            }
        }



        protected void btnManageAttributes_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListManagerAttributes.aspx?action=view&id=" + this.hdnListID.Value);
        }

        protected void btnSelectAttributes_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListSelectAttributes.aspx?action=view&id=" + this.hdnListID.Value);
        }

        protected void btnManageAPI_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListAPIs.aspx?action=view&id=" + this.hdnListID.Value);
        }

        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListManagerView.aspx?action=view&id=" + this.hdnListID.Value);
        }

        public static string ConvertDataTabletoJSONString(DataTable dt)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            var ret = "";
            try
            {
                ret = JsonConvert.SerializeObject(rows);
            }
            catch (Exception er)
            {
                throw new Exception("Error returning data table: " + er.ToString());
            }
            return ret;
        }

        [WebMethod()]
        public static List<availableAttribute> GetAvailableAttributes(string listId)
        {

            string sql = @"SELECT
                            bva.baseviewattribute_id, baseviewnamespaceid, baseviewnamespace, baseview_id, baseviewname, bva.attributename, datatype, ordinalposition, case when sa.baseviewattribute_id is null then false else true end as isselected
                            FROM listsettings.baseviewattribute bva
                            LEFT OUTER JOIN 
                            (SELECT baseviewattribute_id, attributename FROM listsettings.listattribute WHERE list_id = @list_id AND COALESCE(isselected,false) = true) sa
                            ON bva.attributename = sa.attributename                            
                            WHERE baseview_ID IN (SELECT baseview_id FROM listsettings.listmanager WHERE list_id = @list_id) ORDER BY bva.attributename";


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId)
            };

            DataSet ds = new DataSet();

            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return null;
            }

            DataTable dt = ds.Tables[0];
            List<availableAttribute> lst = ds.Tables[0].ToList<availableAttribute>();

            return lst;

        }

        [WebMethod()]
        public static List<selectedAttribute> GetSelectedAttributes(string listId)
        {

            string sql = @"SELECT
                            la.listattribute_id, la.list_id, la.baseviewattribute_id, la.attributename, la.datatype, la.displayname, la.ordinalposition, la.defaultcssclassname
                            FROM listsettings.listattribute la
                            INNER JOIN listsettings.baseviewattribute bva
                            ON la.attributename = bva.attributename
                            WHERE la.list_id = @list_id
                            AND COALESCE(isselected, false) = true
                            AND bva.baseview_ID IN (SELECT baseview_id FROM listsettings.listmanager WHERE list_id = @list_id)
                            ORDER BY la.ordinalposition";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId)
            };

            DataSet ds = new DataSet();

            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return null;
            }

            DataTable dt = ds.Tables[0];

            List<selectedAttribute> lst = ds.Tables[0].ToList<selectedAttribute>();
            return lst;

        }


        [WebMethod()]
        public static int GetNextOrdinalPosition(string listId)
        {

            string sql = @"SELECT
                           max(la.ordinalposition) as MaxOrdinalPosition
                            FROM listsettings.listattribute la
                            WHERE la.list_id = @list_id
                            AND COALESCE(isselected, false) = true";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId)
            };

            DataSet ds = new DataSet();

            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return 1;
            }

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                return 1;
            }

            int ret = 0;
            try
            {
                ret = System.Convert.ToInt16(dt.Rows[0]["MaxOrdinalPosition"].ToString());
            }
            catch { }

            ret += 1;

            return ret;

        }

        [WebMethod()]
        public static int SaveQuickAttributes(string listId, string listattribute_id, string displayname, string defaultcssclassname)
        {

            string sql = @"UPDATE listsettings.listattribute
                            SET displayname = @displayname,
                            defaultcssclassname = @defaultcssclassname
                            WHERE listattribute_id = @listattribute_id AND list_id = @list_id
                            ";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listattribute_id", listattribute_id),
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("displayname", displayname),
                new KeyValuePair<string, string>("defaultcssclassname", defaultcssclassname),
            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return 0;
            }


            return 1;

        }

        [WebMethod()]
        public static String AddAttributeToList(string listId, string attributename, int ordinalposition)
        {


            string sqlCheck = "SELECT COUNT(*) AS recCount FROM listsettings.listattribute WHERE list_id= @list_id AND attributename = @attributename;";
            var paramListCheck = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("attributename", attributename),
            };
            DataSet dsCheck = DataServices.DataSetFromSQL(sqlCheck, paramListCheck);
            DataTable dtCheck = new DataTable();
            dtCheck = dsCheck.Tables[0];
            int recs = 0;
            try
            {
                recs = System.Convert.ToInt16(dtCheck.Rows[0]["recCount"].ToString());
            }
            catch { }

            string sql = "";

            if (recs == 0) //Do insert
            {
                sql = @"INSERT INTO listsettings.listattribute(listattribute_id, list_id, baseviewattribute_id, attributename, datatype, ordinalposition)
                        SELECT uuid_generate_v4() AS listattribute_id, @list_id, baseviewattribute_id, @attributename, datatype, CAST(@ordinalposition AS INT) AS ordinalposition
                        FROM listsettings.baseviewattribute WHERE attributename = @attributename
                        AND baseview_id IN (SELECT baseview_id FROM listsettings.listmanager WHERE list_id = @list_id);";
            }
            else //Do update
            {
                sql = @"UPDATE listsettings.listattribute
                        SET isselected = true,
                            ordinalposition = CAST(@ordinalposition AS INT)
                        WHERE list_id = @list_id
                        AND attributename = @attributename;";
            }

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("attributename", attributename),
                new KeyValuePair<string, string>("ordinalposition", ordinalposition.ToString()),
            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = ex.ToString();

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            return "Attribute added";

        }

        [WebMethod()]
        public static String RemoveAttributeFromList(string listId, string attributename)
        {


            string sqlCheck = "SELECT ordinalposition FROM listsettings.listattribute WHERE list_id= @list_id AND attributename = @attributename;";
            var paramListCheck = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("attributename", attributename),
            };
            DataSet dsCheck = DataServices.DataSetFromSQL(sqlCheck, paramListCheck);
            DataTable dtCheck = new DataTable();
            dtCheck = dsCheck.Tables[0];
            int ordinalposition = 0;
            try
            {
                ordinalposition = System.Convert.ToInt16(dtCheck.Rows[0]["ordinalposition"].ToString());
            }
            catch (Exception ex)
            {
                var a = ex;
            }

            string sql = "";



            sql = @"UPDATE listsettings.listattribute
                        SET isselected = false,
                            ordinalposition = null
                        WHERE list_id = @list_id
                        AND attributename = @attributename;


                    UPDATE listsettings.listattribute
                        SET ordinalposition = ordinalposition - 1
                        WHERE list_id = @list_id
                        AND ordinalposition > CAST(@ordinalposition AS INT);

            ";


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("attributename", attributename),
                new KeyValuePair<string, string>("ordinalposition", ordinalposition.ToString()),
            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = ex.ToString();

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            return "Attribute removed";

        }

        [WebMethod()]
        public static String UpdateOrdinalPosition(string listId, string listattribute_id, int ordinalposition)
        {


            string sql = "UPDATE listsettings.listattribute SET ordinalposition = CAST(@ordinalposition AS int) WHERE list_id= @list_id AND listattribute_id = @listattribute_id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("listattribute_id", listattribute_id),
                new KeyValuePair<string, string>("ordinalposition", ordinalposition.ToString())
            };


            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                var httpErr = new SynapseHTTPError();
                httpErr.ErrorCode = "HTTP.400";
                httpErr.ErrorType = "Client Error";
                httpErr.ErrorDescription = ex.ToString();

                return JsonConvert.SerializeObject(httpErr, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            return "Ordinal Position Updated";

        }

        protected void btnSelectQuestions_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListSelectQuestions.aspx?action=view&id=" + this.hdnListID.Value);
        }

    }

    public class availableAttribute
    {
        public string baseviewattribute_id { get; set; }
        public string baseviewnamespaceid { get; set; }
        public string baseviewnamespace { get; set; }
        public string baseview_id { get; set; }
        public string baseviewname { get; set; }
        public string attributename { get; set; }
        public string datatype { get; set; }
        public int ordinalposition { get; set; }
        public Boolean isselected { get; set; }
    }

    public class selectedAttribute
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

    public class SynapseHTTPError
    {
        public string ErrorType { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}