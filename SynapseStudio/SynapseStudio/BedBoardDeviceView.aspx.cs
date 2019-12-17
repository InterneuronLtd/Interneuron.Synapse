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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class BedBoardDeviceView : System.Web.UI.Page
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

                this.hdnDeviceID.Value = id;

                try
                {
                    this.hdnUserName.Value = Session["userFullName"].ToString();
                }
                catch { }

                var paramList = new List<KeyValuePair<string, string>>()
                {
                };

                BindDropDownList(this.ddlBedBoard, "SELECT bedboard_id, bedboardname FROM eboards.bedboard", "bedboard_id", "bedboardname", 0, paramList);
                BindDropDownList(this.ddllWard, "SELECT wardcode, warddisplay FROM entitystorematerialised.meta_ward ORDER BY warddisplay", "wardcode", "warddisplay", 0, paramList);
                BindFormControls();
                

                this.lblError.Text = string.Empty;
                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;

                string uri = SynapseHelpers.GetEBoardURL();
                this.hlPreview.NavigateUrl = uri + "DynamicBedBoard.aspx";
                this.hlPreview.Target = "_blank";

            }
        }

        private void BindFormControls()
        {
            string sql = "SELECT * FROM eboards.bedboarddevice WHERE bedboarddevice_id = @bedboarddevice_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboarddevice_id", this.hdnDeviceID.Value)
            };
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                StringBuilder sbe = new StringBuilder();
                //sbe.AppendLine("<div class='contentAlertDanger'><h3 style='color: #712f2f'>Sorry, there was an error:</h3>");
                sbe.AppendLine(ex.ToString());
                //sbe.AppendLine("</div>");
                this.lblError.Visible = true;
                this.lblError.Text = sbe.ToString();
                return;
            }

            DataTable dt = ds.Tables[0];

            try
            {
                this.txtDeviceName.Text = dt.Rows[0]["bedboarddevicename"].ToString();
            }
            catch { }

            try
            {
                this.txtIPAddress.Text = dt.Rows[0]["deviceipaddress"].ToString();
            }
            catch { }

            SetDDLSource(this.ddlBedBoard, dt.Rows[0]["bedboard_id"].ToString());

            SetDDLSource(this.ddllWard, dt.Rows[0]["locationward"].ToString());

            BindBayRoomDDL();

            SetDDLSource(this.ddlBayRoom, dt.Rows[0]["locationbayroom"].ToString());

            BindBedDDL();

            SetDDLSource(this.ddlBed, dt.Rows[0]["locationbed"].ToString());


        }

        private void BindBedDDL()
        {
            string sql = "SELECT wardbaybed_id, beddisplay FROM entitystorematerialised.meta_wardbaybed WHERE wardcode = @id AND baycode = @bayroomlocation_id ORDER BY beddisplay;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", this.ddllWard.SelectedValue),
                new KeyValuePair<string, string>("bayroomlocation_id", this.ddlBayRoom.SelectedValue)
            };
            BindDropDownList(this.ddlBed, sql, "wardbaybed_id", "beddisplay", 0, paramList);
        }

        private void BindBayRoomDDL()
        {
            string sql = "SELECT baycode, baydisplay FROM entitystorematerialised.meta_wardbay WHERE wardcode = @id ORDER BY baydisplay;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", this.ddllWard.SelectedValue)
            };
            BindDropDownList(this.ddlBayRoom, sql, "baycode", "baydisplay", 0, paramList);
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
                    try
                    {
                        ddl.Items.Insert(1, items[0]);
                    }
                    catch { }
                }
            }

            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(val));
        }


        protected void ddllWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBayRoomDDL();
            BindBedDDL();
        }

        protected void ddlBayRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBedDDL();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {


            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;

            this.fgDeviceName.CssClass = noerr;
            if (string.IsNullOrEmpty(this.txtDeviceName.Text.ToString()))
            {
                this.lblError.Text = "Please enter the device name";
                this.txtDeviceName.Focus();
                this.lblError.Visible = true;
                this.fgDeviceName.CssClass = haserr;
                return;
            }

            this.fgIPAddress.CssClass = noerr;
            if (string.IsNullOrEmpty(this.txtIPAddress.Text.ToString()))
            {
                this.lblError.Text = "Please enter the IP Address";
                this.txtIPAddress.Focus();
                this.lblError.Visible = true;
                this.fgIPAddress.CssClass = haserr;
                return;
            }

            string sql = @"UPDATE eboards.bedboarddevice SET
                        bedboarddevicename = @bedboarddevicename, bedboard_id = @bedboard_id, deviceipaddress = @deviceipaddress, locationward = @locationward, locationbayroom = @locationbayroom, locationbed = @locationbed
	                    WHERE bedboarddevice_id = @bedboarddevice_id;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboarddevice_id", this.hdnDeviceID.Value),
                new KeyValuePair<string, string>("bedboarddevicename", this.txtDeviceName.Text),
                new KeyValuePair<string, string>("bedboard_id", this.ddlBedBoard.SelectedValue),
                new KeyValuePair<string, string>("deviceipaddress", this.txtIPAddress.Text),
                new KeyValuePair<string, string>("locationward", this.ddllWard.SelectedValue),
                new KeyValuePair<string, string>("locationbayroom", this.ddlBayRoom.SelectedValue),
                new KeyValuePair<string, string>("locationbed", this.ddlBed.SelectedValue)
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

            Response.Redirect("BedBoardDeviceList.aspx");

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BedBoardDeviceList.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM eboards.bedboarddevice WHERE bedboarddevice_id = @bedboarddevice_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboarddevice_id", this.hdnDeviceID.Value)
            };
            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
                Response.Redirect("BedBoardDeviceList.aspx");
            }
            catch (Exception ex)
            {
                this.lblError.Text = ex.ToString();
                this.lblError.Visible = true;
                return;
            }

        }
    }
}