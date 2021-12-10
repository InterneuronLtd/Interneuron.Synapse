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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class ApplicationManagerController : Controller
    {
    
        public IActionResult ApplicationManager()
        {
         
            string token = HttpContext.Session.GetString("access_token");
            List<ApplicationModule> Applicationlist = APIservice.GetList<ApplicationModule>("synapsenamespace=meta&synapseentityname=application", token);
            List<ModulesModel> modulelist = APIservice.GetList<ModulesModel>("synapsenamespace=meta&synapseentityname=module", token);
            ViewBag.Applicationlist = Applicationlist;
            ViewBag.modulelist = modulelist;
            ViewBag.applicationcount = Applicationlist.Count();
            ViewBag.modulecount = modulelist.Count();
        
            return View();
        }
        public IActionResult SaveDisplayName(string listId, string Modelid, string displayname, bool isdefaultmodule)
        {
            string token = HttpContext.Session.GetString("access_token");
            List<ModulesModel> listModulesModel = APIservice.GetList<ModulesModel>("synapsenamespace=meta&synapseentityname=module", token);
            List<Mappedmodule> listMappedmodules = APIservice.GetListById<Mappedmodule>(listId.Trim(), "synapsenamespace=meta&synapseentityname=applicationmodulemapping&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.displayorder).ToList();

            if (isdefaultmodule)
            {

                Mappedmodule getselectedModel = listMappedmodules.FirstOrDefault(s => s.isdefaultmodule == true);
                if (getselectedModel != null)
                {
                    getselectedModel.isdefaultmodule = false;
                    string updatedresult = APIservice.PostObject<Mappedmodule>(getselectedModel, "synapsenamespace=meta&synapseentityname=applicationmodulemapping", token);
                }
            }

            List<Mappedmodule> Mappedmodule = listMappedmodules;
            Mappedmodule Model = Mappedmodule.Single(s => s.applicationmodulemapping_id == Modelid);
            Model.displayname = displayname;
            Model.isdefaultmodule = isdefaultmodule;
            string results = APIservice.PostObject<Mappedmodule>(Model, "synapsenamespace=meta&synapseentityname=applicationmodulemapping", token);
            return new EmptyResult();
        }
        public IActionResult ApplicationModulMapping(string id,string name)
        {
            ViewBag.applicationid = id;
            ViewBag.applicationname = name;
            return View();
        }

        public IActionResult LoadModuleList(string listId)
        {

            string token = HttpContext.Session.GetString("access_token");
            List<ModulesModel> listModulesModel = APIservice.GetList<ModulesModel>("synapsenamespace=meta&synapseentityname=module", token);
            List<Mappedmodule> listMappedmodules = APIservice.GetListById<Mappedmodule>(listId.Trim(), "synapsenamespace=meta&synapseentityname=applicationmodulemapping&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.displayorder).ToList();
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




            return Json(JsonConvert.SerializeObject(mapping));
        }

        public IActionResult GetMappedModules(string listId)
        {
            string token = HttpContext.Session.GetString("access_token");
            List<ModulesModel> listModulesModel = APIservice.GetList<ModulesModel>("synapsenamespace=meta&synapseentityname=module", token);
            List<Mappedmodule> listMappedmodules = APIservice.GetListById<Mappedmodule>(listId.Trim(), "synapsenamespace=meta&synapseentityname=applicationmodulemapping&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.displayorder).ToList();
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



            return Json(JsonConvert.SerializeObject(Mappedmodule));
        }

        public EmptyResult MapModuletoApplication(string listId, string attributename, int ordinalposition)
        {
            string token = HttpContext.Session.GetString("access_token");
            List<ModulesModel> listModulesModel = APIservice.GetList<ModulesModel>("synapsenamespace=meta&synapseentityname=module", token);
            List<Mappedmodule> listMappedmodules = APIservice.GetListById<Mappedmodule>(listId.Trim(), "synapsenamespace=meta&synapseentityname=applicationmodulemapping&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.displayorder).ToList();

            Mappedmodule Model = new Mappedmodule();
            Guid id = Guid.NewGuid();
            ModulesModel moduleobj = listModulesModel.Single(s => s.module_id == attributename);
            Model.applicationmodulemapping_id = id.ToString();
            Model.application_id = listId.Trim();
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

            string results = APIservice.PostObject<Mappedmodule>(Model, "synapsenamespace=meta&synapseentityname=applicationmodulemapping", token);
            return new EmptyResult();
        }

        public EmptyResult UnMapModulefromApplication(string listId, string attributename)
        {
       
            string token = HttpContext.Session.GetString("access_token");
        
            string results = APIservice.DeleteObject(listId, "synapsenamespace=meta&synapseentityname=applicationmodulemapping&id=", token);

            return new EmptyResult();
        }

        public IActionResult AddNewApplication()
        {
           
            return View();
        }
        public ActionResult saveNewApplication(ApplicationModule Actionmodel)
        {
            string token = HttpContext.Session.GetString("access_token");
            List<ApplicationModule> resultj = APIservice.GetList<ApplicationModule>("synapsenamespace=meta&synapseentityname=application", token);
            int count = resultj.Where(x => x.applicationname == Actionmodel.applicationname.Trim()).Count();
            if (count == 0)
            {
                ApplicationModule Model = new ApplicationModule();
                Guid id = Guid.NewGuid();
                Model.application_id = id.ToString();
                Model.applicationname = Actionmodel.applicationname.Trim();
                Model.displayorder = resultj.Max(x => x.displayorder);
                Model.displayorder++;
                string result = APIservice.PostObject<ApplicationModule>(Model, "synapsenamespace=meta&synapseentityname=application", token);                
                return RedirectToAction(nameof(ApplicationManager));
            }
            else
            {
                @ViewBag.errormessage = "Application name already Exists ";
                return View("AddNewApplication");
            }
        }

        public IActionResult AddNewModule()
        {

            return View();
        }

        public ActionResult saveNewModule(ModulesModel ModulesModel)
        {
            string token = HttpContext.Session.GetString("access_token");
            List<ModulesModel> result = APIservice.GetList<ModulesModel>("synapsenamespace=meta&synapseentityname=module", token);
            int count = result.Where(x => x.modulename == ModulesModel.modulename.Trim()).Count();
            if (count == 0)
            {
                ModulesModel Model = new ModulesModel();
                Guid id = Guid.NewGuid();
                Model.module_id = id.ToString();
                Model.modulename = ModulesModel.modulename.Trim();
                Model.jsurl = ModulesModel.jsurl?? "";
                Model.moduledescription = ModulesModel.moduledescription;
                Model.domselector = ModulesModel.domselector?? "";
                Model.displayorder= result.Max(x => x.displayorder);
                Model.displayorder++;
                APIservice.PostObject<ModulesModel>(Model, "synapsenamespace=meta&synapseentityname=module", token);
                return RedirectToAction(nameof(ApplicationManager));
            }
            else
            {
                @ViewBag.errormessage = "(Module name already Exists ";
                return View("AddNewModule");
            }
        }
    }
}