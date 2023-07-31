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
    public partial class EntityManagerView : System.Web.UI.Page
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

                BindGrid();

                this.btnDropEntity.Attributes.Add("onclick", "if(confirm('Are you sure that you want to delete this entity? This cannot be undone!!')){return true;} else {return false;};");

                this.btnShowDeleteEntity.Attributes.Add("onclick", "if(confirm('Are you sure that you want to enable the delete functionality?')){alert('Delete enabled'); return true;} else {return false;};");

                DataTable dtBVs = SynapseHelpers.GetEntityBaseviewsDT(id);
                Int16 bvCount = System.Convert.ToInt16(dtBVs.Rows.Count);
                this.lblDependentBaseviewCount.Text = bvCount.ToString();
                this.lblHasDepencies.Text = bvCount.ToString();

                if(bvCount > 0)
                {
                    this.pnlHasDepencies.Visible = true;
                    this.btnShowDeleteEntity.Visible = false;
                }
                else
                {
                    this.pnlHasDepencies.Visible = false;
                    this.btnShowDeleteEntity.Visible = true;
                }

                this.dgBaseViewDependancies.DataSource = dtBVs;
                this.dgBaseViewDependancies.DataBind();

            }
        }

        private void BindGrid()
        {
            string sql = "SELECT * FROM entitysettings.v_entitydetailsummary WHERE entityid = @entityid ORDER BY orderby";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", this.hdnEntityID.Value)
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

        protected void btnDropEntity_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT entitysettings.dropentityfrommodelbyid(
	                            @p_entityid
                            )";



            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_entityid", this.hdnEntityID.Value)
            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                this.lblError.Text = ex.ToString();
                this.lblError.Visible = true;
                return;
            }

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

        protected void btnShowDeleteEntity_Click(object sender, EventArgs e)
        {
            this.btnDropEntity.Visible = true;
            this.btnShowDeleteEntity.Visible = false;
        }

        protected void btnManageModels_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerModels.aspx?action=view&id=" + this.hdnEntityID.Value);
        }
    }
}