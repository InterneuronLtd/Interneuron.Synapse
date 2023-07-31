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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class BaseviewManagerNew : System.Web.UI.Page
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

                this.hdnNamespaceID.Value = id;

                string local = "";
                try
                {
                    local = Request.QueryString["local"].ToString();
                }
                catch
                {
                }

                this.hdnLocalNamespaceID.Value = local;



                string name = "";
                try
                {
                    name = SynapseHelpers.GetBaseviewNamespaceNameFromID(id);
                }
                catch { }

                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }

                if (String.IsNullOrEmpty(name))
                {
                    Response.Redirect("Error.aspx");
                }

                this.lblNamespaceName.Text = name;

                this.lblError.Text = string.Empty;
                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;
                this.btnCreateNewEntity.Visible = false;
                this.btnCancel.Visible = false;
            }
        }

        protected void btnValidateEntity_Click(object sender, EventArgs e)
        {

            this.txtEntityName.Text = RemoveAndReplaceSpecialCharacters(this.txtEntityName.Text);
            this.txtEntityName.Enabled = true;

            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;
            this.fgEntityName.CssClass = noerr;
            this.fgSQL.CssClass = noerr;

            if (string.IsNullOrEmpty(this.txtEntityName.Text.ToString()))
            {
                this.lblError.Text = "Please enter a new name for the baseview";
                this.txtEntityName.Focus();
                this.lblError.Visible = true;
                this.fgEntityName.CssClass = haserr;
                return;
            }

            if (string.IsNullOrEmpty(this.txtSQL.Text.ToString()))
            {
                this.lblError.Text = "Please enter a the SQL Statement for the new baseview";
                this.txtEntityName.Focus();
                this.lblError.Visible = true;
                this.fgEntityName.CssClass = haserr;
                return;
            }

            //Check if name already exists
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @baseviewnamespaceid and baseviewname = @baseviewname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseviewnamespaceid", this.hdnNamespaceID.Value),
                new KeyValuePair<string, string>("baseviewname", this.txtEntityName.Text)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                this.lblError.Text = "The name of the baseview that you you have entered already exists";
                this.txtSQL.Focus();
                this.lblError.Visible = true;
                this.fgSQL.CssClass = haserr;
                return;
            }

            string tmpViewName = Guid.NewGuid().ToString("N");
            //Check if we can create a temporary view using the supplied SQL Statement
            string sqlCreate = "CREATE VIEW baseviewtemp." + this.lblNamespaceName.Text + "_" + tmpViewName + " AS " + this.txtSQL.Text +  ";";
            var paramListCreate = new List<KeyValuePair<string, string>>() {                
            };

            try
            {
                DataServices.executeSQLStatement(sqlCreate, paramListCreate);
            }
            catch(Exception ex) {
                this.lblError.Text = "Error creating view: " + ex.ToString();
                this.txtSQL.Focus();
                this.lblError.Visible = true;
                this.fgSQL.CssClass = haserr;
                return;
            }

            this.lblSuccess.Visible = true;
            this.lblSuccess.Text = "Validation succeeded";
            this.btnCreateNewEntity.Visible = true;
            this.btnCancel.Visible = true;
            this.btnCreateNewEntity.Focus();
            this.btnValidateEntity.Visible = false;
            this.txtEntityName.Enabled = false;
            this.txtSQL.Enabled = false;



        }

        public static string RemoveBadChars(string word)
        {
            char[] chars = new char[word.Length];
            int myindex = 0;
            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];

                if ((int)c >= 65 && (int)c <= 90)
                {
                    chars[myindex] = c;
                    myindex++;
                }
                else if ((int)c >= 97 && (int)c <= 122)
                {
                    chars[myindex] = c;
                    myindex++;
                }
                else if ((int)c == 44)
                {
                    chars[myindex] = c;
                    myindex++;
                }
            }

            word = new string(chars);

            return word;
        }

        string RemoveAndReplaceSpecialCharacters(string str)
        {
            str = str.Replace(" ", String.Empty);

            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            str = rgx.Replace(str, "");

            return str.ToLower();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtEntityName.Text = string.Empty;
            this.txtEntityName.Enabled = true;
            this.txtEntityName.Focus();
            this.btnValidateEntity.Visible = true;
            this.btnCreateNewEntity.Visible = false;
            this.btnCancel.Visible = false;
            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.txtSQL.Text = string.Empty;
            this.txtSQL.Enabled = true;

            string noerr = "form-group";
            this.fgEntityName.CssClass = noerr;

        }

        protected void btnCreateNewEntity_Click(object sender, EventArgs e)
        {

            string tmpViewName = Guid.NewGuid().ToString("N");
            //Check if we can create a temporary view using the supplied SQL Statement
            string sqlCreate = "CREATE VIEW baseviewcore." + this.lblNamespaceName.Text + "_" + this.txtEntityName.Text + " AS " + this.txtSQL.Text + ";";
            var paramListCreate = new List<KeyValuePair<string, string>>()
            {
            };

            try
            {
                DataServices.executeSQLStatement(sqlCreate, paramListCreate);
            }
            catch (Exception ex)
            {
                this.lblError.Text = "Error creating view: " + ex.ToString();                
                this.lblError.Visible = true;
                return;
            }

            string sql = "SELECT listsettings.createbaseview(@p_baseviewnamespaceid, @p_baseviewnamespace, @p_baseviewname, @p_baseviewdescription, @p_baseviewsqlstatement, @p_username);";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_baseviewnamespaceid", this.hdnNamespaceID.Value),
                new KeyValuePair<string, string>("p_baseviewnamespace", SynapseHelpers.GetBaseviewNamespaceNameFromID(this.hdnNamespaceID.Value)),
                new KeyValuePair<string, string>("p_baseviewname", this.txtEntityName.Text),
                new KeyValuePair<string, string>("p_baseviewdescription", this.txtEntityComments.Text),
                new KeyValuePair<string, string>("p_baseviewsqlstatement", this.txtSQL.Text),
                new KeyValuePair<string, string>("p_username", this.hdnUserName.Value)

            };


            DataSet ds = new DataSet();

            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                this.lblError.Text = "Error creating view: " + System.Environment.NewLine + ex.ToString();
                this.lblError.Visible = true;
                return;
            }

            


            DataTable dt = ds.Tables[0];

            string newGuid = "";

            try
            {
                newGuid = dt.Rows[0][0].ToString();
            }
            catch { }

            Response.Redirect("BaseviewManagerView.aspx?id=" + newGuid);


        }

    }
}