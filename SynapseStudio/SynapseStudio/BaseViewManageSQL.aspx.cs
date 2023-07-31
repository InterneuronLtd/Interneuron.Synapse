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
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class BaseViewManageSQL : System.Web.UI.Page
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

                this.hdnBaseViewID.Value = id;

                try
                {

                }
                catch { }

                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }

                DataTable dt = SynapseHelpers.GetBaseviewDTByID(id);

                try
                {
                    this.txtSQL.Text = dt.Rows[0]["baseviewsqlstatement"].ToString();
                }
                catch { }

                this.lblSummaryType.Text = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
                this.hdnNamespaceID.Value = SynapseHelpers.GetBaseviewNameSpaceIDFromBaseViewID(id);
                this.hdnNextOrdinalPosition.Value = SynapseHelpers.GetNextOrdinalPositionFromID(id);
                this.lblBaseViewName.Text = SynapseHelpers.GetBaseviewNameFromID(id);
                this.lblNamespaceName.Text = SynapseHelpers.GetBaseviewNameSpaceNameFromBaseViewID(id);
                this.lblBaseViewComments.Text = SynapseHelpers.GetBaseviewCommentsFromBaseViewID(id);


                this.lblError.Text = string.Empty;
                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;
                this.btnCreateNewEntity.Visible = false;
                this.btnCancel.Visible = false;

                this.btnValidateEntity.Attributes.Add("onclick", "if(confirm('This will start the process of dropping and recreating the BaseView. Are you sure that you want to continue?')){return true;} else {return false;};");
                this.btnCreateNewEntity.Attributes.Add("onclick", "if(confirm('This will drop and recreate the BaseView. There is currently no validation to ensure that all previous attributes have been maintained in the new statement.  Are you sure that you want to continue?')){return true;} else {return false;};");
            }
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

        //Drop Down Lists
        private void BindDropDownList(DropDownList ddl, string sql, string valueField, string displayField, int addPleaseSelect)
        {
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ddl.DataSource = ds;
            ddl.DataValueField = valueField;
            ddl.DataTextField = displayField;
            ddl.DataBind();

            if (addPleaseSelect == 1)
            {
                ListItem[] items = new ListItem[1];
                items[0] = new ListItem("Please select ...", "0");
                ddl.Items.Insert(0, items[0]);
            }
        }

        private void SetDDLSource(DropDownList ddl, string val)
        {
            if (val.Length > 0)
            {
                int idx = 9999;

                try
                {
                    idx = ddl.Items.IndexOf(ddl.Items.FindByValue(val));
                }
                catch
                {
                    idx = 9999;
                }

                if (idx == 9999 || idx < 0)
                {
                    ListItem[] items = new ListItem[1];
                    items[0] = new ListItem(val + " (old value)", val);
                    ddl.Items.Insert(1, items[0]);
                }
            }

            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(val));
        }

        //RadioButtonList
        private void BindRadioList(RadioButtonList rad, string sql, string valueField, string displayField)
        {
            DataSet ds = DataServices.DataSetFromSQL(sql);
            rad.DataSource = ds;
            rad.DataValueField = valueField;
            rad.DataTextField = displayField;
            rad.DataBind();

        }

        private void SetRadioSource(RadioButtonList rad, string val)
        {
            rad.SelectedIndex = rad.Items.IndexOf(rad.Items.FindByValue(val));
        }

       
        protected void btnManageAttributes_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseViewManagerAttributes.aspx?action=view&id=" + this.hdnBaseViewID.Value);
        }

        protected void btnManageAPI_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseViewManagerAPIs.aspx?action=view&id=" + this.hdnBaseViewID.Value);
        }

        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseViewManagerView.aspx?action=view&id=" + this.hdnBaseViewID.Value);
        }

        protected void btnSQL_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseViewManageSQL.aspx?action=view&id=" + this.hdnBaseViewID.Value);
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //this.txtEntityName.Text = string.Empty;
            //this.txtEntityName.Enabled = true;
            //this.txtEntityName.Focus();
            this.btnValidateEntity.Visible = true;
            this.btnCreateNewEntity.Visible = false;
            this.btnCancel.Visible = false;
            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            //this.txtSQL.Text = string.Empty;
            this.txtSQL.Enabled = true;

            string noerr = "form-group";
            //this.fgEntityName.CssClass = noerr;

        }

        protected void btnValidateEntity_Click(object sender, EventArgs e)
        {

            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;            
            this.fgSQL.CssClass = noerr;

           

            ////Check if name already exists
            //string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @baseviewnamespaceid and baseviewname = @baseviewname;";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("baseviewnamespaceid", this.hdnNamespaceID.Value),
            //    new KeyValuePair<string, string>("baseviewname", this.lblBaseViewName.Text)
            //};

            //DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            //DataTable dt = ds.Tables[0];

            //if (dt.Rows.Count > 0)
            //{
            //    this.lblError.Text = "The name of the baseview that you you have entered already exists";
            //    this.txtSQL.Focus();
            //    this.lblError.Visible = true;
            //    this.fgSQL.CssClass = haserr;
            //    return;
            //}

            string tmpViewName = Guid.NewGuid().ToString("N");
            //Check if we can create a temporary view using the supplied SQL Statement
            string sqlCreate = "CREATE VIEW baseviewtemp." + this.lblNamespaceName.Text + "_" + tmpViewName + " AS " + this.txtSQL.Text + ";";
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
            this.lblBaseViewName.Enabled = false;
            this.txtSQL.Enabled = false;



        }

        protected void btnCreateNewEntity_Click(object sender, EventArgs e)
        {

            //Drop the baseeview
            string sqlDrop = @"SELECT listsettings.dropbaseview(
	                    @p_baseview_id, 
	                    @p_baseviewname
                    )";



            var paramListDrop = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_baseview_id", this.hdnBaseViewID.Value),
                new KeyValuePair<string, string>("p_baseviewname", this.lblSummaryType.Text)
            };

            DataServices.ExcecuteNonQueryFromSQL(sqlDrop, paramListDrop);


            //Recreate the baseview
            //string tmpViewName = Guid.NewGuid().ToString("N");
            //Check if we can create a temporary view using the supplied SQL Statement
            string sqlCreate = "CREATE VIEW baseviewcore." + this.lblNamespaceName.Text + "_" + this.lblBaseViewName.Text + " AS " + this.txtSQL.Text + ";";
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

            string baseviewid = this.hdnBaseViewID.Value;

            string sql = "SELECT listsettings.recreatebaseview(@p_baseviewnamespaceid, @p_baseviewnamespace, @p_baseviewname, @p_baseviewdescription, @p_baseviewsqlstatement, @p_username, @baseviewid);";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_baseviewnamespaceid", this.hdnNamespaceID.Value),
                new KeyValuePair<string, string>("p_baseviewnamespace", this.lblNamespaceName.Text),
                new KeyValuePair<string, string>("p_baseviewname", this.lblBaseViewName.Text),
                new KeyValuePair<string, string>("p_baseviewdescription", this.lblBaseViewComments.Text),
                new KeyValuePair<string, string>("p_baseviewsqlstatement", this.txtSQL.Text),
                new KeyValuePair<string, string>("p_username", this.hdnUserName.Value),
                new KeyValuePair<string, string>("baseviewid", this.hdnBaseViewID.Value),

            };

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                this.lblError.Text = "Error creating view: " + ex.ToString();
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

            Response.Redirect("BaseViewManageSQL.aspx?id=" + newGuid);


        }

    }

}