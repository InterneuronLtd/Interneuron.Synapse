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
    public partial class ManageAPIs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAPIResources();
            }
            resetLabels();
        }

        private void resetLabels()
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;

        }

        protected void LoadAPIResources()
        {
            string sql = "SELECT ir.\"Name\" , ir.\"DisplayName\" , ir.\"Description\", ir.\"Enabled\", ir.\"Id\" FROM \"ApiResources\" ir  ORDER BY \"Name\";";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("listnamespaceid", this.ddlSynapseNamespace.SelectedValue)
            //};

            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.dgAPIResources.DataSource = dt;
            this.dgAPIResources.DataBind();

            this.lblResultCount.Text = dt.Rows.Count.ToString();
        }


        private bool validateInput()
        {
            bool result = true;
            string vmsg = "<b>Please specify:</b> <br>";
            if (string.IsNullOrWhiteSpace(txtResourceName.Text))
            {
                vmsg += "API Name <br>";
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
                string sql = "INSERT INTO \"ApiResources\" (\"Name\",\"DisplayName\",\"Enabled\",\"Description\") VALUES ( @name,@displayname,cast(@enabled as bool),@description);";

                var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("name", txtResourceName.Text),
                new KeyValuePair<string, string>("displayname", txtDisplayName.Text),
                 new KeyValuePair<string, string>("enabled", chkEnabled.Checked.ToString()),
                  new KeyValuePair<string, string>("description", txtDescription.Text)
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
                { ShowSuccess("API " + txtResourceName.Text + " Added"); LoadAPIResources(); }

            }
            else
            {

            }
        }

        protected void dgAPIResource_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString().Split(':')[0];
            string status = e.CommandArgument.ToString().Split(':')[1];
            bool success = true;
            string sql = "update  \"ApiResources\" set \"Enabled\" = cast(@status as bool) where \"Id\" = cast(@id as int);";

            var paramList = new List<KeyValuePair<string, string>>() {
                  new KeyValuePair<string, string>("status",status.ToLower() == "true"? "false":"true"),
                new KeyValuePair<string, string>("id", id),
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
            { ShowSuccess("API " + (status.ToLower() == "true" ? "Disabled" : "Enabled")); LoadAPIResources(); }

        }
    }
}