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
    public partial class LocalNamespaceManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }

               

                this.lblError.Text = string.Empty;
                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;
                this.btnCreateNewNamespace.Visible = false;
                this.btnCancel.Visible = false;

                BindGrid();



            }
        }

        private void BindGrid()
        {
            string sql = "SELECT * FROM entitysettings.localnamespace ORDER BY localnamespacename; ";
            var paramList = new List<KeyValuePair<string, string>>() {               
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            this.dgEntities.DataSource = dt;
            this.dgEntities.DataBind();
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

            this.txtLocalNamespaceName.Text = RemoveAndReplaceSpecialCharacters(this.txtLocalNamespaceName.Text);
            this.txtLocalNamespaceName.Enabled = true;

            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;


            if (string.IsNullOrEmpty(this.txtLocalNamespaceName.Text.ToString()))
            {
                this.lblError.Text = "Please enter a new name for the entity";
                this.txtLocalNamespaceName.Focus();
                this.lblError.Visible = true;
                this.fgLocalNamespaceName.CssClass = haserr;
                return;
            }


            string sql = "SELECT * FROM entitysettings.localnamespace WHERE localnamespacename = @localnamespacename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("localnamespacename", this.txtLocalNamespaceName.Text)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                this.lblError.Text = "There is already a local namespace with that name ";
                this.txtLocalNamespaceName.Focus();
                this.lblError.Visible = true;
                this.fgLocalNamespaceName.CssClass = haserr;
                return;
            }


            this.lblSuccess.Visible = true;
            this.lblSuccess.Text = "Validation succeeded";
            this.btnCreateNewNamespace.Visible = true;
            this.btnCancel.Visible = true;
            this.btnCreateNewNamespace.Focus();
            this.btnValidateEntity.Visible = false;
            this.txtLocalNamespaceName.Enabled = false;


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
            ResetForm();
        }

        private void ResetForm()
        {
            this.btnValidateEntity.Visible = true;
            this.btnCreateNewNamespace.Visible = false;
            this.btnCancel.Visible = false;
            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.txtLocalNamespaceName.Text = string.Empty;
            this.txtLocalNamespaceName.Enabled = true;
            this.txtLocalNamespaceName.Focus();

            string noerr = "form-group";
            this.fgLocalNamespaceName.CssClass = noerr;
            this.txtLocalNamespaceDescription.Text = string.Empty;  
        }


        protected void btnCreateNewNamespace_Click(object sender, EventArgs e)
        {
            string sql = "INSERT INTO entitysettings.localnamespace(_createdsource, localnamespacename, localnamespacedescription, _createdby) VALUES ('Synapse Studio', @localnamespacename, @localnamespacedescription, @p_username);";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("localnamespacename", this.txtLocalNamespaceName.Text),
                new KeyValuePair<string, string>("localnamespacedescription", this.txtLocalNamespaceDescription.Text),
                new KeyValuePair<string, string>("p_username", this.hdnUserName.Value)

            };

            DataServices.executeSQLStatement(sql, paramList);

            BindGrid();
            ResetForm();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerNewLocal.aspx?id=468ac87d-f6a3-4d20-8800-58742b8952b6");
        }
    }
}