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
using SynapseStudio.Models;
using SynapseStudio.Services;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SynapseStudio
{
    public partial class Newapplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;
        }
        /// <summary>
        /// NEw Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            if (GetApplication() != 0)
            {
                lblError.Text = "Application name already exists.";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }
            else
            {
                lblError.Visible = false;
                lblSuccess.Visible = false;
                string responce = NewApplication();
                if (responce == "OK")
                {

                    lblSuccess.Visible = true;
                    lblSuccess.Text = "New Application Added";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = responce;
                }

            }

        }
        /// <summary>
        /// New Application API call
        /// </summary>
        /// <returns></returns>
        public string NewApplication()
        {

            ApplicationModule Model = new ApplicationModule();
            Guid id = Guid.NewGuid();
            Model.application_id = id.ToString();
            Model.applicationname = txtApplication.Text.Trim();
            Model.displayorder = Convert.ToInt32(Request.QueryString["displayorder"]);
            string result = APIservice.PostObject<ApplicationModule>(Model, "synapsenamespace=meta&synapseentityname=application");
            return result;

        }
        /// <summary>
        /// Check Application Name 
        /// </summary>
        /// <returns></returns>
        private int GetApplication()
        {
            List<ApplicationModule> resultj = APIservice.GetList<ApplicationModule>("synapsenamespace=meta&synapseentityname=application");
            return resultj.Where(x => x.applicationname == txtApplication.Text.Trim()).Count();
        }
    }
}