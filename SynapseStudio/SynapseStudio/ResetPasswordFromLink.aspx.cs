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
    public partial class ResetPasswordFromLink : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                this.pnlFailed.Visible = false;
                this.pnlSuccess.Visible = false;
                this.pnlResetWorked.Visible = false;
                this.lblError.Visible = false;

                string id = "";
                try
                {
                    id = Request.QueryString["id"].ToString();
                }
                catch
                {
                    this.pnlFailed.Visible = true;
                    return;
                }

                this.hdnCode.Value = id;

                ValidateCode(id);


            }
        }

        private void ValidateCode(string code)
        {

            string sql = "SELECT * FROM systemsettings.app_user WHERE emailresetstring = @code;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("code", code)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {

                this.pnlSuccess.Visible = true;
                return;

            }
            else
            {
                this.pnlFailed.Visible = true;
                return;
            }



        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string haserr = "form-group has-error";
            string noerr = "form-group";

            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.fgPassword.CssClass = noerr;
            this.fgPassword.CssClass = noerr;
            this.fgConfirmPassword.CssClass = noerr;

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


            string newguid = System.Convert.ToString(System.Guid.NewGuid());
            string sqlUpdate = "UPDATE systemsettings.app_user SET userpassword = crypt(@userpassword, gen_salt('bf', 8)), emailresetstring = @newguid WHERE emailresetstring = @code;";
            var paramListUpdate = new List<KeyValuePair<string, string>>() {
                     new KeyValuePair<string, string>("code", this.hdnCode.Value),
                     new KeyValuePair<string, string>("userpassword", this.txtRegistrationPassword.Text),
                     new KeyValuePair<string, string>("newguid", newguid)
                };

            DataServices.executeSQLStatement(sqlUpdate, paramListUpdate);


            this.pnlSuccess.Visible = false;
            this.pnlResetWorked.Visible = true;


        }
    }
}