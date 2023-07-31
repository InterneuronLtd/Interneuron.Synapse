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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class RoleMappingController : Controller
    {      
        public ActionResult Roleobjectselection(string id)
        {
            string sql = "SELECT \"Name\" as rolename, \"Id\" as id FROM \"AspNetRoles\" order by rolename";
            List<SelectListItem> rolelist = new List<SelectListItem>();
            DataSet dsl = DataServices.DataSetFromSQL(sql, new List<KeyValuePair<string, string>>(), "connectionString_SynapseIdentityStore");
            for(int i=0;i< dsl.Tables[0].Rows.Count;i++)
            {
                rolelist.Add(new SelectListItem()
                {
                    Text = dsl.Tables[0].Rows[i][0].ToString(),
                    Value = dsl.Tables[0].Rows[i][1].ToString()
                });

                if (dsl.Tables[0].Rows[i][1].ToString() == id)
                {
                    ViewBag.Rolename = dsl.Tables[0].Rows[i][0].ToString();
                    ViewBag.Roleid = dsl.Tables[0].Rows[i][1].ToString();
                }
            }

            ViewBag.Rolelist = new SelectList(rolelist, "Value", "Text");
            DataSet ds = DataServices.DataSetFromSQL("SELECT objecttypeid, objecttype FROM rbac.objecttypes order by objecttype");


            List<SelectListItem> ObjList = new List<SelectListItem>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ObjList.Add(new SelectListItem()
                {
                    Text = row["objecttype"].ToString(),
                    Value = row["objecttypeid"].ToString()
                });
            }
            ViewBag.objecttypes = new SelectList(ObjList, "Value", "Text"); 
            return View();
        }

        public ActionResult LoadObjectGrid(string objecttypeid,string roleid)
        {
            string sql = "";
            if (objecttypeid == "1")
            {

                sql = @"SELECT roleprevilage_id as id,applicationname as name,permission_type
                        FROM rbac.roleprevilages rp	 join entitystorematerialised.meta_application a on a.application_id = rp.objectid
                        join  rbac.premissions p on p.permission_id=rp.permissionid
                        inner join rbac.objecttypes ot on ot.objecttypeid=rp.objecttypeid  where objecttype='Application'";

            }
            else if (objecttypeid == "2")
            {

                sql = @"SELECT roleprevilage_id as id,modulename as name,permission_type
                            FROM rbac.roleprevilages rp	join entitystorematerialised.meta_module a on a.module_id=rp.objectid
                            join  rbac.premissions p on p.permission_id=rp.permissionid
                            inner join rbac.objecttypes ot on ot.objecttypeid=rp.objecttypeid  where objecttype='Module'";

            }
            else if (objecttypeid == "3")
            {
                sql = @"SELECT roleprevilage_id as id,actionname as name,permission_type
                            FROM rbac.roleprevilages rp	join  rbac.action a on a.action_id=rp.objectid
                            join  rbac.premissions p on p.permission_id=rp.permissionid
                            inner join rbac.objecttypes ot on ot.objecttypeid=rp.objecttypeid  where objecttype='Action'";

            }
            else if (objecttypeid == "4")
            {
                sql = @"SELECT roleprevilage_id as id,entityname as name,permission_type
                            FROM rbac.roleprevilages rp	join  entitysettings.entitymanager a on a.entityid=rp.objectid
                            join  rbac.premissions p on p.permission_id=rp.permissionid
                            inner join rbac.objecttypes ot on ot.objecttypeid=rp.objecttypeid  where objecttype='Entity'";


            }
            else if (objecttypeid == "5")
            {
                sql = @"SELECT roleprevilage_id as id,listname as name ,permission_type
                            FROM rbac.roleprevilages rp	join listsettings.listmanager a on a.list_id=rp.objectid
                            join  rbac.premissions p on p.permission_id=rp.permissionid
                            inner join rbac.objecttypes ot on ot.objecttypeid=rp.objecttypeid  where objecttype='List'";

            }
            else if (objecttypeid == "6")
            {
                sql = @"SELECT roleprevilage_id as id,personaname as name,permission_type
                        FROM rbac.roleprevilages rp	join entitystorematerialised.meta_persona a on a.persona_id=rp.objectid
                        join  rbac.premissions p on p.permission_id=rp.permissionid
                        inner join rbac.objecttypes ot on ot.objecttypeid=rp.objecttypeid  where objecttype='Persona'";

            }
            DataSet ds = DataServices.DataSetFromSQL(sql+" and roleid='"+ roleid+ "' order by name");

            List<RollGridModel> ObjList = new List<RollGridModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ObjList.Add(new RollGridModel()
                {
                    objectname = row["name"].ToString(),
                    permission_type = row["permission_type"].ToString(),
                    roleprevilage_id = row["id"].ToString()
                });
            }

            return Json(JsonConvert.SerializeObject(ObjList));
        }

        public ActionResult DeleteRolePrevilage(string roleprevilage_id)
        {
            string sqlInsert = "DELETE FROM  rbac.roleprevilages WHERE roleprevilage_id =@roleprevilage_id; ";
            var paramList = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("roleprevilage_id", roleprevilage_id)

            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQLWithObject(sqlInsert, paramList);
            }
            catch { }
            return Json("success");
        }

        [HttpPost]
        public ActionResult savePermition(RolePermissions savepermissiondata)
        {
            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(savepermissiondata.stringselected);
        
            foreach (TreeViewNode item in items)
            {
                if (item.id != "0")
                {
                    string sqldelete = @"DELETE FROM rbac.roleprevilages WHERE objectid=@objectid and objecttypeid=@objecttypeid and roleid=@roleid";
                    var paramListdelete = new List<KeyValuePair<string, object>>()
                      {
                        new KeyValuePair<string, object>("objectid",item.id.ToString()),
                      new KeyValuePair<string, object>("objecttypeid",savepermissiondata.objecttypeid),
                      new KeyValuePair<string, object>("roleid",savepermissiondata.roleid),

                       };
                    try
                    {
                        DataServices.ExcecuteNonQueryFromSQLWithObject(sqldelete, paramListdelete);
                    }
                    catch { }
                    string sqlInsert = "INSERT INTO rbac.roleprevilages(roleprevilage_id, roleid, objectid, permissionid, objecttypeid,rolename)	VALUES (@roleprevilage_id, @roleid, @objectid, @permissionid,@objecttypeid,@rolename)";
                    var paramList = new List<KeyValuePair<string, object>>()
                      {
                 new KeyValuePair<string, object>("roleprevilage_id",System.Guid.NewGuid().ToString()),
                   new KeyValuePair<string, object>("roleid",savepermissiondata.roleid.ToString()),
                      new KeyValuePair<string, object>("objectid",item.id.ToString()),
                         new KeyValuePair<string, object>("permissionid",savepermissiondata.permitions.ToString()),
                   new KeyValuePair<string, object>("objecttypeid",savepermissiondata.objecttypeid.ToString()),
                      new KeyValuePair<string, object>("rolename",savepermissiondata.rolename),
                       };
                    try
                    {
                        DataServices.ExcecuteNonQueryFromSQLWithObject(sqlInsert, paramList);
                    }
                    catch { }
                }

            }

            return Json("success");
        }

        public ActionResult objectypechange(string objecttypeid)
        {

            if (objecttypeid == "1")
            {

                string sqld = @"SELECT application_id as id, applicationname as name FROM entitystorematerialised.meta_application order by name";
                List<TreeViewNode> result = createtreeView(sqld, "Application");
                return Json(JsonConvert.SerializeObject(result));
            }
            else if (objecttypeid == "2")
            {

                string sqld = @"SELECT module_id as id, modulename as name FROM entitystorematerialised.meta_module order by name";
                List<TreeViewNode> result = createtreeView(sqld, "Module");
                return Json(JsonConvert.SerializeObject(result));
            }
            else if (objecttypeid == "3")
            {
                string sql = @"SELECT action_id as id, actionname as name FROM rbac.action order by name";
                List<TreeViewNode> result = createtreeView(sql, "Action");
                return Json(JsonConvert.SerializeObject(result));
            }
            else if (objecttypeid == "4")
            {
                string sql = @"SELECT entityid as id,entityname as name from entitysettings.entitymanager order by synapsenamespacename order by name";
                List<TreeViewNode> result = createtreeView(sql, "Entity");
                return Json(JsonConvert.SerializeObject(result));
            }
            else if (objecttypeid == "5")
            {
                string sql = @"SELECT list_id as id,listname as name from listsettings.listmanager order by name";
                List<TreeViewNode> result = createtreeView(sql, "List");
                return Json(JsonConvert.SerializeObject(result));
            }
            else if (objecttypeid == "6")
            {
                string sql = @"SELECT persona_id as id, personaname as name FROM entitystorematerialised.meta_persona order by name";
                List<TreeViewNode> result = createtreeView(sql, "Persona");
                return Json(JsonConvert.SerializeObject(result));
            }

            return Json(JsonConvert.SerializeObject(""));
        }

        public List<TreeViewNode> createtreeView(string sqlobjecttyle, string objecttype)
        {


            string synapsenamespacename = "";
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            string sqld = sqlobjecttyle;
            DataTable dt = new DataTable();
            try
            {
                int idt = -1;
                dt = DataServices.DataSetFromSQL(sqld).Tables[0];
                //Loop and add the Child Nodes.
                foreach (DataRow subType in dt.Rows)
                {
                    if (synapsenamespacename != objecttype)
                    {
                        idt++;
                        synapsenamespacename = objecttype;
                        nodes.Add(new TreeViewNode { id = idt.ToString(), parent = "#", text = objecttype });

                    }
                    nodes.Add(new TreeViewNode { id = idt.ToString() + "&" + subType[0].ToString(), parent = idt.ToString(), text = subType[1].ToString() });
                }
            }
            catch (Exception ex)
            {

            }

            return nodes;
        }
    }

    public class RolePermissions
    {
        public string objecttypeid { get; set; }
        public string permitions { get; set; } 
        public string stringselected { get; set; }
        public string roleid { get; set; }
        public string rolename { get; set; }
    }
}