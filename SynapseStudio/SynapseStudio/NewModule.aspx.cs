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

using Newtonsoft.Json;
using SynapseStudio.Models;
using SynapseStudio.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;


namespace SynapseStudio
{
    public partial class NewModule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;
        }


        protected  void btnAddNewModule_Click(object sender, EventArgs e)
        {
            if (GetModules() != 0)
            {
                lblError.Text = "Module name already exists.";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }
            else
            {
                lblError.Visible = false;
                lblSuccess.Visible = false;
                string responce = ADDNewModule();
                if (responce == "OK")
                {

                    lblSuccess.Visible = true;
                    lblSuccess.Text = "New Module Added";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = responce;
                }

            }

        }
        public string ADDNewModule()
        {

            ModulesModel Model = new ModulesModel();
            Guid id = Guid.NewGuid();
            Model.module_id = id.ToString();
            Model.modulename = txtmodulename.Text.Trim();
            Model.jsurl = txtjsurl.Text.Trim();
            Model.moduledescription = txtmoduledescription.Text;
            Model.domselector = txtdomselector.Text.Trim();
            Model.displayorder = Convert.ToInt32(Request.QueryString["displayorder"]);
            string result = APIservice.PostObject<ModulesModel>(Model, "synapsenamespace=meta&synapseentityname=module");
            return result;
        }

        private int GetModules()
        {
            List<ModulesModel> resultj = APIservice.GetList<ModulesModel>("synapsenamespace=meta&synapseentityname=module");
            return resultj.Where(x => x.modulename == txtmodulename.Text.Trim()).Count();
            
        }
    }
}