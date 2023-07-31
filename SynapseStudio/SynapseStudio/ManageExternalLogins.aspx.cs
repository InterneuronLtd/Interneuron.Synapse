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


﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class ManageExternalLogins : System.Web.UI.Page
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


        private void resetLabels()
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;

            lblRoleName.Text = Session["SM_rolename"].ToString();

        }

        protected void LoadUsersInRole()
        {
            string sql = "select \"ExternalSubjectId\", u.\"Id\", u.\"Idp\" from \"UserRoles_ExternalProviders\" u where u.\"RoleId\" = cast('" + Session["SM_roleid"].ToString() + "' as text)";
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
            if (string.IsNullOrWhiteSpace(txtExternalUserId.Text))
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




        protected void dgUsersInRole_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            bool success = true;
            string sql = "delete from  \"UserRoles_ExternalProviders\" where \"Id\" = @id ";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("id", e.CommandArgument.ToString())
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            bool success = true;
            string sql = "insert into  \"UserRoles_ExternalProviders\" (\"ExternalSubjectId\",\"RoleId\",\"Idp\",\"Id\") values (@userid,@roleid, @idp,@id) ";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("userid", txtExternalUserId.Text ),
                  new KeyValuePair<string, string>("roleid", Session["SM_roleid"].ToString()),
                   new KeyValuePair<string, string>("idp", ddlIdp.SelectedValue),
                    new KeyValuePair<string, string>("id", Guid.NewGuid().ToString())
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
    }
}