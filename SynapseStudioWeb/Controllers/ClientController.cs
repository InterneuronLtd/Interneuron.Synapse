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


﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        public IActionResult NewClient(string id)
        {
            if (id == null)
            {
               
                ViewBag.buttontext = "Add New Client";
                ClientModel module = new ClientModel();
              
                module.posttype = "add";
                return View(module);
            }
            else
            {
                ViewBag.buttontext = "Update Client";
                return View(loadFormById(id)); 
            }
           
        }
        private ClientModel loadFormById(string id)
        {
            ClientModel module = new ClientModel();
            string sql = "SELECT * FROM \"Clients\" WHERE \"ClientId\" = @ClientId";

            var param = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("ClientId",id)
                };

            DataSet ds = DataServices.DataSetFromSQL(sql, param, "connectionString_SynapseIdentityStore");
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count != 1)
            {

                module.ErrorMessage= "Client Id Not exists";
            }
            else
            {
                module.ErrorMessage = "";
               module.Enabled = ds.Tables[0].Rows[0]["Enabled"].ToString() == "True" ? true : false;
                module.ClientId = ds.Tables[0].Rows[0]["ClientId"].ToString();
                module.ClientSecret = ds.Tables[0].Rows[0]["RequireClientSecret"].ToString() == "True" ? true : false;
                module.ClientName = ds.Tables[0].Rows[0]["ClientName"].ToString();
                module.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                module.Requireconsent = ds.Tables[0].Rows[0]["RequireConsent"].ToString() == "True" ? true : false;
                module.accesstokensbrowser = ds.Tables[0].Rows[0]["AllowAccessTokensViaBrowser"].ToString() == "True" ? true : false;
                module.OfflineAccess = ds.Tables[0].Rows[0]["AllowOfflineAccess"].ToString() == "True" ? true : false;
                module.IdentityToken = ds.Tables[0].Rows[0]["IdentityTokenLifetime"].ToString();
                module.AccessToken = ds.Tables[0].Rows[0]["AccessTokenLifetime"].ToString();
                module.AuthorizationCode = ds.Tables[0].Rows[0]["AuthorizationCodeLifetime"].ToString();
                module.ConsentLifetime = ds.Tables[0].Rows[0]["ConsentLifetime"].ToString();
                module.AbsoluteRefreshToken = ds.Tables[0].Rows[0]["AbsoluteRefreshTokenLifetime"].ToString();
                module.SlidingRefreshToken = ds.Tables[0].Rows[0]["SlidingRefreshTokenLifetime"].ToString();
                module.RefreshTokenUsage = ds.Tables[0].Rows[0]["RefreshTokenUsage"].ToString() == "1" ? true : false;
                module.UpdateAccessToken = ds.Tables[0].Rows[0]["UpdateAccessTokenClaimsOnRefresh"].ToString() == "True" ? true : false;
                module.RefreshTokenExpiration = ds.Tables[0].Rows[0]["RefreshTokenExpiration"].ToString() == "1" ? true : false;
                module.EnableLocalLogin = ds.Tables[0].Rows[0]["EnableLocalLogin"].ToString() == "True" ? true : false;
                module.AlwaysSendClientClaims = ds.Tables[0].Rows[0]["AlwaysSendClientClaims"].ToString() == "True" ? true : false;
                module.ClientClaimsPrefix = ds.Tables[0].Rows[0]["ClientClaimsPrefix"].ToString();
            }
            module.posttype = "update";
            return module;
        }
        public IActionResult CreateNewClient(ClientModel clientModel)
        {
            if (clientModel.posttype == "add")
            {
                clientModel.ErrorMessage = AddNewClient(clientModel);
            }
            else
            {
                clientModel.ErrorMessage=  updateClient(clientModel);
                

            }
            //return View("NewClient", clientModel);
            return RedirectToAction("ClientScopes", "ClientManager", new { id = GetClientId(clientModel.ClientId) });
        }

        public string  updateClient(ClientModel module)
        {
            string sql = "SELECT * FROM \"Clients\" WHERE \"ClientId\" = @ClientId";

            var param = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("ClientId",module.ClientId)
                };

            DataSet ds = DataServices.DataSetFromSQL(sql, param, "connectionString_SynapseIdentityStore");
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count != 1)
            {
                
                return "Client Id Not exists";
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
                    new KeyValuePair<string, string>("Enabled", module.Enabled==true ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientId", module.ClientId),
                    new KeyValuePair<string, string>("RequireClientSecret",module.ClientSecret==true ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientName", module.ClientName),
                    new KeyValuePair<string, string>("Description",module.Description),
                    new KeyValuePair<string, string>("RequireConsent",module.Requireconsent==true ? "1" : "0"),
                    new KeyValuePair<string, string>("AllowAccessTokensViaBrowser", module.accesstokensbrowser==true ? "1" : "0"),
                    new KeyValuePair<string, string>("AllowOfflineAccess", module.OfflineAccess==true ? "1" : "0"),
                    new KeyValuePair<string, string>("IdentityTokenLifetime", module.IdentityToken),
                    new KeyValuePair<string, string>("AccessTokenLifetime", module.AccessToken),
                    new KeyValuePair<string, string>("AuthorizationCodeLifetime", module.AuthorizationCode),
                    new KeyValuePair<string, string>("ConsentLifetime", module.ConsentLifetime),
                    new KeyValuePair<string, string>("AbsoluteRefreshTokenLifetime",  module.AbsoluteRefreshToken),
                    new KeyValuePair<string, string>("SlidingRefreshTokenLifetime", module.SlidingRefreshToken),
                    new KeyValuePair<string, string>("RefreshTokenUsage", module.RefreshTokenUsage==true ? "1" : "0"),
                    new KeyValuePair<string, string>("UpdateAccessTokenClaimsOnRefresh", module.UpdateAccessToken==true ? "1" : "0"),
                    new KeyValuePair<string, string>("RefreshTokenExpiration", module.RefreshTokenExpiration==true ? "1" : "0"),
                    new KeyValuePair<string, string>("EnableLocalLogin", module.EnableLocalLogin==true ? "1" : "0"),
                    new KeyValuePair<string, string>("AlwaysSendClientClaims", module.AlwaysSendClientClaims==true ? "1" : "0"),
                   new KeyValuePair<string, string>("ClientClaimsPrefix", module.ClientClaimsPrefix)
                };

                DataServices.ExcecuteNonQueryFromSQL(insertSQL, paramList, "connectionString_SynapseIdentityStore");
                return "";
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

                DataSet ds = DataServices.DataSetFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");

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
              
                return "An error occurred";
            }
        }
        private string  AddNewClient(ClientModel module)
        {
            try
            {
                string sql = "SELECT * FROM \"Clients\" WHERE \"ClientId\" = @ClientId";

                var param = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("ClientId",module.ClientId)
                };

                DataSet ds = DataServices.DataSetFromSQL(sql, param, "connectionString_SynapseIdentityStore");
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    //this.lblError.Text = "Client Id already exists";
                    //this.txtClientId.Focus();
                    //this.lblError.Visible = true;
                    //this.pnlClientId.CssClass = "form - group has - error";
                    return "Client Id already exists";
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
                    new KeyValuePair<string, string>("Enabled", module.Enabled==true ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientId", module.ClientId),
                    new KeyValuePair<string, string>("RequireClientSecret",module.ClientSecret==true ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientName", module.ClientName),
                    new KeyValuePair<string, string>("Description",module.Description),
                    new KeyValuePair<string, string>("RequireConsent",module.Requireconsent==true ? "1" : "0"),
                    new KeyValuePair<string, string>("AllowAccessTokensViaBrowser", module.accesstokensbrowser==true ? "1" : "0"),
                    new KeyValuePair<string, string>("AllowOfflineAccess", module.OfflineAccess==true ? "1" : "0"),
                    new KeyValuePair<string, string>("IdentityTokenLifetime", module.IdentityToken),
                    new KeyValuePair<string, string>("AccessTokenLifetime", module.AccessToken),
                    new KeyValuePair<string, string>("AuthorizationCodeLifetime", module.AuthorizationCode),
                    new KeyValuePair<string, string>("ConsentLifetime", module.ConsentLifetime),
                    new KeyValuePair<string, string>("AbsoluteRefreshTokenLifetime", module.AbsoluteRefreshToken),
                    new KeyValuePair<string, string>("SlidingRefreshTokenLifetime", module.SlidingRefreshToken),
                    new KeyValuePair<string, string>("RefreshTokenUsage", module.RefreshTokenUsage==true ? "1" : "0"),
                    new KeyValuePair<string, string>("UpdateAccessTokenClaimsOnRefresh", module.UpdateAccessToken==true ? "1" : "0"),
                    new KeyValuePair<string, string>("RefreshTokenExpiration", module.RefreshTokenExpiration==true ? "1" : "0"),
                    new KeyValuePair<string, string>("EnableLocalLogin", module.EnableLocalLogin==true ? "1" : "0"),
                    new KeyValuePair<string, string>("AlwaysSendClientClaims", module.AlwaysSendClientClaims==true ? "1" : "0"),
                    new KeyValuePair<string, string>("ClientClaimsPrefix", module.ClientClaimsPrefix)
                };

                    DataServices.ExcecuteNonQueryFromSQL(insertSQL, paramList, "connectionString_SynapseIdentityStore");
                    return "";
                    //if (!string.IsNullOrEmpty(GetClientId(this.txtClientId.Text)) || GetClientId(this.txtClientId.Text) != "An error occurred")
                    //{
                    //    string clientId = GetClientId(this.txtClientId.Text);

                    //    Response.Redirect("ClientScopes.aspx?id=" + clientId);
                    //}
                }
            }
            catch (Exception ex)
            {
                //this.lblError.Text = ex.ToString();
                //this.lblError.Visible = true;
                return ex.ToString();
            }
        }

    }
}