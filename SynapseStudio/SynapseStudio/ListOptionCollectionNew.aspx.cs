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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class ListOptionCollectionNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
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
            this.fgCollectionName.CssClass = noerr;
            

            if (string.IsNullOrEmpty(this.txtCollectionName.Text.ToString()))
            {
                this.lblError.Text = "Please enter a new name for the collection";
                this.txtCollectionName.Focus();
                this.lblError.Visible = true;
                this.fgCollectionName.CssClass = haserr;
                return;
            }

            string sql = "INSERT INTO listsettings.questionoptioncollection(questionoptioncollection_id, questionoptioncollectionname, questionoptioncollectiondescription) VALUES (@questionoptioncollection_id, @questionoptioncollectionname, @questionoptioncollectiondescription);";

            string id = System.Guid.NewGuid().ToString();
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
                this.lblError.Text = "Error creating question collection: " + System.Environment.NewLine + ex.ToString();
                this.lblError.Visible = true;
                return;
            }


            Response.Redirect("ListOptionCollectionView.aspx?id=" + id);


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListManagerList.aspx");
        }
    }
}