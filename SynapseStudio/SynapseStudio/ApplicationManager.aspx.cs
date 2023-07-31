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
using System.Linq;
using System.Net.Http;

using SynapseStudio.Models;

using SynapseStudio.Services;

namespace SynapseStudio
{
    public partial class ApplicationManager : System.Web.UI.Page
    {
        int maxapporder = 1;
        int maxmodule = 1;
        /// <summary>
        /// load module grid and application grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            GetApplication();
            GetModules();
        }
        private void GetApplication()
        {
            using (var client = new HttpClient())
            {
                List<ApplicationModule> resultj = APIservice.GetList<ApplicationModule>("synapsenamespace=meta&synapseentityname=application");
                if (resultj == null)
                {
                    lblApplicationCount.Text = "0";
                    maxapporder =1;
                    dgApplication.DataSource = resultj;
                    dgApplication.DataBind();
                }
                else
                {
                    lblApplicationCount.Text = resultj.Count.ToString();
                    maxapporder = resultj.Max(x => x.displayorder);
                    dgApplication.DataSource = resultj;
                    dgApplication.DataBind();
                }
             

            }

        }

        private void GetModules()
        {
            using (var client = new HttpClient())
            {
                List<ModulesModel> resultj = APIservice.GetList<ModulesModel>("synapsenamespace=meta&synapseentityname=module");
                if (resultj == null)
                {
                    lblModuleCount.Text = "0";
                    maxmodule = 1;                  
                    dgModule.DataSource = resultj;
                    dgModule.DataBind();
                }
                else
                {
                    lblModuleCount.Text = resultj.Count.ToString();
                    maxmodule = resultj.Max(x => x.displayorder);
                    dgModule.DataSource = resultj;
                    dgModule.DataBind();

                }
            }

        }
        /// <summary>
        /// Redirect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateApplication_Click(object sender, EventArgs e)
        {

            Response.Redirect("NewApplication.aspx?displayorder=" + (maxapporder + 1).ToString());
        }
        /// <summary>
        /// Redirect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnManageIdentityClaims_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewModule.aspx?displayorder=" + (maxmodule + 1).ToString());
        }
    }
}