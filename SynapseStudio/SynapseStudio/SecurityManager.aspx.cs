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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class SecurityManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadClients();
            LoadIdentityClaims();
            LoadAPIResources();
            LoadRoles();
        }

        protected void LoadClients()
        {
            string sql = "SELECT \"Id\", \"ClientId\", case when \"Enabled\" = 'True' then 'Yes' else 'No' end as enabled FROM \"Clients\" ORDER BY \"ClientName\";";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("listnamespaceid", this.ddlSynapseNamespace.SelectedValue)
            //};

            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.dgClients.DataSource = dt;
            this.dgClients.DataBind();

            this.lblClientCount.Text = dt.Rows.Count.ToString();
        }

        protected void LoadIdentityClaims()
        {
            string sql = "SELECT ir.\"Name\" as resourcename, ic.\"Type\" as claimname FROM \"IdentityResources\" ir INNER JOIN \"IdentityClaims\" ic on (ir.\"Id\" = ic.\"IdentityResourceId\")  ORDER BY \"Type\";";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("listnamespaceid", this.ddlSynapseNamespace.SelectedValue)
            //};

            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.dgIdentityClaims.DataSource = dt;
            this.dgIdentityClaims.DataBind();

            this.lblIdentityClaimCount.Text = dt.Rows.Count.ToString();
        }

        protected void LoadAPIResources()
        {
            string sql = "SELECT ar.\"Name\" as resourcename, aps.\"Name\" as scopename, aps.\"Description\" as scopedescription FROM \"ApiResources\" ar INNER JOIN \"ApiScopes\" aps on (ar.\"Id\" = aps.\"ApiResourceId\")  ORDER BY ar.\"Name\" , aps.\"Name\";";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("listnamespaceid", this.ddlSynapseNamespace.SelectedValue)
            //};

            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.dgAPIs.DataSource = dt;
            this.dgAPIs.DataBind();

            this.lblAPICount.Text = dt.Rows.Count.ToString();
        }

        protected void LoadRoles()
        {
            string sql = "SELECT \"Name\" as rolename, \"Id\" as id FROM \"AspNetRoles\" ORDER BY \"Name\";";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("listnamespaceid", this.ddlSynapseNamespace.SelectedValue)
            //};

            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.dgRoles.DataSource = dt;
            this.dgRoles.DataBind();

            this.lblRoleCount.Text = dt.Rows.Count.ToString();
        }

        protected void btnAddRole_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewRole.aspx");
        }

        protected void btnManageAPIs_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageAPIs.aspx");
        }

        protected void btnManageIdentityClaims_Click(object sender, EventArgs e)
        {
            Response.Redirect("IdentityClaimsManager.aspx");
        }

        protected void btnCreateNewEntity_Click(object sender, EventArgs e)
        {
            Response.Redirect("Clients.aspx");
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddUsers.aspx");
        }
    }
}