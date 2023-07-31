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

using Newtonsoft.Json;
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
    public partial class EntityManagerAPIs : System.Web.UI.Page
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

                this.hdnEntityName.Value = SynapseHelpers.GetEntityNameFromID(id);
                this.hdnNamespace.Value = SynapseHelpers.GetNamepsaceFromEntityID(id);

                DataTable dtKey = SynapseHelpers.GetEntityKeyAttributeFromID(id);
                this.hdnKeyAttribute.Value = dtKey.Rows[0][1].ToString();

                this.hdnAPIURL.Value = SynapseHelpers.GetAPIURL();

                this.hlGetList.Text = this.hdnAPIURL.Value + "/GetList" + "?synapsenamespace=" + this.hdnNamespace.Value + "&synapseentityname=" + this.hdnEntityName.Value;
                this.hlGetList.NavigateUrl = this.hlGetList.Text;

                this.hlGetObject.Text = this.hdnAPIURL.Value + "/GetObject" + "?synapsenamespace=" + this.hdnNamespace.Value + "&synapseentityname=" + this.hdnEntityName.Value + "&id={" + this.hdnKeyAttribute.Value + "}";
                this.hlGetObject.NavigateUrl = this.hlGetObject.Text;

                this.hlGetListByID.Text = this.hdnAPIURL.Value + "/GetListByAttribute" + "?synapsenamespace=" + this.hdnNamespace.Value + "&synapseentityname=" + this.hdnEntityName.Value + "&synapseattributename={synapseattributename}&attributevalue={attributevalue}"; 
                this.hlGetListByID.NavigateUrl = this.hlGetListByID.Text;

                this.hlPostObject.Text = this.hdnAPIURL.Value + "/PostObject" + "?synapsenamespace=" + this.hdnNamespace.Value + "&synapseentityname=" + this.hdnEntityName.Value;
                this.hlPostObject.NavigateUrl = this.hlPostObject.Text;

                this.hlDeleteObject.Text = this.hdnAPIURL.Value + "/DeleteObject" + "?synapsenamespace=" + this.hdnNamespace.Value + "&synapseentityname=" + this.hdnEntityName.Value + "&id={" + this.hdnKeyAttribute.Value + "}";
                this.hlDeleteObject.NavigateUrl = this.hlDeleteObject.Text;

                this.hlDeleteByAttribute.Text = this.hdnAPIURL.Value + "/DeleteObjectByAttribute" + "?synapsenamespace=" + this.hdnNamespace.Value + "&synapseentityname=" + this.hdnEntityName.Value + "&synapseattributename={synapseattributename}&attributevalue={attributevalue}";
                this.hlDeleteObject.NavigateUrl = this.hlDeleteObject.Text;

                this.hlGetObjectHistory.Text = this.hdnAPIURL.Value + "/GetObjectHistory" + "?synapsenamespace=" + this.hdnNamespace.Value + "&synapseentityname=" + this.hdnEntityName.Value + "&id={" + this.hdnKeyAttribute.Value + "}";
                this.hlGetObjectHistory.NavigateUrl = this.hlGetObjectHistory.Text;
                

                this.lblentity1.Text = this.hdnEntityName.Value;
                this.lblentity2.Text = this.hdnEntityName.Value;
                this.lblentity3.Text = this.hdnEntityName.Value;
                this.lblentity4.Text = this.hdnEntityName.Value;
                this.lblentity5.Text = this.hdnEntityName.Value;
                this.lblentity6.Text = this.hdnEntityName.Value;

                this.lblnamespace1.Text = this.hdnNamespace.Value;
                this.lblnamespace2.Text = this.hdnNamespace.Value;
                this.lblnamespace3.Text = this.hdnNamespace.Value;
                this.lblnamespace4.Text = this.hdnNamespace.Value;
                this.lblnamespace5.Text = this.hdnNamespace.Value;
                this.lblnamespace6.Text = this.hdnNamespace.Value;

                this.lblkey1.Text = this.hdnKeyAttribute.Value;
                this.lblkey2.Text = this.hdnKeyAttribute.Value;
                this.lblkey3.Text = this.hdnKeyAttribute.Value;
                this.lblkey4.Text = this.hdnKeyAttribute.Value;
                this.lblkey5.Text = this.hdnKeyAttribute.Value;
                this.lblkey6.Text = this.hdnKeyAttribute.Value;

                this.divPostSample.InnerText = SynapseHelpers.GetEntitySampleJSON(id).Rows[0][0].ToString();


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

        protected void btnDropEntity_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT entitysettings.dropentityfrommodelbyid(
	                            @p_entityid
                            )";



            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_entityid", this.hdnEntityID.Value)
            };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

            Response.Redirect("EntityManagerList.aspx");
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