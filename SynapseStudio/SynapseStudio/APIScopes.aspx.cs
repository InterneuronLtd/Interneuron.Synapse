
//Interneuron Synapse

//Copyright(C) 2019  Interneuron CIC

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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class APIScopes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    if (Session["SM_apiid"] == null)
                        Response.Redirect("SecurityManager.aspx");
                }
                else
                    Session["SM_apiid"] = Request.QueryString["id"];
                getAPIName();
                LoadScopesInAPI();


            }
            if (Session["SM_apiid"] == null)
                Response.Redirect("SecurityManager.aspx");
            resetLabels();


            //if (!IsPostBack)
            //{
            //    LoadIdentityResources();
            //}
            //resetLabels();
        }

        protected void getAPIName()
        {
            string sql = "SELECT \"Id\", \"Name\" FROM \"ApiResources\" where \"Id\" = cast('" + Session["SM_apiid"].ToString() + "' as int)  ;";
            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];
            Session["SM_apiname"] = dt.Rows[0]["Name"].ToString();

        }

        private void resetLabels()
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;

            lblAPI.Text = Session["SM_apiname"].ToString();
        }

        protected void LoadScopesInAPI()
        {
            string sql = "SELECT ir.\"Name\" , ir.\"DisplayName\" , ir.\"Description\", ir.\"Id\" FROM \"ApiScopes\" ir where \"ApiResourceId\" = cast('" + Session["SM_apiid"].ToString() + "' as int)  ORDER BY \"Name\";";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("listnamespaceid", this.ddlSynapseNamespace.SelectedValue)
            //};

            DataSet ds = DataServices.DataSetFromSQL(sql, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];

            this.dgIdentityResources.DataSource = dt;
            this.dgIdentityResources.DataBind();

            this.lblResultCount.Text = dt.Rows.Count.ToString();
        }


        private bool validateInput()
        {
            bool result = true;
            string vmsg = "<b>Please specify:</b> <br>";
            if (string.IsNullOrWhiteSpace(txtResourceName.Text))
            {
                vmsg += "Scope Name <br>";
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
                string sql = "INSERT INTO \"ApiScopes\" (\"Name\",\"DisplayName\",\"Description\",\"Required\",\"Emphasize\",\"ShowInDiscoveryDocument\",\"ApiResourceId\") VALUES ( @name,@displayname,@description,false,false,true,cast(@apiid as int));";

                var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("name", txtResourceName.Text),
                new KeyValuePair<string, string>("displayname", txtDisplayName.Text),
                                   new KeyValuePair<string, string>("description", txtDescription.Text),
                    new KeyValuePair<string, string>("apiid", Session["SM_apiid"].ToString())
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
                { ShowSuccess("Scope " + txtResourceName.Text + " Added"); LoadScopesInAPI(); }

            }
            else
            {

            }
        }

        protected void dgIdentityResource_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
           
            bool success = true;
            string sql = "delete from   \"ApiScopes\" where \"Id\" = cast(@id as int);";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("id", id)
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
            { ShowSuccess("Scope removed"); LoadScopesInAPI(); }

        }
    }
}