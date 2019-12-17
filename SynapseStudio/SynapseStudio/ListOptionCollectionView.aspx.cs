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
    public partial class ListOptionCollectionView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = "";
                try
                {
                    id = Request.QueryString["id"].ToString();
                }
                catch
                {
                    Response.Redirect("Error.aspx");
                }


                if (String.IsNullOrEmpty(id))
                {
                    Response.Redirect("Error.aspx");
                }

                this.hdnOptionCollectionID.Value = id;

                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;


                BindFields();
                BindGrid();
            }
        }

        private void BindFields()
        {
            string sql = "SELECT * FROM listsettings.questionoptioncollection WHERE questionoptioncollection_id = @questionoptioncollection_id;";
            string id = this.hdnOptionCollectionID.Value;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoptioncollection_id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            try
            {
                this.txtCollectionName.Text = dt.Rows[0]["questionoptioncollectionname"].ToString();
            }
            catch {}

            try
            {
                this.txtCollectionDescription.Text = dt.Rows[0]["questionoptioncollectiondescription"].ToString();
            }
            catch { }

        }

        private void BindGrid() {
            string sql = "SELECT questionoption_id, questionoptioncollection_id, questionoptioncollectionname, questionoptioncollectionorder, optionvaluetext, optiondisplaytext, optionflag FROM listsettings.questionoption WHERE questionoptioncollection_id = @questionoptioncollection_id;";
            string id = this.hdnOptionCollectionID.Value;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoptioncollection_id", id)
            };


            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            this.dgOptions.DataSource = dt;
            this.dgOptions.DataBind();
            
            this.lblOptionsCount.Text = dt.Rows.Count.ToString();


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;
            this.fgCollectionName.CssClass = noerr;


            if (string.IsNullOrEmpty(this.txtCollectionName.Text.ToString()))
            {
                this.lblError.Text = "Please enter a new name for the collection";
                this.txtCollectionName.Focus();
                this.lblError.Visible = true;
                this.fgCollectionName.CssClass = haserr;
                return;
            }

            string sql = "UPDATE listsettings.questionoptioncollection SET questionoptioncollectionname = @questionoptioncollectionname, questionoptioncollectiondescription = @questionoptioncollectiondescription WHERE questionoptioncollection_id = @questionoptioncollection_id;";

            string id = this.hdnOptionCollectionID.Value;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoptioncollection_id", id),
                new KeyValuePair<string, string>("questionoptioncollectionname", this.txtCollectionName.Text),
                new KeyValuePair<string, string>("questionoptioncollectiondescription", this.txtCollectionDescription.Text)
            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                this.lblError.Text = "Error saving question collection: " + System.Environment.NewLine + ex.ToString();
                this.lblError.Visible = true;
                return;
            }


            Response.Redirect("ListManagerList.aspx");


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListManagerList.aspx");
        }

        protected void btnNewOption_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListOptionNew.aspx?id=" + this.hdnOptionCollectionID.Value);
        }
    }
}