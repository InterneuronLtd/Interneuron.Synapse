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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class BedBoardManagerView : System.Web.UI.Page
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
                hdnBedBoardID.Value = id;

                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }

                HideAllOptions();
                this.ltrlError.Visible = false;
                this.lblSuccess.Visible = false;
                BindDropDownList(this.ddlBaseViewNamespace, "SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace", "baseviewnamespaceid", "baseviewnamespace", 0, null);
                BindDBFields();

                GetPreviewURL();

                this.btnCancel.Attributes.Add("onclick", "if(confirm('Are you sure that you want to cancel any changes?')){return true;} else {return false;};");
                this.btnClone.Attributes.Add("onclick", "if(confirm('Are you sure that you want to clone this board?')){return true;} else {return false;};");
                this.btnDelete.Attributes.Add("onclick", "if(confirm('Are you sure that you want to delete this board?')){return true;} else {return false;};");
            }
        }

        private void BindDBFields()
        {
            string sql = "SELECT * FROM eboards.bedboard WHERE bedboard_id = @bedboard_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboard_id", hdnBedBoardID.Value)
            };
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
                this.txtBedBoardName.Text = dt.Rows[0]["bedboardname"].ToString();
            }
            catch { }

            try
            {
                this.txtBedBoardDescription.Text = dt.Rows[0]["bedboarddescription"].ToString();
            }
            catch { }

            SetDDLSource(this.ddlBaseViewNamespace, dt.Rows[0]["baseviewnamespace_id"].ToString());

            BindBaseViewList();

            SetDDLSource(this.ddlBaseView, dt.Rows[0]["baseview_id"].ToString());

            BindBaseViewFields();

            SetDDLSource(this.ddlPersonIDField, dt.Rows[0]["baseviewpersonidfield"].ToString());
            SetDDLSource(this.ddlEncounterIDField, dt.Rows[0]["baseviewencounteridfield"].ToString());
            SetDDLSource(this.ddlWardField, dt.Rows[0]["baseviewwardfield"].ToString());
            SetDDLSource(this.ddlBedField, dt.Rows[0]["baseviewbedfield"].ToString());

            SetDDLSource(this.ddlTopSetting, dt.Rows[0]["topsetting"].ToString());
            CheckTopOptions();
            SetDDLSource(this.ddlTopField, dt.Rows[0]["topfield"].ToString());
            SetDDLSource(this.ddlTopLeftField, dt.Rows[0]["topleftfield"].ToString());
            SetDDLSource(this.ddlTopRightField, dt.Rows[0]["toprightfield"].ToString());

            SetDDLSource(this.ddlMiddleSetting, dt.Rows[0]["middlesetting"].ToString());
            CheckMiddleOptions();
            SetDDLSource(this.ddlMiddleField, dt.Rows[0]["middlefield"].ToString());
            SetDDLSource(this.ddlMiddleLeftField, dt.Rows[0]["middleleftfield"].ToString());
            SetDDLSource(this.ddlMiddleRightField, dt.Rows[0]["middlerightfield"].ToString());

            SetDDLSource(this.ddlBottomSetting, dt.Rows[0]["bottomsetting"].ToString());
            CheckBottomOptions();
            SetDDLSource(this.ddlBottomField, dt.Rows[0]["bottomfield"].ToString());
            SetDDLSource(this.ddlBottomLeftField, dt.Rows[0]["bottomleftfield"].ToString());
            SetDDLSource(this.ddlBottomRightField, dt.Rows[0]["bottomrightfield"].ToString());

        }

        private void BindBaseViewList()
        {
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @id ORDER BY baseviewname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", this.ddlBaseViewNamespace.SelectedValue)
            };
            BindDropDownList(this.ddlBaseView, sql, "baseview_id", "baseviewname", 1, paramList);

        }

        private void BindBaseViewFields()
        {
            string sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id ORDER BY attributename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", this.ddlBaseView.SelectedValue)
            };
            BindDropDownList(this.ddlPersonIDField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlEncounterIDField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlWardField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlBedField, sql, "attributename", "attributename", 1, paramList);

            BindDropDownList(this.ddlTopField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlTopLeftField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlTopRightField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlMiddleField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlMiddleLeftField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlMiddleRightField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlBottomField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlBottomLeftField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlBottomRightField, sql, "attributename", "attributename", 1, paramList);
        }

        private void HideAllOptions()
        {
            this.ddlPersonIDField.AutoPostBack = false;
            this.ddlEncounterIDField.AutoPostBack = false;
            this.ddlWardField.AutoPostBack = false;
            this.ddlBedField.AutoPostBack = false;

            this.ddlTopField.AutoPostBack = false;
            this.ddlTopLeftField.AutoPostBack = false;
            this.ddlTopRightField.AutoPostBack = false;

            this.ddlMiddleField.AutoPostBack = false;
            this.ddlMiddleLeftField.AutoPostBack = false;
            this.ddlMiddleRightField.AutoPostBack = false;

            this.ddlBottomField.AutoPostBack = false;
            this.ddlBottomLeftField.AutoPostBack = false;
            this.ddlBottomRightField.AutoPostBack = false;

            this.fgTopField.Visible = false;
            this.fgTopLeftField.Visible = false;
            this.fgTopRightField.Visible = false;


            this.fgMiddleField.Visible = false;
            this.fgMiddleLeftField.Visible = false;
            this.fgMiddleRightField.Visible = false;

            this.fgBottomField.Visible = false;
            this.fgBottomLeftField.Visible = false;
            this.fgBottomRightField.Visible = false;

        }

        private void CheckTopOptions()
        {
            if (this.ddlTopSetting.SelectedIndex == 0)
            {
                this.fgTopField.Visible = false;
                this.fgTopLeftField.Visible = false;
                this.fgTopRightField.Visible = false;

            }
            else if (this.ddlTopSetting.SelectedValue == "1")
            {
                this.fgTopField.Visible = true;
                this.fgTopLeftField.Visible = false;
                this.fgTopRightField.Visible = false;
            }
            else if (this.ddlTopSetting.SelectedValue == "2")
            {
                this.fgTopField.Visible = false;
                this.fgTopLeftField.Visible = true;
                this.fgTopRightField.Visible = true;
            }
        }

        private void CheckMiddleOptions()
        {
            if (this.ddlMiddleSetting.SelectedIndex == 0)
            {
                this.fgMiddleField.Visible = false;
                this.fgMiddleLeftField.Visible = false;
                this.fgMiddleRightField.Visible = false;

            }
            else if (this.ddlMiddleSetting.SelectedValue == "1")
            {
                this.fgMiddleField.Visible = true;
                this.fgMiddleLeftField.Visible = false;
                this.fgMiddleRightField.Visible = false;
            }
            else if (this.ddlMiddleSetting.SelectedValue == "2")
            {
                this.fgMiddleField.Visible = false;
                this.fgMiddleLeftField.Visible = true;
                this.fgMiddleRightField.Visible = true;
            }
        }

        private void CheckBottomOptions()
        {
            if (this.ddlBottomSetting.SelectedIndex == 0)
            {
                this.fgBottomField.Visible = false;
                this.fgBottomLeftField.Visible = false;
                this.fgBottomRightField.Visible = false;

            }
            else if (this.ddlBottomSetting.SelectedValue == "1")
            {
                this.fgBottomField.Visible = true;
                this.fgBottomLeftField.Visible = false;
                this.fgBottomRightField.Visible = false;
            }
            else if (this.ddlBottomSetting.SelectedValue == "2")
            {
                this.fgBottomField.Visible = false;
                this.fgBottomLeftField.Visible = true;
                this.fgBottomRightField.Visible = true;
            }
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

        protected void ddlBaseViewNamespace_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBaseViewList();
        }

        protected void ddlBaseView_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBaseViewFields();
        }

        protected void ddlTopSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckTopOptions();
        }

        protected void ddlTopField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlTopLeftField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlTopRightField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlMiddleSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMiddleOptions();
        }

        protected void ddlMiddleField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlMiddleLeftField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlMiddleRightField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlBottomSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBottomOptions();
        }

        protected void ddlBottomField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlBottomLeftField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlBottomRightField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlPersonIDField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEncounterIDField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlWardField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlBedField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BoardManagerList.aspx");
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string haserr = "form-group has-error";
            string noerr = "form-group";

            this.ltrlError.Text = string.Empty;
            this.ltrlError.Visible = false;
            this.lblSuccess.Visible = false;

            int errcount = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class='contentAlertDanger'><h3 style='color: #712f2f'>Please resolve the following errors:</h3><ul>");

            this.fgBedBoardName.CssClass = noerr;
            if (string.IsNullOrWhiteSpace(this.txtBedBoardName.Text))
            {
                sb.AppendLine("<li>" + "Please enter a new name for the Bed Board" + "</li>");
                this.fgBedBoardName.CssClass = haserr;
                errcount++;
            }

            this.fgBaseView.CssClass = noerr;
            if (this.ddlBaseView.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select a baseview" + "</li>");
                this.fgBaseView.CssClass = haserr;
                errcount++;
            }

            this.fgPersonIDField.CssClass = noerr;
            if (this.ddlPersonIDField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the PersonID field from the baseview" + "</li>");
                this.fgPersonIDField.CssClass = haserr;
                errcount++;
            }

            this.fgEncounterIDField.CssClass = noerr;
            if (this.ddlEncounterIDField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the EncounterID field from the baseview" + "</li>");
                this.fgEncounterIDField.CssClass = haserr;
                errcount++;
            }

            this.fgWardField.CssClass = noerr;
            if (this.ddlWardField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Ward Code field from the baseview" + "</li>");
                this.fgWardField.CssClass = haserr;
                errcount++;
            }

            this.fgBedField.CssClass = noerr;
            if (this.ddlBedField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Bed Code field from the baseview" + "</li>");
                this.fgBedField.CssClass = haserr;
                errcount++;
            }

            this.fgTopSetting.CssClass = noerr;
            if (this.ddlTopSetting.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the top section setting" + "</li>");
                this.fgTopSetting.CssClass = haserr;
                errcount++;
            }

            this.fgTopField.CssClass = noerr;
            if (this.ddlTopSetting.SelectedValue == "1" && this.ddlTopField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Top Section" + "</li>");
                this.fgTopField.CssClass = haserr;
                errcount++;
            }

            this.fgTopLeftField.CssClass = noerr;
            if (this.ddlTopSetting.SelectedValue == "2" && this.ddlTopLeftField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Top Left Section" + "</li>");
                this.fgTopField.CssClass = haserr;
                errcount++;
            }

            this.fgTopRightField.CssClass = noerr;
            if (this.ddlTopSetting.SelectedValue == "2" && this.ddlTopRightField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Top Right Section" + "</li>");
                this.fgTopField.CssClass = haserr;
                errcount++;
            }

            this.fgMiddleSetting.CssClass = noerr;
            if (this.ddlMiddleSetting.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Middle section setting" + "</li>");
                this.fgMiddleSetting.CssClass = haserr;
                errcount++;
            }

            this.fgMiddleField.CssClass = noerr;
            if (this.ddlMiddleSetting.SelectedValue == "1" && this.ddlMiddleField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Middle Section" + "</li>");
                this.fgMiddleField.CssClass = haserr;
                errcount++;
            }

            this.fgMiddleLeftField.CssClass = noerr;
            if (this.ddlMiddleSetting.SelectedValue == "2" && this.ddlMiddleLeftField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Middle Left Section" + "</li>");
                this.fgMiddleField.CssClass = haserr;
                errcount++;
            }

            this.fgMiddleRightField.CssClass = noerr;
            if (this.ddlMiddleSetting.SelectedValue == "2" && this.ddlMiddleRightField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Middle Right Section" + "</li>");
                this.fgMiddleField.CssClass = haserr;
                errcount++;
            }

            this.fgBottomSetting.CssClass = noerr;
            if (this.ddlBottomSetting.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Bottom section setting" + "</li>");
                this.fgBottomSetting.CssClass = haserr;
                errcount++;
            }

            this.fgBottomField.CssClass = noerr;
            if (this.ddlBottomSetting.SelectedValue == "1" && this.ddlBottomField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Bottom Section" + "</li>");
                this.fgBottomField.CssClass = haserr;
                errcount++;
            }

            this.fgBottomLeftField.CssClass = noerr;
            if (this.ddlBottomSetting.SelectedValue == "2" && this.ddlBottomLeftField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Bottom Left Section" + "</li>");
                this.fgBottomField.CssClass = haserr;
                errcount++;
            }

            this.fgBottomRightField.CssClass = noerr;
            if (this.ddlBottomSetting.SelectedValue == "2" && this.ddlBottomRightField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Bottom Right Section" + "</li>");
                this.fgBottomField.CssClass = haserr;
                errcount++;
            }



            if (errcount > 0)
            {
                sb.AppendLine("</ul></div>");
                this.ltrlError.Visible = true;
                this.ltrlError.Text = sb.ToString();
                return;
            }

            string sql = @"UPDATE eboards.bedboard SET                                
	                            baseview_id = @baseview_id, 
	                            bedboardname = @bedboardname, 
	                            bedboarddescription = @bedboarddescription, 
	                            baseviewnamespace_id = @baseviewnamespace_id, 	                            
	                            baseviewpersonidfield = @baseviewpersonidfield, 
	                            baseviewencounteridfield = @baseviewencounteridfield, 
	                            baseviewwardfield = @baseviewwardfield, 
	                            baseviewbedfield = @baseviewbedfield, 
	                            topsetting = @topsetting, 
	                            middlesetting = @middlesetting, 
	                            bottomsetting = @bottomsetting, 
	                            topfield = @topfield, 
	                            topleftfield = @topleftfield, 
	                            toprightfield = @toprightfield, 
	                            middlefield = @middlefield, 
	                            middleleftfield = @middleleftfield, 
	                            middlerightfield = @middlerightfield, 
	                            bottomfield = @bottomfield, 
	                            bottomleftfield = @bottomleftfield, 
	                            bottomrightfield = @bottomrightfield
                        WHERE bedboard_id = @bedboard_id";

            string newID = hdnBedBoardID.Value;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("_createdby", this.hdnUserName.Value),
                new KeyValuePair<string, string>("bedboard_id", newID),
                new KeyValuePair<string, string>("bedboardname", this.txtBedBoardName.Text),
                new KeyValuePair<string, string>("bedboarddescription", this.txtBedBoardDescription.Text),
                new KeyValuePair<string, string>("baseviewnamespace_id", this.ddlBaseViewNamespace.SelectedValue),
                new KeyValuePair<string, string>("baseview_id", this.ddlBaseView.SelectedValue),
                new KeyValuePair<string, string>("baseviewpersonidfield", this.ddlPersonIDField.SelectedValue),
                new KeyValuePair<string, string>("baseviewencounteridfield", this.ddlEncounterIDField.SelectedValue),
                new KeyValuePair<string, string>("baseviewwardfield", this.ddlWardField.SelectedValue),
                new KeyValuePair<string, string>("baseviewbedfield", this.ddlBedField.SelectedValue),
                new KeyValuePair<string, string>("topsetting", this.ddlTopSetting.SelectedValue),
                new KeyValuePair<string, string>("middlesetting", this.ddlMiddleSetting.SelectedValue),
                new KeyValuePair<string, string>("bottomsetting", this.ddlBottomSetting.SelectedValue),
                new KeyValuePair<string, string>("topfield", this.ddlTopField.SelectedValue),
                new KeyValuePair<string, string>("topleftfield", this.ddlTopLeftField.SelectedValue),
                new KeyValuePair<string, string>("toprightfield", this.ddlTopRightField.SelectedValue),
                new KeyValuePair<string, string>("middlefield", this.ddlMiddleField.SelectedValue),
                new KeyValuePair<string, string>("middleleftfield", this.ddlMiddleLeftField.SelectedValue),
                new KeyValuePair<string, string>("middlerightfield", this.ddlMiddleRightField.SelectedValue),
                new KeyValuePair<string, string>("bottomfield", this.ddlBottomField.SelectedValue),
                new KeyValuePair<string, string>("bottomleftfield", this.ddlBottomLeftField.SelectedValue),
                new KeyValuePair<string, string>("bottomrightfield", this.ddlBottomRightField.SelectedValue)
            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
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

            Response.Redirect("BedBoardManagerView.aspx?id=" + newID);

        }

        private void GetPreviewURL()
        {
            string sql = "SELECT * FROM eboards.bedboard WHERE bedboard_id = @bedboard_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboard_id", hdnBedBoardID.Value)
            };
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
                //this.ltrlError.Visible = true;
                //this.ltrlError.Text = sbe.ToString();
                return;
            }

            DataTable dt = ds.Tables[0];

            string baseview_id = "";
            try
            {
                baseview_id = dt.Rows[0]["baseview_id"].ToString();
            }
            catch { }

            string baseviewname = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(baseview_id);




            string PersonIDField = "";
            try
            {
                PersonIDField = dt.Rows[0]["baseviewpersonidfield"].ToString();
            }
            catch { }

            string EncounterIDField = "";
            try
            {
                EncounterIDField = dt.Rows[0]["baseviewencounteridfield"].ToString();
            }
            catch { }

            string WardField = "";
            try
            {
                WardField = dt.Rows[0]["baseviewwardfield"].ToString();
            }
            catch { }

            string BedField = "";
            try
            {
                BedField = dt.Rows[0]["baseviewbedfield"].ToString();
            }
            catch { }


            string sqlBoard = "SELECT " + WardField + " as WardField, " + BedField + " as BedField" +
                              " FROM baseview." + baseviewname + " order by random() LIMIT 1;";

            var paramListbOARD = new List<KeyValuePair<string, string>>()
            {
            };

            DataSet dsBoard = new DataSet();
            try
            {
                dsBoard = DataServices.DataSetFromSQL(sqlBoard, paramListbOARD);
            }
            catch (Exception ex)
            {
                StringBuilder sbe = new StringBuilder();
                sbe.AppendLine("<div class='contentAlertDanger'><h3 style='color: #712f2f'>Sorry, there was an error:</h3>");
                sbe.AppendLine(ex.ToString());
                sbe.AppendLine("</div>");
                //this.ltrlError.Visible = true;
                //this.ltrlError.Text = sbe.ToString();
                return;
            }

            DataTable dtBoard = dsBoard.Tables[0];

            string ward = "";
            try
            {
                ward = dtBoard.Rows[0]["WardField"].ToString();
            }
            catch { }

            string bed = "";
            try
            {
                bed = dtBoard.Rows[0]["BedField"].ToString();
            }
            catch { }

            string apiURL = SynapseHelpers.GetAPIURL();

            string uri = SynapseHelpers.GetEBoardURL()  + "bedboard.aspx?BedBoardID=" + this.hdnBedBoardID.Value + "&Ward=" + ward + "&Bed=" + bed;

            this.hlPreview.NavigateUrl = uri;

        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            

            //Page.ClientScript.RegisterStartupScript(
            //this.GetType(), "OpenWindow", "window.open(uri,'blank');", true);
        }

        protected void btnClone_Click(object sender, EventArgs e)
        {
            string haserr = "form-group has-error";
            string noerr = "form-group";

            this.ltrlError.Text = string.Empty;
            this.ltrlError.Visible = false;
            this.lblSuccess.Visible = false;

            int errcount = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class='contentAlertDanger'><h3 style='color: #712f2f'>Please resolve the following errors:</h3><ul>");

            this.fgBedBoardName.CssClass = noerr;
            if (string.IsNullOrWhiteSpace(this.txtBedBoardName.Text))
            {
                sb.AppendLine("<li>" + "Please enter a new name for the Bed Board" + "</li>");
                this.fgBedBoardName.CssClass = haserr;
                errcount++;
            }

            this.fgBaseView.CssClass = noerr;
            if (this.ddlBaseView.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select a baseview" + "</li>");
                this.fgBaseView.CssClass = haserr;
                errcount++;
            }

            this.fgPersonIDField.CssClass = noerr;
            if (this.ddlPersonIDField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the PersonID field from the baseview" + "</li>");
                this.fgPersonIDField.CssClass = haserr;
                errcount++;
            }

            this.fgEncounterIDField.CssClass = noerr;
            if (this.ddlEncounterIDField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the EncounterID field from the baseview" + "</li>");
                this.fgEncounterIDField.CssClass = haserr;
                errcount++;
            }

            this.fgWardField.CssClass = noerr;
            if (this.ddlWardField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Ward Code field from the baseview" + "</li>");
                this.fgWardField.CssClass = haserr;
                errcount++;
            }

            this.fgBedField.CssClass = noerr;
            if (this.ddlBedField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Bed Code field from the baseview" + "</li>");
                this.fgBedField.CssClass = haserr;
                errcount++;
            }

            this.fgTopSetting.CssClass = noerr;
            if (this.ddlTopSetting.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the top section setting" + "</li>");
                this.fgTopSetting.CssClass = haserr;
                errcount++;
            }

            this.fgTopField.CssClass = noerr;
            if (this.ddlTopSetting.SelectedValue == "1" && this.ddlTopField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Top Section" + "</li>");
                this.fgTopField.CssClass = haserr;
                errcount++;
            }

            this.fgTopLeftField.CssClass = noerr;
            if (this.ddlTopSetting.SelectedValue == "2" && this.ddlTopLeftField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Top Left Section" + "</li>");
                this.fgTopField.CssClass = haserr;
                errcount++;
            }

            this.fgTopRightField.CssClass = noerr;
            if (this.ddlTopSetting.SelectedValue == "2" && this.ddlTopRightField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Top Right Section" + "</li>");
                this.fgTopField.CssClass = haserr;
                errcount++;
            }

            this.fgMiddleSetting.CssClass = noerr;
            if (this.ddlMiddleSetting.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Middle section setting" + "</li>");
                this.fgMiddleSetting.CssClass = haserr;
                errcount++;
            }

            this.fgMiddleField.CssClass = noerr;
            if (this.ddlMiddleSetting.SelectedValue == "1" && this.ddlMiddleField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Middle Section" + "</li>");
                this.fgMiddleField.CssClass = haserr;
                errcount++;
            }

            this.fgMiddleLeftField.CssClass = noerr;
            if (this.ddlMiddleSetting.SelectedValue == "2" && this.ddlMiddleLeftField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Middle Left Section" + "</li>");
                this.fgMiddleField.CssClass = haserr;
                errcount++;
            }

            this.fgMiddleRightField.CssClass = noerr;
            if (this.ddlMiddleSetting.SelectedValue == "2" && this.ddlMiddleRightField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Middle Right Section" + "</li>");
                this.fgMiddleField.CssClass = haserr;
                errcount++;
            }

            this.fgBottomSetting.CssClass = noerr;
            if (this.ddlBottomSetting.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Bottom section setting" + "</li>");
                this.fgBottomSetting.CssClass = haserr;
                errcount++;
            }

            this.fgBottomField.CssClass = noerr;
            if (this.ddlBottomSetting.SelectedValue == "1" && this.ddlBottomField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Bottom Section" + "</li>");
                this.fgBottomField.CssClass = haserr;
                errcount++;
            }

            this.fgBottomLeftField.CssClass = noerr;
            if (this.ddlBottomSetting.SelectedValue == "2" && this.ddlBottomLeftField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Bottom Left Section" + "</li>");
                this.fgBottomField.CssClass = haserr;
                errcount++;
            }

            this.fgBottomRightField.CssClass = noerr;
            if (this.ddlBottomSetting.SelectedValue == "2" && this.ddlBottomRightField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Bottom Right Section" + "</li>");
                this.fgBottomField.CssClass = haserr;
                errcount++;
            }



            if (errcount > 0)
            {
                sb.AppendLine("</ul></div>");
                this.ltrlError.Visible = true;
                this.ltrlError.Text = sb.ToString();
                return;
            }

            string sql = @"INSERT INTO eboards.bedboard(
                                _createdby, 
	                            bedboard_id, 
	                            bedboardname, 
	                            bedboarddescription, 
	                            baseviewnamespace_id, 
	                            baseview_id, 
	                            baseviewpersonidfield, 
	                            baseviewencounteridfield, 
	                            baseviewwardfield, 
	                            baseviewbedfield, 
	                            topsetting, 
	                            middlesetting, 
	                            bottomsetting, 
	                            topfield, 
	                            topleftfield, 
	                            toprightfield, 
	                            middlefield, 
	                            middleleftfield, 
	                            middlerightfield, 
	                            bottomfield, 
	                            bottomleftfield, 
	                            bottomrightfield
                            )
                            VALUES(
                                @_createdby,
                                @bedboard_id,
                                @bedboardname,
                                @bedboarddescription,
                                @baseviewnamespace_id,
                                @baseview_id,
                                @baseviewpersonidfield,
                                @baseviewencounteridfield,
                                @baseviewwardfield,
                                @baseviewbedfield,
                                @topsetting,
                                @middlesetting,
                                @bottomsetting,
                                @topfield,
                                @topleftfield,
                                @toprightfield,
                                @middlefield,
                                @middleleftfield,
                                @middlerightfield,
                                @bottomfield,
                                @bottomleftfield,
                                @bottomrightfield
                            )";

            string newID = System.Guid.NewGuid().ToString();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("_createdby", this.hdnUserName.Value),
                new KeyValuePair<string, string>("bedboard_id", newID),
                new KeyValuePair<string, string>("bedboardname", this.txtBedBoardName.Text + "_Clone"),
                new KeyValuePair<string, string>("bedboarddescription", this.txtBedBoardDescription.Text),
                new KeyValuePair<string, string>("baseviewnamespace_id", this.ddlBaseViewNamespace.SelectedValue),
                new KeyValuePair<string, string>("baseview_id", this.ddlBaseView.SelectedValue),
                new KeyValuePair<string, string>("baseviewpersonidfield", this.ddlPersonIDField.SelectedValue),
                new KeyValuePair<string, string>("baseviewencounteridfield", this.ddlEncounterIDField.SelectedValue),
                new KeyValuePair<string, string>("baseviewwardfield", this.ddlWardField.SelectedValue),
                new KeyValuePair<string, string>("baseviewbedfield", this.ddlBedField.SelectedValue),
                new KeyValuePair<string, string>("topsetting", this.ddlTopSetting.SelectedValue),
                new KeyValuePair<string, string>("middlesetting", this.ddlMiddleSetting.SelectedValue),
                new KeyValuePair<string, string>("bottomsetting", this.ddlBottomSetting.SelectedValue),
                new KeyValuePair<string, string>("topfield", this.ddlTopField.SelectedValue),
                new KeyValuePair<string, string>("topleftfield", this.ddlTopLeftField.SelectedValue),
                new KeyValuePair<string, string>("toprightfield", this.ddlTopRightField.SelectedValue),
                new KeyValuePair<string, string>("middlefield", this.ddlMiddleField.SelectedValue),
                new KeyValuePair<string, string>("middleleftfield", this.ddlMiddleLeftField.SelectedValue),
                new KeyValuePair<string, string>("middlerightfield", this.ddlMiddleRightField.SelectedValue),
                new KeyValuePair<string, string>("bottomfield", this.ddlBottomField.SelectedValue),
                new KeyValuePair<string, string>("bottomleftfield", this.ddlBottomLeftField.SelectedValue),
                new KeyValuePair<string, string>("bottomrightfield", this.ddlBottomRightField.SelectedValue)
            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
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

            Response.Redirect("BedBoardManagerView.aspx?id=" + newID);

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string sql = @"DELETE FROM eboards.bedboard WHERE bedboard_id = @bedboard_id;";
            string newID = this.hdnBedBoardID.Value;
            var paramList = new List<KeyValuePair<string, string>>() {                
                new KeyValuePair<string, string>("bedboard_id", newID)
            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
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

            Response.Redirect("BoardManagerList.aspx");
        }

        protected void btnRandom_Click(object sender, EventArgs e)
        {
            GetPreviewURL();
        }
    }
}