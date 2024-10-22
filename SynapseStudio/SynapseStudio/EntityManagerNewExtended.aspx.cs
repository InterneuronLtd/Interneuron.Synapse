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
    public partial class EntityManagerNewExtended : System.Web.UI.Page
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
                    name = SynapseHelpers.GetNamespaceNameFromID(id);
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

                string sql = "SELECT entityid, entityname FROM entitysettings.entitymanager WHERE synapsenamespaceid = 'd8851db1-68f8-45ee-be9a-628666512431' ORDER BY entityname;";
                BindDropDownList(this.ddlCoreEntity, sql, "entityid", "entityname", 1);
               
            }
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

        protected void btnValidateEntity_Click(object sender, EventArgs e)
        {



            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;
            this.fgCoreEntity.CssClass = noerr;

            if (this.ddlCoreEntity.SelectedIndex == 0)
            {
                this.lblError.Text = "Please select the entity that you wish to extend";
                this.ddlCoreEntity.Focus();
                this.lblError.Visible = true;
                this.fgCoreEntity.CssClass = haserr;
                return;
            }


            string sql = "SELECT * FROM entitysettings.entitymanager WHERE synapseNamespaceid = @synapseNamespaceid and entityname = @entityname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("synapseNamespaceid", this.hdnNamespaceID.Value),
                new KeyValuePair<string, string>("entityname", this.ddlCoreEntity.SelectedItem.Text)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                this.lblError.Text = "There is already an extended entity for the entity you have selected";
                this.ddlCoreEntity.Focus();
                this.lblError.Visible = true;
                this.fgCoreEntity.CssClass = haserr;
                return;
            }


            this.lblSuccess.Visible = true;
            this.lblSuccess.Text = "Validation succeeded";
            this.btnCreateNewEntity.Visible = true;
            this.btnCancel.Visible = true;
            this.btnCreateNewEntity.Focus();
            this.btnValidateEntity.Visible = false;
            this.ddlCoreEntity.Enabled = false;



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
            this.ddlCoreEntity.SelectedIndex = 0;
            this.ddlCoreEntity.Enabled = true;
            this.ddlCoreEntity.Focus();
            this.btnValidateEntity.Visible = true;
            this.btnCreateNewEntity.Visible = false;
            this.btnCancel.Visible = false;
            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;

            string noerr = "form-group";
            this.fgCoreEntity.CssClass = noerr;

        }

        protected void btnCreateNewEntity_Click(object sender, EventArgs e)
        {
            string sql = "SELECT entitysettings.createentityfrommodel(@p_synapsenamespaceid, @p_synapsenamespacename, @p_entityname, @p_entitydescription, @p_username, @p_localnamespaceid, @p_localnamespacename)";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_synapsenamespaceid", this.hdnNamespaceID.Value),
                new KeyValuePair<string, string>("p_synapsenamespacename", SynapseHelpers.GetNamespaceNameFromID(this.hdnNamespaceID.Value)),
                new KeyValuePair<string, string>("p_entityname", this.ddlCoreEntity.SelectedItem.Text),
                new KeyValuePair<string, string>("p_entitydescription", this.txtEntityComments.Text),
                new KeyValuePair<string, string>("p_username", this.hdnUserName.Value),
                new KeyValuePair<string, string>("p_localnamespaceid", this.hdnLocalNamespaceID.Value),
                new KeyValuePair<string, string>("p_localnamespacename", SynapseHelpers.GetNamespaceNameFromID(this.hdnLocalNamespaceID.Value)),

            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string newGuid = "";

            try
            {
                newGuid = dt.Rows[0][0].ToString();
            }
            catch { }

            Response.Redirect("EntityManagerAttributes.aspx?id=" + newGuid);


        }

    }
}