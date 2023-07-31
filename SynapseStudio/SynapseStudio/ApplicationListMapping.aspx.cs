//Interneuron Synapse

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
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class ApplicationListMapping : System.Web.UI.Page
    {
        public static string ApplicationId { get; set; }
        public static List<PatientListModel> listModulesModel = new List<PatientListModel>();
        public static List<MappedList> listMappedmodules = new List<MappedList>();
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
                    listModulesModel = getListofListModel();
                    listMappedmodules = APIservice.GetListById<MappedList>(ApplicationId, "synapsenamespace=meta&synapseentityname=applicationlist&synapseattributename=application_id&attributevalue=").OrderBy(o => o.displayorder).ToList();

                }
            }

        }

       public List<PatientListModel> getListofListModel()
        {
            string sql = "SELECT * FROM listsettings.listmanager WHERE '1' = @listnamespaceid ORDER BY listname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listnamespaceid","1")
            };


            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            List<PatientListModel> List = new List<PatientListModel>();
            List = (from DataRow dr in dt.Rows
                           select new PatientListModel()
                           {
                              
                               list_id = Convert.ToString(dr["list_id"]),
                               listname = Convert.ToString(dr["listname"]),
                               listdescription =Convert.ToString( dr["listdescription"])
                           }).ToList();
            return List;
        }
        [WebMethod()]
        public static string SaveDisplayName(string listId, string displayname, bool isdefaultmodule)
        {
            if (isdefaultmodule)
            {

               
            }

            List<MappedList> Mappedmodule = listMappedmodules;
            MappedList Model = Mappedmodule.Single(s => s.applicationlist_id == listId);
            Model.listname = displayname;

            string results = APIservice.PostObject<MappedList>(Model, "synapsenamespace=meta&synapseentityname=applicationlist");
            listMappedmodules = APIservice.GetListById<MappedList>(ApplicationId, "synapsenamespace=meta&synapseentityname=applicationlist&synapseattributename=application_id&attributevalue=").OrderBy(o => o.displayorder).ToList(); ;
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
            List<MappedList> Mappedmodule = listMappedmodules;
            List<PatientListModel> list = listModulesModel;
            List<ApplicationModuleMappingModel> mapping = new List<ApplicationModuleMappingModel>();
            mapping = (from customer in list
                       select customer)
                            .Select(x => new ApplicationModuleMappingModel()
                            {
                                module_id = x.list_id,
                                modulename = x.listname

                            }).ToList();
            var query = from x in mapping
                        join y in Mappedmodule
                            on x.module_id equals y.listid
                        select new { x, y };
            foreach (var match in query)
            {
                match.x.isselected = true;
                match.x.applicationmodulemapping_id = match.y.applicationlist_id;
                match.x.modulename = match.y.listname;
            }


            return mapping;

        }
        /// <summary>
        /// Get All Mapped modules
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [WebMethod()]
        public static List<MappedList> GetMappedModules(string listId)
        {
            List<MappedList> Mappedmodule = listMappedmodules;
            List<PatientListModel> list = listModulesModel;


            List<ApplicationModuleMappingModel> mapping = new List<ApplicationModuleMappingModel>();
            mapping = (from customer in list
                       select customer)
                            .Select(x => new ApplicationModuleMappingModel()
                            {
                                module_id = x.list_id,
                                modulename = x.listname

                            }).ToList();
            var query = from x in mapping
                        join y in Mappedmodule
                            on x.module_id equals y.listid
                        select new { x, y };
            foreach (var match in query)
            {
                match.x.isselected = true;
                match.x.applicationmodulemapping_id = match.y.applicationlist_id;
               

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
            MappedList Model = new MappedList();
            Guid id = Guid.NewGuid();
            PatientListModel moduleobj = listModulesModel.Single(s => s.list_id == attributename);
            Model.applicationlist_id = id.ToString();
            Model.application_id = ApplicationId;
            Model.listid = attributename;
            Model.listname = moduleobj.listname;
         
            if (listMappedmodules.Count == 0)
            {
                Model.displayorder = 1;
            }
            else
            {
                Model.displayorder = listMappedmodules.Max(x => x.displayorder.Value) + 1;
            }

            string results = APIservice.PostObject<MappedList>(Model, "synapsenamespace=meta&synapseentityname=applicationlist");
            listMappedmodules = APIservice.GetListById<MappedList>(ApplicationId, "synapsenamespace=meta&synapseentityname=applicationlist&synapseattributename=application_id&attributevalue=").OrderBy(o => o.displayorder).ToList(); ;


        }

        [WebMethod()]
        public static String UnMapModulefromApplication(string listId, string attributename)
        {
            string results = APIservice.DeleteObject(listId, "synapsenamespace=meta&synapseentityname=applicationlist&id=");
            listMappedmodules = APIservice.GetListById<MappedList>(ApplicationId, "synapsenamespace=meta&synapseentityname=applicationlist&synapseattributename=application_id&attributevalue=").OrderBy(o => o.displayorder).ToList(); ;

            return ""; 
        }
    }
}