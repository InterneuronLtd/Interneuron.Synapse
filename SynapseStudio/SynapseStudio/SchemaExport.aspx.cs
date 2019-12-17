//Interneuron Synapse

//Copyright(C) 2019  Interneuron CIC

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using SynapseStudio.SchemaMigration;

namespace SynapseStudio
{
    public partial class SchemaExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                showTree();
        }

        protected void showTree()
        {
            //Entities

            tv.Nodes.Clear();
            TreeNode entitiesrootnode = new TreeNode("Entities", "entities");
            tv.ShowCheckBoxes = TreeNodeTypes.All;
            TreeNode baseviewsrootnode = new TreeNode("Baseviews", "baseviews");
            TreeNode listsrootnode = new TreeNode("Lists", "lists");

            tv.Nodes.Add(entitiesrootnode);
            tv.Nodes.Add(baseviewsrootnode);
            tv.Nodes.Add(listsrootnode);

            tv.CollapseAll();

            string sql = @"SELECT entityid as id,entityname as name from entitysettings.entitymanager order by entityname";
            DataTable dt = new DataTable();
            try
            {
                dt = DataServices.DataSetFromSQL(sql).Tables[0];
            }
            catch (Exception ex)
            {

            }
            AddNodes(dt, tv.FindNode("entities"));

            sql = @"SELECT baseview_id as id,baseviewname as name from listsettings.baseviewmanager order by baseviewname";
            dt = new DataTable();
            try
            {
                dt = DataServices.DataSetFromSQL(sql).Tables[0];
            }
            catch (Exception ex)
            {

            }
            AddNodes(dt, tv.FindNode("baseviews"));

            sql = @"SELECT list_id as id,listname as name from listsettings.listmanager order by listname";
            dt = new DataTable();
            try
            {
                dt = DataServices.DataSetFromSQL(sql).Tables[0];
            }
            catch (Exception ex)
            {

            }
            AddNodes(dt, tv.FindNode("lists"));




        }

        protected void AddNodes(DataTable dt, TreeNode parent)
        {
            foreach (DataRow item in dt.Rows)
            {
                TreeNode node = new TreeNode();
                node.ShowCheckBox = true;
                node.Text = item["name"].ToString();
                node.Value = item["id"].ToString();
                parent.ChildNodes.Add(node);
            }
        }

        public void createExport()
        {

            Export exp = new Export();
            exp.format = SynapseHelpers.DataSetSerializerType.Json;

            foreach (TreeNode item in tv.FindNode("entities").ChildNodes)
            {
                if (item.Checked)
                {
                    string entityid = item.Value;
                    exp.Entities.Entity.Add(getEntityByIdForExport(entityid));
                }
            }
            foreach (TreeNode item in tv.FindNode("baseviews").ChildNodes)
            {
                if (item.Checked)
                {
                    string baseviewid = item.Value;
                    exp.Baseviews.Baseview.Add(getBaseViewByIdForExport(baseviewid));
                }
            }

            foreach (TreeNode item in tv.FindNode("lists").ChildNodes)
            {
                if (item.Checked)
                {
                    string listid = item.Value;
                    exp.SLists.SList.Add(getListByIdForExport(listid));
                }
            }

            #region oldcode

            //string sql = @"SELECT entityid,entityname from entitysettings.entitymanager limit 1";

            //DataTable dt = new DataTable();

            //try
            //{
            //    dt = DataServices.DataSetFromSQL(sql).Tables[0];
            //}
            //catch (Exception ex)
            //{

            //}

            //if (dt.Rows.Count > 0)
            //{
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        string entityid = row["entityid"].ToString();
            //        string entityname = row["entityname"].ToString();
            //        exp.Entities.Entity.Add(getEntityByIdForExport(entityid));
            //    }
            //}

            //sql = @"SELECT baseview_id,baseviewname from listsettings.baseviewmanager limit 1";

            //dt = new DataTable();

            //try
            //{
            //    dt = DataServices.DataSetFromSQL(sql).Tables[0];
            //}
            //catch (Exception ex)
            //{

            //}

            //if (dt.Rows.Count > 0)
            //{
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        string baseviewid = row["baseview_id"].ToString();
            //        exp.Baseviews.Baseview.Add(getBaseViewByIdForExport(baseviewid));
            //    }
            //}


            //sql = @"SELECT list_id,listnamespace from listsettings.listmanager";

            //dt = new DataTable();

            //try
            //{
            //    dt = DataServices.DataSetFromSQL(sql).Tables[0];
            //}
            //catch (Exception ex)
            //{

            //}
            //if (dt.Rows.Count > 0)
            //{
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        string listid = row["list_id"].ToString();
            //        string listname = row["listnamespace"].ToString();
            //        exp.SLists.SList.Add(getListByIdForExport(listid));
            //    }
            //}

            #endregion

            string xml = exp.SerializeToXML();
            //StreamWriter wr = new StreamWriter("c:\\export" + DateTime.Now.Ticks + ".xml");
            //wr.Write(xml);
            //wr.Close(); wr.Dispose();

            DownloadFile(xml);
        }

        protected void DownloadFile(string text)
        {
            Response.Clear();
            Response.ClearHeaders();

            Response.AppendHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "text/plain";
            Response.AppendHeader("Content-Disposition", "attachment;filename=\"SynapseExport" + DateTime.Now.Ticks + ".xml\"");

            Response.Write(text);
            Response.End();
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

        public Export createObjectFromExport(string filename)
        {
            StreamReader sr = new StreamReader("c:\\export\\" + filename);
            Export ex = SynapseHelpers.DeserialiseExport(sr.ReadToEnd());
            sr.Close(); sr.Dispose();

            return ex;
        }

        protected void tv_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.Value.Equals("entities") || e.Node.Value.Equals("baseviews") || e.Node.Value.Equals("lists"))
            {
                if (e.Node.Checked)
                    foreach (TreeNode item in e.Node.ChildNodes)
                    {
                        item.Checked = true;
                    }
                else
                    foreach (TreeNode item in e.Node.ChildNodes)
                    {
                        item.Checked = false;
                    }

            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            createExport();
            showTree();
        }

        protected void btnNavigateToImport_Click(object sender, EventArgs e)
        {
            Response.Redirect("SchemaImport.aspx");
        }
    }
}