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
using SynapseStudio.Models;
using SynapseStudio.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class ApplicationModuleMapping : System.Web.UI.Page
    {
        public static string ApplicationId { get; set; }
        public static List<ModulesModel> listModulesModel = new List<ModulesModel>();
        public static List<Mappedmodule> listMappedmodules = new List<Mappedmodule>();
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplicationId = Request.QueryString["id"].ToString();

            if (!IsPostBack)

            {
                List<ApplicationModule> listapplication = APIservice.GetList<ApplicationModule>("synapsenamespace=meta&synapseentityname=application");
                if (listapplication != null)
                {
                    string applicationname = listapplication.FirstOrDefault(s => s.application_id == ApplicationId).applicationname;
                    lblapplicationname.Text = applicationname;
                    listModulesModel = APIservice.GetList<ModulesModel>("synapsenamespace=meta&synapseentityname=module");
                    listMappedmodules = APIservice.GetListById<Mappedmodule>(ApplicationId, "synapsenamespace=meta&synapseentityname=applicationmodulemapping&synapseattributename=application_id&attributevalue=").OrderBy(o => o.displayorder).ToList();

                }
            }

        }


        [WebMethod()]
        public static string SaveDisplayName(string listId, string displayname, bool isdefaultmodule)
        {
            if (isdefaultmodule)
            {
                
                Mappedmodule getselectedModel = listMappedmodules.FirstOrDefault(s => s.isdefaultmodule == true);
                if (getselectedModel != null)
                {
                    getselectedModel.isdefaultmodule = false;
                    string updatedresult = APIservice.PostObject<Mappedmodule>(getselectedModel, "synapsenamespace=meta&synapseentityname=applicationmodulemapping");
                }
            }

            List<Mappedmodule> Mappedmodule = listMappedmodules;
            Mappedmodule Model = Mappedmodule.Single(s => s.applicationmodulemapping_id == listId);
            Model.displayname = displayname;
            Model.isdefaultmodule = isdefaultmodule;
            string results = APIservice.PostObject<Mappedmodule>(Model, "synapsenamespace=meta&synapseentityname=applicationmodulemapping");
            listMappedmodules = APIservice.GetListById<Mappedmodule>(ApplicationId, "synapsenamespace=meta&synapseentityname=applicationmodulemapping&synapseattributename=application_id&attributevalue=").OrderBy(o => o.displayorder).ToList(); ;
            return results;
        }



        /// <summary>
        /// Load Modules List
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>All Modules</returns>
        [WebMethod()]
        public static List<ApplicationModuleMappingModel> LoadModuleList(string listId)
        {
            List<Mappedmodule> Mappedmodule = listMappedmodules;
            List<ModulesModel> list = listModulesModel;
            List<ApplicationModuleMappingModel> mapping = new List<ApplicationModuleMappingModel>();
            mapping = (from customer in list
                       select customer)
                            .Select(x => new ApplicationModuleMappingModel()
                            {
                                module_id = x.module_id,
                                modulename = x.modulename

                            }).ToList();
            var query = from x in mapping
                        join y in Mappedmodule
                            on x.module_id equals y.module_id
                        select new { x, y };
            foreach (var match in query)
            {
                match.x.isselected = true;
                match.x.applicationmodulemapping_id = match.y.applicationmodulemapping_id;
                match.x.modulename = match.y.displayname;
            }


            return mapping;

        }
        /// <summary>
        /// Get All Mapped modules
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [WebMethod()]
        public static List<Mappedmodule> GetMappedModules(string listId)
        {
            List<Mappedmodule> Mappedmodule = listMappedmodules;
            List<ModulesModel> list = listModulesModel;


            List<ApplicationModuleMappingModel> mapping = new List<ApplicationModuleMappingModel>();
            mapping = (from customer in list
                       select customer)
                            .Select(x => new ApplicationModuleMappingModel()
                            {
                                module_id = x.module_id,
                                modulename = x.modulename

                            }).ToList();
            var query = from x in mapping
                        join y in Mappedmodule
                            on x.module_id equals y.module_id
                        select new { x, y };
            foreach (var match in query)
            {
                match.x.isselected = true;
                match.x.applicationmodulemapping_id = match.y.applicationmodulemapping_id;
                if (match.y.isdefaultmodule.HasValue)
                {
                    if (match.y.isdefaultmodule.Value != true)
                    {

                        match.y.isdefaultmodule = false;
                    }



                }
                else
                {
                    match.x.modulename = match.y.displayname;
                }

                match.x.displayorder = match.y.displayorder;
            }


            return Mappedmodule;

        }
        /// <summary>
        /// Map new module to application
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="attributename"></param>
        /// <param name="ordinalposition"></param>
        [WebMethod()]
        public static void MapModuletoApplication(string listId, string attributename, int ordinalposition)
        {
            Mappedmodule Model = new Mappedmodule();
            Guid id = Guid.NewGuid();
            ModulesModel moduleobj = listModulesModel.Single(s => s.module_id == attributename);
            Model.applicationmodulemapping_id = id.ToString();
            Model.application_id = ApplicationId;
            Model.module_id = attributename;
            Model.displayname = moduleobj.modulename;
            Model.isdefaultmodule = false;
            if (listMappedmodules.Count == 0)
            {
                Model.displayorder = 1;
            }
            else
            {
                Model.displayorder = listMappedmodules.Max(x => x.displayorder.Value) + 1;
            }

            string results = APIservice.PostObject<Mappedmodule>(Model, "synapsenamespace=meta&synapseentityname=applicationmodulemapping");
            listMappedmodules = APIservice.GetListById<Mappedmodule>(ApplicationId, "synapsenamespace=meta&synapseentityname=applicationmodulemapping&synapseattributename=application_id&attributevalue=").OrderBy(o => o.displayorder).ToList(); ;


        }

        [WebMethod()]
        public static String UnMapModulefromApplication(string listId, string attributename)
        {
            string results = APIservice.DeleteObject(listId, "synapsenamespace=meta&synapseentityname=applicationmodulemapping&id=");
            listMappedmodules = APIservice.GetListById<Mappedmodule>(ApplicationId, "synapsenamespace=meta&synapseentityname=applicationmodulemapping&synapseattributename=application_id&attributevalue=").OrderBy(o => o.displayorder).ToList(); ;

            return results;
        }
    }
}