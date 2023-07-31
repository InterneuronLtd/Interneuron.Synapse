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


﻿

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class ListManagerNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = "";
                try
                {
                    id = Request.QueryString["id"].ToString();
                }
                catch
                {
                    Response.Redirect("Error.aspx");
                }


                if (String.IsNullOrEmpty(id))
                {
                    Response.Redirect("Error.aspx");
                }

                this.hdnNamespaceID.Value = id;

                string local = "";
                try
                {
                    local = Request.QueryString["local"].ToString();
                }
                catch
                {
                }

                this.hdnLocalNamespaceID.Value = local;


                  BindGrid();
                string name = "";
                try
                {
                    name = SynapseHelpers.GetListNamespaceNameFromID(id);
                }
                catch { }

                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }

                if (String.IsNullOrEmpty(name))
                {
                    Response.Redirect("Error.aspx");
                }

                BindDropDownList(this.ddlBaseViewNamespace, "SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace", "baseviewnamespaceid", "baseviewnamespace", 0, null);
                BindBaseViewList();

                BindDropDownList(this.ddlDefaultContext, "SELECT entityid, synapsenamespacename || '.' || entityname as entitydisplayname, keycolumn FROM entitysettings.entitymanager order by 2", "entityid", "entitydisplayname", 1, null);
                BindBaseViewContextFields();



                this.lblNamespaceName.Text = name;

                this.lblError.Text = string.Empty;
                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;
                this.btnCreateNewList.Visible = false;
                this.btnCancel.Visible = false;
            }
        }

        private void BindBaseViewList()
        {
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @id ORDER BY baseviewname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", this.ddlBaseViewNamespace.SelectedValue)
            };
            BindDropDownList(this.ddlBaseView, sql, "baseview_id", "baseviewname", 1, paramList);

        }


        private void BindBaseViewContextFields()
        {
            string sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id ORDER BY attributename;";
            string sqlpersona = "SELECT persona_id, displayname, personaname	FROM entitystorematerialised.meta_persona;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", this.ddlBaseView.SelectedValue)
            };
            BindDropDownList(this.ddlPatientBannerField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlMatchedContextField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlRowCSSField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.DDLpersonaField, sqlpersona, "persona_id", "displayname", 1, paramList);
            BindDropDownList(this.DDlbaseviewfield, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlWardPersonaContextField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlCUPersonaContextField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlSpecialtyPersonaContextField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlTeamPersonaContextField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlSnapshotLine1, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlSnapshotLine2, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlSnapshotBadge, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlDefaultSortColumn, sql, "attributename", "attributename", 1, paramList);
        }



        //Drop Down Lists
        private void BindDropDownList(DropDownList ddl, string sql, string valueField, string displayField, int addPleaseSelect, List<KeyValuePair<string, string>> parameters = null)
        {
            DataSet ds = DataServices.DataSetFromSQL(sql, parameters);
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

        protected void btnValidateList_Click(object sender, EventArgs e)
        {

            //this.txtListName.Text = RemoveAndReplaceSpecialCharacters(this.txtListName.Text);
            this.btnValidateList.Visible = true;
            this.txtListName.Enabled = true;
            this.ddlBaseView.Enabled = true;
            this.ddlBaseViewNamespace.Enabled = true;
            this.txtListComments.Enabled = true;


            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;

            this.fgListName.CssClass = noerr;
            if (string.IsNullOrEmpty(this.txtListName.Text.ToString()))
            {
                this.lblError.Text = "Please enter a new name for the List";
                this.txtListName.Focus();
                this.lblError.Visible = true;
                this.fgListName.CssClass = haserr;
                return;
            }

            this.fgBaseView.CssClass = noerr;
            if (this.ddlBaseView.SelectedIndex == 0)
            {
                this.lblError.Text = "Please select the associated baseview for this list";
                this.ddlBaseView.Focus();
                this.lblError.Visible = true;
                this.fgBaseView.CssClass = haserr;
                return;
            }

            this.fgDefaultContext.CssClass = noerr;
            if (this.ddlDefaultContext.SelectedIndex == 0)
            {
                this.lblError.Text = "Please select the entity that defines the default context";
                this.ddlDefaultContext.Focus();
                this.lblError.Visible = true;
                this.fgDefaultContext.CssClass = haserr;
                return;
            }

            //this.fgPatientBannerField.CssClass = noerr;
            //if (this.ddlBaseView.SelectedIndex == 0)
            //{
            //    this.lblError.Text = "Please select the patient banner field";
            //    this.ddlPatientBannerField.Focus();
            //    this.lblError.Visible = true;
            //    this.fgPatientBannerField.CssClass = haserr;
            //    return;
            //}

            this.fgMatchedContextField.CssClass = noerr;
            if (this.ddlMatchedContextField.SelectedIndex == 0)
            {
                this.lblError.Text = "Please select the field from the baseview that matches the default context key defined";
                this.ddlMatchedContextField.Focus();
                this.lblError.Visible = true;
                this.fgMatchedContextField.CssClass = haserr;
                return;
            }


            //this.fgRowCSSField.CssClass = noerr;
            //if (this.ddlRowCSSField.SelectedIndex == 0)
            //{
            //    this.lblError.Text = "Please select the row css field";
            //    this.ddlRowCSSField.Focus();
            //    this.lblError.Visible = true;
            //    this.fgRowCSSField.CssClass = haserr;
            //    return;               
            //}

            this.lblSuccess.Visible = true;
            this.lblSuccess.Text = "Validation succeeded";
            this.btnCreateNewList.Visible = true;
            this.btnCancel.Visible = true;
            this.btnCreateNewList.Focus();

            this.btnValidateList.Visible = false;
            this.txtListName.Enabled = false;
            this.ddlBaseView.Enabled = false;
            this.ddlBaseViewNamespace.Enabled = false;
            this.txtListComments.Enabled = false;




        }

        public static string RemoveBadChars(string word)
        {
            char[] chars = new char[word.Length];
            int myindex = 0;
            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];

                if ((int)c >= 65 && (int)c <= 90)
                {
                    chars[myindex] = c;
                    myindex++;
                }
                else if ((int)c >= 97 && (int)c <= 122)
                {
                    chars[myindex] = c;
                    myindex++;
                }
                else if ((int)c == 44)
                {
                    chars[myindex] = c;
                    myindex++;
                }
            }

            word = new string(chars);

            return word;
        }

        string RemoveAndReplaceSpecialCharacters(string str)
        {
            str = str.Replace(" ", String.Empty);

            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            str = rgx.Replace(str, "");

            return str.ToLower();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtListName.Text = string.Empty;
            this.txtListName.Enabled = true;
            this.txtListName.Focus();
            this.btnValidateList.Visible = true;
            this.btnCreateNewList.Visible = false;
            this.btnCancel.Visible = false;
            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;

            string noerr = "form-group";
            this.fgListName.CssClass = noerr;

        }

        protected void btnCreateNewList_Click(object sender, EventArgs e)
        {

            CreateNewList();

        }

        private void CreateNewList()
        {
            string sql = "INSERT INTO listsettings.listmanager(list_id, listname, listdescription, listcontextkey, baseview_id, listnamespaceid, " +
                         "listnamespace, defaultcontext, defaultcontextfield, matchedcontextfield, tablecssstyle, tableheadercssstyle, defaultrowcssstyle, " +
                         "patientbannerfield, rowcssfield, wardpersonacontextfield, clinicalunitpersonacontextfield, specialtypersonacontextfield, teampersonacontextfield, snapshottemplateline1, " +
                         "snapshottemplateline2, snapshottemplatebadge, defaultsortcolumn, defaultsortorder) " +
                         "VALUES (@list_id, @listname, @listdescription, @listcontextkey, @baseview_id, @listnamespaceid, " +
                         "@listnamespace, @defaultcontext, @defaultcontextfield, @matchedcontextfield, @tablecssstyle, @tableheadercssstyle, @defaultrowcssstyle, @patientbannerfield, @rowcssfield, " +
                         "@wardpersonacontextfield, @clinicalunitpersonacontextfield, @specialtypersonacontextfield, @teampersonacontextfield, @snapshottemplateline1, @snapshottemplateline2, @snapshottemplatebadge, " +
                         "@defaultsortcolumn, @defaultsortorder)";

            string newId = System.Guid.NewGuid().ToString();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", newId),
                new KeyValuePair<string, string>("listname", this.txtListName.Text),
                new KeyValuePair<string, string>("listdescription", this.txtListComments.Text),
                new KeyValuePair<string, string>("listcontextkey", ""),
                new KeyValuePair<string, string>("baseview_id", this.ddlBaseView.SelectedValue),
                new KeyValuePair<string, string>("listnamespaceid", this.hdnNamespaceID.Value),
                new KeyValuePair<string, string>("listnamespace", SynapseHelpers.GetListNamespaceNameFromID(this.hdnNamespaceID.Value)),
                new KeyValuePair<string, string>("defaultcontext", this.ddlDefaultContext.SelectedValue),
                new KeyValuePair<string, string>("defaultcontextfield", this.lblDefaultContextField.Text),
                new KeyValuePair<string, string>("matchedcontextfield", this.ddlMatchedContextField.SelectedValue),
                new KeyValuePair<string, string>("patientbannerfield", this.ddlPatientBannerField.SelectedValue),
                new KeyValuePair<string, string>("rowcssfield", this.ddlRowCSSField.SelectedValue),
                new KeyValuePair<string, string>("tablecssstyle", this.txtTableClass.Text),
                new KeyValuePair<string, string>("tableheadercssstyle", this.txtTableHeaderClass.Text),
                new KeyValuePair<string, string>("defaultrowcssstyle", this.txtDefaultTableRowCSS.Text),
                new KeyValuePair<string, string>("wardpersonacontextfield", this.ddlWardPersonaContextField.SelectedValue == "0" ? null : this.ddlWardPersonaContextField.SelectedValue),
                new KeyValuePair<string, string>("clinicalunitpersonacontextfield", this.ddlCUPersonaContextField.SelectedValue == "0" ? null : this.ddlCUPersonaContextField.SelectedValue),
                new KeyValuePair<string, string>("specialtypersonacontextfield", this.ddlSpecialtyPersonaContextField.SelectedValue == "0" ? null : this.ddlSpecialtyPersonaContextField.SelectedValue),
                new KeyValuePair<string, string>("teampersonacontextfield", this.ddlTeamPersonaContextField.SelectedValue == "0" ? null : this.ddlTeamPersonaContextField.SelectedValue),
                new KeyValuePair<string, string>("snapshottemplateline1", this.ddlSnapshotLine1.SelectedValue == "0" ? null : this.ddlSnapshotLine1.SelectedValue),
                new KeyValuePair<string, string>("snapshottemplateline2", this.ddlSnapshotLine2.SelectedValue == "0" ? null : this.ddlSnapshotLine2.SelectedValue),
                new KeyValuePair<string, string>("snapshottemplatebadge", this.ddlSnapshotBadge.SelectedValue == "0" ? null : this.ddlSnapshotBadge.SelectedValue),
                new KeyValuePair<string, string>("defaultsortcolumn", this.ddlDefaultSortColumn.SelectedValue == "0" ? null : this.ddlDefaultSortColumn.SelectedValue),
                // Don't save defaultsortorder if defaultsortcolumn is not selected
                new KeyValuePair<string, string>("defaultsortorder", this.ddlDefaultSortColumn.SelectedValue == "0" ? null : this.ddlDefaultSortOrder.SelectedValue)

            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
                updateTeminusFilter(newId);
            }
            catch (Exception ex)
            {
                this.lblError.Text = ex.ToString();
                this.lblError.Visible = true;
                return;
            }


            Response.Redirect("ListManagerAttributes.aspx?id=" + newId);
        }

        protected void ddlBaseViewNamespace_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBaseViewList();
        }

        protected void ddlDefaultContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblDefaultContextField.Text = SynapseHelpers.GetKeyColumnForEntity(this.ddlDefaultContext.SelectedValue);
            BindBaseViewContextFields();
        }

        public void updateTeminusFilter(string listid)
        {

            DataTable dt = (DataTable)Session["personfilterTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string sqlAddfilter = "INSERT INTO entitystorematerialised.meta_listcontexts(listcontexts_id,persona_id, field, list_id) " +

                            "VALUES (@listcontexts_id, @persona_id, @field, @list_id) ";

                string newId = System.Guid.NewGuid().ToString();
                var paramListAddlist = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listcontexts_id", newId),
                new KeyValuePair<string, string>("persona_id",dt.Rows[i]["persona_id"].ToString()),
                new KeyValuePair<string, string>("field", dt.Rows[i]["field"].ToString()),
                new KeyValuePair<string, string>("list_id", dt.Rows[i]["list_id"].ToString())       };

                DataServices.ExcecuteNonQueryFromSQL(sqlAddfilter, paramListAddlist);
            }
        }

        private void BindGrid()
        {
          
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id","newid")
            };
            string pesonaListsql = "SELECT mp.displayname,lc.persona_id, field, list_id	FROM entitystorematerialised.meta_listcontexts lc join entitystorematerialised.meta_persona mp on mp.persona_id=lc.persona_id WHERE list_id = @list_id";


            DataSet dspersona = DataServices.DataSetFromSQL(pesonaListsql, paramList);
            DataTable dtpersona = dspersona.Tables[0];
            Session["personfilterTable"] = dtpersona;
            this.dgpersonafilter.DataSource = dtpersona;
            this.dgpersonafilter.DataBind();

        }
        protected void ADDpersona_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["personfilterTable"];
            //dt =(DataTable)this.dgpersonafilter.DataSource;
            if (this.DDLpersonaField.SelectedIndex != 0)
            {
                if (this.DDlbaseviewfield.SelectedIndex != 0)
                {
                    lblerrorfilter.Text = " ";
                    DataRow dr = dt.NewRow();
                    dr["displayname"] = this.DDLpersonaField.SelectedItem.Text;
                    dr["persona_id"] = this.DDLpersonaField.SelectedValue;
                    dr["field"] = this.DDlbaseviewfield.SelectedValue;
                    dr["list_id"] = Request.QueryString["id"].ToString();
                    dt.Rows.Add(dr);
                    this.DDLpersonaField.Items.RemoveAt(this.DDLpersonaField.SelectedIndex);
                    this.DDlbaseviewfield.Items.RemoveAt(this.DDlbaseviewfield.SelectedIndex);
                }
                else
                {
                    lblerrorfilter.Text = "Please Select BaseView field ";
                }
            }
            else
            {
                lblerrorfilter.Text = "Please Select Persona";
            }
            this.dgpersonafilter.DataSource = dt;
            this.dgpersonafilter.DataBind();


        }

        protected void dgpersonafilter_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int i = e.Item.ItemIndex;
            DataTable dt = (DataTable)Session["personfilterTable"];
            dt.Rows.RemoveAt(i);
            this.DDLpersonaField.Items.Add(new ListItem(e.Item.Cells[0].Text, e.Item.Cells[1].Text));
            this.DDlbaseviewfield.Items.Add(new ListItem(e.Item.Cells[2].Text, e.Item.Cells[2].Text));
            this.dgpersonafilter.DataSource = dt;
            this.dgpersonafilter.DataBind();
        }

    }
}