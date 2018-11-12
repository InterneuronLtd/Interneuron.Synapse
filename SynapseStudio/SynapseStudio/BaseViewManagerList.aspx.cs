//Interneuron Synapse

//Copyright(C) 2018  Interneuron CIC

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
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class BaseViewManagerList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownList(this.ddlSynapseNamespace, "SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace", "baseviewnamespaceid", "baseviewnamespace", 0);

                BindEntityGrid();
            }
        }

        private void BindEntityGrid()
        {
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @baseviewnamespaceid ORDER BY baseviewname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseviewnamespaceid", this.ddlSynapseNamespace.SelectedValue)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            this.dgBaseviews.DataSource = dt;
            this.dgBaseviews.DataBind();

            this.lblResultCount.Text = dt.Rows.Count.ToString();
            this.lblNamespaceName.Text = this.ddlSynapseNamespace.SelectedItem.Text.ToUpper();

            this.btnCreateNew.Text = "New " + this.ddlSynapseNamespace.SelectedItem.Text + " Base View";
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

        protected void btnCreateNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseviewManagerNew.aspx?id=" + this.ddlSynapseNamespace.SelectedValue);
        }

        protected void btnManageLocalNamespaces_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseviewNamespaceManager.aspx");
        }
    }
}