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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class RegisterAsSuperUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                Page.Form.DefaultButton = this.btnRegister.UniqueID;
                Page.Form.DefaultFocus = this.ddlMatchedOrganisation.UniqueID;

                //BindDropDownList(this.ddlMatchedOrganisation, "SELECT * FROM systemsettings.app_organisation ORDER BY organisationname", "organisationID", "organisationname", 1);

                this.lblError.Visible = false;

                Page.MaintainScrollPositionOnPostBack = true;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)

        {

            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.fgEmail.CssClass = noerr;
            this.fgPassword.CssClass = noerr;
            this.fgEmail.CssClass = noerr;
            this.fgPassword.CssClass = noerr;
            this.fgConfirmPassword.CssClass = noerr;
            this.fgMatchedOrganisation.CssClass = noerr;
            this.fgFirstName.CssClass = noerr;
            this.fgLastName.CssClass = noerr;
            this.fgGMCCode.CssClass = noerr;

            //if (this.ddlMatchedOrganisation.SelectedIndex == 0)
            //{
            //    this.lblError.Text = "Please select an organisation";
            //    this.lblError.Visible = true;
            //    this.fgMatchedOrganisation.CssClass = haserr;
            //    return;
            //}


            if (string.IsNullOrEmpty(this.txtFirstName.Text.ToString()))
            {
                this.lblError.Text = "Please enter your first name";
                this.lblError.Visible = true;
                this.fgFirstName.CssClass = haserr;
                return;
            }

            if (string.IsNullOrEmpty(this.txtLastName.Text.ToString()))
            {
                this.lblError.Text = "Please enter your last name";
                this.lblError.Visible = true;
                this.fgLastName.CssClass = haserr;
                return;
            }

            //if (string.IsNullOrEmpty(this.txtGMCCode.Text.ToString()))
            //{
            //    this.lblError.Text = "Please enter your GMC Number";
            //    this.lblError.Visible = true;
            //    this.fgGMCCode.CssClass = haserr;
            //    return;
            //}


            if (string.IsNullOrEmpty(this.txtRegistrationEmail.Text.ToString()))
            {
                this.lblError.Text = "Please enter your email address";
                this.lblError.Visible = true;
                this.fgEmail.CssClass = haserr;
                return;
            }

            if (CheckEmailAddress() == 1)
            {
                this.lblError.Text = "This email address has already been registered";
                this.lblError.Visible = true;
                this.fgEmail.CssClass = haserr;
                return;
            }

            if (string.IsNullOrEmpty(this.txtRegistrationPassword.Text.ToString()))
            {
                this.lblError.Text = "Please enter a password";
                this.lblError.Visible = true;
                this.fgPassword.CssClass = haserr;
                return;
            }

            if (string.IsNullOrEmpty(this.txtConfirmPassword.Text.ToString()))
            {
                this.lblError.Text = "Please confirm your password";
                this.lblError.Visible = true;
                this.fgConfirmPassword.CssClass = haserr;
                return;
            }

            if (this.txtRegistrationPassword.Text != this.txtConfirmPassword.Text)
            {
                this.lblError.Text = "Passwords do not match";
                this.lblError.Visible = true;
                this.fgConfirmPassword.CssClass = haserr;
                this.fgPassword.CssClass = haserr;
                return;
            }





            string sql = "INSERT INTO systemsettings.app_user(usertype, userpassword,  emailaddress, firstname, lastname, isactive, emailconfirmed, issysadmin, isauthorised)";
            sql += " VALUES (@usertype, crypt(@userpassword, gen_salt('bf', 8)), @emailaddress, @firstname, @lastname, true, true, true, true)";

            var paramListSave = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("usertype", this.ddlUserType.SelectedValue),
                    new KeyValuePair<string, string>("userpassword", this.txtRegistrationPassword.Text),
                    new KeyValuePair<string, string>("emailaddress", this.txtRegistrationEmail.Text),
                    new KeyValuePair<string, string>("firstname", this.txtFirstName.Text),
                    new KeyValuePair<string, string>("lastname", this.txtLastName.Text)

                };
            DataServices.executeSQLStatement(sql, paramListSave);

            Response.Redirect("RegistrationThankYou.aspx?id=patient");


        }


        private int CheckEmailAddress()
        {
            /*
             * 0 Okay
             * 1 Email exists
             * 2 Email not correct extension
             */

            string sql = "SELECT * from systemsettings.app_user where emailaddress = @email;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("email", this.txtRegistrationEmail.Text),
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                return 1;
            }
            else
            {
                return 0;
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
    }
}