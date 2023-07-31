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
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string redirectURL = "";

                try { redirectURL = Convert.ToString(Request.QueryString["redirectURL"].ToString()); }
                catch { redirectURL = "Default.aspx"; }

                this.lblRedirect.Text = redirectURL;

                String loggedInUser = "";
                try
                {
                    loggedInUser = Session["userID"].ToString();
                }
                catch { }

                //if (loggedInUser.Length > 0)
                //{
                //    Response.Redirect(redirectURL);
                //}

                Page.Form.DefaultButton = btnLogin.UniqueID;
                Page.Form.DefaultFocus = this.txtEmail.UniqueID;

                this.lblError.Visible = false;
                this.btnResendValidationEmail.Visible = false;
                this.divThankYou.Visible = false;
                this.divError.Visible = false;
                this.lblEmailError.Visible = false;

                string msgStatus = "";

            }
        }


        private void SendEmail(string msg, string subject)
        {
            string msgStatus = "";

            int EmailStatus = EmailHelper.SendMail(msg, subject, this.hdnEmail.Value, out msgStatus);

            if (EmailStatus == 1)
            {
                this.divThankYou.Visible = true;
            }
            else
            {
                this.divThankYou.Visible = false;
                this.lblEmailError.Text = msgStatus;
                this.divError.Visible = true;
                this.lblEmailError.Visible = true;
            }

        }


        private void sendConfirmationEmail()
        {
            this.divThankYou.Visible = false;
            this.divError.Visible = false;
            this.lblEmailError.Visible = false;

            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;

            this.txtEmail.CssClass = this.txtEmail.CssClass.Replace("has-error", "");
            this.fgtxtEmail.CssClass = this.fgtxtEmail.CssClass.Replace("has-error", "");


            if (string.IsNullOrEmpty(this.txtEmail.Text.ToString()))
            {
                this.lblError.Text = "Please enter your email address";
                this.lblError.Visible = true;
                this.fgtxtEmail.CssClass = this.fgtxtEmail.CssClass.Replace("form-group", "form-group has-error");
                return;
            }
            else
            {
                //Show the success message anyway so as not to give away user does not exist
                this.divThankYou.Visible = true;
            }



            string siteURL = GetSiteURL();

            string msg = "We have received a registration request to access the physical health site.<br /><br />";

            msg += "Please click the link below (or right click and copy link into your browser) to confirm your account:<br />";


            string emailLink = siteURL + "ConfirmAccount.aspx?id=";

            string emailvalidationstring = "";
            if (ValidateUser(out emailvalidationstring))
            {
                //Build site url
                emailLink += emailvalidationstring;

                msg += "<h3>" + emailLink + "</h3></br />";

                string subject = "Confirmation Email";

                SendEmail(msg, subject);

            }
        }


        private string GetSiteURL()
        {
            string sql = "SELECT * FROM systemsettings.systemsetup WHERE systemsetupid = 1;";
            string siteURL = "";

            DataSet ds = DataServices.DataSetFromSQL(sql, null);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                try { siteURL = dt.Rows[0]["siteurl"].ToString(); } catch { }
            }

            return siteURL;
        }

        private bool ValidateUser(out string emailvalidationstring)
        {

            emailvalidationstring = "";
            string sql = "SELECT * FROM systemsettings.app_user WHERE emailaddress = @email;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("email", this.txtEmail.Text)
            };



            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                try { emailvalidationstring = dt.Rows[0]["emailvalidationstring"].ToString(); } catch { }

                return true;
            }


            return false;

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.btnResendValidationEmail.Visible = false;

            this.txtEmail.CssClass = this.txtEmail.CssClass.Replace("has-error", "");
            this.txtPassword.CssClass = this.txtPassword.CssClass.Replace("has-error", "");
            this.fgtxtEmail.CssClass = this.fgtxtEmail.CssClass.Replace("has-error", "");
            this.fgtxtPassword.CssClass = this.fgtxtPassword.CssClass.Replace("has-error", "");

            if (string.IsNullOrEmpty(this.txtEmail.Text.ToString()))
            {
                this.lblError.Text = "Please enter your email address";
                this.lblError.Visible = true;
                this.fgtxtEmail.CssClass = this.fgtxtEmail.CssClass.Replace("form-group", "form-group has-error");
                return;
            }

            if (string.IsNullOrEmpty(this.txtPassword.Text.ToString()))
            {
                this.lblError.Text = "Please enter your password";
                this.lblError.Visible = true;
                this.fgtxtPassword.CssClass = this.fgtxtPassword.CssClass.Replace("form-group", "form-group has-error");
                return;
            }


            string IPAddress = "";
            try
            {
                IPAddress = GetIPAddress();
            }
            catch { }

            string sql = "SELECT * FROM systemsettings.app_user WHERE emailaddress = @email AND userpassword = crypt(@password, userpassword);";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("email", this.txtEmail.Text),
                new KeyValuePair<string, string>("password", this.txtPassword.Text)
            };



            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                //Valid User
                Session["UserDetailsSxn"] = dt;

                //Record Login
                string userid = "0";
                try
                {
                    userid = dt.Rows[0]["userid"].ToString();
                }
                catch { }
                Session["userID"] = userid;

                string emailconfirmed = "False";
                try
                {
                    emailconfirmed = dt.Rows[0]["emailconfirmed"].ToString();
                }
                catch { }

                string userFullName = ""; 
                try
                {
                    userFullName = dt.Rows[0]["firstname"].ToString() + " " + dt.Rows[0]["lastname"].ToString();
                }
                catch { }
                Session["userFullName"] = userFullName;

                string userType = "";
                try
                {
                    userType = dt.Rows[0]["usertype"].ToString();
                }
                catch
                {
                    //Response.Redirect("Login.aspx");
                }
                Session["userType"] = userType;

                string matchedclinicianid = "";
                try
                {
                    matchedclinicianid = dt.Rows[0]["matchedclinicianid"].ToString();
                }
                catch
                {
                    //Response.Redirect("Login.aspx");
                }
                Session["matchedclinicianid"] = matchedclinicianid;


                this.hdnEmail.Value = this.txtEmail.Text;

                if (emailconfirmed == "False")
                {
                    this.lblError.Text = "Your account has been created but you have not confirmed your email address yet.<br /><br />Please check your spam folder for the email containing the link to confirm your account";
                    this.btnResendValidationEmail.Visible = true;
                    this.lblError.Visible = true;
                    return;
                }

                string isauthorised = "False";
                try
                {
                    isauthorised = dt.Rows[0]["isauthorised"].ToString();
                }
                catch { }

                if (isauthorised == "False")
                {
                    this.lblError.Text = "Your account has not been authorised yet";
                    this.lblError.Visible = true;
                    return;
                }

                sql = "INSERT INTO systemsettings.loginhistory (userid, emailaddress, ipaddress) VALUES (CAST(@userid AS INT), @emailaddress, @ipaddress);";
                var paramListHistory = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("userid", userid),
                    new KeyValuePair<string, string>("emailaddress", this.txtEmail.Text),
                    new KeyValuePair<string, string>("ipaddress",IPAddress)
                };
                DataServices.executeSQLStatement(sql, paramListHistory);

                Response.Redirect(this.lblRedirect.Text);
            }
            else
            {






                //Invalid User
                sql = "INSERT INTO systemsettings.failedlogin(emailaddress, ipaddress)	VALUES ( @emailaddress, @ipaddress); ";
                var paramListFail = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("emailaddress", this.txtEmail.Text),
                    new KeyValuePair<string, string>("ipaddress",IPAddress)
                };
                DataServices.executeSQLStatement(sql, paramListFail);
                this.lblError.Text = "Invalid Username or Password";
                this.lblError.Visible = true;
            }
        }

        private string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        protected void btnResendValidationEmail_Click(object sender, EventArgs e)
        {
            sendConfirmationEmail();
            this.btnResendValidationEmail.Visible = false;
        }
    }

}