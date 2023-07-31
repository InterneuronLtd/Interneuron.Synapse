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
    public partial class EntityManagerAttributes : System.Web.UI.Page
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

                BindDropDownList(this.ddlDataType, "SELECT * FROM entitysettings.systemdatatype WHERE availabletoenduser = true ORDER BY orderby", "datatypeID", "datatypedisplay", 1);
                BindEntityGrid();

                //this.btnCreateNewAttribute.Attributes.Add("onclick", "if(confirm('Are you sure that you want to add the attribute as defined? This cannot be undone!!')){return true;} else {return false;};");                
               
            }
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
            string sql = "SELECT * FROM entitysettings.v_entityattribute WHERE entityid = @entityid ORDER BY ordinal_position;";
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

            this.txtAttributeName.Text = RemoveAndReplaceSpecialCharacters(this.txtAttributeName.Text);
            this.txtAttributeName.Enabled = true;

            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;
            this.fgAttributeName.CssClass = noerr;
            this.fgDataType.CssClass = noerr;

            if (string.IsNullOrEmpty(this.txtAttributeName.Text.ToString()))
            {
                this.lblError.Text = "Please enter a new name for the attribute";
                this.txtAttributeName.Focus();
                this.lblError.Visible = true;
                this.fgAttributeName.CssClass = haserr;
                return;
            }




            char stringFirstCharacter = this.txtAttributeName.Text.ToCharArray().ElementAt(0);
            if (char.IsNumber(stringFirstCharacter))
            {
                this.lblError.Text = "Attribute name cannot start with numeric characters";
                this.txtAttributeName.Focus();
                this.lblError.Visible = true;
                this.fgAttributeName.CssClass = haserr;
                return;
            }

            if (this.ddlDataType.SelectedIndex == 0)
            {
                this.lblError.Text = "Please select a data type";
                this.ddlDataType.Focus();
                this.lblError.Visible = true;
                this.fgDataType.CssClass = haserr;
                return;
            }


            string sql = "SELECT * FROM entitysettings.entityattribute WHERE entityid = @entityid and attributename = @attributename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", this.hdnEntityID.Value),
                new KeyValuePair<string, string>("attributename", this.txtAttributeName.Text)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                this.lblError.Text = "The name of the attribute that you you have entered already exists in this entitiy";
                this.txtAttributeName.Focus();
                this.lblError.Visible = true;
                this.fgAttributeName.CssClass = haserr;
                return;
            }


            this.lblSuccess.Visible = true;
            this.lblSuccess.Text = "Validation succeeded";
            this.btnCreateNewAttribute.Visible = true;
            this.btnCancel.Visible = true;
            this.btnCreateNewAttribute.Focus();
            this.btnValidateAttribute.Visible = false;
            this.txtAttributeName.Enabled = false;
            this.ddlDataType.Enabled = false;




            CreateNewAttribute();
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {

            ClearAttributesForm();

        }


        private void ClearAttributesForm()
        {
            this.txtAttributeName.Text = string.Empty;
            this.txtAttributeName.Enabled = true;
            this.txtAttributeName.Focus();
            this.btnValidateAttribute.Visible = true;
            this.btnCreateNewAttribute.Visible = false;
            this.btnCancel.Visible = false;
            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;
            this.lblSuccess.Text = string.Empty;
            this.ddlDataType.Enabled = true;
            this.ddlDataType.SelectedIndex = 0;

            string noerr = "form-group";
            this.fgAttributeName.CssClass = noerr;
            this.fgDataType.CssClass = noerr;


        }

        protected void btnCreateNewAttribute_Click(object sender, EventArgs e)
        {
            CreateNewAttribute();
        }


        private void CreateNewAttribute()
        {

            DataTable dtBVs = SynapseHelpers.GetEntityBaseviewsDT(this.hdnEntityID.Value);

            this.hdnDataType.Value = SynapseHelpers.GetDataTypeFromID(this.ddlDataType.SelectedValue);

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

            DataTable dt = SynapseHelpers.GetEntityDSFromID(this.hdnEntityID.Value);

            string maxLength = "";
            if (this.ddlDataType.SelectedItem.Text == "Short String")
            {
                maxLength = "255";
            }
            if (this.ddlDataType.SelectedItem.Text == "Long String")
            {
                maxLength = "1000";
            }

            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_entityid", this.hdnEntityID.Value),
                new KeyValuePair<string, string>("p_entityname", SynapseHelpers.GetEntityNameFromID(this.hdnEntityID.Value)),
                new KeyValuePair<string, string>("p_synapsenamespaceid", dt.Rows[0]["synapsenamespaceid"].ToString()),
                new KeyValuePair<string, string>("p_synapsenamespacename", dt.Rows[0]["synapsenamespacename"].ToString()),
                new KeyValuePair<string, string>("p_username", this.hdnUserName.Value),
                new KeyValuePair<string, string>("p_attributename", this.txtAttributeName.Text),
                new KeyValuePair<string, string>("p_attributedescription", "Description"),
                new KeyValuePair<string, string>("p_datatype", this.hdnDataType.Value),
                new KeyValuePair<string, string>("p_datatypeid", this.ddlDataType.SelectedValue),
                new KeyValuePair<string, string>("p_ordinal_position", this.hdnNextOrdinalPosition.Value),
                new KeyValuePair<string, string>("p_attributedefault", ""),
                new KeyValuePair<string, string>("p_maximumlength", maxLength),
                new KeyValuePair<string, string>("p_commondisplayname", ""),
                new KeyValuePair<string, string>("p_isnullsetting", ""),
                new KeyValuePair<string, string>("p_entityversionid", SynapseHelpers.GetCurrentEntityVersionFromID(this.hdnEntityID.Value))
            };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

            ClearAttributesForm();
            this.lblSuccess.Text = "Attribute created";
            this.lblSuccess.Visible = true;

            this.hdnNextOrdinalPosition.Value = SynapseHelpers.GetNextOrdinalPositionFromID(this.hdnEntityID.Value);


            
            foreach (DataRow row in dtBVs.Rows)
            {
                string baseview_id = row["baseview_id"].ToString();
                string sqlRecreate = "SELECT listsettings.recreatebaseview(@baseview_id);";
                var paramListRecreate = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("baseview_id",baseview_id)
                };

                try
                {
                    DataServices.ExcecuteNonQueryFromSQL(sqlRecreate, paramListRecreate);
                }
                catch(Exception ex) {
                    var a = ex.ToString();
                }

            }


            BindEntityGrid();
        }

        protected void ddlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.hdnDataType.Value = SynapseHelpers.GetDataTypeFromID(this.ddlDataType.SelectedValue);
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