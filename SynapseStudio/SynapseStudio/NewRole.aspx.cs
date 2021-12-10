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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class NewRole : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;
            this.fgRoleName.CssClass = noerr;


            if (string.IsNullOrEmpty(this.txtRoleName.Text.ToString()))
            {
                this.lblError.Text = "Please enter a new name for the Role";
                this.txtRoleName.Focus();
                this.lblError.Visible = true;
                this.fgRoleName.CssClass = haserr;
                return;
            }

            string sql = "INSERT INTO \"AspNetRoles\"(\"Id\", \"Name\", \"NormalizedName\") VALUES (@id, @name, @nname);";

            string id = System.Guid.NewGuid().ToString();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id),
                new KeyValuePair<string, string>("name", this.txtRoleName.Text),
                new KeyValuePair<string, string>("nname", this.txtRoleName.Text.ToUpper())
            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            }
            catch (Exception ex)
            {
                this.lblError.Text = "Error creating  Role: " + System.Environment.NewLine + ex.ToString();
                this.lblError.Visible = true;
                return;
            }


            Response.Redirect("ManageRole.aspx?id=" + id);


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("SecurityManager.aspx");
        }
    }
}