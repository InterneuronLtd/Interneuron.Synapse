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
﻿

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class EntityManagerList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindDropDownList(this.ddlSynapseNamespace, "SELECT * FROM entitysettings.synapsenamespace ORDER BY synapsenamespacename", "synapsenamespaceid", "synapsenamespacename", 0);

                BindEntityGrid();
            }
        }

        private void BindEntityGrid()
        {
            string sql = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid = @synapsenamespaceid ORDER BY entityname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("synapsenamespaceid", this.ddlSynapseNamespace.SelectedValue)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            this.dgEntities.DataSource = dt;
            this.dgEntities.DataBind();

            this.lblResultCount.Text = dt.Rows.Count.ToString();
            this.lblNamespaceName.Text = this.ddlSynapseNamespace.SelectedItem.Text.ToUpper();

            this.btnCreateNewEntity.Text = "New " + this.ddlSynapseNamespace.SelectedItem.Text + " entity";
        }

        //Drop Down Lists
        private void BindDropDownList(DropDownList ddl, string sql, string valueField, string displayField, int addPleaseSelect)
        {
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ddl.DataSource = ds;
            ddl.DataValueField = valueField;
            ddl.DataTextField = displayField;
            ddl.DataBind();

            if (addPleaseSelect == 1)
            {
                ListItem[] items = new ListItem[1];
                items[0] = new ListItem("Please select ...", "0");
                ddl.Items.Insert(0, items[0]);
            }
        }

        private void SetDDLSource(DropDownList ddl, string val)
        {
            if (val.Length > 0)
            {
                int idx = 9999;

                try
                {
                    idx = ddl.Items.IndexOf(ddl.Items.FindByValue(val));
                }
                catch
                {
                    idx = 9999;
                }

                if (idx == 9999 || idx < 0)
                {
                    ListItem[] items = new ListItem[1];
                    items[0] = new ListItem(val + " (old value)", val);
                    ddl.Items.Insert(1, items[0]);
                }
            }

            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(val));
        }

        //RadioButtonList
        private void BindRadioList(RadioButtonList rad, string sql, string valueField, string displayField)
        {
            DataSet ds = DataServices.DataSetFromSQL(sql);
            rad.DataSource = ds;
            rad.DataValueField = valueField;
            rad.DataTextField = displayField;
            rad.DataBind();

        }

        private void SetRadioSource(RadioButtonList rad, string val)
        {
            rad.SelectedIndex = rad.Items.IndexOf(rad.Items.FindByValue(val));
        }

        protected void ddlSynapseNamespace_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEntityGrid();
        }

        protected void btnCreateNewEntity_Click(object sender, EventArgs e)
        {
            switch(this.ddlSynapseNamespace.SelectedItem.Text.ToLower())
            {
                case "core":
                    Response.Redirect("EntityManagerNew.aspx?id=" + this.ddlSynapseNamespace.SelectedValue);
                    break;
                case "extended":
                    Response.Redirect("EntityManagerNewExtended.aspx?id=" + this.ddlSynapseNamespace.SelectedValue);
                    break;
                case "local":
                    Response.Redirect("EntityManagerNewLocal.aspx?id=" + this.ddlSynapseNamespace.SelectedValue);
                    break;
                case "meta":
                    Response.Redirect("EntityManagerNewMeta.aspx?id=" + this.ddlSynapseNamespace.SelectedValue);
                    break;
                case "master":
                    Response.Redirect("EntityManagerNew.aspx?id=" + this.ddlSynapseNamespace.SelectedValue);
                    break;
                default:
                    Response.Redirect("EntityManagerNew.aspx?id=" + this.ddlSynapseNamespace.SelectedValue);
                    break;
            }
            
        }
    }
}