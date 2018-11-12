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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class LocatorBoardDeviceView : System.Web.UI.Page
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

                BindDropDownList(this.ddlLocatorBoard, "SELECT Locatorboard_id, Locatorboardname FROM eboards.Locatorboard;", "Locatorboard_id", "Locatorboardname", 0, paramList);

                BindFormControls();

                this.lblError.Text = string.Empty;
                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;

                string uri = SynapseHelpers.GetEBoardURL();
                this.hlPreview.NavigateUrl =  uri + "DynamicLocatorBoard.aspx";
                this.hlPreview.Target = "_blank";
            }
        }


        private void BindFormControls()
        {
            string sql = "SELECT * FROM eboards.locatorboarddevice WHERE locatorboarddevice_id = @locatorboarddevice_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("locatorboarddevice_id", this.hdnDeviceID.Value)
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
                this.txtDeviceName.Text = dt.Rows[0]["Locatorboarddevicename"].ToString();
            }
            catch { }

            try
            {
                this.txtIPAddress.Text = dt.Rows[0]["deviceipaddress"].ToString();
            }
            catch { }

            SetDDLSource(this.ddlLocatorBoard, dt.Rows[0]["Locatorboard_id"].ToString());

            try
            {
                this.txtLocationCode.Text = dt.Rows[0]["locationid"].ToString();
            }
            catch { }

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


            this.fgLocationCode.CssClass = noerr;
            if (string.IsNullOrEmpty(this.txtLocationCode.Text.ToString()))
            {
                this.lblError.Text = "Please enter the Location Code";
                this.txtLocationCode.Focus();
                this.lblError.Visible = true;
                this.fgLocationCode.CssClass = haserr;
                return;
            }

            string sql = @"UPDATE eboards.Locatorboarddevice SET
                        Locatorboarddevicename = @Locatorboarddevicename, Locatorboard_id = @Locatorboard_id, deviceipaddress = @deviceipaddress, locationid = @locationid
	                    WHERE Locatorboarddevice_id = @Locatorboarddevice_id;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("Locatorboarddevice_id", this.hdnDeviceID.Value),
                new KeyValuePair<string, string>("Locatorboarddevicename", this.txtDeviceName.Text),
                new KeyValuePair<string, string>("Locatorboard_id", this.ddlLocatorBoard.SelectedValue),
                new KeyValuePair<string, string>("deviceipaddress", this.txtIPAddress.Text),
                new KeyValuePair<string, string>("locationid", this.txtLocationCode.Text)
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

            Response.Redirect("LocatorBoardDeviceView.aspx?id=" + this.hdnDeviceID.Value);

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("LocatorBoardDeviceList.aspx");
        }
    }
}