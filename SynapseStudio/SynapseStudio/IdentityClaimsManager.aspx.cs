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
    public partial class IdentityClaimsManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadIdentityClaims();
                LoadIdentityResoucess();
            }
            resetLabels();
        }

        private void resetLabels()
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;

        }

        protected void LoadIdentityClaims()
        {
            string sql = "SELECT ir.\"Name\" as resourcename, ic.\"Type\" as claimname ,ic.\"Id\" as claimid FROM \"IdentityResources\" ir INNER JOIN \"IdentityClaims\" ic on (ir.\"Id\" = ic.\"IdentityResourceId\")  ORDER BY \"Type\";";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("listnamespaceid", this.ddlSynapseNamespace.SelectedValue)
            //};

            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.dgIdentityClaims.DataSource = dt;
            this.dgIdentityClaims.DataBind();

            this.lblResultCount.Text = dt.Rows.Count.ToString();
        }

        protected void LoadIdentityResoucess()
        {

            string sql = " Select 'Please Select' as Name, '0' as resourceid UNION ALL SELECT ir.\"Name\" , ir.\"Id\" as resourceid FROM \"IdentityResources\" ir  where ir.\"Enabled\" = true;";


            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.ddlResource.DataSource = dt;
            this.ddlResource.DataBind();
        }
        private bool validateInput()
        {
            bool result = true;
            string vmsg = "Please specify: <br>";
            if (string.IsNullOrWhiteSpace(txtClaimName.Text))
            {
                vmsg += "Claim type <br>";
                result = false;
            }
            if (ddlResource.SelectedIndex == 0)
            {
                vmsg += "Resource type";
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

        protected void btnValidateAndCreate_Click(object sender, EventArgs e)
        {
            if (validateInput())
            {
                bool success = true;
                string sql = "INSERT INTO \"IdentityClaims\" (\"Type\", \"IdentityResourceId\") VALUES ( @type, cast(@resourceid as int));";

                var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("type", txtClaimName.Text),
                new KeyValuePair<string, string>("resourceid", ddlResource.SelectedValue),
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
                { ShowSuccess("Claim " + txtClaimName.Text + " Added"); LoadIdentityClaims(); }

            }
            else
            {

            }
        }

        protected void dgIdentityClaims_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            bool success = true;
            string sql = "delete from  \"IdentityClaims\" where \"Id\" = cast(@id as int);";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("id", e.CommandArgument.ToString()),
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
            { ShowSuccess("Claim removed"); LoadIdentityClaims(); }

        }
    }
}