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
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class ListManagerView : System.Web.UI.Page
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

                this.hdnListID.Value = id;

                try
                {

                }
                catch { }

                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }


                this.lblSummaryType.Text = SynapseHelpers.GetListNameFromID(id);
                this.hdnNextOrdinalPosition.Value = SynapseHelpers.GetNextOrdinalPositionFromID(id);

                BindGrid();

                this.btnDropBaseView.Attributes.Add("onclick", "if(confirm('Are you sure that you want to delete this BaseView? This cannot be undone!!')){return true;} else {return false;};");

                this.btnShowDeleteBaseView.Attributes.Add("onclick", "if(confirm('Are you sure that you want to enable the delete functionality?')){alert('Delete enabled'); return true;} else {return false;};");

                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;
                this.ltrlError.Visible = false;

                this.btnShowDeleteBaseView.Visible = false;

                string uri = SynapseHelpers.GetEBoardURL() + "ListPreview.aspx?id=" + this.hdnListID.Value;

                this.hlPreview.NavigateUrl = uri;
                this.hlPreview.Target = "_blank";

                
                BindFormFields();

            }
        }

        private void BindFormFields()
        {
            string sql = "SELECT * FROM listsettings.listmanager WHERE list_id = @list_id;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", this.hdnListID.Value)
            };
            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                StringBuilder sbe = new StringBuilder();
                sbe.AppendLine("<div class='contentAlertDanger'><h3 style='color: #712f2f'>Sorry, there was an error:</h3>");
                sbe.AppendLine(ex.ToString());
                sbe.AppendLine("</div>");
                this.ltrlError.Visible = true;
                this.ltrlError.Text = sbe.ToString();
                return;
            }

            DataTable dt = ds.Tables[0];

            try
            {
                this.hdnBaseView.Value = dt.Rows[0]["baseview_id"].ToString();
            }
            catch {}

            BindBaseViewContextFields();

            try
            {
                this.txtListName.Text = dt.Rows[0]["listname"].ToString();
            }
            catch { }

            try
            {
                this.txtListComments.Text = dt.Rows[0]["listdescription"].ToString();
            }
            catch { }

            try
            {
                this.txtDefaultTableRowCSS.Text = dt.Rows[0]["defaultrowcssstyle"].ToString();
            }
            catch { }

            try
            {
                this.txtTableClass.Text = dt.Rows[0]["tablecssstyle"].ToString();
            }
            catch { }

            try
            {
                this.txtTableHeaderClass.Text = dt.Rows[0]["tableheadercssstyle"].ToString();
            }
            catch { }

            try
            {
                SetDDLSource(this.ddlMatchedContextField, dt.Rows[0]["matchedcontextfield"].ToString());
            }
            catch {}

            try
            {
                SetDDLSource(this.ddlPatientBannerField, dt.Rows[0]["patientbannerfield"].ToString());
            }
            catch { }

            try
            {
                SetDDLSource(this.ddlRowCSSField, dt.Rows[0]["rowcssfield"].ToString());
            }
            catch { }




        }


        //private void BindBaseViewList()
        //{
        //    string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @id ORDER BY baseviewname;";
        //    var paramList = new List<KeyValuePair<string, string>>() {
        //        new KeyValuePair<string, string>("id", this.ddlBaseViewNamespace.SelectedValue)
        //    };
        //    BindDropDownList(this.ddlBaseView, sql, "baseview_id", "baseviewname", 1, paramList);

        //}


        private void BindBaseViewContextFields()
        {
            string sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id ORDER BY attributename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", this.hdnBaseView.Value)
            };
            BindDropDownList(this.ddlPatientBannerField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlMatchedContextField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlRowCSSField, sql, "attributename", "attributename", 1, paramList);
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

        private void BindGrid()
        {
            string sql = "SELECT * FROM listsettings.v_listdetailsummary WHERE list_id = @list_id ORDER BY orderby";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", this.hdnListID.Value)
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

            //this.lblError.Visible = false;
            //this.lblSuccess.Visible = false;

            //string sql = @"SELECT listsettings.dropbaseview(
	           //         @p_baseview_id, 
	           //         @p_baseviewname
            //        )";



            //var paramList = new List<KeyValuePair<string, string>>()
            //{
            //    new KeyValuePair<string, string>("p_baseview_id", this.hdnListID.Value),
            //    new KeyValuePair<string, string>("p_baseviewname", this.lblSummaryType.Text)
            //};

            //try
            //{
            //    DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            //}
            //catch (Exception ex)
            //{
            //    this.lblError.Visible = true;
            //    this.lblError.Text = ex.ToString();
            //    return;
            //}
            //Response.Redirect("BaseViewManagerList.aspx");
        }



        protected void btnManageAttributes_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListManagerAttributes.aspx?action=view&id=" + this.hdnListID.Value);
        }

        protected void btnSelectAttributes_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListSelectAttributes.aspx?action=view&id=" + this.hdnListID.Value);
        }

        protected void btnManageAPI_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListAPIs.aspx?action=view&id=" + this.hdnListID.Value);
        }

        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListManagerView.aspx?action=view&id=" + this.hdnListID.Value);
        }




        protected void btnShowDeleteBaseView_Click(object sender, EventArgs e)
        {
            //this.btnDropBaseView.Visible = true;
            //this.btnShowDeleteBaseView.Visible = false;
        }

        protected void btnRecreateBaseview_Click(object sender, EventArgs e)
        {
            //this.lblError.Visible = false;
            //this.lblSuccess.Visible = false;

            //this.lblRecreateBaseView.Text = "";

            //string sql = @"SELECT listsettings.recreatebaseview(
	           //         @p_baseview_id
            //        )";



            //var paramList = new List<KeyValuePair<string, string>>()
            //{
            //    new KeyValuePair<string, string>("p_baseview_id", this.hdnBaseViewID.Value),
            //};




            //try
            //{
            //    DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            //}
            //catch (Exception ex)
            //{
            //    this.lblError.Text = "Error creating view: " + ex.ToString();
            //    this.lblError.Visible = true;
            //    return;
            //}


            //this.lblSuccess.Visible = true;
            //this.lblSuccess.Text = "Baseview recreated";
        }

        private void SaveNewList()
        {
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


            this.fgMatchedContextField.CssClass = noerr;
            if (this.ddlMatchedContextField.SelectedIndex == 0)
            {
                this.lblError.Text = "Please select the field from the baseview that matches the default context key defined";
                this.ddlMatchedContextField.Focus();
                this.lblError.Visible = true;
                this.fgMatchedContextField.CssClass = haserr;
                return;
            }


            string sql = "UPDATE listsettings.listmanager SET listname = @listname, listdescription = @listdescription, matchedcontextfield = @matchedcontextfield, tablecssstyle = @tablecssstyle, tableheadercssstyle = @tableheadercssstyle, defaultrowcssstyle = @defaultrowcssstyle, patientbannerfield = @patientbannerfield, rowcssfield = @rowcssfield WHERE list_id = @list_id;";

            string newId = this.hdnListID.Value;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", newId),
                new KeyValuePair<string, string>("listname", this.txtListName.Text),
                new KeyValuePair<string, string>("listdescription", this.txtListComments.Text),                                                
                new KeyValuePair<string, string>("matchedcontextfield", this.ddlMatchedContextField.SelectedValue),
                new KeyValuePair<string, string>("patientbannerfield", this.ddlPatientBannerField.SelectedValue),
                new KeyValuePair<string, string>("rowcssfield", this.ddlRowCSSField.SelectedValue),
                new KeyValuePair<string, string>("tablecssstyle", this.txtTableClass.Text),
                new KeyValuePair<string, string>("tableheadercssstyle", this.txtTableHeaderClass.Text),
                new KeyValuePair<string, string>("defaultrowcssstyle", this.txtDefaultTableRowCSS.Text)

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


            Response.Redirect("ListManagerList.aspx");



        }

        protected void btnSelectQuestions_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListSelectQuestions.aspx?action=view&id=" + this.hdnListID.Value);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListManagerList.aspx");
        }

        protected void btnCreateNewList_Click(object sender, EventArgs e)
        {
            SaveNewList();
        }
    }
}