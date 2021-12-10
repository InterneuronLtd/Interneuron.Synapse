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
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using SynapseStudioWeb.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class ListController : Controller
    {
        public static List<PersonaListFilter> personaListFilters = new List<PersonaListFilter>();
        public static DataSet PersonaField;
        public static DataSet PersonaBaseviewField;
        public static string namespaceId = string.Empty;
        public static string context = string.Empty;

        public IActionResult ListManagerList()
        {
            //dropdown
            DataSet ds = DataServices.DataSetFromSQL("SELECT listnamespaceid, listnamespace FROM listsettings.listnamespace ORDER BY listnamespace");
            ViewBag.ListNamespace = ToSelectList(ds.Tables[0], "listnamespaceid", "listnamespace");

            ListModel listModel = new ListModel();
            string sqlQuestion = "SELECT * FROM listsettings.question ORDER BY questionquickname;";
            var paramListQuestion = new List<KeyValuePair<string, string>>()
            {

            };
            DataSet dsQuestion = DataServices.DataSetFromSQL(sqlQuestion, paramListQuestion);
            listModel.QuestionDto = dsQuestion.Tables[0].ToList<QuestionDto>();

            string sqlCollection = "SELECT * FROM listsettings.questionoptioncollection ORDER BY questionoptioncollectionname;";
            var paramListCollection = new List<KeyValuePair<string, string>>()
            {

            };
            DataSet dsCOllection = DataServices.DataSetFromSQL(sqlCollection, paramListCollection);
            listModel.QuestionCollectioDto = dsCOllection.Tables[0].ToList<QuestionCollectioDto>();


            listModel.EntityId = HttpContext.Session.GetString(SynapseSession.ListId);

            return View(listModel);
        }
        public IActionResult ListManagerNew(string id)
        {
            namespaceId = id;
            context = "new";
            personaListFilters = new List<PersonaListFilter>();
            ListManagerNewModel model = new ListManagerNewModel();
            model.NamespaceId = id;
            model.PersonaListFilters = personaListFilters;
            DataSet dsBaseview = DataServices.DataSetFromSQL("SELECT baseviewnamespaceid, baseviewnamespace FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace");
            ViewBag.BaseviewNamespace = ToSelectList(dsBaseview.Tables[0], "baseviewnamespaceid", "baseviewnamespace");

            DataSet dsDefaultContext = DataServices.DataSetFromSQL("SELECT entityid, synapsenamespacename || '.' || entityname as entitydisplayname, keycolumn FROM entitysettings.entitymanager order by 2");
            ViewBag.DefaultContext = ToSelectList(dsDefaultContext.Tables[0], "entityid", "entitydisplayname");

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "Please Select...",
                Value = "0"
            });
            ViewBag.AttributeList = new SelectList(list, "Value", "Text");
            ViewBag.Persona = new SelectList(list, "Value", "Text");
            List<SelectListItem> listSortOrder = new List<SelectListItem>();
            listSortOrder.Add(new SelectListItem()
            {
                Text = "ASC",
                Value = "asc"
            });
            listSortOrder.Add(new SelectListItem()
            {
                Text = "DESC",
                Value = "desc"
            });
            ViewBag.SortOrderList = new SelectList(listSortOrder, "Value", "Text");
            ViewBag.Summary = SynapseHelpers.GetListNamespaceNameFromID(id);


            return View(model);
        }
        public IActionResult ListOptionCollectionNew(string id)
        {
            return View();
        }
        public IActionResult ListQuestionNew(string id)
        {
            DataSet dsDefaultContext = DataServices.DataSetFromSQL("SELECT entityid, synapsenamespacename || '.' || entityname as entitydisplayname, keycolumn FROM entitysettings.entitymanager order by 2");
            ViewBag.DefaultContext = ToSelectList(dsDefaultContext.Tables[0], "entityid", "entitydisplayname");

            DataSet dsQuestionType = DataServices.DataSetFromSQL("SELECT questiontype_id, questiontypetext, htmltemplate FROM listsettings.questiontype WHERE isenabled = true ORDER BY displayorder;");
            ViewBag.QuestionType = ToSelectList(dsQuestionType.Tables[0], "questiontype_id", "questiontypetext");

            DataSet dsOptionType = DataServices.DataSetFromSQL("SELECT questionoptiontype_id, questionoptiontypename FROM listsettings.questionoptiontype ORDER BY displayorder;");
            ViewBag.OptionType = ToSelectList(dsOptionType.Tables[0], "questionoptiontype_id", "questionoptiontypename");

            DataSet dsOptionCollection = DataServices.DataSetFromSQL("SELECT questionoptioncollection_id, questionoptioncollectionname, questionoptioncollectiondescription FROM listsettings.questionoptioncollection ORDER BY questionoptioncollectionname;");
            ViewBag.OptionCollection = ToSelectList(dsOptionCollection.Tables[0], "questionoptioncollection_id", "questionoptioncollectionname");


            return View();
        }
        [HttpPost]
        public IActionResult ListManagerNewSave(ListManagerNewModel model)
        {
            DataSet dsDefaultContext = DataServices.DataSetFromSQL("SELECT entityid, synapsenamespacename || '.' || entityname as entitydisplayname, keycolumn FROM entitysettings.entitymanager order by 2");
            dsDefaultContext.Tables[0].DefaultView.RowFilter = "entityid='" + model.DefaultContextId + "'";
            DataTable dtDefaultContext = (dsDefaultContext.Tables[0].DefaultView).ToTable();

            string sql = "INSERT INTO listsettings.listmanager(list_id, listname, listdescription, listcontextkey, baseview_id, listnamespaceid, " +
                          "listnamespace, defaultcontext, defaultcontextfield, matchedcontextfield, tablecssstyle, tableheadercssstyle, defaultrowcssstyle, " +
                          "patientbannerfield, rowcssfield, wardpersonacontextfield, clinicalunitpersonacontextfield, specialtypersonacontextfield, teampersonacontextfield, snapshottemplateline1, " +
                          "snapshottemplateline2, snapshottemplatebadge, defaultsortcolumn, defaultsortorder, datecontextfield) " +
                          "VALUES (@list_id, @listname, @listdescription, @listcontextkey, @baseview_id, @listnamespaceid, " +
                          "@listnamespace, @defaultcontext, @defaultcontextfield, @matchedcontextfield, @tablecssstyle, @tableheadercssstyle, @defaultrowcssstyle, @patientbannerfield, @rowcssfield, " +
                          "@wardpersonacontextfield, @clinicalunitpersonacontextfield, @specialtypersonacontextfield, @teampersonacontextfield, @snapshottemplateline1, @snapshottemplateline2, @snapshottemplatebadge, " +
                          "@defaultsortcolumn, @defaultsortorder, @datecontextfield)";

            string newId = System.Guid.NewGuid().ToString();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", newId),
                new KeyValuePair<string, string>("listname", model.ListName),
                new KeyValuePair<string, string>("listdescription",  model.ListComments==null ? "" : model.ListComments),
                new KeyValuePair<string, string>("listcontextkey", ""),
                new KeyValuePair<string, string>("baseview_id", model.BaseViewId),
                new KeyValuePair<string, string>("listnamespaceid", model.NamespaceId),
                new KeyValuePair<string, string>("listnamespace", SynapseHelpers.GetListNamespaceNameFromID(model.NamespaceId)),
                new KeyValuePair<string, string>("defaultcontext", model.DefaultContextId),
                new KeyValuePair<string, string>("defaultcontextfield", dtDefaultContext.Rows[0]["entitydisplayname"].ToString()),
                new KeyValuePair<string, string>("matchedcontextfield", model.MatchContextFieldId),
                new KeyValuePair<string, string>("patientbannerfield", model.PatientBannerFieldId),
                new KeyValuePair<string, string>("rowcssfield", model.RowCSSFieldId),
                new KeyValuePair<string, string>("tablecssstyle", model.TableClass),
                new KeyValuePair<string, string>("tableheadercssstyle", model.TableHeaderClass),
                new KeyValuePair<string, string>("defaultrowcssstyle", model.DefaultTableRowCSS),
                new KeyValuePair<string, string>("wardpersonacontextfield", model.WardPersonaContextFieldId == "0" ? null : model.WardPersonaContextFieldId),
                new KeyValuePair<string, string>("clinicalunitpersonacontextfield", model.CUPersonaContextFieldId == "0" ? null : model.CUPersonaContextFieldId),
                new KeyValuePair<string, string>("specialtypersonacontextfield", model.SpecialtyPersonaContextFieldId == "0" ? null : model.SpecialtyPersonaContextFieldId),
                new KeyValuePair<string, string>("teampersonacontextfield", model.TeamPersonaContextFieldId == "0" ? null : model.TeamPersonaContextFieldId),
                new KeyValuePair<string, string>("snapshottemplateline1", model.SnapshotLine1Id == "0" ? null : model.SnapshotLine1Id),
                new KeyValuePair<string, string>("snapshottemplateline2", model.SnapshotLine2Id == "0" ? null : model.SnapshotLine2Id),
                new KeyValuePair<string, string>("snapshottemplatebadge", model.SnapshotBadgeId == "0" ? null : model.SnapshotBadgeId),
                new KeyValuePair<string, string>("defaultsortcolumn", model.DefaultSortColumnId == "0" ? null : model.DefaultSortColumnId),
                // Don't save defaultsortorder if defaultsortcolumn is not selected
                new KeyValuePair<string, string>("defaultsortorder", model.DefaultSortColumnId == "0" ? null : model.DefaultSortOrderId),
                new KeyValuePair<string, string>("datecontextfield", model.DateContextField == "0" ? null : model.DateContextField)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            ListAddTeminusFilter(newId);
            return Json(newId);
        }
        public void ListAddTeminusFilter(string list_id)
        {
            for (int i = 0; i < personaListFilters.Count; i++)
            {
                string sqlAddfilter = "INSERT INTO entitystorematerialised.meta_listcontexts(listcontexts_id,persona_id, field, list_id) " +

                            "VALUES (@listcontexts_id, @persona_id, @field, @list_id) ";
                string newId = System.Guid.NewGuid().ToString();
                var paramListAddlist = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listcontexts_id", newId),
                new KeyValuePair<string, string>("persona_id",personaListFilters[i].persona_id.ToString()),
                new KeyValuePair<string, string>("field", personaListFilters[i].field.ToString()),
                new KeyValuePair<string, string>("list_id",list_id)
                };
                DataServices.ExcecuteNonQueryFromSQL(sqlAddfilter, paramListAddlist);
            }
        }
        public void ListUpdateTeminusFilter(string list_id)
        {

            string sql = "delete	FROM entitystorematerialised.meta_listcontexts WHERE list_id = @list_id;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", list_id)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

            for (int i = 0; i < personaListFilters.Count; i++)
            {

                string sqlAddfilter = "INSERT INTO entitystorematerialised.meta_listcontexts(listcontexts_id,persona_id, field, list_id) " +

                            "VALUES (@listcontexts_id, @persona_id, @field, @list_id) ";

                string newId = System.Guid.NewGuid().ToString();
                var paramListAddlist = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listcontexts_id", newId),
                new KeyValuePair<string, string>("persona_id",personaListFilters[i].persona_id.ToString()),
                new KeyValuePair<string, string>("field", personaListFilters[i].field.ToString()),
                new KeyValuePair<string, string>("list_id",list_id)    };

                DataServices.ExcecuteNonQueryFromSQL(sqlAddfilter, paramListAddlist);
            }
        }
        [HttpPost]
        public IActionResult ListQuestionNewSave(ListQuestionNewModel model)
        {
            DataSet dsDefaultContext = DataServices.DataSetFromSQL("SELECT entityid, synapsenamespacename || '.' || entityname as entitydisplayname, keycolumn FROM entitysettings.entitymanager order by 2");
            dsDefaultContext.Tables[0].DefaultView.RowFilter = "entityid='" + model.DefaultContextId + "'";
            DataTable dtDefaultContext = (dsDefaultContext.Tables[0].DefaultView).ToTable();

            DataSet dsQuestionType = DataServices.DataSetFromSQL("SELECT questiontype_id, questiontypetext, htmltemplate FROM listsettings.questiontype WHERE isenabled = true ORDER BY displayorder;");
            dsQuestionType.Tables[0].DefaultView.RowFilter = "questiontype_id='" + model.QuestionTypeId + "'";
            DataTable dtQuestionType = (dsQuestionType.Tables[0].DefaultView).ToTable();

            string sql = "INSERT INTO listsettings.question(question_id, defaultcontext, defaultcontextfieldname, questiontype_id, questiontypetext, labeltext, defaultvaluetext, defaultvaluedatetime, questionquickname, questionview_id, questionviewname, questionviewsql, optiontype, questionoptioncollection_id, questionoptionsqlstatement, questioncustomhtml, questioncustomhtmlalt) values (@question_id, @defaultcontext, @defaultcontextfieldname, @questiontype_id, @questiontypetext, @labeltext, @defaultvaluetext, null, @questionquickname, @questionview_id, @questionviewname, @questionviewsql, @optiontype, @questionoptioncollection_id, @questionoptionsqlstatement, @questioncustomhtml, @questioncustomhtmlalt);";
            string question_id = System.Guid.NewGuid().ToString();
            string defaultcontext = model.DefaultContextId;
            string defaultcontextfieldname = dtDefaultContext.Rows[0]["entitydisplayname"].ToString();
            string questiontype_id = model.QuestionTypeId;
            string questiontypetext = dtQuestionType.Rows[0]["questiontypetext"].ToString();
            string labeltext = "";
            string questioncustomhtml = "";
            string defaultvaluetext = "";
            string questioncustomhtmlalt = "";

            if (model.QuestionTypeId != "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf") //"Not HTML Tag (Label, Custom HTML)"       3
            {

                defaultvaluetext = model.DefaultValueText ?? "";
                labeltext = model.LabelText;

            }

            if (model.QuestionTypeId == "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf") //"HTML Tag (Label, Custom HTML)"       3
            {
                questioncustomhtml = model.CustomHTML;
            }

            if (model.QuestionTypeId == "164c31d5-d32e-4c97-91d6-a0d01822b9b6" || model.QuestionTypeId == "221ca4a0-3a39-42ff-a0f4-885ffde0f0bd")  //"Single Checkbox (Binary)" or "Checkbox Image (Binary)" 
            {
                questioncustomhtml = model.CustomHTML;
                questioncustomhtmlalt = model.CustomHTMLAlt;
            }



            string questionquickname = model.QuickName;
            string questionview_id = "";
            string questionviewname = "";
            string questionviewsql = "";


            string optiontype = "";
            string questionoptioncollection_id = "";
            string questionoptionsqlstatement = "";


            if (model.QuestionTypeId == "fc1f2643-b491-4889-8d1a-910619b65722" ||
                 model.QuestionTypeId == "3d236e17-e40e-472d-95a5-5e45c5e02faf" ||
                 model.QuestionTypeId == "4f31c02d-fa36-4033-8977-8f25bef33d52" ||
                 model.QuestionTypeId == "ca1f1b24-b490-4e57-8921-9f680819e47c" ||
                 model.QuestionTypeId == "71490eff-a54b-455a-86b1-a4d5ab676f32"
                 )
            // "Drop Down List"                      4
            // "Check Box List"                      5
            // "Auto-complete Selection List"        8
            // "Radio Button List"
            // "Radio Button Image List"
            {
                optiontype = model.OptionTypeId;
                if (model.OptionTypeId == "e9e6feda-f02d-4388-8c5b-9fc97558c684")//Internal Option Collection
                {
                    questionoptioncollection_id = model.OptionCollectionId;
                }


                if (model.OptionTypeId == "638dadd6-fca7-4f9b-b25f-692c45172524") //Custom SQL Statement
                {
                    questionoptionsqlstatement = model.OptionSQLStatement;
                }
            }


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("question_id", question_id),
                new KeyValuePair<string, string>("defaultcontext", defaultcontext),
                new KeyValuePair<string, string>("defaultcontextfieldname", defaultcontextfieldname),
                new KeyValuePair<string, string>("questiontype_id", questiontype_id),
                new KeyValuePair<string, string>("questiontypetext", questiontypetext),
                new KeyValuePair<string, string>("labeltext", labeltext),
                new KeyValuePair<string, string>("questioncustomhtml", questioncustomhtml),
                new KeyValuePair<string, string>("questionquickname", questionquickname),
                new KeyValuePair<string, string>("defaultvaluetext", defaultvaluetext),
                new KeyValuePair<string, string>("questionview_id", questionview_id),
                new KeyValuePair<string, string>("questionviewname", questionviewname),
                new KeyValuePair<string, string>("questionviewsql", questionviewsql),
                new KeyValuePair<string, string>("optiontype", optiontype),
                new KeyValuePair<string, string>("questionoptioncollection_id", questionoptioncollection_id),
                new KeyValuePair<string, string>("questionoptionsqlstatement", questionoptionsqlstatement),
                new KeyValuePair<string, string>("questioncustomhtmlalt", questioncustomhtmlalt)
            };

            DataServices.executeSQLStatement(sql, paramList);
            return Json("OK");
        }
        [HttpPost]
        public IActionResult ListOptionCollectionNewSave(ListOptionCollectionNewModel model)
        {
            string sql = "INSERT INTO listsettings.questionoptioncollection(questionoptioncollection_id, questionoptioncollectionname, questionoptioncollectiondescription) VALUES (@questionoptioncollection_id, @questionoptioncollectionname, @questionoptioncollectiondescription);";

            string id = System.Guid.NewGuid().ToString();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoptioncollection_id", id),
                new KeyValuePair<string, string>("questionoptioncollectionname", model.CollectionName),
                new KeyValuePair<string, string>("questionoptioncollectiondescription", model.CollectionDescription==null ? "" : model.CollectionDescription)
            };
            DataServices.executeSQLStatement(sql, paramList);
            return Json("OK");
        }
        public IActionResult ListManagerViewUpdate(ListManagerNewModel model)
        {
            DataSet dsDefaultContext = DataServices.DataSetFromSQL("SELECT entityid, synapsenamespacename || '.' || entityname as entitydisplayname, keycolumn FROM entitysettings.entitymanager order by 2");
            dsDefaultContext.Tables[0].DefaultView.RowFilter = "entityid='" + model.DefaultContextId + "'";
            DataTable dtDefaultContext = (dsDefaultContext.Tables[0].DefaultView).ToTable();

            string sql = "UPDATE listsettings.listmanager SET " +
                         "listname = @listname, " +
                         "listdescription = @listdescription, " +
                         "matchedcontextfield = @matchedcontextfield, " +
                         "tablecssstyle = @tablecssstyle, " +
                         "tableheadercssstyle = @tableheadercssstyle, " +
                         "defaultrowcssstyle = @defaultrowcssstyle, " +
                         "patientbannerfield = @patientbannerfield, " +
                         "rowcssfield = @rowcssfield, " +
                         "wardpersonacontextfield = @wardpersonacontextfield, " +
                         "clinicalunitpersonacontextfield = @clinicalunitpersonacontextfield, " +
                         "specialtypersonacontextfield = @specialtypersonacontextfield, " +
                         "teampersonacontextfield = @teampersonacontextfield, " +
                         "snapshottemplateline1 = @snapshottemplateline1, " +
                         "snapshottemplateline2 = @snapshottemplateline2, " +
                         "snapshottemplatebadge = @snapshottemplatebadge, " +
                         "defaultsortcolumn = @defaultsortcolumn, " +
                         "defaultsortorder = @defaultsortorder, " +
                         "datecontextfield = @datecontextfield " +
                         "WHERE list_id = @list_id;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", model.ListId),
                new KeyValuePair<string, string>("listname", model.ListName),
                new KeyValuePair<string, string>("listdescription", model.ListComments==null ? "" : model.ListComments),
                new KeyValuePair<string, string>("matchedcontextfield", model.MatchContextFieldId),
                new KeyValuePair<string, string>("patientbannerfield", model.PatientBannerFieldId),
                new KeyValuePair<string, string>("rowcssfield", model.RowCSSFieldId),
                new KeyValuePair<string, string>("tablecssstyle", model.TableClass),
                new KeyValuePair<string, string>("tableheadercssstyle", model.TableHeaderClass),
                new KeyValuePair<string, string>("defaultrowcssstyle", model.DefaultTableRowCSS),
                new KeyValuePair<string, string>("wardpersonacontextfield", model.WardPersonaContextFieldId== "0" ? null : model.WardPersonaContextFieldId),
                new KeyValuePair<string, string>("clinicalunitpersonacontextfield", model.CUPersonaContextFieldId == "0" ? null : model.CUPersonaContextFieldId),
                new KeyValuePair<string, string>("specialtypersonacontextfield", model.SpecialtyPersonaContextFieldId == "0" ? null : model.SpecialtyPersonaContextFieldId),
                new KeyValuePair<string, string>("teampersonacontextfield", model.TeamPersonaContextFieldId == "0" ? null : model.TeamPersonaContextFieldId),
                new KeyValuePair<string, string>("snapshottemplateline1", model.SnapshotLine1Id == "0" ? null : model.SnapshotLine1Id),
                new KeyValuePair<string, string>("snapshottemplateline2", model.SnapshotLine2Id == "0" ? null : model.SnapshotLine2Id),
                new KeyValuePair<string, string>("snapshottemplatebadge", model.SnapshotBadgeId == "0" ? null : model.SnapshotBadgeId),
                new KeyValuePair<string, string>("defaultsortcolumn", model.DefaultSortColumnId == "0" ? null : model.DefaultSortColumnId),
                // Don't save defaultsortorder if defaultsortcolumn is not selected
                new KeyValuePair<string, string>("defaultsortorder", model.DefaultSortOrderId == "0" ? null : model.DefaultSortOrderId),
                new KeyValuePair<string, string>("datecontextfield", model.DateContextField == "0" ? null : model.DateContextField)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            ListUpdateTeminusFilter(model.ListId);
            return Json("OK");
        }
        public IActionResult ListOptionCollectionView(string id)
        {
            string sql = "SELECT * FROM listsettings.questionoptioncollection WHERE questionoptioncollection_id = @questionoptioncollection_id;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoptioncollection_id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            ListOptionCollectionNewModel listOptionCollectionNew = new ListOptionCollectionNewModel();
            listOptionCollectionNew.OptionCollectionID = id;
            listOptionCollectionNew.CollectionName = dt.Rows[0]["questionoptioncollectionname"].ToString();
            listOptionCollectionNew.CollectionDescription = dt.Rows[0]["questionoptioncollectiondescription"].ToString();

            string sqlOption = "SELECT questionoption_id, questionoptioncollection_id, questionoptioncollectionname, questionoptioncollectionorder, optionvaluetext, optiondisplaytext, optionflag FROM listsettings.questionoption WHERE questionoptioncollection_id = @questionoptioncollection_id;";

            var paramListOption = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoptioncollection_id", id)
            };


            DataSet dsOption = DataServices.DataSetFromSQL(sqlOption, paramListOption);
            List<ListOptionDto> ListOptionDto = dsOption.Tables[0].ToList<ListOptionDto>();
            listOptionCollectionNew.ListOptionDto = ListOptionDto;
            return View(listOptionCollectionNew);
        }
        public IActionResult ListOptionCollectionViewSave(ListOptionCollectionNewModel model)
        {
            string sql = "UPDATE listsettings.questionoptioncollection SET questionoptioncollectionname = @questionoptioncollectionname, questionoptioncollectiondescription = @questionoptioncollectiondescription WHERE questionoptioncollection_id = @questionoptioncollection_id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoptioncollection_id", model.OptionCollectionID),
                new KeyValuePair<string, string>("questionoptioncollectionname", model.CollectionName),
                new KeyValuePair<string, string>("questionoptioncollectiondescription", model.CollectionDescription==null ? "" : model.CollectionDescription)
            };
            DataServices.executeSQLStatement(sql, paramList);

            return Json("OK");
        }

        public IActionResult ListOptionNew(string id)
        {
            ListOptionNewModel model = new ListOptionNewModel();
            string sql = "SELECT * FROM listsettings.questionoptioncollection WHERE questionoptioncollection_id = @questionoptioncollection_id;";

            var paramList = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("questionoptioncollection_id", id)
                };
            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            model.OptionCollectionID = id;
            model.OptionCollectionText = dt.Rows[0]["questionoptioncollectionname"].ToString();
            return View(model);
        }
        public IActionResult ListOptionNewSave(ListOptionNewModel model)
        {

            string sql = "INSERT INTO listsettings.questionoption (questionoption_id, questionoptioncollection_id, questionoptioncollectionname, questionoptioncollectionorder, optionvaluetext, optiondisplaytext, optionflag, optionflagalt) VALUES (@questionoption_id, @questionoptioncollection_id, @questionoptioncollectionname, CAST(@questionoptioncollectionorder AS INT), @optionvaluetext, @optiondisplaytext, @optionflag, @optionflagalt);";

            string id = System.Guid.NewGuid().ToString();
            int order = 0;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoption_id", id),
                new KeyValuePair<string, string>("questionoptioncollection_id", model.OptionCollectionID),
                new KeyValuePair<string, string>("questionoptioncollectionname", model.OptionCollectionText),
                new KeyValuePair<string, string>("questionoptioncollectionorder", order.ToString()),
                new KeyValuePair<string, string>("optionvaluetext", model.OptionValueText),
                new KeyValuePair<string, string>("optiondisplaytext", model.OptionDisplayText),
                new KeyValuePair<string, string>("optionflag", model.OptionFlag?? ""),
                new KeyValuePair<string, string>("optionflagalt", model.OptionFlagAlt?? ""),
            };

            DataServices.executeSQLStatement(sql, paramList);
            return Json(model.OptionCollectionID);
        }
        public IActionResult ListOptionView(string id)
        {
            string sql = "SELECT * FROM listsettings.questionoption WHERE questionoption_id = @questionoption_id;";
            ListOptionNewModel model = new ListOptionNewModel();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoption_id", id)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            model.OptionCollectionID = dt.Rows[0]["questionoptioncollection_id"].ToString();
            model.OptionID = id;
            try
            {
                model.OptionValueText = dt.Rows[0]["optionvaluetext"].ToString();
            }
            catch { }

            try
            {
                model.OptionDisplayText = dt.Rows[0]["optiondisplaytext"].ToString();
            }
            catch { }

            try
            {
                model.OptionFlag = dt.Rows[0]["optionflag"].ToString();
            }
            catch { }

            try
            {
                model.OptionFlagAlt = dt.Rows[0]["optionflagalt"].ToString();
            }
            catch { }

            try
            {
                model.OptionCollectionText = dt.Rows[0]["questionoptioncollectionname"].ToString();
            }
            catch { }

            return View(model);
        }
        public IActionResult ListOptionViewSave(ListOptionNewModel model)
        {
            string sql = "UPDATE listsettings.questionoption  SET  optionvaluetext = @optionvaluetext, optiondisplaytext = @optiondisplaytext, optionflag = @optionflag, optionflagalt = @optionflagalt WHERE questionoption_id = @questionoption_id;";

            string id = model.OptionID;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoption_id", id),
                new KeyValuePair<string, string>("optionvaluetext", model.OptionValueText),
                new KeyValuePair<string, string>("optiondisplaytext", model.OptionDisplayText),
                new KeyValuePair<string, string>("optionflag", model.OptionFlag?? ""),
                new KeyValuePair<string, string>("optionflagalt", model.OptionFlagAlt?? ""),
            };
            DataServices.executeSQLStatement(sql, paramList);

            return Json(model.OptionCollectionID);
        }
        public IActionResult ListOptionDelete(string id, string optionCollection)
        {
            string sql = "DELETE FROM listsettings.questionoption WHERE questionoption_id = @questionoption_id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("questionoption_id", id)
            };
            DataServices.executeSQLStatement(sql, paramList);
            return Json(optionCollection);
        }
        public IActionResult ListQuestionView(string id)
        {
            ListQuestionNewModel model = new ListQuestionNewModel();
            DataSet dsDefaultContext = DataServices.DataSetFromSQL("SELECT entityid, synapsenamespacename || '.' || entityname as entitydisplayname, keycolumn FROM entitysettings.entitymanager order by 2");
            ViewBag.DefaultContext = ToSelectList(dsDefaultContext.Tables[0], "entityid", "entitydisplayname");

            DataSet dsQuestionType = DataServices.DataSetFromSQL("SELECT questiontype_id, questiontypetext, htmltemplate FROM listsettings.questiontype WHERE isenabled = true ORDER BY displayorder;");
            ViewBag.QuestionType = ToSelectList(dsQuestionType.Tables[0], "questiontype_id", "questiontypetext");

            DataSet dsOptionType = DataServices.DataSetFromSQL("SELECT questionoptiontype_id, questionoptiontypename FROM listsettings.questionoptiontype ORDER BY displayorder;");
            ViewBag.OptionType = ToSelectList(dsOptionType.Tables[0], "questionoptiontype_id", "questionoptiontypename");

            DataSet dsOptionCollection = DataServices.DataSetFromSQL("SELECT questionoptioncollection_id, questionoptioncollectionname, questionoptioncollectiondescription FROM listsettings.questionoptioncollection ORDER BY questionoptioncollectionname;");
            ViewBag.OptionCollection = ToSelectList(dsOptionCollection.Tables[0], "questionoptioncollection_id", "questionoptioncollectionname");

            ViewBag.PreviewURL = SynapseHelpers.GetEBoardURL() + "ListPreview.aspx?id=" + id;

            string sql = "SELECT * FROM listsettings.question WHERE question_id = @question_id;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("question_id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);

            DataTable dt = ds.Tables[0];
            model.QuestionID = id;
            try
            {
                model.CustomHTML = dt.Rows[0]["questioncustomhtml"].ToString();
            }
            catch
            {
            }


            try
            {
                model.CustomHTMLAlt = dt.Rows[0]["questioncustomhtmlalt"].ToString();
            }
            catch
            {
            }

            try
            {
                model.DefaultContextFieldName = dt.Rows[0]["defaultcontextfieldname"].ToString();
            }
            catch
            {
            }

            try
            {
                model.DefaultValueText = dt.Rows[0]["defaultvaluetext"].ToString();
            }
            catch { }

            try
            {
                model.LabelText = dt.Rows[0]["labeltext"].ToString();
            }
            catch { }

            try
            {
                model.OptionSQLStatement = dt.Rows[0]["questionoptionsqlstatement"].ToString();
            }
            catch { }


            try
            {
                model.QuickName = dt.Rows[0]["questionquickname"].ToString();
            }
            catch { }



            try
            {
                model.DefaultContextId = dt.Rows[0]["defaultcontext"].ToString();

            }
            catch { }

            try
            {
                model.OptionCollectionId = dt.Rows[0]["questionoptioncollection_id"].ToString();

            }
            catch { }

            try
            {
                model.OptionTypeId = dt.Rows[0]["optiontype"].ToString();

            }
            catch { }


            try
            {
                model.QuestionTypeId = dt.Rows[0]["questiontype_id"].ToString();

            }
            catch { }


            return View(model);
        }
        [HttpPost]
        public IActionResult ListQuestionViewSave(ListQuestionNewModel model)
        {
            string sql = "UPDATE listsettings.question SET defaultcontext = @defaultcontext, defaultcontextfieldname = @defaultcontextfieldname, questiontype_id = @questiontype_id, questiontypetext = @questiontypetext, labeltext = @labeltext, defaultvaluetext = @defaultvaluetext, defaultvaluedatetime = null, questionquickname = @questionquickname, questionview_id = @questionview_id, questionviewname = @questionviewname, questionviewsql = @questionviewsql, optiontype = @optiontype, questionoptioncollection_id = @questionoptioncollection_id, questionoptionsqlstatement = @questionoptionsqlstatement, questioncustomhtml = @questioncustomhtml, questioncustomhtmlalt = @questioncustomhtmlalt WHERE question_id= @question_id;";


            DataSet dsDefaultContext = DataServices.DataSetFromSQL("SELECT entityid, synapsenamespacename || '.' || entityname as entitydisplayname, keycolumn FROM entitysettings.entitymanager order by 2");
            dsDefaultContext.Tables[0].DefaultView.RowFilter = "entityid='" + model.DefaultContextId + "'";
            DataTable dtDefaultContext = (dsDefaultContext.Tables[0].DefaultView).ToTable();

            DataSet dsQuestionType = DataServices.DataSetFromSQL("SELECT questiontype_id, questiontypetext, htmltemplate FROM listsettings.questiontype WHERE isenabled = true ORDER BY displayorder;");
            dsQuestionType.Tables[0].DefaultView.RowFilter = "questiontype_id='" + model.QuestionTypeId + "'";
            DataTable dtQuestionType = (dsQuestionType.Tables[0].DefaultView).ToTable();

            //, , , , , , , , , , , , , , 

            string question_id = model.QuestionID;
            string defaultcontext = model.DefaultContextId;
            string defaultcontextfieldname = dtDefaultContext.Rows[0]["entitydisplayname"].ToString(); ;
            string questiontype_id = model.QuestionTypeId;
            string questiontypetext = dtQuestionType.Rows[0]["questiontypetext"].ToString();
            string labeltext = "";
            string defaultvaluetext = "";
            string questioncustomhtml = "";
            string questioncustomhtmlalt = "";

            if (model.QuestionTypeId != "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf") //"Not HTML Tag (Label, Custom HTML)"       3
            {

                defaultvaluetext = model.DefaultValueText;
                labeltext = model.LabelText;

            }

            if (model.QuestionTypeId == "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf") //"HTML Tag (Label, Custom HTML)"       3
            {
                questioncustomhtml = model.CustomHTML;
            }

            if (model.QuestionTypeId == "164c31d5-d32e-4c97-91d6-a0d01822b9b6" || model.QuestionTypeId == "221ca4a0-3a39-42ff-a0f4-885ffde0f0bd")  //"Single Checkbox (Binary)" or "Checkbox Image (Binary)" 
            {
                questioncustomhtml = model.CustomHTML;
                questioncustomhtmlalt = model.CustomHTMLAlt;
            }

            string questionquickname = model.QuickName;
            string questionview_id = "";
            string questionviewname = "";
            string questionviewsql = "";


            string optiontype = "";
            string questionoptioncollection_id = "";
            string questionoptionsqlstatement = "";

            if (model.QuestionTypeId == "fc1f2643-b491-4889-8d1a-910619b65722" ||
                 model.QuestionTypeId == "3d236e17-e40e-472d-95a5-5e45c5e02faf" ||
                 model.QuestionTypeId == "4f31c02d-fa36-4033-8977-8f25bef33d52" ||
                 model.QuestionTypeId == "ca1f1b24-b490-4e57-8921-9f680819e47c" ||
                 model.QuestionTypeId == "71490eff-a54b-455a-86b1-a4d5ab676f32"
                 )
            // "Drop Down List"                      4
            // "Check Box List"                      5
            // "Auto-complete Selection List"        8
            // "Radio Button List"
            // "Radio Button Image List"
            {
                optiontype = model.OptionTypeId;
                if (model.OptionTypeId == "e9e6feda-f02d-4388-8c5b-9fc97558c684")//Internal Option Collection
                {
                    questionoptioncollection_id = model.OptionCollectionId;
                }


                if (model.OptionTypeId == "638dadd6-fca7-4f9b-b25f-692c45172524") //Custom SQL Statement
                {
                    questionoptionsqlstatement = model.OptionSQLStatement;
                }
            }


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("question_id", question_id),
                new KeyValuePair<string, string>("defaultcontext", defaultcontext),
                new KeyValuePair<string, string>("defaultcontextfieldname", defaultcontextfieldname),
                new KeyValuePair<string, string>("questiontype_id", questiontype_id),
                new KeyValuePair<string, string>("questiontypetext", questiontypetext),
                new KeyValuePair<string, string>("labeltext", labeltext),
                new KeyValuePair<string, string>("questioncustomhtml", questioncustomhtml),
                new KeyValuePair<string, string>("questionquickname", questionquickname),
                new KeyValuePair<string, string>("defaultvaluetext", defaultvaluetext),
                new KeyValuePair<string, string>("questionview_id", questionview_id),
                new KeyValuePair<string, string>("questionviewname", questionviewname),
                new KeyValuePair<string, string>("questionviewsql", questionviewsql),
                new KeyValuePair<string, string>("optiontype", optiontype),
                new KeyValuePair<string, string>("questionoptioncollection_id", questionoptioncollection_id),
                new KeyValuePair<string, string>("questionoptionsqlstatement", questionoptionsqlstatement),
                new KeyValuePair<string, string>("questioncustomhtmlalt", questioncustomhtmlalt)
            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {

            }

            return Json("OK");
        }
        public IActionResult ListManagerView(string id)
        {
            context = "edit";
            ViewBag.PreviewURL = SynapseHelpers.GetEBoardURL() + "ListPreview.aspx?id=" + id;
            ViewBag.Id = id;
            DataSet dsBaseview = DataServices.DataSetFromSQL("SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace");
            ViewBag.BaseviewNamespace = ToSelectList(dsBaseview.Tables[0], "baseviewnamespaceid", "baseviewnamespace");

            DataSet dsDefaultContext = DataServices.DataSetFromSQL("SELECT entityid, synapsenamespacename || '.' || entityname as entitydisplayname, keycolumn FROM entitysettings.entitymanager order by 2");
            ViewBag.DefaultContext = ToSelectList(dsDefaultContext.Tables[0], "entityid", "entitydisplayname");


            personaListFilters = new List<PersonaListFilter>();
            string pesonaListsql = "SELECT mp.displayname,lc.persona_id, field, list_id	FROM entitystorematerialised.meta_listcontexts lc join entitystorematerialised.meta_persona mp on mp.persona_id=lc.persona_id WHERE list_id = @list_id";
            var paramListPesona = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", id)
            };

            DataSet dsPesonaList = DataServices.DataSetFromSQL(pesonaListsql, paramListPesona);
            foreach (DataRow row in dsPesonaList.Tables[0].Rows)
            {
                personaListFilters.Add(new PersonaListFilter()
                {
                    field = row["field"].ToString(),
                    displayname = row["displayname"].ToString(),
                    list_id = id,
                    persona_id = row["persona_id"].ToString()
                });
            }

            ViewBag.Persona = new SelectList(new List<SelectListItem>(), "Value", "Text");

            List<SelectListItem> listSortOrder = new List<SelectListItem>();
            listSortOrder.Add(new SelectListItem()
            {
                Text = "ASC",
                Value = "asc"
            });
            listSortOrder.Add(new SelectListItem()
            {
                Text = "DESC",
                Value = "desc"
            });
            ViewBag.SortOrderList = new SelectList(listSortOrder, "Value", "Text");
            ViewBag.Summary = SynapseHelpers.GetListNameFromID(id);

            string sql = "SELECT * FROM listsettings.v_listdetailsummary WHERE list_id = @list_id ORDER BY orderby";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", id)
            };
            ListManagerNewModel model = new ListManagerNewModel();
            model.NamespaceId = id;
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            List<ListDetailDto> listDetailDtos = ds.Tables[0].ToList<ListDetailDto>();
            model.ListDetailDto = listDetailDtos;

            string sqlDetail = "SELECT * FROM listsettings.listmanager WHERE list_id = @list_id;";
            var paramListDetail = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id",id)
            };
            DataSet dsData = DataServices.DataSetFromSQL(sqlDetail, paramListDetail);

            DataTable dt = dsData.Tables[0];
            // attrbute list
            string sqlAtt = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id ORDER BY attributename;";
            var paramListAtt = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", dt.Rows[0]["baseview_id"].ToString())
            };
            DataSet AttribueList = DataServices.DataSetFromSQL(sqlAtt, paramListAtt);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "Please Select...",
                Value = "0"
            });
            list.AddRange(ToSelectList(AttribueList.Tables[0], "attributename", "attributename"));
            ViewBag.AttributeList = new SelectList(list, "Value", "Text");

            //Date Context
            string sqlDateContext = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id AND datatype IN ('date','timestamp with time zone','timestamp without time zone') ORDER BY attributename;";
            var paramDateContext = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", dt.Rows[0]["baseview_id"].ToString())
            };
            DataSet DateContextList = DataServices.DataSetFromSQL(sqlDateContext, paramDateContext);
            List<SelectListItem> dateContextlist = new List<SelectListItem>();
            dateContextlist.Add(new SelectListItem()
            {
                Text = "Please Select...",
                Value = "0"
            });
            dateContextlist.AddRange(ToSelectList(DateContextList.Tables[0], "attributename", "attributename"));
            ViewBag.DateContextList = new SelectList(dateContextlist, "Value", "Text");

            model.ListId = id;
            model.BaseViewId = dt.Rows[0]["baseview_id"].ToString();
            model.ListName = dt.Rows[0]["listname"].ToString();
            model.DefaultContextId = dt.Rows[0]["defaultcontext"].ToString();
            model.DateContextField = dt.Rows[0]["datecontextfield"].ToString();

            try
            {
                model.ListComments = dt.Rows[0]["listdescription"].ToString();
            }
            catch { }

            try
            {
                model.DefaultTableRowCSS = dt.Rows[0]["defaultrowcssstyle"].ToString();
            }
            catch { }

            try
            {
                model.TableClass = dt.Rows[0]["tablecssstyle"].ToString();
            }
            catch { }

            try
            {
                model.TableHeaderClass = dt.Rows[0]["tableheadercssstyle"].ToString();
            }
            catch { }

            try
            {
                model.MatchContextFieldId = dt.Rows[0]["matchedcontextfield"].ToString();
            }
            catch { }

            try
            {
                model.PatientBannerFieldId = dt.Rows[0]["patientbannerfield"].ToString();

            }
            catch { }

            try
            {
                model.RowCSSFieldId = dt.Rows[0]["rowcssfield"].ToString();

            }
            catch { }

            try
            {
                model.WardPersonaContextFieldId = dt.Rows[0]["wardpersonacontextfield"].ToString();

            }
            catch { }

            try
            {
                model.CUPersonaContextFieldId = dt.Rows[0]["clinicalunitpersonacontextfield"].ToString();
            }
            catch { }

            try
            {
                model.SpecialtyPersonaContextFieldId = dt.Rows[0]["specialtypersonacontextfield"].ToString();

            }
            catch { }

            try
            {
                model.TeamPersonaContextFieldId = dt.Rows[0]["teampersonacontextfield"].ToString();

            }
            catch { }

            try
            {
                model.SnapshotLine1Id = dt.Rows[0]["snapshottemplateline1"].ToString();
            }
            catch { }

            try
            {
                model.SnapshotLine2Id = dt.Rows[0]["snapshottemplateline2"].ToString();
            }
            catch { }

            try
            {
                model.SnapshotBadgeId = dt.Rows[0]["snapshottemplatebadge"].ToString();
            }
            catch { }

            try
            {
                model.DefaultSortColumnId = dt.Rows[0]["defaultsortcolumn"].ToString();
            }
            catch { }

            try
            {
                model.DefaultSortOrderId = dt.Rows[0]["defaultsortorder"].ToString();
            }
            catch { }
            model.PersonaListFilters = personaListFilters;
            return View(model);
        }
        public IActionResult ListSelectAttributes(string id)
        {
            ViewBag.PreviewURL = SynapseHelpers.GetEBoardURL() + "ListPreview.aspx?id=" + id;
            ViewBag.Summary = SynapseHelpers.GetListNameFromID(id);
            ViewBag.Id = id;
            return View();
        }
        public IActionResult ListSelectQuestions(string id)
        {
            ViewBag.PreviewURL = SynapseHelpers.GetEBoardURL() + "ListPreview.aspx?id=" + id;
            ViewBag.Summary = SynapseHelpers.GetListNameFromID(id);
            ViewBag.Id = id;
            DataTable dtt = SynapseHelpers.GetListDT(id);
            try
            {
                ViewBag.ContextField = dtt.Rows[0]["defaultcontextfield"].ToString();
            }
            catch { }

            try
            {
                ViewBag.DefaultContext = dtt.Rows[0]["defaultcontext"].ToString();
            }
            catch { }

            return View();
        }
        public IActionResult ListAPIs(string id)
        {
            ViewBag.PreviewURL = SynapseHelpers.GetEBoardURL() + "ListPreview.aspx?id=" + id;
            ViewBag.Summary = SynapseHelpers.GetListNameFromID(id);
            ViewBag.Id = id;
            return View();
        }
        public IActionResult GetAvailableAttributes(string listId)
        {

            string sql = @"SELECT
                            bva.baseviewattribute_id, baseviewnamespaceid, baseviewnamespace, baseview_id, baseviewname, bva.attributename, datatype, ordinalposition, case when sa.baseviewattribute_id is null then false else true end as isselected
                            FROM listsettings.baseviewattribute bva
                            LEFT OUTER JOIN 
                            (SELECT baseviewattribute_id, attributename FROM listsettings.listattribute WHERE list_id = @list_id AND COALESCE(isselected,false) = true) sa
                            ON bva.attributename = sa.attributename                            
                            WHERE baseview_ID IN (SELECT baseview_id FROM listsettings.listmanager WHERE list_id = @list_id) ORDER BY bva.attributename";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId)
            };
            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
            DataTable dt = ds.Tables[0];
            List<AvailableAttribute> lst = ds.Tables[0].ToList<AvailableAttribute>();
            return Json(lst);
        }
        public IActionResult GetSelectedAttributes(string listId)
        {

            string sql = @"SELECT
                            la.listattribute_id, la.list_id, la.baseviewattribute_id, la.attributename, la.datatype, la.displayname, la.ordinalposition, la.defaultcssclassname
                            FROM listsettings.listattribute la
                            INNER JOIN listsettings.baseviewattribute bva
                            ON la.attributename = bva.attributename
                            WHERE la.list_id = @list_id
                            AND COALESCE(isselected, false) = true
                            AND bva.baseview_ID IN (SELECT baseview_id FROM listsettings.listmanager WHERE list_id = @list_id)
                            ORDER BY la.ordinalposition";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId)
            };

            DataSet ds = new DataSet();

            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return null;
            }

            DataTable dt = ds.Tables[0];

            List<SelectedAttribute> lst = ds.Tables[0].ToList<SelectedAttribute>();
            return Json(lst);

        }

        public int GetNextOrdinalPosition(string listId)
        {

            string sql = @"SELECT
                           max(la.ordinalposition) as MaxOrdinalPosition
                            FROM listsettings.listattribute la
                            WHERE la.list_id = @list_id
                            AND COALESCE(isselected, false) = true";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId)
            };

            DataSet ds = new DataSet();

            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return 1;
            }

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                return 1;
            }

            int ret = 0;
            try
            {
                ret = System.Convert.ToInt16(dt.Rows[0]["MaxOrdinalPosition"].ToString());
            }
            catch { }

            ret += 1;

            return ret;

        }
        public int SaveQuickAttributes(string listId, string listattribute_id, string displayname, string defaultcssclassname)
        {

            string sql = @"UPDATE listsettings.listattribute
                            SET displayname = @displayname,
                            defaultcssclassname = @defaultcssclassname
                            WHERE listattribute_id = @listattribute_id AND list_id = @list_id
                            ";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listattribute_id", listattribute_id),
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("displayname", displayname),
                new KeyValuePair<string, string>("defaultcssclassname", defaultcssclassname),
            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return 0;
            }


            return 1;

        }
        public string AddAttributeToList(string listId, string attributename, int ordinalposition)
        {


            string sqlCheck = "SELECT COUNT(*) AS recCount FROM listsettings.listattribute WHERE list_id= @list_id AND attributename = @attributename;";
            var paramListCheck = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("attributename", attributename),
            };
            DataSet dsCheck = DataServices.DataSetFromSQL(sqlCheck, paramListCheck);
            DataTable dtCheck = new DataTable();
            dtCheck = dsCheck.Tables[0];
            int recs = 0;
            try
            {
                recs = System.Convert.ToInt16(dtCheck.Rows[0]["recCount"].ToString());
            }
            catch { }

            string sql = "";

            if (recs == 0) //Do insert
            {
                sql = @"INSERT INTO listsettings.listattribute(listattribute_id, list_id, baseviewattribute_id, attributename, datatype, ordinalposition)
                        SELECT uuid_generate_v4() AS listattribute_id, @list_id, baseviewattribute_id, @attributename, datatype, CAST(@ordinalposition AS INT) AS ordinalposition
                        FROM listsettings.baseviewattribute WHERE attributename = @attributename
                        AND baseview_id IN (SELECT baseview_id FROM listsettings.listmanager WHERE list_id = @list_id);";
            }
            else //Do update
            {
                sql = @"UPDATE listsettings.listattribute
                        SET isselected = true,
                            ordinalposition = CAST(@ordinalposition AS INT)
                        WHERE list_id = @list_id
                        AND attributename = @attributename;";
            }

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("attributename", attributename),
                new KeyValuePair<string, string>("ordinalposition", ordinalposition.ToString()),
            };
            DataServices.executeSQLStatement(sql, paramList);
            return "Attribute added";

        }
        public string RemoveAttributeFromList(string listId, string attributename)
        {


            string sqlCheck = "SELECT ordinalposition FROM listsettings.listattribute WHERE list_id= @list_id AND attributename = @attributename;";
            var paramListCheck = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("attributename", attributename),
            };
            DataSet dsCheck = DataServices.DataSetFromSQL(sqlCheck, paramListCheck);
            DataTable dtCheck = new DataTable();
            dtCheck = dsCheck.Tables[0];
            int ordinalposition = 0;
            try
            {
                ordinalposition = System.Convert.ToInt16(dtCheck.Rows[0]["ordinalposition"].ToString());
            }
            catch (Exception ex)
            {
                var a = ex;
            }

            string sql = "";



            sql = @"UPDATE listsettings.listattribute
                        SET isselected = false,
                            ordinalposition = null
                        WHERE list_id = @list_id
                        AND attributename = @attributename;


                    UPDATE listsettings.listattribute
                        SET ordinalposition = ordinalposition - 1
                        WHERE list_id = @list_id
                        AND ordinalposition > CAST(@ordinalposition AS INT);

            ";


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("attributename", attributename),
                new KeyValuePair<string, string>("ordinalposition", ordinalposition.ToString()),
            };
            DataServices.executeSQLStatement(sql, paramList);
            return "Attribute removed";

        }
        public string UpdateOrdinalPosition(string listId, string listattribute_id, int ordinalposition)
        {


            string sql = "UPDATE listsettings.listattribute SET ordinalposition = CAST(@ordinalposition AS int) WHERE list_id= @list_id AND listattribute_id = @listattribute_id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("listattribute_id", listattribute_id),
                new KeyValuePair<string, string>("ordinalposition", ordinalposition.ToString())
            };
            DataServices.executeSQLStatement(sql, paramList);
            return "Ordinal Position Updated";

        }

        public List<AvailableQuestion> GetAvailableQuestions(string listId, string defaultContext)
        {

            string sql = @"SELECT 
                            q.question_id, 
                            '<strong>' || questionquickname || '</strong><br />' || questiontypetext || '<br />' || coalesce(labeltext, 'HTML Snippet' ) as questiondisplay,
                            case when lq.question_id is null then false else true end as isselected
                            FROM listsettings.question q
                            LEFT OUTER JOIN (
                            SELECT 
	                            lq.list_id,
	                            lq.question_id
	                            FROM listsettings.listmanager lm
	                            LEFT OUTER JOIN listsettings.listquestion lq	
	                            ON lm.list_id = lq.list_id
	                            WHERE lm.list_id = @list_id
                                AND isselected = true
	                            ) lq
                            ON q.question_id = lq.question_id
                            WHERE defaultcontext = @defaultcontext
                            ORDER BY 2
                            ";


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("defaultcontext", defaultContext)
            };

            DataSet ds = new DataSet();

            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return null;
            }

            DataTable dt = ds.Tables[0];
            List<AvailableQuestion> lst = ds.Tables[0].ToList<AvailableQuestion>();

            return lst;

        }
        public List<SelectedQuestion> GetSelectedQuestions(string listId)
        {

            string sql = @"SELECT 
                                lq.listquestion_id,
	                            lq.list_id,
	                            lq.question_id,
                                '<strong>' || questionquickname || '</strong><br />' || questiontypetext || '<br />' || coalesce(labeltext, 'HTML Snippet' ) as questiondisplay                                
	                            FROM listsettings.listmanager lm
	                            INNER JOIN listsettings.listquestion lq	
	                            ON lm.list_id = lq.list_id
                                LEFT OUTER JOIN listsettings.question q
                                ON lq.question_id = q.question_id
	                            WHERE lm.list_id = @list_id
                                AND COALESCE(isselected, false) = true
                                ORDER BY lq.ordinalposition";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId)
            };

            DataSet ds = new DataSet();

            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return null;
            }

            DataTable dt = ds.Tables[0];

            List<SelectedQuestion> lst = ds.Tables[0].ToList<SelectedQuestion>();
            return lst;

        }
        public string AddQuestionToList(string listId, string question_id, int ordinalposition)
        {


            string sqlCheck = "SELECT COUNT(*) AS recCount FROM listsettings.listquestion WHERE list_id= @list_id AND question_id = @question_id;";
            var paramListCheck = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("question_id", question_id),
            };
            DataSet dsCheck = DataServices.DataSetFromSQL(sqlCheck, paramListCheck);
            DataTable dtCheck = new DataTable();
            dtCheck = dsCheck.Tables[0];
            int recs = 0;
            try
            {
                recs = System.Convert.ToInt16(dtCheck.Rows[0]["recCount"].ToString());
            }
            catch { }

            string sql = "";

            if (recs == 0) //Do insert
            {
                sql = @"INSERT INTO listsettings.listquestion(listquestion_id, list_id, question_id, ordinalposition)
                        SELECT uuid_generate_v4() AS listquestion_id, @list_id, question_id, CAST(@ordinalposition AS INT) AS ordinalposition
                        FROM listsettings.question WHERE question_id = @question_id;";
            }
            else //Do update
            {
                sql = @"UPDATE listsettings.listquestion
                        SET isselected = true,
                            ordinalposition = CAST(@ordinalposition AS INT)
                        WHERE list_id = @list_id
                        AND question_id = @question_id;";
            }

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("question_id", question_id),
                new KeyValuePair<string, string>("ordinalposition", ordinalposition.ToString()),
            };

            DataServices.executeSQLStatement(sql, paramList);
            return "Question added";

        }
        public string RemoveQuestionFromList(string listId, string question_id)
        {


            string sqlCheck = "SELECT ordinalposition FROM listsettings.listquestion WHERE list_id= @list_id AND question_id = @question_id;";
            var paramListCheck = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("question_id", question_id),
            };
            DataSet dsCheck = DataServices.DataSetFromSQL(sqlCheck, paramListCheck);
            DataTable dtCheck = new DataTable();
            dtCheck = dsCheck.Tables[0];
            int ordinalposition = 0;
            try
            {
                ordinalposition = System.Convert.ToInt16(dtCheck.Rows[0]["ordinalposition"].ToString());
            }
            catch (Exception ex)
            {
                var a = ex;
            }

            string sql = "";



            sql = @"UPDATE listsettings.listquestion
                        SET isselected = false,
                            ordinalposition = null
                        WHERE list_id = @list_id
                        AND question_id = @question_id;


                    UPDATE listsettings.listquestion
                        SET ordinalposition = ordinalposition - 1
                        WHERE list_id = @list_id
                        AND ordinalposition > CAST(@ordinalposition AS INT);

            ";


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("question_id", question_id),
                new KeyValuePair<string, string>("ordinalposition", ordinalposition.ToString()),
            };
            DataServices.executeSQLStatement(sql, paramList);

            return "Question removed";

        }
        public int GetNextOrdinalPositionQuestion(string listId)
        {
            string sql = @"SELECT
                           max(la.ordinalposition) as MaxOrdinalPosition
                            FROM listsettings.listquestion la
                            WHERE la.list_id = @list_id
                            AND COALESCE(isselected, false) = true";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId)
            };

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return 1;
            }
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                return 1;
            }
            int ret = 0;
            try
            {
                ret = System.Convert.ToInt16(dt.Rows[0]["MaxOrdinalPosition"].ToString());
            }
            catch { }
            ret += 1;
            return ret;

        }
        public string UpdateOrdinalPositionQuestion(string listId, string listquestion_id, int ordinalposition)
        {
            string sql = "UPDATE listsettings.listquestion SET ordinalposition = CAST(@ordinalposition AS int) WHERE list_id= @list_id AND listquestion_id = @listquestion_id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("listquestion_id", listquestion_id),
                new KeyValuePair<string, string>("ordinalposition", ordinalposition.ToString())
            };
            DataServices.executeSQLStatement(sql, paramList);
            return "Ordinal Position Updated";
        }


        public JsonResult QuestionTypeJsonList(string id)
        {
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @id ORDER BY baseviewname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };
            DataSet dsEntity = DataServices.DataSetFromSQL(sql, paramList);
            return Json(dsEntity.Tables[0]);
        }
        public JsonResult BaseViewJsonList(string id)
        {
            try
            {
                string sql = "SELECT baseview_id, baseviewname FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @id ORDER BY baseviewname;";
                var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };
                DataSet dsEntity = DataServices.DataSetFromSQL(sql, paramList);
                return Json(dsEntity.Tables[0]);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        public JsonResult BaseViewContextFieldsJson(string id)
        {
            string sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id ORDER BY attributename;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            PersonaField = DataServices.DataSetFromSQL(sql, paramList);

            return Json(PersonaField.Tables[0]);
        }
        public JsonResult DateContextJsonList(string id)
        {
            string sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id AND datatype IN ('date','timestamp with time zone','timestamp without time zone') ORDER BY attributename; ";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet DateContext = DataServices.DataSetFromSQL(sql, paramList);

            return Json(DateContext.Tables[0]);
        }
        public JsonResult BaseViewFieldsJson(string id)
        {
            var commaDelimited = string.Join(",", personaListFilters.Select(i => "'" + i.field + "'"));
            
            string sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id and attributename NOT IN(" + commaDelimited + ") ORDER BY attributename;";
            
            if (string.IsNullOrWhiteSpace(commaDelimited))
                sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id ORDER BY attributename;";
            
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };
            
            DataSet dsEntity = DataServices.DataSetFromSQL(sql, paramList);

            return Json(ToSelectList(dsEntity.Tables[0], "attributename", "attributename"));
        }
        public JsonResult PesonaJson()
        {
            var commaDelimited = string.Join(",", personaListFilters.Select(i => "'" + i.persona_id + "'"));
            string sql = "SELECT persona_id, displayname, personaname	FROM entitystorematerialised.meta_persona where persona_id NOT IN(" + commaDelimited + ");";
            if (string.IsNullOrWhiteSpace(commaDelimited))
                sql = "SELECT persona_id, displayname, personaname	FROM entitystorematerialised.meta_persona;";
            var paramList = new List<KeyValuePair<string, string>>()
            {

            };
            DataSet dsPesona = DataServices.DataSetFromSQL(sql, paramList);
            return Json(ToSelectList(dsPesona.Tables[0], "persona_id", "displayname"));
        }
        public PartialViewResult AddPersona(string id, string dispayname, string baseview, string list_id)
        {
            PersonaListFilter personaListFilter = new PersonaListFilter();
            personaListFilter.persona_id = id;
            personaListFilter.displayname = dispayname;
            personaListFilter.field = baseview;
            personaListFilter.list_id = list_id;
            personaListFilters.Add(personaListFilter);
            return PartialView("_ListPersona", personaListFilters);
        }
        public PartialViewResult DeletePersona(string id)
        {
            PersonaListFilter personaListFilter = personaListFilters.Where(e => e.persona_id == id).FirstOrDefault();
            if (personaListFilter != null)
                personaListFilters.Remove(personaListFilter);
            return PartialView("_ListPersona", personaListFilters);
        }
        public PartialViewResult GetListManager(string id)
        {
            HttpContext.Session.SetString(SynapseSession.ListId, id);
            string sql = "SELECT * FROM listsettings.listmanager WHERE listnamespaceid = @listnamespaceid ORDER BY listname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listnamespaceid", id)
            };


            DataSet dsList = DataServices.DataSetFromSQL(sql, paramList);
            List<ListDto> ListDto = dsList.Tables[0].ToList<ListDto>();
            return PartialView("_ListManagerList", ListDto);
        }
        [NonAction]
        public SelectList ToSelectList(DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        #region "List namespance"
        public IActionResult ListNamespace()
        {
            string sql = "SELECT * FROM listsettings.listnamespace ORDER BY listnamespace; ";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            ListNamespaceModel listNamespaceModel = new ListNamespaceModel();
            List<ListNamespaceDto> listNamespaceDto = ds.Tables[0].ToList<ListNamespaceDto>();
            listNamespaceModel.ListNamespaceDto = listNamespaceDto;
            return View(listNamespaceModel);
        }
        [HttpPost]
        public IActionResult ListNamespaceSave(ListNamespaceModel model)
        {
            string sql = "INSERT INTO listsettings.listnamespace(_createdsource, listnamespace, listnamespacedescription, _createdby) VALUES ('Synapse Studio', @listnamespace, @listnamespacedescription, @p_username);";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listnamespace", model.ListNamespace),
                new KeyValuePair<string, string>("listnamespacedescription", model.ListNamespaceDescription==null ? "" : model.ListNamespaceDescription),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName))

            };
            DataServices.executeSQLStatement(sql, paramList);
            return Json("OK");
        }
        public PartialViewResult GetListNamespaceList()
        {
            string sql = "SELECT * FROM listsettings.listnamespace ORDER BY listnamespace; ";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            ListNamespaceModel listNamespaceModel = new ListNamespaceModel();
            List<ListNamespaceDto> listNamespaceDto = ds.Tables[0].ToList<ListNamespaceDto>();
            return PartialView("_ListNamespace", listNamespaceDto);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyListNamespace(string listNamespace)
        {
            listNamespace = listNamespace == null ? "" : listNamespace;
            string sql = "SELECT * FROM listsettings.listnamespace WHERE listnamespace = @listnamespace;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listnamespace", listNamespace)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Json("There is already a list namespace with that name");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyListName(string listName)
        {
            if (context == "new")
            {
                listName = listName == null ? "" : listName;
                string sql = "SELECT * FROM listsettings.listmanager WHERE listnamespaceid = @listnamespaceid and listname = @listname;";
                var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listnamespaceid", namespaceId),
                new KeyValuePair<string, string>("listname", listName)
                };

                DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return Json("There is already a list called " + listName);
                }
            }

            return Json(true);
        }

        #endregion
    }
}

