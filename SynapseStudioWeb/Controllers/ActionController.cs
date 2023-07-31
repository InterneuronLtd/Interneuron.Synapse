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


﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SynapseStudioWeb.Helpers;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Interneuron.Common.Extensions;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class ActionController : Controller
    {

        public ActionResult saveNewAction(ActionModel Actionmodel)
        {
            string sqlInsert = string.Empty;

            //string sqlselect = "SELECT action_id, actionname, actiondescription, isendpoint FROM rbac.action where action_id = @action_id";
            //var param = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("action_id", Actionmodel.action_id)
            //};

            //DataSet ds = DataServices.DataSetFromSQL(sqlselect, param);

            if (Actionmodel.action_id is null)
            {
                sqlInsert = "INSERT INTO rbac.Action(action_id, ActionName, ActionDescription, isEndpoint)	VALUES(@action_id, @ActionName, @ActionDescription, @isEndpoint); ";

                Actionmodel.action_id = System.Guid.NewGuid().ToString();
            }
            else
            {
                sqlInsert = "UPDATE rbac.Action SET ActionName = @ActionName, ActionDescription = @ActionDescription, isEndpoint = @isEndpoint WHERE action_id = @action_id;";
            }
            var paramList = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("action_id", Actionmodel.action_id),
                   new KeyValuePair<string, object>("ActionName",Actionmodel.ActionName),
                      new KeyValuePair<string, object>("ActionDescription",Actionmodel.ActionDescription),
                new KeyValuePair<string, object>("isEndpoint",Actionmodel.isEndpoint)
            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQLWithObject(sqlInsert, paramList);
            }
            catch { }
            return RedirectToAction(nameof(ActionList));
        }


        public ActionResult NewAction()
        {
            return View();
        }

        public ActionResult ActionList()
        {
            DataSet ds = DataServices.DataSetFromSQL("SELECT action_id, actionname, actiondescription, isendpoint FROM rbac.action");

            @ViewBag.ActionList = "";
            return View(ds.Tables[0].ToList<SynapseStudioWeb.Models.ActionModel>());
        }

        public ActionResult EntityActionList(string id)
        {
            DataSet ds = DataServices.DataSetFromSQL(
                "SELECT actionentityassociations_id, actionname, concat_ws('.',  synapsenamespacename ,entityname) as entityname, permission_type "
               + " FROM rbac.actionentityassociations aa"
                + " join  rbac.premissions p on p.permission_id = aa.permission_id"
                + " join  rbac.action a on a.action_id = aa.action_id"
                + " join  entitysettings.entitymanager e on e.entityid = aa.entity_id where aa.action_id='" + id + "'");
            @ViewBag.actionName = DataServices.DataSetFromSQL("SELECT actionname FROM rbac.action where action_id='"+ id + "'").Tables[0].Rows[0][0].ToString();
            @ViewBag.Actionid = id;
            return View(ds.Tables[0].ToList<SynapseStudioWeb.Models.ActionentityAssociations>());
        }

        // GET: Action/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

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
        // GET: Action/Create
        public ActionResult Mapping(string id)
        {
            DataSet ds = DataServices.DataSetFromSQL("SELECT permission_id as ID, permission_type as Name	FROM rbac.premissions");
            ViewBag.permision = ToSelectList(ds.Tables[0], "ID", "Name");
          
            string synapsenamespacename = "";
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            string sql = @"SELECT entityid as id,entityname as name,synapsenamespacename from entitysettings.entitymanager order by synapsenamespacename, name";
            DataTable dt = new DataTable();
            try
            {
                int idt = 0;
                dt = DataServices.DataSetFromSQL(sql).Tables[0];
                //Loop and add the Child Nodes.
                foreach (DataRow subType in dt.Rows)
                {
                    if (synapsenamespacename != subType[2].ToString())
                    {
                        idt++;
                        synapsenamespacename = subType[2].ToString();
                        nodes.Add(new TreeViewNode { id = idt.ToString(), parent = "#", text = subType[2].ToString() });
                    }
                    nodes.Add(new TreeViewNode { id = idt.ToString() + "&" + subType[0].ToString(), parent = idt.ToString(), text = subType[1].ToString() });
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.actionid = id;
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }

        [HttpPost]
        public ActionResult Index(IFormCollection collection)
        {
            var selected = collection["list"];
            var selectedItems = collection["selectedItems"];
            var actionid = collection["actionid"];


            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

            if (items != null)
            {
                foreach (TreeViewNode item in items)
                {
                    int i = 0;
                    string s = item.id;
                    bool result = int.TryParse(s, out i);
                    if (!result)
                    {
                        string sqldelete = @"DELETE FROM rbac.actionentityassociations WHERE entity_id=@entity_id";
                        var paramListdelete = new List<KeyValuePair<string, object>>()
                      {

                   new KeyValuePair<string, object>("entity_id",item.id.ToString()),

                       };
                        try
                        {
                            DataServices.ExcecuteNonQueryFromSQLWithObject(sqldelete, paramListdelete);
                        }
                        catch { }
                        string sqlInsert = "INSERT INTO rbac.actionentityassociations(actionentityassociations_id, action_id, entity_id, permission_id)	VALUES (@actionentityassociations_id, @action_id, @entity_id, @permission_id)";
                        var paramList = new List<KeyValuePair<string, object>>()
                      {
                 new KeyValuePair<string, object>("actionentityassociations_id",System.Guid.NewGuid().ToString()),
                   new KeyValuePair<string, object>("action_id",actionid.ToString()),
                      new KeyValuePair<string, object>("entity_id",item.id.ToString()),
                   new KeyValuePair<string, object>("permission_id",selected.ToString())
                       };
                        try
                        {
                            DataServices.ExcecuteNonQueryFromSQLWithObject(sqlInsert, paramList);
                        }
                        catch { }
                    }

                }
            }
            return RedirectToAction("EntityActionList", "Action", new { @id = actionid });

        }
        // POST: Action/Create


        // GET: Action/Edit/5
        public ActionResult Edit(string id)
        {
            string sqlselect = "SELECT action_id, actionname, actiondescription, isendpoint FROM rbac.action where action_id = @action_id";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("action_id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sqlselect, paramList);
            DataRow actionRow = ds.Tables[0].Rows[0];

            ActionModel actionModel = new ActionModel() {
                action_id = actionRow["action_id"].ToString(),
                ActionName = actionRow["actionname"].ToString(),
                ActionDescription = actionRow["actiondescription"].ToString(),
                isEndpoint = bool.Parse(actionRow["isendpoint"].ToString())
            };

            return View("NewAction", actionModel);
        }

        // POST: Action/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(NewAction));
            }
            catch
            {
                return View();
            }
        }

        // GET: Action/Delete/5
        public ActionResult Delete(string id)
        {
            string sqlInsert = "DELETE FROM rbac.Action WHERE action_id =@action_id; ";
            var paramList = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("action_id", id)

            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQLWithObject(sqlInsert, paramList);
            }
            catch { }
            return RedirectToAction(nameof(ActionList));
        }
        public ActionResult DeleteEntitymapping(string id,string actionid)
        {
            string sqlInsert = "DELETE FROM rbac.actionentityassociations WHERE actionentityassociations_id =@actionentityassociations_id; ";
            var paramList = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("actionentityassociations_id", id)

            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQLWithObject(sqlInsert, paramList);
            }
            catch { }
            return RedirectToAction("EntityActionList", "Action", new { @id = actionid });
        }
        // POST: Action/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(NewAction));
            }
            catch
            {
                return View();
            }
        }
    }


    public class TreeViewNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
    }
}