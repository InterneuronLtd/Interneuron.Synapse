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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class Clients : System.Web.UI.Page
    {
        string clientupdateid = "";


        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                try
                {
                    clientupdateid = Request.QueryString["id"].ToString();
                    if (clientupdateid.Trim() != "")
                    {
                        btnAddNewClient.Text = "Update Client";
                        lblformname.Text = "Update Client";
                        txtClientId.Enabled = false;
                        loadFormById(clientupdateid);
                    }
                    else
                    {
                        txtClientId.Enabled = true;
                        lblformname.Text = "New Client";
                        btnAddNewClient.Text = "Add new client";
                    }
                }
                catch
                {

                }
                this.lblError.Text = string.Empty;
                this.lblError.Visible = false;
                this.lblSuccess.Text = string.Empty;
                this.lblSuccess.Visible = false;
            }
        }

        private void loadFormById(string id)
        {
            string sql = "SELECT * FROM \"Clients\" WHERE \"ClientId\" = @ClientId";

            var param = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("ClientId",id)
                };

            DataSet ds = DataServices.DataSetFromSQL(sql, param, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
            DataTable dt = ds.Tables[0];
           chkEnabled.Checked = ds.Tables[0].Rows[0]["Enabled"].ToString() == "True" ? true : false;
           txtClientId.Text = ds.Tables[0].Rows[0]["ClientId"].ToString();
           chkReqClntSecret.Checked = ds.Tables[0].Rows[0]["RequireClientSecret"].ToString() == "True" ? true : false;
           txtClientName.Text = ds.Tables[0].Rows[0]["ClientName"].ToString();
           txtDesc.Text = ds.Tables[0].Rows[0]["Description"].ToString();
           chkReqCon.Checked = ds.Tables[0].Rows[0]["RequireConsent"].ToString() == "True" ? true : false;
           chkAllAccTkn.Checked = ds.Tables[0].Rows[0]["AllowAccessTokensViaBrowser"].ToString() == "True" ? true : false;
           chkAllOffAcc.Checked = ds.Tables[0].Rows[0]["AllowOfflineAccess"].ToString() == "True" ? true : false;
          txtIdTknLt.Text = ds.Tables[0].Rows[0]["IdentityTokenLifetime"].ToString();
           txtAccTknLt.Text = ds.Tables[0].Rows[0]["AccessTokenLifetime"].ToString();
           txtAuthCodeLt.Text = ds.Tables[0].Rows[0]["AuthorizationCodeLifetime"].ToString();
            txtConLt.Text = ds.Tables[0].Rows[0]["ConsentLifetime"].ToString();
            txtAbReTknLt.Text = ds.Tables[0].Rows[0]["AbsoluteRefreshTokenLifetime"].ToString();
            txtSlReTknLt.Text = ds.Tables[0].Rows[0]["SlidingRefreshTokenLifetime"].ToString();
            chkReTknUs.Checked = ds.Tables[0].Rows[0]["RefreshTokenUsage"].ToString() == "1" ? true : false;
            chkUpdAccTknClRe.Checked = ds.Tables[0].Rows[0]["UpdateAccessTokenClaimsOnRefresh"].ToString() == "True" ? true : false;
            chkReTknExp.Checked = ds.Tables[0].Rows[0]["RefreshTokenExpiration"].ToString() == "1" ? true : false;
            chkEnLocLog.Checked = ds.Tables[0].Rows[0]["EnableLocalLogin"].ToString() == "True" ? true : false;
            chkAlSenClntCl.Checked = ds.Tables[0].Rows[0]["AlwaysSendClientClaims"].ToString() == "True" ? true : false;
            txtClntClPre.Text = ds.Tables[0].Rows[0]["ClientClaimsPrefix"].ToString();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            chkEnabled.Checked = true;
            txtClientId.Text = string.Empty;
            chkReqClntSecret.Checked = false;
            txtClientName.Text = string.Empty;
            txtDesc.Text = string.Empty;
            chkReqCon.Checked = false;
            chkAllAccTkn.Checked = false;
            chkAllOffAcc.Checked = true;
            txtIdTknLt.Text = "1800";
            txtAccTknLt.Text = "1800";
            txtAuthCodeLt.Text = "1800";
            txtConLt.Text = "1800";
            txtAbReTknLt.Text = "2592000";
            txtSlReTknLt.Text = "1296000";
            chkReTknUs.Checked = true;
            chkUpdAccTknClRe.Checked = true;
            chkReTknExp.Checked = true;
            chkEnLocLog.Checked = true;
            chkAlSenClntCl.Checked = false;
            txtClntClPre.Text = string.Empty;

            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSuccess.Text = string.Empty;
            lblSuccess.Visible = false;
        }

        protected void btnAddNewClient_Click(object sender, EventArgs e)
        {
          
            if (btnAddNewClient.Text == "Update Client")
            {
                updateClient();
            }
            else
            {
                AddNewClient();
            }
        }
        private void updateClient()
        {
            try
            {
                string sql = "SELECT * FROM \"Clients\" WHERE \"ClientId\" = @ClientId";

                var param = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("ClientId", txtClientId.Text)
                };

                DataSet ds = DataServices.DataSetFromSQL(sql, param, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count != 1)
                {
                    lblError.Text = "Client Id Not exists";
                    txtClientId.Focus();
                    lblError.Visible = true;
                    pnlClientId.CssClass = "form - group has - error";
                    return;
                }
                else
                {
                    string insertSQL = "  UPDATE \"Clients\" SET "
                        + "  \"Enabled\" =CAST(@Enabled AS BOOLEAN), "

                        + "  \"RequireClientSecret\" =  CAST(@RequireClientSecret AS BOOLEAN), "
                        + "  \"ClientName\" = @ClientName,"
                        + "  \"Description\" =  @Description, "
                        + "  \"RequireConsent\" = CAST(@RequireConsent AS BOOLEAN), "

                        + "  \"AllowAccessTokensViaBrowser\" = CAST(@AllowAccessTokensViaBrowser AS BOOLEAN), "

                        + "  \"AllowOfflineAccess\" =  CAST(@AllowOfflineAccess AS BOOLEAN), "
                        + "  \"IdentityTokenLifetime\" =  CAST(@IdentityTokenLifetime AS INT), "
                        + "  \"AccessTokenLifetime\" =  CAST(@AccessTokenLifetime AS INT), "
                        + "  \"AuthorizationCodeLifetime\" =  CAST(@AuthorizationCodeLifetime AS INT), "
                        + "  \"ConsentLifetime\" = CAST(@ConsentLifetime AS INT), "
                        + "  \"AbsoluteRefreshTokenLifetime\" =  CAST(@AbsoluteRefreshTokenLifetime AS INT), "
                        + "  \"SlidingRefreshTokenLifetime\" =  CAST(@SlidingRefreshTokenLifetime AS INT), "
                        + "  \"RefreshTokenUsage\" =  CAST(@RefreshTokenUsage AS INT), "
                        + "  \"UpdateAccessTokenClaimsOnRefresh\" =  CAST(@UpdateAccessTokenClaimsOnRefresh AS BOOLEAN), "
                        + "  \"RefreshTokenExpiration\" =CAST(@RefreshTokenExpiration AS INT), "
                        + "  \"EnableLocalLogin\" = CAST(@EnableLocalLogin AS BOOLEAN), "
                        + "  \"AlwaysSendClientClaims\" =CAST(@AlwaysSendClientClaims AS BOOLEAN), "
                        + "  \"ClientClaimsPrefix\" = @ClientClaimsPrefix "
                        + " WHERE \"ClientId\" = @ClientId ";


                    var paramList = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("Enabled", chkEnabled.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientId", txtClientId.Text),
                    new KeyValuePair<string, string>("RequireClientSecret", chkReqClntSecret.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientName", txtClientName.Text),
                    new KeyValuePair<string, string>("Description", txtDesc.Text),
                    new KeyValuePair<string, string>("RequireConsent", chkReqCon.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("AllowAccessTokensViaBrowser", chkAllAccTkn.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("AllowOfflineAccess", chkAllOffAcc.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("IdentityTokenLifetime", txtIdTknLt.Text),
                    new KeyValuePair<string, string>("AccessTokenLifetime", txtAccTknLt.Text),
                    new KeyValuePair<string, string>("AuthorizationCodeLifetime", txtAuthCodeLt.Text),
                    new KeyValuePair<string, string>("ConsentLifetime", txtConLt.Text),
                    new KeyValuePair<string, string>("AbsoluteRefreshTokenLifetime", txtAbReTknLt.Text),
                    new KeyValuePair<string, string>("SlidingRefreshTokenLifetime", txtSlReTknLt.Text),
                    new KeyValuePair<string, string>("RefreshTokenUsage", chkReTknUs.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("UpdateAccessTokenClaimsOnRefresh", chkUpdAccTknClRe.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("RefreshTokenExpiration", chkReTknExp.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("EnableLocalLogin", chkEnLocLog.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("AlwaysSendClientClaims", chkAlSenClntCl.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientClaimsPrefix", txtClntClPre.Text)
                };

                    DataServices.ExcecuteNonQueryFromSQL(insertSQL, paramList, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);

                    if (!string.IsNullOrEmpty(GetClientId(this.txtClientId.Text)) || GetClientId(this.txtClientId.Text) != "An error occurred")
                    {
                        string clientId = GetClientId(this.txtClientId.Text);

                        Response.Redirect("ClientScopes.aspx?id=" + clientId);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.ToString();
                lblError.Visible = true;
                return;
            }
        }
        private void AddNewClient()
        {
            try
            {
                string sql = "SELECT * FROM \"Clients\" WHERE \"ClientId\" = @ClientId";

                var param = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("ClientId", this.txtClientId.Text)
                };

                DataSet ds = DataServices.DataSetFromSQL(sql, param, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    this.lblError.Text = "Client Id already exists";
                    this.txtClientId.Focus();
                    this.lblError.Visible = true;
                    this.pnlClientId.CssClass = "form - group has - error";
                    return;
                }
                else
                {
                    string insertSQL = "INSERT INTO \"Clients\" (\"Enabled\", \"ClientId\", \"ProtocolType\", \"RequireClientSecret\", \"ClientName\", "
                           + " \"Description\", \"RequireConsent\", \"AllowRememberConsent\", \"AlwaysIncludeUserClaimsInIdToken\", "
                           + " \"RequirePkce\", \"AllowPlainTextPkce\", \"AllowAccessTokensViaBrowser\", \"FrontChannelLogoutSessionRequired\", "
                           + " \"BackChannelLogoutSessionRequired\", \"AllowOfflineAccess\", \"IdentityTokenLifetime\", \"AccessTokenLifetime\", "
                           + " \"AuthorizationCodeLifetime\", \"ConsentLifetime\", \"AbsoluteRefreshTokenLifetime\", \"SlidingRefreshTokenLifetime\", "
                           + " \"RefreshTokenUsage\", \"UpdateAccessTokenClaimsOnRefresh\", \"RefreshTokenExpiration\", \"AccessTokenType\", "
                           + " \"EnableLocalLogin\", \"IncludeJwtId\", \"AlwaysSendClientClaims\", \"ClientClaimsPrefix\") "
                           + " VALUES(CAST(@Enabled AS BOOLEAN), @ClientId, 'oidc', CAST(@RequireClientSecret AS BOOLEAN), @ClientName, "
                           + " @Description, CAST(@RequireConsent AS BOOLEAN), true, true, "
                           + " false, true, CAST(@AllowAccessTokensViaBrowser AS BOOLEAN), false, "
                           + " false, CAST(@AllowOfflineAccess AS BOOLEAN), CAST(@IdentityTokenLifetime AS INT), CAST(@AccessTokenLifetime AS INT), "
                           + " CAST(@AuthorizationCodeLifetime AS INT), CAST(@ConsentLifetime AS INT), CAST(@AbsoluteRefreshTokenLifetime AS INT), CAST(@SlidingRefreshTokenLifetime AS INT), "
                           + " CAST(@RefreshTokenUsage AS INT), CAST(@UpdateAccessTokenClaimsOnRefresh AS BOOLEAN), CAST(@RefreshTokenExpiration AS INT), 0, "
                           + " CAST(@EnableLocalLogin AS BOOLEAN), false, CAST(@AlwaysSendClientClaims AS BOOLEAN), @ClientClaimsPrefix);";

                    var paramList = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("Enabled", this.chkEnabled.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientId", this.txtClientId.Text),
                    new KeyValuePair<string, string>("RequireClientSecret", this.chkReqClntSecret.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientName", this.txtClientName.Text),
                    new KeyValuePair<string, string>("Description", this.txtDesc.Text),
                    new KeyValuePair<string, string>("RequireConsent", this.chkReqCon.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("AllowAccessTokensViaBrowser", this.chkAllAccTkn.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("AllowOfflineAccess", this.chkAllOffAcc.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("IdentityTokenLifetime", this.txtIdTknLt.Text),
                    new KeyValuePair<string, string>("AccessTokenLifetime", this.txtAccTknLt.Text),
                    new KeyValuePair<string, string>("AuthorizationCodeLifetime", this.txtAuthCodeLt.Text),
                    new KeyValuePair<string, string>("ConsentLifetime", this.txtConLt.Text),
                    new KeyValuePair<string, string>("AbsoluteRefreshTokenLifetime", this.txtAbReTknLt.Text),
                    new KeyValuePair<string, string>("SlidingRefreshTokenLifetime", this.txtSlReTknLt.Text),
                    new KeyValuePair<string, string>("RefreshTokenUsage", this.chkReTknUs.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("UpdateAccessTokenClaimsOnRefresh", this.chkUpdAccTknClRe.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("RefreshTokenExpiration", this.chkReTknExp.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("EnableLocalLogin", this.chkEnLocLog.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("AlwaysSendClientClaims", this.chkAlSenClntCl.Checked ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientClaimsPrefix", this.txtClntClPre.Text)
                };

                    DataServices.ExcecuteNonQueryFromSQL(insertSQL, paramList, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);

                    if (!string.IsNullOrEmpty(GetClientId(this.txtClientId.Text)) || GetClientId(this.txtClientId.Text) != "An error occurred")
                    {
                        string clientId = GetClientId(this.txtClientId.Text);

                        Response.Redirect("ClientScopes.aspx?id=" + clientId);
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = ex.ToString();
                this.lblError.Visible = true;
                return;
            }
        }

        private string GetClientId(string clientId)
        {
            try
            {
                string sql = "SELECT \"Id\" FROM public.\"Clients\" WHERE \"ClientId\" = @ClientId;";

                var paramList = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("ClientId", clientId)
                };

                DataSet ds = DataServices.DataSetFromSQL(sql, paramList, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);

                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = ex.ToString();
                this.lblError.Visible = true;
                return "An error occurred";
            }
        }
    }
}