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
    public partial class ManageRoleScope : System.Web.UI.Page
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
                LoadScopesInRole();
                LoadApiScopes();

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

        protected void LoadScopesInRole()
        {
            string sql = "select \"Name\", ap.\"Id\", u.\"Description\" from \"ApiScopes\" u inner join \"ApiScopePermissions\" ap on ap.\"ApiScopeId\" = u.\"Id\" where ap.\"RoleId\" = cast('" + Session["SM_roleid"].ToString() + "' as text)";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("listnamespaceid", this.ddlSynapseNamespace.SelectedValue)
            //};

            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.dgScopesInRole.DataSource = dt;
            this.dgScopesInRole.DataBind();

            this.lblResultCount.Text = dt.Rows.Count.ToString();
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

        protected void LoadApiScopes()
        {

            string sql = " Select 'Please Select' as Name, '0' as Id UNION ALL SELECT ir.\"Name\" , ir.\"Id\"  FROM \"ApiScopes\" ir";


            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.ddlApiScopes.DataSource = dt;
            this.ddlApiScopes.DataBind();
        }
        private bool validateInput()
        {
            bool result = true;
            string vmsg = "<b>Please specify: </b><br>";

            if (ddlApiScopes.SelectedIndex == 0)
            {
                vmsg += "Api Scope";
                result = false;
            }
            if (result == false)
                ShowError(vmsg);
            return result;
        }

        protected void dgScopesInRole_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            bool success = true;
            string sql = "delete from  \"ApiScopePermissions\" where \"Id\" = @id ";

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
            { ShowSuccess("Scope removed"); LoadScopesInRole(); }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (validateInput())
            {
                bool success = true;
                string sql = "insert into  \"ApiScopePermissions\" (\"ApiScopeId\",\"RoleId\",\"Id\") values (cast(@userid as int),@roleid ,@id) ";

                var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("userid", ddlApiScopes.SelectedValue ),
                  new KeyValuePair<string, string>("roleid", Session["SM_roleid"].ToString()),

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
                { ShowSuccess("Scope Added"); LoadScopesInRole(); }
            }


        }
    }
}