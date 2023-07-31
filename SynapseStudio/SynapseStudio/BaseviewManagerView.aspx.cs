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
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class BaseviewManagerView : System.Web.UI.Page
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


                this.lblSummaryType.Text = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(id);
                this.hdnNextOrdinalPosition.Value = SynapseHelpers.GetNextOrdinalPositionFromID(id);

                BindGrid();

                this.btnDropBaseView.Attributes.Add("onclick", "if(confirm('Are you sure that you want to delete this BaseView? This cannot be undone!!')){return true;} else {return false;};");

                this.btnShowDeleteBaseView.Attributes.Add("onclick", "if(confirm('Are you sure that you want to enable the delete functionality?')){alert('Delete enabled'); return true;} else {return false;};");

                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;

            }
        }

        private void BindGrid()
        {
            string sql = "SELECT * FROM listsettings.v_baseviewdetailsummary WHERE baseview_id = @baseview_id ORDER BY orderby";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseview_id", this.hdnBaseViewID.Value)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            this.dgEntities.DataSource = dt;
            this.dgEntities.DataBind();

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

        protected void btnDropBaseView_Click(object sender, EventArgs e)
        {

            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;

            string sql = @"SELECT listsettings.dropbaseview(
	                    @p_baseview_id, 
	                    @p_baseviewname
                    )";



            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_baseview_id", this.hdnBaseViewID.Value),
                new KeyValuePair<string, string>("p_baseviewname", this.lblSummaryType.Text)
            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                this.lblError.Visible = true;
                this.lblError.Text = ex.ToString();
                return;
            }
            Response.Redirect("BaseViewManagerList.aspx");
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

        protected void btnShowDeleteBaseView_Click(object sender, EventArgs e)
        {
            this.btnDropBaseView.Visible = true;
            this.btnShowDeleteBaseView.Visible = false;
        }

        protected void btnRecreateBaseview_Click(object sender, EventArgs e)
        {
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;

            this.lblRecreateBaseView.Text = "";

            string sql = @"SELECT listsettings.recreatebaseview(
	                    @p_baseview_id
                    )";



            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_baseview_id", this.hdnBaseViewID.Value),                
            };



            
            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                this.lblError.Text = "Error creating view: " + ex.ToString();
                this.lblError.Visible = true;
                return;
            }


            this.lblSuccess.Visible = true;
            this.lblSuccess.Text = "Baseview recreated";
        }
    }
}