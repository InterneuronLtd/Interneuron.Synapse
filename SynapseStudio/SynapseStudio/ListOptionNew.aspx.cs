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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class ListOptionNew : System.Web.UI.Page
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

                string sql = "SELECT * FROM listsettings.questionoptioncollection WHERE questionoptioncollection_id = @questionoptioncollection_id;";

                var paramList = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("questionoptioncollection_id", id)
                };
                DataSet ds = new DataSet();
                try
                {
                    ds = DataServices.DataSetFromSQL(sql, paramList);
                }
                catch (Exception ex)
                {
                    StringBuilder sbe = new StringBuilder();
                    sbe.AppendLine("<div class='contentAlertDanger'><h3 style='color: #712f2f'>Sorry, there was an error:</h3>");
                    sbe.AppendLine(ex.ToString());
                    sbe.AppendLine("</div>");
                    this.ltrlError.Visible = true;
                    this.ltrlError.Text = sbe.ToString();
                    return;
                }

                DataTable dt = ds.Tables[0];

                try
                {
                    this.lblListOptionCollection.Text = dt.Rows[0]["questionoptioncollectionname"].ToString();
                }
                catch
                {
                }

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
            this.fgOptionValueText.CssClass = noerr;
            this.fgOptionDisplayText.CssClass = noerr;


            if (string.IsNullOrEmpty(this.txtOptionValueText.Text.ToString()))
            {
                this.lblError.Text = "Please enter value associated with the option";
                this.txtOptionValueText.Focus();
                this.lblError.Visible = true;
                this.fgOptionValueText.CssClass = haserr;
                return;
            }

            if (string.IsNullOrEmpty(this.txtOptionDisplayText.Text.ToString()))
            {
                this.lblError.Text = "Please enter what you would like to display to the end user for this option";
                this.txtOptionDisplayText.Focus();
                this.lblError.Visible = true;
                this.fgOptionDisplayText.CssClass = haserr;
                return;
            }

            string sql = "INSERT INTO listsettings.questionoption (questionoption_id, questionoptioncollection_id, questionoptioncollectionname, questionoptioncollectionorder, optionvaluetext, optiondisplaytext, optionflag, optionflagalt) VALUES (@questionoption_id, @questionoptioncollection_id, @questionoptioncollectionname, CAST(@questionoptioncollectionorder AS INT), @optionvaluetext, @optiondisplaytext, @optionflag, @optionflagalt);";

            string id = System.Guid.NewGuid().ToString();
            int order = 0;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoption_id", id),
                new KeyValuePair<string, string>("questionoptioncollection_id", this.hdnOptionCollectionID.Value),
                new KeyValuePair<string, string>("questionoptioncollectionname", this.lblListOptionCollection.Text),
                new KeyValuePair<string, string>("questionoptioncollectionorder", order.ToString()),
                new KeyValuePair<string, string>("optionvaluetext", this.txtOptionValueText.Text),
                new KeyValuePair<string, string>("optiondisplaytext", this.txtOptionDisplayText.Text),
                new KeyValuePair<string, string>("optionflag", this.txtOptionFlag.Text),
                new KeyValuePair<string, string>("optionflagalt", this.txtOptionFlagAlt.Text),
            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                this.lblError.Text = "Error creating question option: " + System.Environment.NewLine + ex.ToString();
                this.lblError.Visible = true;
                return;
            }


            Response.Redirect("ListOptionView.aspx?id=" + id);


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListOptionCollectionView.aspx?id=" + this.hdnOptionCollectionID.Value);
        }
    }
}
