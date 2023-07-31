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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using NToastNotify;
using Serilog;
using Microsoft.AspNetCore.Mvc.Filters;
using NHapi.Model.V25.Segment;
using Microsoft.Extensions.DependencyInjection;
using SynapseStudioWeb.AppCode.Filters;
using System.Net;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class EntityManagerController : Controller
    {
        public EntityManagerController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
        }
        public static string namespaceId = string.Empty;
        private IToastNotification toastNotification;

        public static string localNamespaceIDSelected = string.Empty;

        public IActionResult EntityManagerList()
        {
            //dropdown
            DataSet ds = DataServices.DataSetFromSQL("SELECT * FROM entitysettings.synapsenamespace ORDER BY synapsenamespacename");
            ViewBag.SynapseNamespaceList = ToSelectList(ds.Tables[0], "SynapseNamespaceId", "SynapseNamespaceName");
            EntityModel entityModel = new EntityModel();
            entityModel.EntityId = HttpContext.Session.GetString(SynapseSession.EntityId);
            return View(entityModel);
        }
        public IActionResult EntityManagerNew(string id)
        {
            namespaceId = id;
            ViewBag.Name = SynapseHelpers.GetNamespaceNameFromID(id);
            return View();
        }
        public IActionResult EntityManagerExtendedNew(string id)
        {
            namespaceId = id;

            string sql = "SELECT entityid, entityname FROM entitysettings.entitymanager WHERE synapsenamespaceid = 'd8851db1-68f8-45ee-be9a-628666512431' ORDER BY entityname;";
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ViewBag.SynapseEntityList = ToSelectList(ds.Tables[0], "entityname", "entityname");

            return View();
        }
        public IActionResult EntityManagerMetaNew(string id)
        {
            namespaceId = id;
            return View();
        }
        public IActionResult EntityManagerLocalNew(string id)
        {
            namespaceId = id;
            //dropdown            
            string sql = "SELECT localnamespaceid, localnamespacename FROM entitysettings.localnamespace order by localnamespacename;";
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ViewBag.SynapseNamespaceList = ToSelectList(ds.Tables[0], "localnamespaceid", "localnamespacename");
            return View();
        }
        public IActionResult EntityManagerView(string id)
        {
            ViewBag.Id = id;
            namespaceId = id;
            string sql = "SELECT * FROM entitysettings.v_entitydetailsummary WHERE entityid = @entityid ORDER BY orderby";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", id)
            };
            ViewBag.Summary = SynapseHelpers.GetEntityNameAndNamespaceFromID(id);
            DataSet dsEntityDetail = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dtBVs = SynapseHelpers.GetEntityBaseviewsDT(id);
            DetailModel detailModel = new DetailModel();
            List<DetailDto> detailDto = dsEntityDetail.Tables[0].ToList<DetailDto>();
            List<BaseViewDto> baseViewDto = dtBVs.ToList<BaseViewDto>();
            detailModel.DetailDto = detailDto;
            detailModel.BaseViewDto = baseViewDto;
            return View(detailModel);
        }
        public IActionResult RelationView(string id)
        {
            ViewBag.Id = namespaceId;
            ViewBag.RelationId = id;
            ViewBag.Summary = SynapseHelpers.GetAttributeNameFromAttributeID(id);
            string sql = "SELECT * FROM entitysettings.v_attributedetailsummary WHERE attributeid = @attributeid ORDER BY orderby";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("attributeid", id)
            };
            DataSet dsEntityDetail = DataServices.DataSetFromSQL(sql, paramList);

            List<DetailDto> detailDto = dsEntityDetail.Tables[0].ToList<DetailDto>();

            return View(detailDto);
        }
        public IActionResult RelationDelete(string id)
        {
            ViewBag.Id = namespaceId;
            ViewBag.RelationId = id;
            string sql = @"SELECT entitysettings.droprelationfromentity(
                                @p_attributeid,
	                            @p_entityid
                            )";
            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_attributeid", id),
                new KeyValuePair<string, string>("p_entityid", namespaceId)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return RedirectToAction("EntityManagerRelation", new { id = namespaceId });
        }
        public IActionResult EntityManagerDelete()
        {
            string sql = @"SELECT entitysettings.dropentityfrommodelbyid(
	                            @p_entityid
                            )";



            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_entityid", namespaceId)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

            return RedirectToAction("EntityManagerList");
        }
        public IActionResult EntityManagerAttribute(string id)
        {
            ViewBag.Id = id;
            namespaceId = id;
            DataSet ds = DataServices.DataSetFromSQL("SELECT * FROM entitysettings.systemdatatype WHERE availabletoenduser = true ORDER BY orderby");
            ViewBag.SystemDataTypeList = ToSelectList(ds.Tables[0], "DataTypeID", "DataTypeDisplay");
            ViewBag.Summary = SynapseHelpers.GetEntityNameAndNamespaceFromID(id);
            string sql = "SELECT * FROM entitysettings.v_entityattribute WHERE entityid = @entityid ORDER BY ordinal_position;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", id)
            };

            DataSet dsAttribute = DataServices.DataSetFromSQL(sql, paramList);
            AttributeModel attributeModel = new AttributeModel();
            List<AttributeDto> attributeDto = dsAttribute.Tables[0].ToList<AttributeDto>();
            attributeModel.AttributeDtos = attributeDto;

            return View(attributeModel);
        }
        public IActionResult EntityManagerAttributeView(string id)
        {

            namespaceId = SynapseHelpers.GetEntityIDFromAttributeID(id);
            ViewBag.Id = namespaceId;
            ViewBag.AttributeId = id;
            ViewBag.Heading = SynapseHelpers.GetAttributeNameFromAttributeID(id);
            ViewBag.AttributeName = SynapseHelpers.GetAttributeNameFromAttributeID(id);

            ViewBag.Summary = SynapseHelpers.GetEntityNameAndNamespaceFromID(namespaceId);

            string sql = "SELECT * FROM entitysettings.v_attributedetailsummary WHERE attributeid = @attributeid ORDER BY orderby";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("attributeid", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            ViewBag.Error = "";
            foreach (DataRow drow in dt.Rows)
            {
                string colName = drow["entitydetail"].ToString();
                string colVal = drow["entitydescription"].ToString();

                if (colName == "Is Key Attribute")
                {
                    if (colVal == "1")
                    {

                        ViewBag.Error = "Drop Attribute button not available as it is not possible to delete a key attribute";
                    }
                }

                if (colName == "Is System Attribute")
                {
                    if (colVal == "1")
                    {

                        ViewBag.Error = "Drop Attribute button not available as it is not possible to delete a system attribute";
                    }
                }

                if (colName == "Is Relation Attribute")
                {
                    if (colVal == "1")
                    {

                        ViewBag.Error = "Drop Attribute button not available as it is not possible to delete a relation attribute from this screen";
                    }
                }
            }


            DataTable dtBVs = SynapseHelpers.GetEntityBaseviewsDT(namespaceId);
            Int16 bvCount = System.Convert.ToInt16(dtBVs.Rows.Count);
            DetailModel detailModel = new DetailModel();
            List<DetailDto> detailDto = dt.ToList<DetailDto>();
            List<BaseViewDto> baseViewDto = dtBVs.ToList<BaseViewDto>();
            detailModel.DetailDto = detailDto;
            detailModel.BaseViewDto = baseViewDto;
            return View(detailModel);
        }
        public IActionResult EntityManagerAttributeDelete(string id)
        {

            DataTable dtBVs = SynapseHelpers.GetEntityBaseviewsDT(namespaceId);


            string sql = @"SELECT entitysettings.dropattributefromentity(
                                @p_attributeid,
	                            @p_entityid
                            )";



            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_attributeid", id),
                new KeyValuePair<string, string>("p_entityid", namespaceId)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

            foreach (DataRow row in dtBVs.Rows)
            {
                string baseview_id = row["baseview_id"].ToString();
                string sqlRecreate = "SELECT listsettings.recreatebaseview(@baseview_id);";
                var paramListRecreate = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("baseview_id",baseview_id)
                };
                DataServices.ExcecuteNonQueryFromSQL(sqlRecreate, paramListRecreate);

            }

            return RedirectToAction("EntityManagerAttribute", new { id = namespaceId });
        }
        public IActionResult EntityManagerRelation(string id)
        {
            ViewBag.Id = id;
            namespaceId = id;
            DataSet ds = DataServices.DataSetFromSQL("SELECT * FROM entitysettings.synapsenamespace WHERE synapsenamespaceid <> 'e8b78b52-641d-46eb-bb8b-16e2feb86fe7' ORDER BY synapsenamespacename");
            ViewBag.SynapseNamespaceList = ToSelectList(ds.Tables[0], "SynapseNamespaceId", "SynapseNamespaceName");
            ViewBag.EntityList = new SelectList(new List<SelectListItem> { });
            DataSet dsAttribute = DataServices.DataSetFromSQL("SELECT entityid, attributeid, attributename FROM entitysettings.entityattribute WHERE entityid='" + id + "' AND coalesce(isrelationattribute,0) = 0 ORDER BY attributename;");
            ViewBag.AttributeList = ToSelectList(dsAttribute.Tables[0], "AttributeId", "AttributeName");
            ViewBag.Summary = SynapseHelpers.GetEntityNameAndNamespaceFromID(id);
            string sql = "SELECT * FROM entitysettings.v_relationsattributes WHERE entityid = @entityid ORDER BY entityname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", id)
            };

            DataSet dsRelation = DataServices.DataSetFromSQL(sql, paramList);
            RelationModel relationModel = new RelationModel();
            List<RelationDto> relationDto = dsRelation.Tables[0].ToList<RelationDto>();
            relationModel.RelationDto = relationDto;
            return View(relationModel);
        }
        public IActionResult EntityManagerAPI(string id)
        {
            ViewBag.Id = id;
            namespaceId = id;
            ViewBag.Summary = SynapseHelpers.GetEntityNameAndNamespaceFromID(id);
            APIModel aPIModel = new APIModel();
            aPIModel.EntityName = SynapseHelpers.GetEntityNameFromID(id);
            aPIModel.Namespance = SynapseHelpers.GetNamepsaceFromEntityID(id);
            DataTable dtKey = SynapseHelpers.GetEntityKeyAttributeFromID(id);
            aPIModel.KeyAttribute = dtKey.Rows[0][1].ToString();
            string apiURL = SynapseHelpers.GetAPIURL();
            aPIModel.GetList = apiURL + "/GetList" + "?synapsenamespace=" + aPIModel.Namespance + "&synapseentityname=" + aPIModel.EntityName;
            aPIModel.GetObject = apiURL + "/GetObject" + "?synapsenamespace=" + aPIModel.Namespance + "&synapseentityname=" + aPIModel.EntityName + "&id={" + aPIModel.KeyAttribute + "}";
            aPIModel.GetListByAttribute = apiURL + "/GetListByAttribute" + "?synapsenamespace=" + aPIModel.Namespance + "&synapseentityname=" + aPIModel.EntityName + "&synapseattributename={synapseattributename}&attributevalue={attributevalue}";
            aPIModel.PostObject = apiURL + "/PostObject" + "?synapsenamespace=" + aPIModel.Namespance + "&synapseentityname=" + aPIModel.EntityName;
            aPIModel.DeleteObject = apiURL + "/DeleteObject" + "?synapsenamespace=" + aPIModel.Namespance + "&synapseentityname=" + aPIModel.EntityName + "&id={" + aPIModel.KeyAttribute + "}";
            aPIModel.DeleteObjectByAttribute = apiURL + "/DeleteObjectByAttribute" + "?synapsenamespace=" + aPIModel.Namespance + "&synapseentityname=" + aPIModel.EntityName + "&synapseattributename={synapseattributename}&attributevalue={attributevalue}";
            aPIModel.GetObjectHistory = apiURL + "/GetObjectHistory" + "?synapsenamespace=" + aPIModel.Namespance + "&synapseentityname=" + aPIModel.EntityName + "&id={" + aPIModel.KeyAttribute + "}";
            aPIModel.SamplePostJson = SynapseHelpers.GetEntitySampleJSON(id).Rows[0][0].ToString();
            return View(aPIModel);
        }
        public IActionResult EntityManagerModel(string id)
        {
            ViewBag.Id = id;
            namespaceId = id;
            ClassDataModel classDataModel = new ClassDataModel();
            ViewBag.Summary = SynapseHelpers.GetEntityNameAndNamespaceFromID(id);
            string sqlcSharp = "SELECT entitysettings.getcsharpmodel(@entityid);";
            var paramListcSharp = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", id)
            };

            DataSet dsCSharp = DataServices.DataSetFromSQL(sqlcSharp, paramListcSharp);
            DataTable dtcSharp = dsCSharp.Tables[0];
            classDataModel.CSharp = dtcSharp.Rows[0][0].ToString();
            string sql = "SELECT entitysettings.getswiftmodel(@entityid);";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", id)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            classDataModel.Swift = dt.Rows[0][0].ToString();
            return View(classDataModel);
        }
        public IActionResult EntityManagerLocalNamespace()
        {

            string sql = "SELECT * FROM entitysettings.localnamespace ORDER BY localnamespacename; ";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };

            DataSet dsLocalNamespace = DataServices.DataSetFromSQL(sql, paramList);
            LocalNamespaceModel localNamespaceModel = new LocalNamespaceModel();
            List<LocalNamespaceDto> localNamespaceDto = dsLocalNamespace.Tables[0].ToList<LocalNamespaceDto>();
            localNamespaceModel.LocalNamespaceDto = localNamespaceDto;
            return View(localNamespaceModel);
        }


        [HttpPost]
        public IActionResult EntityManagerNewSave(CoreEntityModel model)
        {
            string sql = "SELECT entitysettings.createentityfrommodel(@p_synapsenamespaceid, @p_synapsenamespacename, @p_entityname, @p_entitydescription, @p_username, @p_localnamespaceid, @p_localnamespacename)";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_synapsenamespaceid", namespaceId),
                new KeyValuePair<string, string>("p_synapsenamespacename", SynapseHelpers.GetNamespaceNameFromID(namespaceId)),
                new KeyValuePair<string, string>("p_entityname",model.EntityName),
                new KeyValuePair<string, string>("p_entitydescription", model.EntityDescription==null ? "" : model.EntityDescription),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("p_localnamespaceid", ""),
                new KeyValuePair<string, string>("p_localnamespacename", ""),

            };
            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            string newGuid = dt.Rows[0][0].ToString();
            return Json(newGuid);
        }

        [HttpPost]
        public IActionResult EntityManagerExtendedNewSave(ExtendedEntityModel model)
        {
            string sql = "SELECT entitysettings.createentityfrommodel(@p_synapsenamespaceid, @p_synapsenamespacename, @p_entityname, @p_entitydescription, @p_username, @p_localnamespaceid, @p_localnamespacename)";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_synapsenamespaceid", namespaceId),
                new KeyValuePair<string, string>("p_synapsenamespacename", SynapseHelpers.GetNamespaceNameFromID(namespaceId)),
                new KeyValuePair<string, string>("p_entityname", model.EntityName),
                new KeyValuePair<string, string>("p_entitydescription", model.EntityDescription==null ? "" : model.EntityDescription),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("p_localnamespaceid", ""),
                new KeyValuePair<string, string>("p_localnamespacename", ""),

            };

            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            string newGuid = dt.Rows[0][0].ToString();
            return Json(newGuid);
        }

        [HttpPost]
        public IActionResult EntityManagerMetaNewSave(MetaEntityModel model)
        {
            string sql = "SELECT entitysettings.createentityfrommodel(@p_synapsenamespaceid, @p_synapsenamespacename, @p_entityname, @p_entitydescription, @p_username, @p_localnamespaceid, @p_localnamespacename)";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_synapsenamespaceid", namespaceId),
                new KeyValuePair<string, string>("p_synapsenamespacename", SynapseHelpers.GetNamespaceNameFromID(namespaceId)),
                new KeyValuePair<string, string>("p_entityname", model.EntityName),
                new KeyValuePair<string, string>("p_entitydescription", model.EntityDescription==null ? "" : model.EntityDescription),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("p_localnamespaceid", ""),
                new KeyValuePair<string, string>("p_localnamespacename", ""),

            };

            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            string newGuid = dt.Rows[0][0].ToString();
            return Json(newGuid);
        }
        [HttpPost]
        [StudioExceptionFilter()]
        public IActionResult EntityManagerLocalNewSave(LocalEntityModel model)
        {
            string sql = "SELECT entitysettings.createentityfrommodel(@p_synapsenamespaceid, @p_synapsenamespacename, @p_entityname, @p_entitydescription, @p_username, @p_localnamespaceid, @p_localnamespacename)";
            string localtablename = SynapseHelpers.GetLocalNamespaceNameFromID(model.LocalNamespaceId) + "_" + model.EntityName;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_synapsenamespaceid", namespaceId),
                new KeyValuePair<string, string>("p_synapsenamespacename", SynapseHelpers.GetNamespaceNameFromID(namespaceId)),
                new KeyValuePair<string, string>("p_entityname", localtablename),
                new KeyValuePair<string, string>("p_entitydescription", model.EntityDescription==null ? "" : model.EntityDescription),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("p_localnamespaceid", localNamespaceIDSelected),
                new KeyValuePair<string, string>("p_localnamespacename", SynapseHelpers.GetLocalNamespaceNameFromID(localNamespaceIDSelected)),

            };

            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            string newGuid = dt.Rows[0][0].ToString();
            return Json(newGuid);
        }

        [HttpPost]
        public IActionResult EntityManagerAttributeSave(AttributeModel model)
        {
            DataTable dtBVs = SynapseHelpers.GetEntityBaseviewsDT(namespaceId);

            string dataType = SynapseHelpers.GetDataTypeFromID(model.DataTypeId);
            string dataTypeDisplay = SynapseHelpers.GetDataTypeDisplayFromID(model.DataTypeId);
            string nextOrdinalPosition = SynapseHelpers.GetNextOrdinalPositionFromID(namespaceId);
            string sql = @"SELECT entitysettings.addattributetoentity(
	                            @p_entityid, 
	                            @p_entityname, 
	                            @p_synapsenamespaceid, 
	                            @p_synapsenamespacename, 
	                            @p_username, 
	                            @p_attributename, 
	                            @p_attributedescription, 
	                            @p_datatype, 
	                            @p_datatypeid, 
	                            CAST(@p_ordinal_position AS integer),
	                            @p_attributedefault, 
	                            @p_maximumlength, 
	                            @p_commondisplayname, 
	                            @p_isnullsetting, 
	                            @p_entityversionid
                            )";

            DataTable dt = SynapseHelpers.GetEntityDSFromID(namespaceId);

            string maxLength = "";
            if (dataTypeDisplay == "Short String")
            {
                maxLength = "255";
            }
            if (dataTypeDisplay == "Long String")
            {
                maxLength = "1000";
            }

            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_entityid", namespaceId),
                new KeyValuePair<string, string>("p_entityname", SynapseHelpers.GetEntityNameFromID(namespaceId)),
                new KeyValuePair<string, string>("p_synapsenamespaceid", dt.Rows[0]["synapsenamespaceid"].ToString()),
                new KeyValuePair<string, string>("p_synapsenamespacename", dt.Rows[0]["synapsenamespacename"].ToString()),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("p_attributename", model.AttributeName),
                new KeyValuePair<string, string>("p_attributedescription", "Description"),
                new KeyValuePair<string, string>("p_datatype", dataType),
                new KeyValuePair<string, string>("p_datatypeid", model.DataTypeId),
                new KeyValuePair<string, string>("p_ordinal_position", nextOrdinalPosition),
                new KeyValuePair<string, string>("p_attributedefault", ""),
                new KeyValuePair<string, string>("p_maximumlength", maxLength),
                new KeyValuePair<string, string>("p_commondisplayname", ""),
                new KeyValuePair<string, string>("p_isnullsetting", ""),
                new KeyValuePair<string, string>("p_entityversionid", SynapseHelpers.GetCurrentEntityVersionFromID(namespaceId))
            };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

            foreach (DataRow row in dtBVs.Rows)
            {
                string baseview_id = row["baseview_id"].ToString();
                string sqlRecreate = "SELECT listsettings.recreatebaseview(@baseview_id);";
                var paramListRecreate = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("baseview_id",baseview_id)
                };
                DataServices.ExcecuteNonQueryFromSQL(sqlRecreate, paramListRecreate);

            }
            return Json(namespaceId);
        }

        [HttpPost]
        public IActionResult EntityManagerLocalNamespaceSave(LocalNamespaceModel model)
        {
            string sql = "INSERT INTO entitysettings.localnamespace(_createdsource, localnamespacename, localnamespacedescription, _createdby) VALUES ('Synapse Studio', @localnamespacename, @localnamespacedescription, @p_username);";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("localnamespacename", model.LocalNamespaceName),
                new KeyValuePair<string, string>("localnamespacedescription", model.LocalNamespaceDescription==null ? "" : model.LocalNamespaceDescription),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName))

            };

            DataServices.executeSQLStatement(sql, paramList);
            return Json("OK");
        }
        [HttpPost]
        public IActionResult EntityManagerRelationSave(RelationModel model)
        {
            if (namespaceId == model.EntityId)
            {
                return Json("You are unable to create a relation to the same entity");
            }
            string sqlValidation = "";
            var paramListValidation = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", namespaceId),
                new KeyValuePair<string, string>("parententityid", model.EntityId)
            };
            if (model.AttributeId != "0" && model.AttributeId != "")
            {
                sqlValidation = "SELECT * FROM entitysettings.entityrelation WHERE entityid = @entityid and parententityid = @parententityid;";
            }
            else
            {
                sqlValidation = "SELECT * FROM entitysettings.entityrelation WHERE entityid = @entityid and parententityid = @parententityid and localentityattributeid = @localentityattributeid;";
                paramListValidation.Add(new KeyValuePair<string, string>("localentityattributeid", model.AttributeId));
            }
            DataSet ds = DataServices.DataSetFromSQL(sqlValidation, paramListValidation);
            DataTable dtValidation = ds.Tables[0];
            if (dtValidation.Rows.Count > 0)
            {
                return Json("The relation that you are trying to create already exists for this entity");
            }

            string sql = @"SELECT entitysettings.addrelationtoentity(
	                            @p_entityid, 
	                            @p_entityname, 
	                            @p_synapsenamespaceid, 
	                            @p_synapsenamespacename, 	 
                                @p_parententityid,
                                @p_parententityname,
                                @p_parentsynapsenamespaceid, 
	                            @p_parentsynapsenamespacename, 
                                @p_attributeid,
	                            @p_attributename,                            
	                            CAST(@p_ordinal_position AS integer),
	                            @p_entityversionid,
                                @p_username,
                                @p_localentityattributeid,
                                @p_localentityattributename
                            )";




            DataTable dt = SynapseHelpers.GetEntityDSFromID(namespaceId);
            DataTable dtParent = SynapseHelpers.GetEntityDSFromID(model.EntityId);
            DataTable dtKey = SynapseHelpers.GetEntityKeyAttributeFromID(model.EntityId);
            string entityName = SynapseHelpers.GetEntityNameFromID(model.EntityId);
            string localAttributeName = string.Empty;
            string attributename = dtKey.Rows[0]["attributename"].ToString();
            string nextOrdinalPosition = SynapseHelpers.GetNextOrdinalPositionFromID(namespaceId);
            if (model.AttributeId != "0" && !string.IsNullOrWhiteSpace(model.AttributeId))
            {
                localAttributeName = SynapseHelpers.GetAttributeNameFromAttributeID(model.AttributeId);
                attributename += "_" + localAttributeName;
            }


            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_entityid", namespaceId),
                new KeyValuePair<string, string>("p_entityname", SynapseHelpers.GetEntityNameFromID(namespaceId)),
                new KeyValuePair<string, string>("p_synapsenamespaceid", dt.Rows[0]["synapsenamespaceid"].ToString()),
                new KeyValuePair<string, string>("p_synapsenamespacename", dt.Rows[0]["synapsenamespacename"].ToString()),
                new KeyValuePair<string, string>("p_parententityid", model.EntityId),
                new KeyValuePair<string, string>("p_parententityname", entityName),
                new KeyValuePair<string, string>("p_parentsynapsenamespaceid", dtParent.Rows[0]["synapsenamespaceid"].ToString()),
                new KeyValuePair<string, string>("p_parentsynapsenamespacename", dtParent.Rows[0]["synapsenamespacename"].ToString()),
                new KeyValuePair<string, string>("p_attributeid", dtKey.Rows[0]["attributeid"].ToString()),
                new KeyValuePair<string, string>("p_attributename", attributename),
                new KeyValuePair<string, string>("p_ordinal_position",nextOrdinalPosition),
                new KeyValuePair<string, string>("p_entityversionid", SynapseHelpers.GetCurrentEntityVersionFromID(namespaceId)),
                new KeyValuePair<string, string>("p_username", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("p_localentityattributeid", model.AttributeId),
                new KeyValuePair<string, string>("p_localentityattributename", localAttributeName)
            };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json(namespaceId);
        }
        public PartialViewResult GetEntityList(string synapsenamespaceid)
        {
            HttpContext.Session.SetString(SynapseSession.EntityId, synapsenamespaceid);
            // entity list
            string sql = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid = @synapsenamespaceid ORDER BY entityname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("synapsenamespaceid", synapsenamespaceid)
            };

            DataSet dsEntity = DataServices.DataSetFromSQL(sql, paramList);
            List<CoreEntityModel> EntityModel = dsEntity.Tables[0].ToList<CoreEntityModel>();
            return PartialView("_EntityManagerList", EntityModel);
        }
        public PartialViewResult GetAttributeList(string id)
        {
            string sql = "SELECT * FROM entitysettings.v_entityattribute WHERE entityid = @entityid ORDER BY ordinal_position;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", id)
            };

            DataSet dsAttribute = DataServices.DataSetFromSQL(sql, paramList);
            List<AttributeDto> attributeDto = dsAttribute.Tables[0].ToList<AttributeDto>();
            return PartialView("_EntityManagerAttribute", attributeDto);
        }
        public PartialViewResult GetRelationList(string id)
        {
            string sql = "SELECT * FROM entitysettings.v_relationsattributes WHERE entityid = @entityid ORDER BY entityname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", id)
            };
            DataSet dsRelation = DataServices.DataSetFromSQL(sql, paramList);
            List<RelationDto> relationDto = dsRelation.Tables[0].ToList<RelationDto>();
            return PartialView("_EntityManagerRelation", relationDto);
        }
        public JsonResult GetEntityJsonList(string synapsenamespaceid)
        {
            // entity list
            string sql = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid = @synapsenamespaceid ORDER BY entityname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("synapsenamespaceid", synapsenamespaceid)
            };

            DataSet dsEntity = DataServices.DataSetFromSQL(sql, paramList);
            List<CoreEntityModel> EntityModel = dsEntity.Tables[0].ToList<CoreEntityModel>();
            return Json(EntityModel);
        }
        public JsonResult GetAttributeJosnList(string id)
        {
            string sql = "SELECT entityid, attributeid, attributename FROM entitysettings.entityattribute WHERE entityid=@entityid AND coalesce(isrelationattribute,0) = 0 ORDER BY attributename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", id)
            };

            DataSet dsAttribute = DataServices.DataSetFromSQL(sql, paramList);
            List<AttributeDto> attributeDto = dsAttribute.Tables[0].ToList<AttributeDto>();
            return Json(attributeDto);
        }
        public PartialViewResult GetLocalNamespaceList()
        {
            string sql = "SELECT * FROM entitysettings.localnamespace ORDER BY localnamespacename; ";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };

            DataSet dsLocalNamespace = DataServices.DataSetFromSQL(sql, paramList);
            List<LocalNamespaceDto> localNamespaceDto = dsLocalNamespace.Tables[0].ToList<LocalNamespaceDto>();
            return PartialView("_EntityManagerLocalNamespace", localNamespaceDto);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEntity(string entityName)
        {
            entityName = entityName == null ? "" : entityName;
            string sql = "SELECT * FROM entitysettings.entitymanager WHERE synapseNamespaceid = @synapseNamespaceid and entityname = @entityname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("synapseNamespaceid", namespaceId),
                new KeyValuePair<string, string>("entityname", entityName)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                //return Json("The name of the entity that you have entered already exists");
                return Json("There is already an entity called " + entityName);
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyExtendedEntity(string entityName)
        {
            entityName = entityName == null ? "" : entityName;
            string sql = "SELECT * FROM entitysettings.entitymanager WHERE synapseNamespaceid = @synapseNamespaceid and entityname = @entityname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("synapseNamespaceid", namespaceId),
                new KeyValuePair<string, string>("entityname", entityName)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                return Json("There is already an extended entity called " + entityName);
            }

            return Json(true);
        }

        public IActionResult SetLocalNamespaceId(string localNamespaceId)
        {
            localNamespaceIDSelected = localNamespaceId;

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyLocalEntity(string entityName)
        {
            entityName = entityName == null ? "" : entityName;

            string localtablename = SynapseHelpers.GetLocalNamespaceNameFromID(localNamespaceIDSelected) + "_" + entityName;

            string sql = "SELECT * FROM entitysettings.entitymanager WHERE synapseNamespaceid = @synapseNamespaceid and entityname = @entityname;";
            
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("synapseNamespaceid", namespaceId),
                new KeyValuePair<string, string>("entityname", localtablename)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                //return Json("The name of the entity that you have entered already exists");
                return Json("There is already a local entity called " + localtablename);
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyAttribute(string attributeName)
        {
            attributeName = attributeName == null ? "" : attributeName;
            string sql = "SELECT * FROM entitysettings.entityattribute WHERE entityid = @entityid and attributename = @attributename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", namespaceId),
                new KeyValuePair<string, string>("attributename", attributeName)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Json("The name of the attribute that you have entered already exists in this entitiy");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyLocalNamespace(string localNamespaceName)
        {
            localNamespaceName = localNamespaceName == null ? "" : localNamespaceName;
            string sql = "SELECT * FROM entitysettings.localnamespace WHERE localnamespacename = @localnamespacename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("localnamespacename", localNamespaceName)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Json("There is already a local namespace with that name");
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