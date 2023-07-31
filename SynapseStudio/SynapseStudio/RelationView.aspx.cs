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
    public partial class RelationView : System.Web.UI.Page
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

                this.hdnAttributeID.Value = id;

                this.hdnEntityID.Value = SynapseHelpers.GetEntityIDFromAttributeID(id);

                this.lblHeading.Text = SynapseHelpers.GetAttributeNameFromAttributeID(id);

                this.lblAttributeName.Text = SynapseHelpers.GetAttributeNameFromAttributeID(id);


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

                this.btnDropAttribute.Attributes.Add("onclick", "if(confirm('Are you sure that you want to delete this relation? This cannot be undone!!')){return true;} else {return false;};");


            }
        }

        private void BindGrid()
        {
            string sql = "SELECT * FROM entitysettings.v_attributedetailsummary WHERE attributeid = @attributeid ORDER BY orderby";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("attributeid", this.hdnAttributeID.Value)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            //foreach (DataRow drow in dt.Rows)
            //{
            //    string colName = drow["entitydetail"].ToString();
            //    string colVal = drow["entitydescription"].ToString();
            //    if (colName == "Is Key Attribute")
            //    {
            //        if (colVal == "1")
            //        {
            //            this.btnDropAttribute.Visible = false;
            //            this.lblError.Text = "Drop Attribute button not available as it is not possible to delete a key attribute";
            //        }
            //    }

            //    if (colName == "Is System Attribute")
            //    {
            //        if (colVal == "1")
            //        {
            //            this.btnDropAttribute.Visible = false;
            //            this.lblError.Text = "Drop Attribute button not available as it is not possible to delete a system attribute";
            //        }
            //    }

            //    if (colName == "Is Relation Attribute")
            //    {
            //        if (colVal == "1")
            //        {
            //            this.btnDropAttribute.Visible = false;
            //            this.lblError.Text = "Drop Attribute button not available as it is not possible to delete a relation attribute";
            //        }
            //    }
            //}

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


        protected void btnDropAttribute_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT entitysettings.droprelationfromentity(
                                @p_attributeid,
	                            @p_entityid
                            )";



            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_attributeid", this.hdnAttributeID.Value),
                new KeyValuePair<string, string>("p_entityid", this.hdnEntityID.Value)
            };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

            Response.Redirect("EntityManagerRelations.aspx?action=view&id=" + this.hdnEntityID.Value);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerRelations.aspx?action=view&id=" + this.hdnEntityID.Value);
        }
    }
}