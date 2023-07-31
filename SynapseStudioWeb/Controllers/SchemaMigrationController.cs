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
using Newtonsoft.Json;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Action = SynapseStudioWeb.Models.Action;
using Microsoft.AspNetCore.Authorization;
using SynapseStudioWeb.AppCode.Filters;
using NToastNotify;

namespace SynapseStudioWeb.Controllers
{
    public class ImportFileUpload
    {
        [Required(ErrorMessage = "Please select the Synapse Schema Export file to process")]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
    }
    [Authorize]
    public class SchemaMigrationController : Controller
    {
        private IToastNotification toastNotification;

        public SchemaMigrationController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
        }

        public IActionResult SchemaExport()
        {
            string sql = @"SELECT entityid as id,entityname as name, synapsenamespacename as synamespace from entitysettings.entitymanager order by synamespace, name";
            DataTable dt = new DataTable();
            List<TreeViewNode> nodes = new List<TreeViewNode>();
            dt = DataServices.DataSetFromSQL(sql).Tables[0];
            try
            {
                dt = DataServices.DataSetFromSQL(sql).Tables[0];
                nodes.Add(new TreeViewNode { id = "1", parent = "#", text = "Entities" });
                nodes.Add(new TreeViewNode { id = "2", parent = "#", text = "Baseview" });
                nodes.Add(new TreeViewNode { id = "3", parent = "#", text = "Lists" });
                //Loop and add the Child Nodes.
                foreach (DataRow item in dt.Rows)
                {
                    TreeViewNode node = new TreeViewNode();
                    //node.text = item["name"].ToString();
                    node.text = item["synamespace"].ToString() + "_" + item["name"].ToString();
                    node.id = item["id"].ToString();
                    node.parent = "1";
                    nodes.Add(node);
                }
            }
            catch (Exception ex)
            {

            }

            sql = @"SELECT baseview_id as id,baseviewname as name, baseviewnamespace as nspace from listsettings.baseviewmanager order by nspace, baseviewname";
            dt = new DataTable();
            try
            {
                dt = DataServices.DataSetFromSQL(sql).Tables[0];
            }
            catch (Exception ex)
            {

            }
            foreach (DataRow item in dt.Rows)
            {
                TreeViewNode node = new TreeViewNode();
                //node.text = item["name"].ToString();
                node.text = item["nspace"].ToString() + "_" + item["name"].ToString();
                node.id = item["id"].ToString();
                node.parent = "2";
                nodes.Add(node);
            }

            sql = @"SELECT list_id as id,listname as name, listnamespace as lspace from listsettings.listmanager order by lspace, listname";
            dt = new DataTable();
            try
            {
                dt = DataServices.DataSetFromSQL(sql).Tables[0];
            }
            catch (Exception ex)
            {

            }
            foreach (DataRow item in dt.Rows)
            {
                TreeViewNode node = new TreeViewNode();
                //node.text = item["name"].ToString();
                node.text = item["lspace"].ToString() + "_" + item["name"].ToString();
                node.id = item["id"].ToString();
                node.parent = "3";
                nodes.Add(node);
            }
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }
        public IActionResult SchemaImport(string import)
        {
            ViewBag.Message = "";
            if (import == "yes")
                ViewBag.Message = "Import has been done successfully.";
            return View();
        }
        [HttpPost]
        public IActionResult ImportFile(IFormFile formFile)
        {

            //lblError.Text = string.Empty;
            //showSelectObjects();
            //Session.Remove("export");
            Export ex;
            try
            {
                Stream memoryStream = formFile.OpenReadStream();
                StreamReader sr = new StreamReader(memoryStream);
                ex = SynapseHelpers.DeserialiseExport(sr.ReadToEnd());
                sr.Close(); sr.Dispose();
                SynapseHelpers.CompareSchema(ex, SynapseHelpers.DataSetSerializerType.Json);
                HttpContext.Session.SetString("ExportSchema", JsonConvert.SerializeObject(ex));
                ViewBag.Json = JsonConvert.SerializeObject(ConsentTreeView(ex));

            }
            catch (Exception ex1)
            {
                //Session.Remove("export");
                //lblError.Text = ex1.Message;
                // lblError.Text += "Please try a different export";
                //showFileUpload();
            }
            return View("SchemaImport");
        }

