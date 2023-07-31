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


﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using SynapseStudio.SchemaMigration;
using System.Threading.Tasks;

namespace SynapseStudio
{
    public partial class SchemaImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                showFileUpload();
                Session.Remove("export");
            }
        }
        protected void showFileUpload()
        {
            pnlSelectFile.Visible = true;
            pnlSelectObjects.Visible = false;
            pnlStatus.Visible = false;
            tv.Nodes.Clear();
        }
        protected void showSelectObjects()
        {
            pnlSelectFile.Visible = false;
            pnlSelectObjects.Visible = true;
            pnlStatus.Visible = false;
        }
        protected void showProcessMsgs()
        {
            pnlSelectFile.Visible = false;
            pnlSelectObjects.Visible = false;
            pnlStatus.Visible = true;
        }

        protected void AddNode(string name, string value, TreeNode parent)
        {
            TreeNode node = new TreeNode();
            node.ShowCheckBox = true;
            node.Text = name;
            node.Value = value;
            parent.ChildNodes.Add(node);
        }

        protected void ConsentTreeView(Export ex)
        {
            tv.Nodes.Clear();
            TreeNode entitiesrootnode = new TreeNode("Entities", "entities");
            TreeNode baseviewsrootnode = new TreeNode("Baseviews", "baseviews");
            TreeNode listsrootnode = new TreeNode("Lists", "lists");

            entitiesrootnode.ChildNodes.Add(new TreeNode("New", "entitiesnew"));
            entitiesrootnode.ChildNodes.Add(new TreeNode("Update", "entitiesupdate"));
            baseviewsrootnode.ChildNodes.Add(new TreeNode("New", "baseviewsnew"));
            baseviewsrootnode.ChildNodes.Add(new TreeNode("Update", "baseviewsupdate"));
            listsrootnode.ChildNodes.Add(new TreeNode("New", "listsnew"));
            listsrootnode.ChildNodes.Add(new TreeNode("Update", "listsupdate"));

            tv.Nodes.Add(entitiesrootnode);
            tv.Nodes.Add(baseviewsrootnode);
            tv.Nodes.Add(listsrootnode);

            tv.CollapseAll();
            tv.ShowCheckBoxes = TreeNodeTypes.All;

            foreach (Entity e in ex.Entities.Entity)
            {
                if (e.action == SynapseHelpers.MigrationAction.New)
                {
                    AddNode(e.Name, e.Id, tv.FindNode("entities/entitiesnew"));
                }
                else if (e.action == SynapseHelpers.MigrationAction.Update)
                {
                    List<SchemaMigration.Action> createActionsAttribute = e.attributeActions.Where(x => x.migrationAction == SynapseHelpers.MigrationAction.New && x.sourceContextId == e.Id && x.dbTableName.Equals("entitysettings.entityattribute")).ToList();
                    List<SchemaMigration.Action> createActionsRelations = e.attributeActions.Where(x => x.migrationAction == SynapseHelpers.MigrationAction.New && x.sourceContextId == e.Id && x.dbTableName.Equals("entitysettings.entityrelation")).ToList();

                    if (createActionsAttribute.Count != 0 || createActionsRelations.Count != 0)
                        AddNode(e.Name, e.Id, tv.FindNode("entities/entitiesupdate"));

                    if (createActionsAttribute.Count != 0)
                    {
                        AddNode("New Attributes", "newattributes", tv.FindNode("entities/entitiesupdate/" + e.Id));
                        foreach (var action in createActionsAttribute)
                        {
                            AddNode(action.componentKeyValue, action.sourceComponentId, tv.FindNode("entities/entitiesupdate/" + e.Id + "/newattributes"));
                        }
                    }
                    if (createActionsRelations.Count != 0)
                    {
                        AddNode("New Relations", "newrelations", tv.FindNode("entities/entitiesupdate/" + e.Id));
                        foreach (var action in createActionsRelations)
                        {
                            AddNode(action.componentKeyValue, action.sourceComponentId, tv.FindNode("entities/entitiesupdate/" + e.Id + "/newrelations"));
                        }
                    }
                }
            }

            foreach (Baseview b in ex.Baseviews.Baseview)
            {
                if (b.action == SynapseHelpers.MigrationAction.New)
                {
                    AddNode(b.Name, b.Id, tv.FindNode("baseviews/baseviewsnew"));
                }
                else
                    if (b.action == SynapseHelpers.MigrationAction.Update)
                {
                    AddNode(b.Name, b.Id, tv.FindNode("baseviews/baseviewsupdate"));
                }
            }

            foreach (SList l in ex.SLists.SList)
            {
                if (l.action == SynapseHelpers.MigrationAction.New)
                {
                    AddNode(l.Name, l.Id, tv.FindNode("lists/listsnew"));
                }
                else
                    if (l.action == SynapseHelpers.MigrationAction.Update)
                {
                    AddNode(l.Name, l.Id, tv.FindNode("lists/listsupdate"));
                }
            }

            if (tv.FindNode("entities/entitiesnew").ChildNodes.Count == 0)
                tv.FindNode("entities").ChildNodes.Remove(tv.FindNode("entities/entitiesnew"));
            if (tv.FindNode("entities/entitiesupdate").ChildNodes.Count == 0)
                tv.FindNode("entities").ChildNodes.Remove(tv.FindNode("entities/entitiesupdate"));
            if (tv.FindNode("entities").ChildNodes.Count == 0)
                tv.Nodes.Remove(tv.FindNode("entities"));

            if (tv.FindNode("baseviews/baseviewsnew").ChildNodes.Count == 0)
                tv.FindNode("baseviews").ChildNodes.Remove(tv.FindNode("baseviews/baseviewsnew"));
            if (tv.FindNode("baseviews/baseviewsupdate").ChildNodes.Count == 0)
                tv.FindNode("baseviews").ChildNodes.Remove(tv.FindNode("baseviews/baseviewsupdate"));
            if (tv.FindNode("baseviews").ChildNodes.Count == 0)
                tv.Nodes.Remove(tv.FindNode("baseviews"));

            if (tv.FindNode("lists/listsnew").ChildNodes.Count == 0)
                tv.FindNode("lists").ChildNodes.Remove(tv.FindNode("lists/listsnew"));
            if (tv.FindNode("lists/listsupdate").ChildNodes.Count == 0)
                tv.FindNode("lists").ChildNodes.Remove(tv.FindNode("lists/listsupdate"));
            if (tv.FindNode("lists").ChildNodes.Count == 0)
                tv.Nodes.Remove(tv.FindNode("lists"));
        }

        protected void tv_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.ChildNodes.Count != 0)
                if (e.Node.Checked)
                    foreach (TreeNode item in e.Node.ChildNodes)
                    {
                        item.Checked = true;
                        tv_TreeNodeCheckChanged(this.tv, new TreeNodeEventArgs(item));
                    }
                else
                    foreach (TreeNode item in e.Node.ChildNodes)
                    {
                        item.Checked = false;
                        tv_TreeNodeCheckChanged(this.tv, new TreeNodeEventArgs(item));
                    }
        }

        public Export createObjectFromExport(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);
            Export ex = SynapseHelpers.DeserialiseExport(sr.ReadToEnd());
            sr.Close(); sr.Dispose();
            return ex;
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (Session["export"] == null)
            {
                lblError.Text = "There was an error generating export object. Please try again.";
                tv.Nodes.Clear();
                showFileUpload();
                return;
            }

            try
            {
                Export ex = (Export)Session["export"];

                //update export.object.action with the user consent 
                //foreach entity, baseview, list in export, check and update consent
                if (tv.Nodes.Count != 0)
                {
                    foreach (var item in ex.Entities.Entity)
                    {
                        TreeNode n = tv.FindNode("entities/entities" + item.action.ToString().ToLower() + "/" + item.Id);
                        if (n != null && !n.Checked)
                        {
                            if (item.action == SynapseHelpers.MigrationAction.Update)
                            {
                                List<SchemaMigration.Action> createActionsAttribute = item.attributeActions.Where(x => x.migrationAction == SynapseHelpers.MigrationAction.New && x.sourceContextId == item.Id && x.dbTableName.Equals("entitysettings.entityattribute")).ToList();
                                List<SchemaMigration.Action> createActionsRelations = item.attributeActions.Where(x => x.migrationAction == SynapseHelpers.MigrationAction.New && x.sourceContextId == item.Id && x.dbTableName.Equals("entitysettings.entityrelation")).ToList();


                                if (createActionsAttribute.Count != 0)
                                {
                                    foreach (var action in createActionsAttribute)
                                    {
                                        TreeNode n1 = tv.FindNode("entities/entitiesupdate/" + item.Id + "/newattributes/" + action.sourceComponentId);

                                        if (!n1.Checked)
                                            action.migrationAction = SynapseHelpers.MigrationAction.Skip;
                                    }
                                }
                                if (createActionsRelations.Count != 0)
                                {
                                    foreach (var action in createActionsRelations)
                                    {
                                        TreeNode n1 = tv.FindNode("entities/entitiesupdate/" + item.Id + "/newrelations/" + action.sourceComponentId);

                                        if (!n1.Checked)
                                            action.migrationAction = SynapseHelpers.MigrationAction.Skip;
                                    }
                                }
                            }

                            item.action = SynapseHelpers.MigrationAction.Skip;
                        }

                    }

                    foreach (Baseview b in ex.Baseviews.Baseview)
                    {

                        TreeNode n = tv.FindNode("baseviews/baseviews" + b.action.ToString().ToLower() + "/" + b.Id);

                        if (n != null && !n.Checked)
                        {
                            b.action = SynapseHelpers.MigrationAction.Skip;
                        }

                    }

                    foreach (SList l in ex.SLists.SList)
                    {
                        TreeNode n = tv.FindNode("lists/lists" + l.action.ToString().ToLower() + "/" + l.Id);

                        if (n != null && !n.Checked)
                        {
                            l.action = SynapseHelpers.MigrationAction.Skip;
                        }

                    }

                    Session["export"] = ex;

                    Task task = new Task(() => SynapseHelpers.ApplySchemaChanges(ex));

                    task.Start();
                    showProcessMsgs();
                    //try
                    //{
                    //    Thread thread = new Thread(() => SynapseHelpers.ApplySchemaChanges(ex));
                    //    thread.Start();

                    //}catch(Exception exx)
                    //{
                    //    SynapseHelpers.AddImportMsg(exx.Message);
                    //}

                    //while (thread.ThreadState.Equals(ThreadState.Running))
                    //{
                    //    lblMsg.Text = DateTime.Now.Second.ToString();
                    //    Response.Flush();
                    //}


                    // SynapseHelpers.ApplySchemaChanges(ex);
                    tv.Nodes.Clear();
                }
                else
                {
                    lblErrorSelectObjects.Text = "Looks like there is nothing to import!";
                }
            }
            catch (Exception exception)
            {
                lblError.Text = "There was an error processing the export. Please try again. ";
                lblError.Text += exception.Message;
                tv.Nodes.Clear();
                showFileUpload();
            }
        }
        protected void UpdateTimer_Tick(object sender, EventArgs e)
        {

            List<ImportStatusMessage> s = SynapseHelpers.statusMessages.Where(x => x.seen == false).OrderBy(b => b.serialno).ToList<ImportStatusMessage>();

            foreach (var item in s)
            {
                //txtmsg.Text += "\n" + item.timestamp + " : " + item.message;
                //item.seen = true;
                string color = string.Empty;
                switch (item.type.ToLower())
                {
                    case "info":
                        color = "black";
                        break;
                    case "warning":
                        color = "yellow";
                        break;
                    case "error":
                        color = "red";
                        break;
                    case "success":
                        color = "green";
                        break;
                    default:
                        color = "black";
                        break;
                }

                divmsg.InnerHtml += "<br><font color=" + color + ">" + item.timestamp + " : " + item.message + "</font>";

                item.seen = true;

            }
        }

        protected void btnPreProcessExport_Click(object sender, EventArgs e)
        {
            if (!fImport.HasFile)
                lblError.Text = "Please select a Synapse Schema Export file to process";
            else
            {
                lblError.Text = string.Empty;
                showSelectObjects();
                Session.Remove("export");
                Export ex;
                try
                {
                    ex = createObjectFromExport(fImport.PostedFile.InputStream);
                    Session["Export"] = ex;
                    SynapseHelpers.CompareSchema(ex, SynapseHelpers.DataSetSerializerType.Json);
                    ConsentTreeView(ex);

                }
                catch (Exception ex1)
                {
                    Session.Remove("export");
                    lblError.Text = ex1.Message;
                    lblError.Text += "Please try a different export";
                    showFileUpload();
                }

            }
        }

        protected void btnOtherFile_Click(object sender, EventArgs e)
        {
            lblErrorSelectObjects.Text = string.Empty;
            showFileUpload();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            showFileUpload();
        }
    }
}