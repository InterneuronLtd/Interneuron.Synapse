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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class BaseViewManagerAPIs : System.Web.UI.Page
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

                this.hdnBaseViewID.Value = id;

                try
                {

                }
                catch { }

                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }

                DataTable dt = SynapseHelpers.GetBaseviewDTByID(id);

                this.lblSummaryType.Text = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);

                this.hdnNextOrdinalPosition.Value = SynapseHelpers.GetNextOrdinalPositionFromID(id);

                this.hdnAPIURL.Value = SynapseHelpers.GetAPIURL();

                this.hlGetList.Text = this.hdnAPIURL.Value + "/GetBaseViewList/" + SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
                this.hlGetList.NavigateUrl = this.hlGetList.Text;

                this.hlGetListByID.Text = this.hdnAPIURL.Value + "/GetBaseViewListObjectByAttribute/" + SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id) + "?synapseattributename={synapseattributename}&attributevalue={attributevalue}";
                this.hlGetListByID.NavigateUrl = this.hlGetListByID.Text;

                this.hlPostObject.Text = this.hdnAPIURL.Value + "/GetBaseViewListByPost/" + SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
                this.hlPostObject.NavigateUrl = this.hlPostObject.Text;
                

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


        protected void btnManageAttributes_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseViewManagerAttributes.aspx?action=view&id=" + this.hdnBaseViewID.Value);
        }

        protected void btnManageAPI_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseViewManagerAPIs.aspx?action=view&id=" + this.hdnBaseViewID.Value);
        }

        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseViewManagerView.aspx?action=view&id=" + this.hdnBaseViewID.Value);
        }

        protected void btnSQL_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseViewManageSQL.aspx?action=view&id=" + this.hdnBaseViewID.Value);
        }


    }
}