        [HttpPost]
        [StudioExceptionFilter()]
        public IActionResult SaveImportFile(IFormCollection collection)
        {

            //try
            {
                var selectedItems = collection["treeViewNodes"];
                List<TreeViewNode> treeViewNodes = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);
                var value = HttpContext.Session.GetString("ExportSchema");
                var valueab = HttpContext.Session.GetString("ExportSchemaab");
                Export ex = JsonConvert.DeserializeObject<Export>(value);

                //update export.object.action with the user consent 
                //foreach entity, baseview, list in export, check and update consent
                if (treeViewNodes.Count != 0)
                {
                    foreach (var item in ex.Entities.Entity)
                    {
                        TreeViewNode n = treeViewNodes.Where(e => e.id == item.Id).FirstOrDefault();
                        if (n != null)
                        {
                            if (item.action == SynapseHelpers.MigrationAction.Update)
                            {
                                List<Action> createActionsAttribute = item.attributeActions.Where(x => x.migrationAction == SynapseHelpers.MigrationAction.New && x.sourceContextId == item.Id && x.dbTableName.Equals("entitysettings.entityattribute")).ToList();
                                List<Action> createActionsRelations = item.attributeActions.Where(x => x.migrationAction == SynapseHelpers.MigrationAction.New && x.sourceContextId == item.Id && x.dbTableName.Equals("entitysettings.entityrelation")).ToList();


                                if (createActionsAttribute.Count != 0)
                                {
                                    foreach (var action in createActionsAttribute)
                                    {
                                        TreeViewNode n1 = treeViewNodes.Where(e => e.id == "newattributes").FirstOrDefault();
                                        if (n1 == null)
                                            action.migrationAction = SynapseHelpers.MigrationAction.Skip;
                                    }
                                }
                                if (createActionsRelations.Count != 0)
                                {
                                    foreach (var action in createActionsRelations)
                                    {
                                        TreeViewNode n1 = treeViewNodes.Where(e => e.id == "newrelations").FirstOrDefault();
                                        //TreeNode n1 = tv.FindNode("entities/entitiesupdate/" + item.Id + "/newrelations/" + action.sourceComponentId);
                                        if (n1 == null)
                                            action.migrationAction = SynapseHelpers.MigrationAction.Skip;
                                    }
                                }
                            }

                            //item.action = SynapseHelpers.MigrationAction.Skip;
                        }

                    }

                    foreach (Baseview b in ex.Baseviews.Baseview)
                    {
                        TreeViewNode n = treeViewNodes.Where(e => e.id == b.Id).FirstOrDefault();
                        //TreeNode n = tv.FindNode("baseviews/baseviews" + b.action.ToString().ToLower() + "/" + b.Id);

                        if (n == null)
                        {
                            b.action = SynapseHelpers.MigrationAction.Skip;
                        }

                    }

                    foreach (SList l in ex.SLists.SList)
                    {
                        TreeViewNode n = treeViewNodes.Where(e => e.id == l.Id).FirstOrDefault();
                        // TreeNode n = tv.FindNode("lists/lists" + l.action.ToString().ToLower() + "/" + l.Id);

                        if (n == null)
                        {
                            l.action = SynapseHelpers.MigrationAction.Skip;
                        }

                    }



                    //Task task = new Task(() => SynapseHelpers.ApplySchemaChanges(ex));
                    //task.Start();

                    new TaskFactory().StartNew(() => SynapseHelpers.ApplySchemaChanges(ex));
                    //showProcessMsgs();
                    ViewBag.Json = new List<TreeViewNode>();
                    this.toastNotification.AddSuccessToastMessage("Successfully migrated the schema.");
                }
                else
                {
                    //lblErrorSelectObjects.Text = "Looks like there is nothing to import!";
                    this.toastNotification.AddInfoToastMessage("Looks like there is nothing to import!");
                }
            }
            /*catch (Exception exception)
            {
                //lblError.Text = "There was an error processing the export. Please try again. ";
                //lblError.Text += exception.Message;
                //tv.Nodes.Clear();
                //showFileUpload();
                ViewBag.Json = new List<TreeViewNode>();
            }*/
            return RedirectToAction("SchemaImport", new { import = "yes" });
        }
        public IActionResult showMessage()
        {
            List<ImportStatusMessage> s = SynapseHelpers.statusMessages.Where(x => x.seen == false).OrderBy(b => b.serialno).ToList<ImportStatusMessage>();
            return Json(s);
        }
        protected List<TreeViewNode> ConsentTreeView(Export ex)
        {
            List<TreeViewNode> treeViewNodes = new List<TreeViewNode>();
            TreeViewNode entitiesrootnode = new TreeViewNode() { id = "entities", parent = "#", text = "Entities" };
            TreeViewNode baseviewsrootnode = new TreeViewNode() { id = "baseviews", parent = "#", text = "Baseviews" };
            TreeViewNode listsrootnode = new TreeViewNode() { id = "lists", parent = "#", text = "Lists" };

            TreeViewNode enititychildnodenew = new TreeViewNode() { id = "entitiesnew", parent = "entities", text = "New" };
            TreeViewNode enititychildnodeupdate = new TreeViewNode() { id = "entitiesupdate", parent = "entities", text = "Update" };
            TreeViewNode baseviewschildnodenew = new TreeViewNode() { id = "baseviewsnew", parent = "baseviews", text = "New" };
            TreeViewNode baseviewschildnodeupdate = new TreeViewNode() { id = "baseviewsupdate", parent = "baseviews", text = "Update" };
            TreeViewNode listschildnodenew = new TreeViewNode() { id = "listsnew", parent = "lists", text = "New" };
            TreeViewNode listschildnodeupdate = new TreeViewNode() { id = "listsupdate", parent = "lists", text = "Update" };


            treeViewNodes.Add(entitiesrootnode);
            treeViewNodes.Add(baseviewsrootnode);
            treeViewNodes.Add(listsrootnode);
            treeViewNodes.Add(enititychildnodenew);
            treeViewNodes.Add(enititychildnodeupdate);
            treeViewNodes.Add(baseviewschildnodenew);
            treeViewNodes.Add(baseviewschildnodeupdate);
            treeViewNodes.Add(listschildnodenew);
            treeViewNodes.Add(listschildnodeupdate);

            foreach (Entity e in ex.Entities.Entity)
            {
                if (e.action == SynapseHelpers.MigrationAction.New)
                {
                    treeViewNodes.Add(new TreeViewNode() { id = e.Id, text = e.Name, parent = "entitiesnew" });
                }
                else if (e.action == SynapseHelpers.MigrationAction.Update)
                {
                    List<Action> createActionsAttribute = e.attributeActions.Where(x => x.migrationAction == SynapseHelpers.MigrationAction.New && x.sourceContextId == e.Id && x.dbTableName.Equals("entitysettings.entityattribute")).ToList();
                    List<Action> createActionsRelations = e.attributeActions.Where(x => x.migrationAction == SynapseHelpers.MigrationAction.New && x.sourceContextId == e.Id && x.dbTableName.Equals("entitysettings.entityrelation")).ToList();

                    if (createActionsAttribute.Count != 0 || createActionsRelations.Count != 0)
                        treeViewNodes.Add(new TreeViewNode() { id = e.Id, text = e.Name, parent = "entitiesupdate" });

                    if (createActionsAttribute.Count != 0)
                    {
                        treeViewNodes.Add(new TreeViewNode() { id = "newattributes", text = "New Attributes", parent = e.Id });
                        foreach (var action in createActionsAttribute)
                        {
                            treeViewNodes.Add(new TreeViewNode() { id = action.sourceComponentId, text = action.componentKeyValue, parent = "newattributes" });
                        }
                    }
                    if (createActionsRelations.Count != 0)
                    {
                        treeViewNodes.Add(new TreeViewNode() { id = "newrelations", text = "New Relations", parent = e.Id });
                        foreach (var action in createActionsAttribute)
                        {
                            treeViewNodes.Add(new TreeViewNode() { id = action.sourceComponentId, text = action.componentKeyValue, parent = "newrelations" });
                        }
                    }
                }
            }

            foreach (Baseview b in ex.Baseviews.Baseview)
            {
                if (b.action == SynapseHelpers.MigrationAction.New)
                {
                    treeViewNodes.Add(new TreeViewNode() { id = b.Id, text = b.Name, parent = "baseviewsnew" });

                }
                else
                    if (b.action == SynapseHelpers.MigrationAction.Update)
                {
                    treeViewNodes.Add(new TreeViewNode() { id = b.Id, text = b.Name, parent = "baseviewsupdate" });
                }
            }

            foreach (SList l in ex.SLists.SList)
            {
                if (l.action == SynapseHelpers.MigrationAction.New)
                {
                    treeViewNodes.Add(new TreeViewNode() { id = l.Id, text = l.Name, parent = "listsnew" });

                }
                else if (l.action == SynapseHelpers.MigrationAction.Update)
                {
                    treeViewNodes.Add(new TreeViewNode() { id = l.Id, text = l.Name, parent = "listsupdate" });

                }
            }

            if (!treeViewNodes.Exists(e => e.parent == "entitiesnew" || e.parent == "entitiesupdate"))
            {
                TreeViewNode rnode = treeViewNodes.Where(e => e.id == "entities").FirstOrDefault();
                treeViewNodes.Remove(rnode);
            }
            if (!treeViewNodes.Exists(e => e.parent == "baseviewsnew" || e.parent == "baseviewsupdate"))
            {
                TreeViewNode rnode = treeViewNodes.Where(e => e.id == "baseviews").FirstOrDefault();
                treeViewNodes.Remove(rnode);
            }
            if (!treeViewNodes.Exists(e => e.parent == "listsnew" || e.parent == "listsupdate"))
            {
                TreeViewNode rnode = treeViewNodes.Where(e => e.id == "lists").FirstOrDefault();
                treeViewNodes.Remove(rnode);
            }

            if (!treeViewNodes.Exists(e => e.parent == "entitiesnew"))
            {
                TreeViewNode rnode = treeViewNodes.Where(e => e.id == "entitiesnew").FirstOrDefault();
                treeViewNodes.Remove(rnode);
            }
            if (!treeViewNodes.Exists(e => e.parent == "baseviewsnew"))
            {
                TreeViewNode rnode = treeViewNodes.Where(e => e.id == "baseviewsnew").FirstOrDefault();
                treeViewNodes.Remove(rnode);
            }

            if (!treeViewNodes.Exists(e => e.parent == "listsnew"))
            {
                TreeViewNode rnode = treeViewNodes.Where(e => e.id == "listsnew").FirstOrDefault();
                treeViewNodes.Remove(rnode);
            }
            if (!treeViewNodes.Exists(e => e.parent == "entitiesupdate"))
            {
                TreeViewNode rnode = treeViewNodes.Where(e => e.id == "entitiesupdate").FirstOrDefault();
                treeViewNodes.Remove(rnode);
            }
            if (!treeViewNodes.Exists(e => e.parent == "baseviewsupdate"))
            {
                TreeViewNode rnode = treeViewNodes.Where(e => e.id == "baseviewsupdate").FirstOrDefault();
                treeViewNodes.Remove(rnode);
            }
            if (!treeViewNodes.Exists(e => e.parent == "listsupdate"))
            {
                TreeViewNode rnode = treeViewNodes.Where(e => e.id == "listsupdate").FirstOrDefault();
                treeViewNodes.Remove(rnode);
            }

            //if (treeViewNodes.FindNode("entities/entitiesnew").ChildNodes.Count == 0)
            //    treeViewNodes.Add(new TreeViewNode() { id = l.Id, text = l.Name, parent = "listsupdate" });
            //tv.FindNode("entities").ChildNodes.Remove(tv.FindNode("entities/entitiesnew"));
            //if (tv.FindNode("entities/entitiesupdate").ChildNodes.Count == 0)
            //    tv.FindNode("entities").ChildNodes.Remove(tv.FindNode("entities/entitiesupdate"));
            //if (tv.FindNode("entities").ChildNodes.Count == 0)
            //    tv.Nodes.Remove(tv.FindNode("entities"));

            //if (tv.FindNode("baseviews/baseviewsnew").ChildNodes.Count == 0)
            //    tv.FindNode("baseviews").ChildNodes.Remove(tv.FindNode("baseviews/baseviewsnew"));
            //if (tv.FindNode("baseviews/baseviewsupdate").ChildNodes.Count == 0)
            //    tv.FindNode("baseviews").ChildNodes.Remove(tv.FindNode("baseviews/baseviewsupdate"));
            //if (tv.FindNode("baseviews").ChildNodes.Count == 0)
            //    tv.Nodes.Remove(tv.FindNode("baseviews"));

            //if (tv.FindNode("lists/listsnew").ChildNodes.Count == 0)
            //    tv.FindNode("lists").ChildNodes.Remove(tv.FindNode("lists/listsnew"));
            //if (tv.FindNode("lists/listsupdate").ChildNodes.Count == 0)
            //    tv.FindNode("lists").ChildNodes.Remove(tv.FindNode("lists/listsupdate"));
            //if (tv.FindNode("lists").ChildNodes.Count == 0)
            //    tv.Nodes.Remove(tv.FindNode("lists"));
            return treeViewNodes;
        }


        [HttpPost]
        public IActionResult ExportFile(IFormCollection collection)
        {
            Export exp = new Export();
            exp.format = SynapseHelpers.DataSetSerializerType.Json;
            var selectedItems = collection["selectedItems"];
            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

            foreach (TreeViewNode treeViewNode in items)
            {
                if (treeViewNode.parent == "1")
                {
                    exp.Entities.Entity.Add(getEntityByIdForExport(treeViewNode.id));
                }
                if (treeViewNode.parent == "2")
                {
                    exp.Baseviews.Baseview.Add(getBaseViewByIdForExport(treeViewNode.id));
                }
                if (treeViewNode.parent == "3")
                {
                    exp.SLists.SList.Add(getListByIdForExport(treeViewNode.id));
                }
            }
            string xml = exp.SerializeToXML();
            var bytes = Encoding.UTF8.GetBytes(xml);
            return File(bytes, "text/plain", "SynapseExport" + DateTime.Now.Ticks + ".xml");
        }
        public SList getListByIdForExport(string listid)
        {
            SList l = new SList();
            l.Id = listid;
            l.Namespace = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetListNamespace(listid), SynapseHelpers.DataSetSerializerType.Json);
            DataSet dsListManager = SynapseHelpers.GetListManager(listid);
            l.ListManager = SynapseHelpers.SerializeDataSet(dsListManager, SynapseHelpers.DataSetSerializerType.Json);
            l.ListAttribute = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetListAttribute(listid), SynapseHelpers.DataSetSerializerType.Json);
            l.ListAttributeStyleRule = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetListAttributeStyleRule(listid), SynapseHelpers.DataSetSerializerType.Json);
            l.ListBaseviewFilter = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetListBaseviewfilter(listid), SynapseHelpers.DataSetSerializerType.Json);
            l.ListBaseviewParameter = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetListBaseviewParameter(listid), SynapseHelpers.DataSetSerializerType.Json);
            l.ListQuestion = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetListQuestion(listid), SynapseHelpers.DataSetSerializerType.Json);
            l.listQuestionResponse = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetListQuestionResponse(listid), SynapseHelpers.DataSetSerializerType.Json);

            l.Name = dsListManager.Tables[0].Rows[0]["listname"].ToString();
            string entityid = dsListManager.Tables[0].Rows[0]["defaultcontext"].ToString();
            string baseviewid = dsListManager.Tables[0].Rows[0]["baseview_id"].ToString();
            l.entity = getEntityByIdForExport(entityid);
            l.baseview = getBaseViewByIdForExport(baseviewid);
            return l;

        }
        public Baseview getBaseViewByIdForExport(string baseviewid)
        {
            Baseview b = new Baseview();
            DataSet dtBVManager = SynapseHelpers.GetBaseviewManager(baseviewid);
            b.BaseviewManager = SynapseHelpers.SerializeDataSet(dtBVManager, SynapseHelpers.DataSetSerializerType.Json);
            b.Name = dtBVManager.Tables[0].Rows[0]["baseviewname"].ToString();
            b.Id = dtBVManager.Tables[0].Rows[0]["baseview_id"].ToString();
            b.Namespace = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetBaseviewNamespace(baseviewid), SynapseHelpers.DataSetSerializerType.Json);

            return b;
        }

        public Entity getEntityByIdForExport(string entityid)
        {
            Entity e = new Entity();
            if (!string.IsNullOrEmpty(entityid))
            {
                e.Id = entityid;
                e.EntityAttributes = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetEntityAttributes(entityid), SynapseHelpers.DataSetSerializerType.Json);
                DataSet dsEntityManager = SynapseHelpers.GetEntityManager(entityid);
                e.EntityManager = SynapseHelpers.SerializeDataSet(dsEntityManager, SynapseHelpers.DataSetSerializerType.Json);
                e.EntityRelation = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetEntityRelation(entityid), SynapseHelpers.DataSetSerializerType.Json);
                e.Namespace = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetEntityNamespace(entityid), SynapseHelpers.DataSetSerializerType.Json);
                e.SystemNamespace = SynapseHelpers.SerializeDataSet(SynapseHelpers.GetEntitySystemNamespace(entityid), SynapseHelpers.DataSetSerializerType.Json);
                e.Name = dsEntityManager.Tables[0].Rows[0]["entityname"].ToString();
            }

            return e;
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

