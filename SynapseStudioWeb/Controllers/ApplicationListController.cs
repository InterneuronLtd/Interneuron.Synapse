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
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using NToastNotify;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class ApplicationListController : Controller
    {
        private IToastNotification toastNotification;

        public ApplicationListController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
        }

        public IActionResult ApplicationList()
        {

            string token = HttpContext.Session.GetString("access_token");
            List<ApplicationModule> Applicationlist = APIservice.GetList<ApplicationModule>("synapsenamespace=meta&synapseentityname=application", token);
            List<PatientListModel> resultlist = getListofListModel();
            ViewBag.Applicationlist = Applicationlist;
            ViewBag.list = resultlist;
            ViewBag.applicationcount = Applicationlist.Count();
            ViewBag.listcount = resultlist.Count();

            return View();
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
                        listdescription = Convert.ToString(dr["listdescription"])
                    }).ToList();
            return List;
        }

        public IActionResult SaveDisplayName(string listId, string Modelid, string displayname, bool isdefaultmodule)
        {
            string token = HttpContext.Session.GetString("access_token");
            List<MappedList> Mappedmodule = APIservice.GetListById<MappedList>(listId.Trim(), "synapsenamespace=meta&synapseentityname=applicationlist&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.displayorder).ToList();
            MappedList Model = Mappedmodule.Single(s => s.applicationlist_id == Modelid);
            Model.listname = displayname;

            string results = APIservice.PostObject<MappedList>(Model, "synapsenamespace=meta&synapseentityname=applicationlist", token);
            return new EmptyResult();
        }

        public IActionResult ApplicationListMapping(string id, string name)
        {
            ViewBag.applicationid = id.Trim();
            ViewBag.applicationname = name;
            return View();
        }

        public IActionResult LoadModuleList(string listId)
        {
            string token = HttpContext.Session.GetString("access_token");
            List<MappedList> Mappedmodule = APIservice.GetListById<MappedList>(listId.Trim(), "synapsenamespace=meta&synapseentityname=applicationlist&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.displayorder).ToList();
            List<PatientListModel> list = getListofListModel();
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
            return Json(JsonConvert.SerializeObject(mapping));
        }

        public IActionResult GetMappedModules(string listId)
        {
            string token = HttpContext.Session.GetString("access_token");
            List<MappedList> Mappedmodule = APIservice.GetListById<MappedList>(listId.Trim(), "synapsenamespace=meta&synapseentityname=applicationlist&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.displayorder).ToList();
            List<PatientListModel> list = getListofListModel();

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




            return Json(JsonConvert.SerializeObject(Mappedmodule));
        }

        public EmptyResult MapModuletoApplication(string listId, string attributename, int ordinalposition)
        {
            string token = HttpContext.Session.GetString("access_token");
            List<PatientListModel> listModulesModel = getListofListModel();
            List<MappedList> listMappedmodules = APIservice.GetListById<MappedList>(listId.Trim(), "synapsenamespace=meta&synapseentityname=applicationlist&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.displayorder).ToList();

            MappedList Model = new MappedList();
            Guid id = Guid.NewGuid();
            PatientListModel moduleobj = listModulesModel.Single(s => s.list_id == attributename);
            Model.applicationlist_id = id.ToString();
            Model.application_id = listId.Trim();
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

            string results = APIservice.PostObject<MappedList>(Model, "synapsenamespace=meta&synapseentityname=applicationlist", token);
            listMappedmodules = APIservice.GetListById<MappedList>(listId.Trim(), "synapsenamespace=meta&synapseentityname=applicationlist&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.displayorder).ToList(); ;

            return new EmptyResult();
        }

        public EmptyResult UnMapModulefromApplication(string listId, string attributename)
        {

            string token = HttpContext.Session.GetString("access_token");

            string results = APIservice.DeleteObject(listId.Trim(), "synapsenamespace=meta&synapseentityname=applicationlist&id=", token);

            return new EmptyResult();
        }

        public IActionResult ApplicationQuestionMapping(string id, string name)
        {
            ViewBag.applicationid = id.Trim();
            ViewBag.applicationname = name;
            return View();
        }

        public List<Question> getListofQuestions()
        {
            string sql = "SELECT question_id, questionquickname, labeltext FROM listsettings.question ORDER BY questionquickname;";
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("listnamespaceid","1")
            //};

            DataSet ds = DataServices.DataSetFromSQL(sql);
            DataTable dt = ds.Tables[0];
            List<Question> List = new List<Question>();
            List = (from DataRow dr in dt.Rows
                    select new Question()
                    {

                        question_id = Convert.ToString(dr["question_id"]),
                        questionquickname = Convert.ToString(dr["questionquickname"]),
                        labeltext = Convert.ToString(dr["labeltext"])
                    }).ToList();

            return List;
        }

        public IActionResult LoadApplicationQuestion(string applicationId)
        {
            string token = HttpContext.Session.GetString("access_token");

            List<Question> questionList = getListofQuestions();

            List<ApplicationQuestion> applicationQuestion = APIservice.GetListById<ApplicationQuestion>(applicationId.Trim(), "synapsenamespace=meta&synapseentityname=applicationquestion&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.ordinalposition).ToList();

            List<ApplicationQuestionMapping> mappingList;

            mappingList = (from qlist in questionList
                           select new ApplicationQuestionMapping()
                           {
                               application_id = applicationId,
                               questionid = qlist.question_id,
                               questionquickname = qlist.questionquickname,
                               labeltext = qlist.labeltext,
                               isselected = false
                           }).ToList();

            foreach (ApplicationQuestion ques in applicationQuestion)
            {
                int mappedIndex = mappingList.FindIndex(m => m.questionid == ques.questionid);

                if (mappedIndex > -1)
                {
                    mappingList[mappedIndex].applicationquestion_id = ques.applicationquestion_id;
                    mappingList[mappedIndex].isselected = true;
                }
            }

            return Json(JsonConvert.SerializeObject(mappingList));
        }

        public EmptyResult MapQuestiontoApplication(string applicationId, string questionid, int ordinalposition)
        {
            string token = HttpContext.Session.GetString("access_token");

            List<ApplicationQuestion> applicationQuestion = APIservice.GetListById<ApplicationQuestion>(applicationId.Trim(), "synapsenamespace=meta&synapseentityname=applicationquestion&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.ordinalposition).ToList();

            int? ordinalpos = applicationQuestion.Select(app => app.ordinalposition).Max();

            ApplicationQuestion Model = new ApplicationQuestion();
            Guid id = Guid.NewGuid();

            Model.applicationquestion_id = id.ToString();
            Model.application_id = applicationId.Trim();
            Model.questionid = questionid;
            Model.ordinalposition = ordinalpos is null? 1: ordinalpos + 1;
            Model.isselected = true;

            string results = APIservice.PostObject<ApplicationQuestion>(Model, "synapsenamespace=meta&synapseentityname=applicationquestion", token);

            if (results == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Question is mapped to the application.");
            }

            return new EmptyResult();
        }

        public EmptyResult UnMapQuestionfromApplication(string applicationId, string questionid)
        {
            string token = HttpContext.Session.GetString("access_token");

            List<ApplicationQuestion> applicationQuestion = APIservice.GetListById<ApplicationQuestion>(applicationId.Trim(), "synapsenamespace=meta&synapseentityname=applicationquestion&synapseattributename=application_id&attributevalue=", token).OrderBy(o => o.ordinalposition).ToList();

            string applicationquestion_id = applicationQuestion.Where(app => app.questionid == questionid).Select(app => app.applicationquestion_id).FirstOrDefault();

            string results = APIservice.DeleteObject(applicationquestion_id.Trim(), "synapsenamespace=meta&synapseentityname=applicationquestion&id=", token);

            if (results == "OK")
            {
                this.toastNotification.AddSuccessToastMessage("Question is unmapped from the application.");
            }

            return new EmptyResult();
        }
    }
}