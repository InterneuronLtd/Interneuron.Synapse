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
    public class BaseViewController : Controller
    {
        public static string namespaceId = string.Empty;
        public IActionResult BaseViewManagerList()
        {
            //dropdown
            DataSet ds = DataServices.DataSetFromSQL("SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace");
            ViewBag.BaseViewNamespaceList = ToSelectList(ds.Tables[0], "baseviewnamespaceid", "baseviewnamespace");
            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.Baseview_id = HttpContext.Session.GetString(SynapseSession.BaseViewId);
            return View(baseViewModel);
        }
        public PartialViewResult GetBaseViewList(string id)
        {
            HttpContext.Session.SetString(SynapseSession.BaseViewId, id);
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @baseviewnamespaceid ORDER BY baseviewname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseviewnamespaceid", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            List<BaseViewListDto> BaseViewListDto = ds.Tables[0].ToList<BaseViewListDto>();
            return PartialView("_BaseViewManagerList", BaseViewListDto);
        }
        public IActionResult BaseViewManagerNew(string id)
        {
            namespaceId = id;
            BaseViewModel model = new BaseViewModel();
            model.BaseviewNamespaceName = SynapseHelpers.GetBaseviewNamespaceNameFromID(namespaceId);
            return View(model);
        }
        [HttpPost]
        public IActionResult BaseViewManagerNewSave(BaseViewModel model, string command)
        {
            if (command == "Validate")
            {
                string tmpViewName = Guid.NewGuid().ToString("N");
                //Check if we can create a temporary view using the supplied SQL Statement
                string sqlTempCreate = "CREATE VIEW baseviewtemp." + model.BaseviewNamespaceName + "_" + tmpViewName + " AS " + model.BaseviewSQL + ";";
                var paramTempListCreate = new List<KeyValuePair<string, string>>()
                {
                };

                try
                {
                    DataServices.executeSQLStatement(sqlTempCreate, paramTempListCreate);
                }
                catch (Exception ex)
                {
                    return Json("Error creating view: " + ex.ToString());
                }
                return Json("OK");
            }

            string sqlCreate = "CREATE VIEW baseviewcore." + model.BaseviewNamespaceName + "_" + model.BaseviewName + " AS " + model.BaseviewSQL + ";";
            var paramListCreate = new List<KeyValuePair<string, string>>()
            {
            };
            DataServices.executeSQLStatement(sqlCreate, paramListCreate);
            string sql = "SELECT listsettings.createbaseview(@p_baseviewnamespaceid, @p_baseviewnamespace, @p_baseviewname, @p_baseviewdescription, @p_baseviewsqlstatement, @p_username);";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_baseviewnamespaceid", namespaceId),
                new KeyValuePair<string, string>("p_baseviewnamespace", SynapseHelpers.GetBaseviewNamespaceNameFromID(namespaceId)),
                new KeyValuePair<string, string>("p_baseviewname", model.BaseviewName),
                new KeyValuePair<string, string>("p_baseviewdescription", model.BaseviewDesc==null ? "" : model.BaseviewDesc),
                new KeyValuePair<string, string>("p_baseviewsqlstatement", model.BaseviewSQL),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName))

            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            string newGuid = dt.Rows[0][0].ToString();
            return Json(newGuid);
        }
        public IActionResult BaseViewManagerView(string id)
        {
            ViewBag.Id = id;
            namespaceId = id;
            string sql = "SELECT * FROM listsettings.v_baseviewdetailsummary WHERE baseview_id = @baseview_id ORDER BY orderby";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseview_id", id)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.BaseviewNamespaceName = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
            List<BaseViewDetailDto> BaseViewDetailDto = ds.Tables[0].ToList<BaseViewDetailDto>();
            baseViewModel.BaseViewDetailDto = BaseViewDetailDto;
            return View(baseViewModel);
        }
        public IActionResult BaseViewManagerDelete(string baseViewName)
        {
            string sql = @"SELECT listsettings.dropbaseview(
	                    @p_baseview_id, 
	                    @p_baseviewname
                    )";
            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_baseview_id", namespaceId),
                new KeyValuePair<string, string>("p_baseviewname", baseViewName)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

            return RedirectToAction("BaseViewManagerList");
        }
        public IActionResult BaseViewManagerReCreate()
        {
            string sql = @"SELECT listsettings.recreatebaseview(
	                    @p_baseview_id
                    )";
            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_baseview_id", namespaceId),
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }
        public IActionResult BaseViewManagerSQL(string id)
        {
            ViewBag.Id = id;
            namespaceId = id;
            BaseViewSQLModel model = new BaseViewSQLModel();
            DataTable dt = SynapseHelpers.GetBaseviewDTByID(id);
            model.SQL = dt.Rows[0]["baseviewsqlstatement"].ToString();
            model.Summary = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
            model.NamespaceId = SynapseHelpers.GetBaseviewNameSpaceIDFromBaseViewID(id);
            model.NextOrdinalPosition = SynapseHelpers.GetNextOrdinalPositionFromID(id);
            model.ViewName = SynapseHelpers.GetBaseviewNameFromID(id);
            model.NamespaceName = SynapseHelpers.GetBaseviewNameSpaceNameFromBaseViewID(id);
            model.BaseViewComments = SynapseHelpers.GetBaseviewCommentsFromBaseViewID(id);
            return View(model);
        }
        public IActionResult BaseViewManagerSQLValidate(BaseViewSQLModel model, string command)
        {
            if (command == "Re-Validate")
            {
                string tmpViewName = Guid.NewGuid().ToString("N");
                //Check if we can create a temporary view using the supplied SQL Statement
                string sqlValidate = "CREATE VIEW baseviewtemp." + model.NamespaceName + "_" + tmpViewName + " AS " + model.SQL + ";";
                var paramListvalidate = new List<KeyValuePair<string, string>>()
                {
                };
                try
                {
                    DataServices.executeSQLStatement(sqlValidate, paramListvalidate);
                }
                catch (Exception ex)
                {
                    return Json("Error creating view: " + ex.Message);
                }
                return Json(command);
            }
            string sqlDrop = @"SELECT listsettings.dropbaseview(
	                    @p_baseview_id, 
	                    @p_baseviewname
                    )";

            var paramListDrop = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_baseview_id", namespaceId),
                new KeyValuePair<string, string>("p_baseviewname", model.Summary)
            };

            DataServices.ExcecuteNonQueryFromSQL(sqlDrop, paramListDrop);


            //Recreate the baseview
            //string tmpViewName = Guid.NewGuid().ToString("N");
            //Check if we can create a temporary view using the supplied SQL Statement
            string sqlCreate = "CREATE VIEW baseviewcore." + model.NamespaceName + "_" + model.ViewName + " AS " + model.SQL + ";";
            var paramListCreate = new List<KeyValuePair<string, string>>()
            {
            };

            try
            {
                DataServices.executeSQLStatement(sqlCreate, paramListCreate);
            }
            catch (Exception ex)
            {
                return Json("Error creating view: " + ex.Message);
            }
            string baseviewid = namespaceId;

            string sql = "SELECT listsettings.recreatebaseview(@p_baseviewnamespaceid, @p_baseviewnamespace, @p_baseviewname, @p_baseviewdescription, @p_baseviewsqlstatement, @p_username, @baseviewid);";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_baseviewnamespaceid", model.NamespaceId),
                new KeyValuePair<string, string>("p_baseviewnamespace", model.NamespaceName),
                new KeyValuePair<string, string>("p_baseviewname", model.ViewName),
                new KeyValuePair<string, string>("p_baseviewdescription",  model.BaseViewComments==null ? "" : model.BaseViewComments),
                new KeyValuePair<string, string>("p_baseviewsqlstatement", model.SQL),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("baseviewid", baseviewid),

            };
            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return Json("Error creating view: " + ex.Message);
            }
            DataTable dt = ds.Tables[0];
            string newGuid = dt.Rows[0][0].ToString();
            return Json(command + "|" + newGuid);
        }
        public IActionResult BaseViewManagerAPI(string id)
        {
            ViewBag.Id = id;
            namespaceId = id;
            BaseviewAPI aPIModel = new BaseviewAPI();
            DataTable dt = SynapseHelpers.GetBaseviewDTByID(id);

            ViewBag.Summary = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
            ViewBag.NextOrdinalPosition = SynapseHelpers.GetNextOrdinalPositionFromID(id);

            string apiURL = SynapseHelpers.GetAPIURL();
            aPIModel.GetList = apiURL + "/GetBaseViewList/" + SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
            aPIModel.GetListByAttribute = apiURL + "/GetBaseViewListObjectByAttribute/" + SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id) + "?synapseattributename={synapseattributename}&attributevalue={attributevalue}";
            aPIModel.PostObject = apiURL + "/GetBaseViewListByPost/" + SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
            return View(aPIModel);
        }
        public IActionResult BaseViewManagerAttribute(string id)
        {
            ViewBag.Id = id;
            namespaceId = id;
            string sql = "SELECT * FROM listsettings.baseviewattribute WHERE baseview_id = @baseview_id ORDER BY ordinalposition;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseview_id", id)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            ViewBag.BaseviewNamespaceName = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
            List<BaseViewAttributeDto> BaseViewAttributeDto = ds.Tables[0].ToList<BaseViewAttributeDto>();
            return View(BaseViewAttributeDto);
        }
        public IActionResult BaseviewNamespace()
        {
            string sql = "SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace; ";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            BaseviewNamespaceModel baseviewNamespaceModel = new BaseviewNamespaceModel();
            List<BaseviewNamespaceDto> baseviewNamespaceDto = ds.Tables[0].ToList<BaseviewNamespaceDto>();
            baseviewNamespaceModel.BaseviewNamespaceDto = baseviewNamespaceDto;
            return View(baseviewNamespaceModel);
        }
        [HttpPost]
        public IActionResult BaseviewNamespaceSave(BaseviewNamespaceModel model)
        {
            string sql = "INSERT INTO listsettings.baseviewnamespace(_createdsource, baseviewnamespace, baseviewnamespacedescription, _createdby) VALUES ('Synapse Studio', @baseviewnamespace, @baseviewnamespacedescription, @p_username);";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseviewnamespace", model.BaseviewNamespace),
                new KeyValuePair<string, string>("baseviewnamespacedescription", model.BaseviewNamespaceDescription==null ? "" : model.BaseviewNamespaceDescription),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName))

            };
            DataServices.executeSQLStatement(sql, paramList);
            return Json("OK");
        }
        public PartialViewResult GetBaseviewNamespaceList()
        {
            string sql = "SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace; ";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            BaseviewNamespaceModel baseviewNamespaceModel = new BaseviewNamespaceModel();
            List<BaseviewNamespaceDto> baseviewNamespaceDto = ds.Tables[0].ToList<BaseviewNamespaceDto>();
            return PartialView("_BaseviewNamespace", baseviewNamespaceDto);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyBaseviewNamespace(string baseviewNamespace)
        {
            baseviewNamespace = baseviewNamespace == null ? "" : baseviewNamespace;
            string sql = "SELECT * FROM listsettings.baseviewnamespace WHERE baseviewnamespace = @baseviewnamespace;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseviewnamespace", baseviewNamespace)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Json("There is already a baseview namespace with that name");
            }

            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyBaseview(string baseviewname)
        {
            baseviewname = baseviewname == null ? "" : baseviewname;
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @baseviewnamespaceid and baseviewname = @baseviewname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseviewnamespaceid", namespaceId),
                new KeyValuePair<string, string>("baseviewname", baseviewname)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Json("The name of the baseview that you have entered already exists");
            }

            return Json(true);
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
    }
}
