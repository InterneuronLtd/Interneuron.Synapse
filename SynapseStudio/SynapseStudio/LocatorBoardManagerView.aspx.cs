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
    public partial class LocatorBoardManagerView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
            hdnBoardID.Value = id;

            if (!IsPostBack)
            {
                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }

                this.ltrlError.Visible = false;
                this.lblSuccess.Visible = false;
                BindDropDownList(this.ddlBaseViewNamespace, "SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace", "baseviewnamespaceid", "baseviewnamespace", 0, null);
                BindDropDownList(this.ddlList, "SELECT * FROM listsettings.listmanager ORDER BY listname", "list_id", "listname", 1, null);
                BindDBFields();
                //BindListBaseViewFields();
                //BindBaseViewList();
                //BindBaseViewFields();
                this.btnCancel.Attributes.Add("onclick", "if(confirm('Are you sure that you want to cancel any changes?')){return true;} else {return false;};");
                this.btnDelete.Attributes.Add("onclick", "if(confirm('Are you sure that you want to delete this board?')){return true;} else {return false;};");

                string uri = SynapseHelpers.GetEBoardURL() + "locatorboard.aspx?id=" + this.hdnBoardID.Value + "&Location=XXXXX";

               this.hlPreview.NavigateUrl = uri;
            }
        }

        private void BindDBFields()
        {
            string sql = "SELECT * FROM eboards.locatorboard WHERE locatorboard_id = @locatorboard_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("locatorboard_id", hdnBoardID.Value)
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
                this.txtLocatorBoardName.Text = dt.Rows[0]["locatorboardname"].ToString();
            }
            catch { }

            try
            {
                this.txtLocatorBoardDescription.Text = dt.Rows[0]["locatorboarddescription"].ToString();
            }
            catch { }

            SetDDLSource(this.ddlBaseViewNamespace, dt.Rows[0]["locationbaseviewnamespace_id"].ToString());

            BindBaseViewList();


            SetDDLSource(this.ddlList, dt.Rows[0]["list_id"].ToString());
            BindListBaseViewFields();
            SetDDLSource(this.ddlListLocationField, dt.Rows[0]["listlocationfield"].ToString());




            SetDDLSource(this.ddlBaseView, dt.Rows[0]["locationbaseview_id"].ToString());
            BindBaseViewFields();


            SetDDLSource(this.ddlLocationIDField, dt.Rows[0]["locationidfield"].ToString());
            SetDDLSource(this.ddlHeading, dt.Rows[0]["locationdisplayfield"].ToString());



            SetDDLSource(this.ddlTopLeftField, dt.Rows[0]["topleftfield"].ToString());
            SetDDLSource(this.ddlTopRightField, dt.Rows[0]["toprightfield"].ToString());


        }

        private void BindBaseViewList()
        {
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @id ORDER BY baseviewname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", this.ddlBaseViewNamespace.SelectedValue)
            };
            BindDropDownList(this.ddlBaseView, sql, "baseview_id", "baseviewname", 1, paramList);

        }

        private void BindListBaseViewFields()
        {
            string sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id IN (SELECT baseview_id FROM listsettings.listmanager WHERE list_id =  @id) ORDER BY attributename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", this.ddlList.SelectedValue)
            };
            BindDropDownList(this.ddlListLocationField, sql, "attributename", "attributename", 1, paramList);

        }

        private void BindBaseViewFields()
        {
            string sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id ORDER BY attributename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", this.ddlBaseView.SelectedValue)
            };
            BindDropDownList(this.ddlLocationIDField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlHeading, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlTopLeftField, sql, "attributename", "attributename", 1, paramList);
            BindDropDownList(this.ddlTopRightField, sql, "attributename", "attributename", 1, paramList);

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

            this.fgLocatorBoardName.CssClass = noerr;
            if (string.IsNullOrWhiteSpace(this.txtLocatorBoardName.Text))
            {
                sb.AppendLine("<li>" + "Please enter a new name for the Bed Board" + "</li>");
                this.fgLocatorBoardName.CssClass = haserr;
                errcount++;
            }



            //Lists
            this.fgList.CssClass = noerr;
            if (this.ddlList.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the underlying list for the board" + "</li>");
                this.fgList.CssClass = haserr;
                errcount++;
            }

            this.fgListLocationField.CssClass = noerr;
            if (this.ddlListLocationField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the location id field for the list" + "</li>");
                this.fgListLocationField.CssClass = haserr;
                errcount++;
            }

            this.fgBaseViewNamespace.CssClass = noerr;
            if (this.ddlBaseViewNamespace.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the location specific information baseview" + "</li>");
                this.fgBaseViewNamespace.CssClass = haserr;
                errcount++;
            }

            this.fgBaseView.CssClass = noerr;
            if (this.ddlBaseView.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select a location specific baseview" + "</li>");
                this.fgBaseView.CssClass = haserr;
                errcount++;
            }




            this.fgLocationIDField.CssClass = noerr;
            if (this.ddlLocationIDField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Bed Code field from the baseview" + "</li>");
                this.fgLocationIDField.CssClass = haserr;
                errcount++;
            }


            this.fgHeading.CssClass = noerr;
            if (this.ddlHeading.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Heading column from the baseview" + "</li>");
                this.fgHeading.CssClass = haserr;
                errcount++;
            }

            this.fgTopLeftField.CssClass = noerr;
            if (this.ddlTopLeftField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Top Left Section" + "</li>");
                this.fgTopLeftField.CssClass = haserr;
                errcount++;
            }

            this.fgTopRightField.CssClass = noerr;
            if (ddlTopRightField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Top Right Section" + "</li>");
                this.fgTopRightField.CssClass = haserr;
                errcount++;
            }




            if (errcount > 0)
            {
                sb.AppendLine("</ul></div>");
                this.ltrlError.Visible = true;
                this.ltrlError.Text = sb.ToString();
                return;
            }

            string sql = @"UPDATE eboards.LocatorBoard SET
                                _createdby = @_createdby, 	                             
	                            LocatorBoardname = @LocatorBoardname, 
	                            LocatorBoarddescription = @LocatorBoarddescription, 
	                            list_id = @list_id,
                                listlocationfield = @listlocationfield,
                                locationbaseviewnamespace_id = @locationbaseviewnamespace_id, 
	                            locationbaseview_id = @locationbaseview_id,              
                                locationidfield = @locationidfield,
                                locationdisplayfield = @locationdisplayfield,
	                            topleftfield = @topleftfield, 
	                            toprightfield = @toprightfield
                            WHERE LocatorBoard_id = @LocatorBoard_id";

            string newID = this.hdnBoardID.Value;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("_createdby", this.hdnUserName.Value),
                new KeyValuePair<string, string>("LocatorBoard_id", newID),
                new KeyValuePair<string, string>("LocatorBoardname", this.txtLocatorBoardName.Text),
                new KeyValuePair<string, string>("LocatorBoarddescription", this.txtLocatorBoardDescription.Text),

                new KeyValuePair<string, string>("list_id", this.ddlList.SelectedValue),
                new KeyValuePair<string, string>("listlocationfield", this.ddlListLocationField.SelectedValue),

                new KeyValuePair<string, string>("locationbaseviewnamespace_id", this.ddlBaseViewNamespace.SelectedValue),
                new KeyValuePair<string, string>("locationbaseview_id", this.ddlBaseView.SelectedValue),
                new KeyValuePair<string, string>("locationidfield", this.ddlLocationIDField.SelectedValue),
                new KeyValuePair<string, string>("locationdisplayfield", this.ddlHeading.SelectedValue),
                new KeyValuePair<string, string>("topleftfield", this.ddlTopLeftField.SelectedValue),
                new KeyValuePair<string, string>("toprightfield", this.ddlTopRightField.SelectedValue)
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

            Response.Redirect("LocatorBoardManagerView.aspx?id=" + newID);

        }

        protected void ddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindListBaseViewFields();
        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string sql = @"DELETE FROM eboards.LocatorBoard WHERE LocatorBoard_id = @LocatorBoard_id;";
            string newID = this.hdnBoardID.Value;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("LocatorBoard_id", newID)
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

        private void GetPreviewURL()
        {
            //string sql = "SELECT * FROM eboards.LocatorBoard WHERE LocatorBoard_id = @LocatorBoard_id;";
            //DataSet ds = new DataSet();
            //var paramList = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("LocatorBoard_id", hdnBoardID.Value)
            //};
            //try
            //{
            //    ds = DataServices.DataSetFromSQL(sql, paramList);
            //}
            //catch (Exception ex)
            //{
            //    StringBuilder sbe = new StringBuilder();
            //    sbe.AppendLine("<div class='contentAlertDanger'><h3 style='color: #712f2f'>Sorry, there was an error:</h3>");
            //    sbe.AppendLine(ex.ToString());
            //    sbe.AppendLine("</div>");
            //    //this.ltrlError.Visible = true;
            //    //this.ltrlError.Text = sbe.ToString();
            //    return;
            //}

            //DataTable dt = ds.Tables[0];

            //string baseview_id = "";
            //try
            //{
            //    baseview_id = dt.Rows[0]["baseview_id"].ToString();
            //}
            //catch { }

            //string baseviewname = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(baseview_id);




            //string PersonIDField = "";
            //try
            //{
            //    PersonIDField = dt.Rows[0]["baseviewpersonidfield"].ToString();
            //}
            //catch { }

            //string EncounterIDField = "";
            //try
            //{
            //    EncounterIDField = dt.Rows[0]["baseviewencounteridfield"].ToString();
            //}
            //catch { }

            //string WardField = "";
            //try
            //{
            //    WardField = dt.Rows[0]["baseviewwardfield"].ToString();
            //}
            //catch { }

            //string BedField = "";
            //try
            //{
            //    BedField = dt.Rows[0]["baseviewbedfield"].ToString();
            //}
            //catch { }


            //string sqlBoard = "SELECT " + WardField + " as WardField, " + BedField + " as BedField" +
            //                  " FROM baseview." + baseviewname + " order by random() LIMIT 1;";

            //var paramListbOARD = new List<KeyValuePair<string, string>>()
            //{
            //};

            //DataSet dsBoard = new DataSet();
            //try
            //{
            //    dsBoard = DataServices.DataSetFromSQL(sqlBoard, paramListbOARD);
            //}
            //catch (Exception ex)
            //{
            //    StringBuilder sbe = new StringBuilder();
            //    sbe.AppendLine("<div class='contentAlertDanger'><h3 style='color: #712f2f'>Sorry, there was an error:</h3>");
            //    sbe.AppendLine(ex.ToString());
            //    sbe.AppendLine("</div>");
            //    //this.ltrlError.Visible = true;
            //    //this.ltrlError.Text = sbe.ToString();
            //    return;
            //}

            //DataTable dtBoard = dsBoard.Tables[0];

            //string ward = "";
            //try
            //{
            //    ward = dtBoard.Rows[0]["WardField"].ToString();
            //}
            //catch { }

            //string bed = "";
            //try
            //{
            //    bed = dtBoard.Rows[0]["BedField"].ToString();
            //}
            //catch { }

            //string apiURL = SynapseHelpers.GetAPIURL();

            //string uri = "http://XXXXXXXXXXXXXX/LocatorBoard.aspx?LocatorBoardID=" + this.hdnBoardID.Value + "&Ward=" + ward + "&Bed=" + bed;

            //this.hlPreview.NavigateUrl = uri;

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

            this.fgLocatorBoardName.CssClass = noerr;
            if (string.IsNullOrWhiteSpace(this.txtLocatorBoardName.Text))
            {
                sb.AppendLine("<li>" + "Please enter a new name for the Bed Board" + "</li>");
                this.fgLocatorBoardName.CssClass = haserr;
                errcount++;
            }



            //Lists
            this.fgList.CssClass = noerr;
            if (this.ddlList.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the underlying list for the board" + "</li>");
                this.fgList.CssClass = haserr;
                errcount++;
            }

            this.fgListLocationField.CssClass = noerr;
            if (this.ddlListLocationField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the location id field for the list" + "</li>");
                this.fgListLocationField.CssClass = haserr;
                errcount++;
            }

            this.fgBaseViewNamespace.CssClass = noerr;
            if (this.ddlBaseViewNamespace.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the location specific information baseview" + "</li>");
                this.fgBaseViewNamespace.CssClass = haserr;
                errcount++;
            }

            this.fgBaseView.CssClass = noerr;
            if (this.ddlBaseView.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select a location specific baseview" + "</li>");
                this.fgBaseView.CssClass = haserr;
                errcount++;
            }




            this.fgLocationIDField.CssClass = noerr;
            if (this.ddlLocationIDField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Bed Code field from the baseview" + "</li>");
                this.fgLocationIDField.CssClass = haserr;
                errcount++;
            }


            this.fgHeading.CssClass = noerr;
            if (this.ddlHeading.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the Heading column from the baseview" + "</li>");
                this.fgHeading.CssClass = haserr;
                errcount++;
            }

            this.fgTopLeftField.CssClass = noerr;
            if (this.ddlTopLeftField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Top Left Section" + "</li>");
                this.fgTopLeftField.CssClass = haserr;
                errcount++;
            }

            this.fgTopRightField.CssClass = noerr;
            if (ddlTopRightField.SelectedIndex == 0)
            {
                sb.AppendLine("<li>" + "Please select the field column from the baseview for the Top Right Section" + "</li>");
                this.fgTopRightField.CssClass = haserr;
                errcount++;
            }




            if (errcount > 0)
            {
                sb.AppendLine("</ul></div>");
                this.ltrlError.Visible = true;
                this.ltrlError.Text = sb.ToString();
                return;
            }

            string sql = @"INSERT INTO eboards.LocatorBoard(
                                _createdby, 
	                            LocatorBoard_id, 
	                            LocatorBoardname, 
	                            LocatorBoarddescription, 
	                            list_id,
                                listlocationfield,
                                locationbaseviewnamespace_id, 
	                            locationbaseview_id,              
                                locationidfield,
                                locationdisplayfield,
	                            topleftfield, 
	                            toprightfield
                            )
                            VALUES(
                                @_createdby, 
	                            @LocatorBoard_id, 
	                            @LocatorBoardname, 
	                            @LocatorBoarddescription, 
	                            @list_id,
                                @listlocationfield,
                                @locationbaseviewnamespace_id, 
	                            @locationbaseview_id,       
                                @locationidfield,
                                @locationdisplayfield,
	                            @topleftfield, 
	                            @toprightfield
                            )";

            string newID = this.hdnBoardID.Value;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("_createdby", this.hdnUserName.Value),
                new KeyValuePair<string, string>("LocatorBoard_id", newID),
                new KeyValuePair<string, string>("LocatorBoardname", this.txtLocatorBoardName.Text),
                new KeyValuePair<string, string>("LocatorBoarddescription", this.txtLocatorBoardDescription.Text),

                new KeyValuePair<string, string>("list_id", this.ddlList.SelectedValue),
                new KeyValuePair<string, string>("listlocationfield", this.ddlListLocationField.SelectedValue),

                new KeyValuePair<string, string>("locationbaseviewnamespace_id", this.ddlBaseViewNamespace.SelectedValue),
                new KeyValuePair<string, string>("locationbaseview_id", this.ddlBaseView.SelectedValue),
                new KeyValuePair<string, string>("locationidfield", this.ddlLocationIDField.SelectedValue),
                new KeyValuePair<string, string>("locationdisplayfield", this.ddlHeading.SelectedValue),
                new KeyValuePair<string, string>("topleftfield", this.ddlTopLeftField.SelectedValue),
                new KeyValuePair<string, string>("toprightfield", this.ddlTopRightField.SelectedValue)
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

            Response.Redirect("LocatorBoardManagerView.aspx?id=" + newID);

        }
    }
}