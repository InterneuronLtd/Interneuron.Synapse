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

using SynapseStudio;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;


namespace EBoards
{
    public partial class callback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPostback_Click(object sender, EventArgs e)
        {
            Session["access_token"] = hTkn.Value;
            //Response.Redirect("Login.aspx");

            string token = string.Empty;
            try
            {
                token = Session["access_token"].ToString();

            }
            catch
            {
                //not authenticated; browser will invoke oidc signin-redirect to SIS               
            }

            if (token != string.Empty)
            {
                string IPAddress = "";
                try
                {
                    IPAddress = GetIPAddress();
                }
                catch { }


                var jwttoken = SynapseHelpers.DecodeJWTToken(token);

                if (!(jwttoken is JwtSecurityToken))
                {
                    recordFailedLoginAttempt("-1", IPAddress, "unknown");
                    Response.Redirect("Logout.aspx");
                }

                if (((JwtSecurityToken)jwttoken).ValidTo <= DateTime.UtcNow)
                {
                    Response.Redirect("Logout.aspx");
                }


                if (((JwtSecurityToken)jwttoken).Claims.Count() > 1)
                {
                    var winAccNameClaim = ((JwtSecurityToken)jwttoken).Claims.Where(x => x.Type.ToLower() == "ipuid").FirstOrDefault();
                    var userFullName = ((JwtSecurityToken)jwttoken).Claims.Where(x => x.Type.ToLower() == "name").FirstOrDefault();
                    if (((JwtSecurityToken)jwttoken).Claims.Where(x => x.Type.ToLower() == "idp").FirstOrDefault().Value == "local")
                    {
                        winAccNameClaim = ((JwtSecurityToken)jwttoken).Claims.Where(x => x.Type.ToLower() == "email").FirstOrDefault();

                    }

                    if (winAccNameClaim == null && string.IsNullOrWhiteSpace(winAccNameClaim.Value))
                    {
                        recordFailedLoginAttempt("-1", IPAddress, "Studio");
                        Response.Redirect("Logout.aspx");
                    }
                    else
                    {
                        Session["SynapseUser_UserID"] = winAccNameClaim.Value;
                    }

                    if (userFullName != null)
                        if (userFullName.Value.Split(' ').Length > 1)
                        {
                            string firstName = userFullName.Value.Replace(",", "").Split(' ')[1];
                            string lastName = userFullName.Value.Replace(",", "").Split(' ')[0];

                            Session["userFullName"] = firstName + " " + lastName;
                        }
                        else
                        { Session["userFullName"] = userFullName.Value; }
                    else
                    {
                        recordFailedLoginAttempt("-1", IPAddress, "Studio");
                        Response.Redirect("Logout.aspx");
                    }



                    string returnURL = "Default.aspx";
                    try
                    {
                        returnURL = Request.Cookies[Session["SynapseUser_UserID"].ToString().ToLower() + "_SynapseStudio_ReturnURL"].Value.ToString(); ;
                    }
                    catch { }

                    recordSuccessfulLoginAttempt("0", winAccNameClaim.Value, IPAddress, "Studio");
                    Response.Redirect(returnURL);
                }
            }
        }



        private void recordFailedLoginAttempt(string userid, string IPAddress, string logintype)
        {
            string sql = "INSERT INTO systemsettings.failedlogin(emailaddress, ipaddress)	VALUES ( @emailaddress, @ipaddress); ";
            var paramListFail = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("emailaddress", userid),
                    new KeyValuePair<string, string>("ipaddress",IPAddress)
                };
            DataServices.executeSQLStatement(sql, paramListFail);
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

        private void recordSuccessfulLoginAttempt(string userid, string username, string IPAddress, string logintype)
        {
            string sql = "INSERT INTO systemsettings.loginhistory (userid, emailaddress, ipaddress) VALUES (CAST(@userid AS INT), @emailaddress, @ipaddress);";
            var paramList = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("userid", userid),
                    new KeyValuePair<string, string>("emailaddress", username),
                    new KeyValuePair<string, string>("ipaddress",IPAddress)
                };
            DataServices.executeSQLStatement(sql, paramList);
        }
    }
}