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
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class EntityManagerRelations : System.Web.UI.Page
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

                this.hdnEntityID.Value = id;

                try
                {

                }
                catch { }

                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }


                this.lblSummaryType.Text = SynapseHelpers.GetEntityNameAndNamespaceFromID(id);
                this.hdnNextOrdinalPosition.Value = SynapseHelpers.GetNextOrdinalPositionFromID(id);

                this.lblError.Text = string.Empty;
                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;
                this.btnCreateNewAttribute.Visible = false;
                this.btnCancel.Visible = false;
                
                BindDropDownList(this.ddlSynapseNamespace, "SELECT * FROM entitysettings.synapsenamespace WHERE synapsenamespaceid <> 'e8b78b52-641d-46eb-bb8b-16e2feb86fe7' ORDER BY synapsenamespacename", "synapsenamespaceid", "synapsenamespacename", 1);
                BindDropDownList(this.ddlEntity, "SELECT entityid, entityname FROM entitysettings.entitymanager WHERE synapsenamespaceid = '" + this.ddlSynapseNamespace.SelectedValue + "' ORDER BY entityname", "entityid", "entityname", 1);

                BindDropDownListNone(this.ddlLocalAttribute, "SELECT entityid, attributeid, attributename FROM entitysettings.entityattribute WHERE entityid='" + this.hdnEntityID.Value + "' AND coalesce(isrelationattribute,0) = 0 ORDER BY attributename;", "attributeid", "attributename", 1);

                BindEntityGrid();

                //this.btnCreateNewAttribute.Attributes.Add("onclick", "if(confirm('Are you sure that you want to add the attribute as defined? This cannot be undone!!')){return true;} else {return false;};");                

            }
        }

        protected void ddlSynapseNamespace_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownList(this.ddlEntity, "SELECT entityid, entityname FROM entitysettings.entitymanager WHERE synapsenamespaceid = '" + this.ddlSynapseNamespace.SelectedValue + "' ORDER BY entityname", "entityid", "entityname", 1);
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

        private void BindEntityGrid()
        {
            string sql = "SELECT * FROM entitysettings.v_relationsattributes WHERE entityid = @entityid ORDER BY entityname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", this.hdnEntityID.Value)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            this.dgEntities.DataSource = dt;
            this.dgEntities.DataBind();

            this.lblResultCount.Text = dt.Rows.Count.ToString();

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

        private void BindDropDownListNone(DropDownList ddl, string sql, string valueField, string displayField, int addPleaseSelect)
        {
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ddl.DataSource = ds;
            ddl.DataValueField = valueField;
            ddl.DataTextField = displayField;
            ddl.DataBind();

            if (addPleaseSelect == 1)
            {
                ListItem[] items = new ListItem[1];
                items[0] = new ListItem("No Local Attribute", "0");
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

        protected void btnValidateAttribute_Click(object sender, EventArgs e)
        {

            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;
            this.fgEntity.CssClass = noerr;
            this.fgSynapseNamespace.CssClass = noerr;


            if (this.ddlSynapseNamespace.SelectedIndex == 0)
            {
                this.lblError.Text = "Please select a namespace";
                this.ddlSynapseNamespace.Focus();
                this.lblError.Visible = true;
                this.fgSynapseNamespace.CssClass = haserr;
                return;
            }

            if (this.ddlEntity.SelectedIndex == 0)
            {
                this.lblError.Text = "Please select an entity";
                this.ddlEntity.Focus();
                this.lblError.Visible = true;
                this.fgEntity.CssClass = haserr;
                return;
            }

            if(this.ddlEntity.SelectedValue == this.hdnEntityID.Value)
            {
                this.lblError.Text = "You are unable to create a relation to the same entity";
                this.ddlEntity.Focus();
                this.lblError.Visible = true;
                this.fgEntity.CssClass = haserr;
                return;
            }

            string sql = "";            
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", this.hdnEntityID.Value),
                new KeyValuePair<string, string>("parententityid", this.ddlEntity.SelectedValue)
            };
            if (this.ddlLocalAttribute.SelectedIndex == 0)
            {
                sql = "SELECT * FROM entitysettings.entityrelation WHERE entityid = @entityid and parententityid = @parententityid;";                
            }
            else
            {
                sql = "SELECT * FROM entitysettings.entityrelation WHERE entityid = @entityid and parententityid = @parententityid and localentityattributeid = @localentityattributeid;";
                paramList.Add(new KeyValuePair<string, string>("localentityattributeid", this.ddlLocalAttribute.SelectedValue));
            }
            

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                this.lblError.Text = "The relation that you are trying to create already exists for this entity";
                this.ddlEntity.Focus();
                this.lblError.Visible = true;
                this.fgEntity.CssClass = haserr;
                return;
            }


            this.lblSuccess.Visible = true;
            this.lblSuccess.Text = "Validation succeeded";
            this.btnCreateNewAttribute.Visible = true;
            this.btnCancel.Visible = true;
            this.btnCreateNewAttribute.Focus();
            this.btnValidateAttribute.Visible = false;
            this.ddlEntity.Enabled = false;
            this.ddlSynapseNamespace.Enabled = false;

            CreateRelation();
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {

            ClearAttributesForm();

        }


        private void ClearAttributesForm()
        {
            this.ddlEntity.SelectedIndex = 0;
            this.ddlEntity.Enabled = true;
            this.ddlEntity.Focus();
            this.btnValidateAttribute.Visible = true;
            this.btnCreateNewAttribute.Visible = false;
            this.btnCancel.Visible = false;
            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;
            this.lblSuccess.Text = string.Empty;

            this.ddlSynapseNamespace.Enabled = true;
            this.ddlSynapseNamespace.SelectedIndex = 0;
            this.ddlLocalAttribute.SelectedIndex = 0;

            string noerr = "form-group";
            this.fgEntity.CssClass = noerr;
            this.fgSynapseNamespace.CssClass = noerr;
        }

        protected void btnCreateNewAttribute_Click(object sender, EventArgs e)
        {
            CreateRelation();
        }


        private void CreateRelation()
        {
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




            DataTable dt = SynapseHelpers.GetEntityDSFromID(this.hdnEntityID.Value);
            DataTable dtParent = SynapseHelpers.GetEntityDSFromID(this.ddlEntity.SelectedValue);
            DataTable dtKey = SynapseHelpers.GetEntityKeyAttributeFromID(this.ddlEntity.SelectedValue);

            string attributename = dtKey.Rows[0]["attributename"].ToString();
            if (this.ddlLocalAttribute.SelectedIndex > 0)
            {
                attributename += "_" + this.ddlLocalAttribute.SelectedItem.Text;
            }
            

            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_entityid", this.hdnEntityID.Value),
                new KeyValuePair<string, string>("p_entityname", SynapseHelpers.GetEntityNameFromID(this.hdnEntityID.Value)),
                new KeyValuePair<string, string>("p_synapsenamespaceid", dt.Rows[0]["synapsenamespaceid"].ToString()),
                new KeyValuePair<string, string>("p_synapsenamespacename", dt.Rows[0]["synapsenamespacename"].ToString()),
                new KeyValuePair<string, string>("p_parententityid", this.ddlEntity.SelectedValue),
                new KeyValuePair<string, string>("p_parententityname", this.ddlEntity.SelectedItem.Text),
                new KeyValuePair<string, string>("p_parentsynapsenamespaceid", dtParent.Rows[0]["synapsenamespaceid"].ToString()),
                new KeyValuePair<string, string>("p_parentsynapsenamespacename", dtParent.Rows[0]["synapsenamespacename"].ToString()),
                new KeyValuePair<string, string>("p_attributeid", dtKey.Rows[0]["attributeid"].ToString()),
                new KeyValuePair<string, string>("p_attributename", attributename),
                new KeyValuePair<string, string>("p_ordinal_position", this.hdnNextOrdinalPosition.Value),
                new KeyValuePair<string, string>("p_entityversionid", SynapseHelpers.GetCurrentEntityVersionFromID(this.hdnEntityID.Value)),
                new KeyValuePair<string, string>("p_username", this.hdnUserName.Value),
                new KeyValuePair<string, string>("p_localentityattributeid", this.ddlLocalAttribute.SelectedValue),
                new KeyValuePair<string, string>("p_localentityattributename", this.ddlLocalAttribute.SelectedItem.Text)
            };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

            ClearAttributesForm();            
            this.hdnNextOrdinalPosition.Value = SynapseHelpers.GetNextOrdinalPositionFromID(this.hdnEntityID.Value);
            BindEntityGrid();

            this.lblSuccess.Text = "Relation created";
            this.lblSuccess.Visible = true;
        }

        protected void btnManageAttributes_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerAttributes.aspx?action=view&id=" + this.hdnEntityID.Value);
        }

        protected void btnManageRelations_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerRelations.aspx?action=view&id=" + this.hdnEntityID.Value);
        }

        protected void btnManageAPI_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerAPIs.aspx?action=view&id=" + this.hdnEntityID.Value);
        }

        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerView.aspx?action=view&id=" + this.hdnEntityID.Value);
        }

        protected void btnManageModels_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerModels.aspx?action=view&id=" + this.hdnEntityID.Value);
        }
    }
}