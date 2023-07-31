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
    public partial class ManageRole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    if (Session["SM_roleid"] == null)
                        Response.Redirect("SecurityManager.aspx");
                }
                else
                    Session["SM_roleid"] = Request.QueryString["id"];
                getRoleName();
                LoadUsersInRole();

            }
            if (Session["SM_roleid"] == null)
                Response.Redirect("SecurityManager.aspx");
            resetLabels();
        }
        protected void getRoleName()
        {
            string sql = "SELECT \"Id\", \"Name\" FROM \"AspNetRoles\" where \"Id\" = cast('" + Session["SM_roleid"].ToString() + "' as text)  ;";
            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];
            Session["SM_rolename"] = dt.Rows[0]["Name"].ToString();

        }
        protected void Search(object sender, EventArgs e)
        {
            this.SearchLocalLogins();
        }
        private void SearchLocalLogins()
        {
            string sql = "SELECT \"Id\", \"UserName\" FROM \"AspNetUsers\" where \"Id\" not in (select \"UserId\" from \"AspNetUserLogins\" ) and \"UserName\" like '" + txtLocalUserId.Text + "%'  ;";


            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.gvLocalLogins.DataSource = dt;
            this.gvLocalLogins.DataBind();


        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvLocalLogins.PageIndex = e.NewPageIndex;
            this.SearchLocalLogins();
        }

        private void resetLabels()
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;

            lblRoleName.Text = Session["SM_rolename"].ToString();

        }

        protected void LoadUsersInRole()
        {
            string sql = "select \"UserName\", u.\"Id\" from \"AspNetUsers\" u inner join \"AspNetUserRoles\" ur on ur.\"UserId\" = u.\"Id\" and ur.\"RoleId\" = cast('" + Session["SM_roleid"].ToString() + "' as text)";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("listnamespaceid", this.ddlSynapseNamespace.SelectedValue)
            //};

            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.dgUsersInRole.DataSource = dt;
            this.dgUsersInRole.DataBind();

            this.lblResultCount.Text = dt.Rows.Count.ToString();
        }


        private bool validateInput()
        {
            bool result = true;
            string vmsg = "Please specify: <br>";
            if (string.IsNullOrWhiteSpace(txtLocalUserId.Text))
            {
                vmsg += "Claim type <br>";
                result = false;
            }

            if (result == false)
                ShowError(vmsg);
            return result;
        }

        private void ShowError(string msg)
        {
            lblError.Text = msg;
            lblError.Visible = true;

        }

        private void ShowSuccess(string msg)
        {
            lblSuccess.Visible = true;
            lblSuccess.Text = msg;
        }


        protected void gvLocalLogins_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool success = true;
            string sql = "insert into  \"AspNetUserRoles\" (\"UserId\",\"RoleId\") values (@userid,@roleid) ";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("userid", e.CommandArgument.ToString()),
                  new KeyValuePair<string, string>("roleid", Session["SM_roleid"].ToString())
                            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                success = false;
            }

            if (success)
            { ShowSuccess("User Added"); LoadUsersInRole(); }
        }

        protected void dgUsersInRole_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            bool success = true;
            string sql = "delete from  \"AspNetUserRoles\" where \"UserId\" = @userid and \"RoleId\" = @roleid;";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("userid", e.CommandArgument.ToString()),
                   new KeyValuePair<string, string>("roleid", Session["SM_roleid"].ToString()),
                            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                success = false;
            }

            if (success)
            { ShowSuccess("User removed"); LoadUsersInRole(); }
        }
    }
}