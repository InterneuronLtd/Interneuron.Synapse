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
    public partial class ListSelectQuestions : System.Web.UI.Page
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

                DataTable dtt = SynapseHelpers.GetListDT(id);
                try
                {
                    this.lblContextField.Text = dtt.Rows[0]["defaultcontextfield"].ToString();
                }
                catch { }

                try
                {
                    this.lblDefaultContext.Text = dtt.Rows[0]["defaultcontext"].ToString();
                }
                catch { }


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
        public static List<availableQuestion> GetAvailableQuestions(string listId, string defaultContext)
        {

            string sql = @"SELECT 
                            q.question_id, 
                            '<strong>' || questionquickname || '</strong><br />' || questiontypetext || '<br />' || coalesce(labeltext, 'HTML Snippet' ) as questiondisplay,
                            case when lq.question_id is null then false else true end as isselected
                            FROM listsettings.question q
                            LEFT OUTER JOIN (
                            SELECT 
	                            lq.list_id,
	                            lq.question_id
	                            FROM listsettings.listmanager lm
	                            LEFT OUTER JOIN listsettings.listquestion lq	
	                            ON lm.list_id = lq.list_id
	                            WHERE lm.list_id = @list_id
                                AND isselected = true
	                            ) lq
                            ON q.question_id = lq.question_id
                            WHERE defaultcontext = @defaultcontext
                            ORDER BY 2
                            ";


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("defaultcontext", defaultContext)
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
            List<availableQuestion> lst = ds.Tables[0].ToList<availableQuestion>();

            return lst;

        }

        [WebMethod()]
        public static List<selectedQuestion> GetSelectedQuestions(string listId)
        {

            string sql = @"SELECT 
                                lq.listquestion_id,
	                            lq.list_id,
	                            lq.question_id,
                                '<strong>' || questionquickname || '</strong><br />' || questiontypetext || '<br />' || coalesce(labeltext, 'HTML Snippet' ) as questiondisplay                                
	                            FROM listsettings.listmanager lm
	                            INNER JOIN listsettings.listquestion lq	
	                            ON lm.list_id = lq.list_id
                                LEFT OUTER JOIN listsettings.question q
                                ON lq.question_id = q.question_id
	                            WHERE lm.list_id = @list_id
                                AND COALESCE(isselected, false) = true
                                ORDER BY lq.ordinalposition";

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

            List<selectedQuestion> lst = ds.Tables[0].ToList<selectedQuestion>();
            return lst;

        }
                     
        
        [WebMethod()]
        public static String AddQuestionToList(string listId, string question_id, int ordinalposition)
        {


            string sqlCheck = "SELECT COUNT(*) AS recCount FROM listsettings.listquestion WHERE list_id= @list_id AND question_id = @question_id;";
            var paramListCheck = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("question_id", question_id),
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
                sql = @"INSERT INTO listsettings.listquestion(listquestion_id, list_id, question_id, ordinalposition)
                        SELECT uuid_generate_v4() AS listquestion_id, @list_id, question_id, CAST(@ordinalposition AS INT) AS ordinalposition
                        FROM listsettings.question WHERE question_id = @question_id;";
            }
            else //Do update
            {
                sql = @"UPDATE listsettings.listquestion
                        SET isselected = true,
                            ordinalposition = CAST(@ordinalposition AS INT)
                        WHERE list_id = @list_id
                        AND question_id = @question_id;";
            }

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("question_id", question_id),
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

            return "Question added";

        }

        [WebMethod()]
        public static String RemoveQuestionFromList(string listId, string question_id)
        {


            string sqlCheck = "SELECT ordinalposition FROM listsettings.listquestion WHERE list_id= @list_id AND question_id = @question_id;";
            var paramListCheck = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("question_id", question_id),
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



            sql = @"UPDATE listsettings.listquestion
                        SET isselected = false,
                            ordinalposition = null
                        WHERE list_id = @list_id
                        AND question_id = @question_id;


                    UPDATE listsettings.listquestion
                        SET ordinalposition = ordinalposition - 1
                        WHERE list_id = @list_id
                        AND ordinalposition > CAST(@ordinalposition AS INT);

            ";


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("question_id", question_id),
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

            return "Question removed";

        }

        [WebMethod()]
        public static int GetNextOrdinalPosition(string listId)
        {

            string sql = @"SELECT
                           max(la.ordinalposition) as MaxOrdinalPosition
                            FROM listsettings.listquestion la
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
        public static String UpdateOrdinalPosition(string listId, string listquestion_id, int ordinalposition)
        {


            string sql = "UPDATE listsettings.listquestion SET ordinalposition = CAST(@ordinalposition AS int) WHERE list_id= @list_id AND listquestion_id = @listquestion_id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("listquestion_id", listquestion_id),
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




    public class availableQuestion
    {
        public string question_id { get; set; }
        public string questiondisplay { get; set; }
        public Boolean isselected { get; set; }
    }



    public class selectedQuestion
    {
        public string listquestion_id { get; set; }
        public string list_id { get; set; }
        public string question_id { get; set; }
        public string questiondisplay { get; set; }        
    }

}