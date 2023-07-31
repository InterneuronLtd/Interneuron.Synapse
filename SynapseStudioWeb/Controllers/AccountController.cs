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
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;
namespace SynapseStudioWeb.Controllers
{

    public class AccountController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index(string access_token, string code)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    string accessToken =  await HttpContext.GetTokenAsync("access_token");
            //    string idToken = await  HttpContext.GetTokenAsync("id_token");

            //    // Now you can use them. For more info on when and how to use the 
            //    // access_token and id_token, see https://auth0.com/docs/tokens
            //}
            string token = await HttpContext.GetTokenAsync("id_token");
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpContext.Session.SetString("access_token", accessToken);
            var jwttoken = SynapseHelpers.DecodeJWTToken(accessToken);

            //if (!(jwttoken is JwtSecurityToken))
            //{
            //    recordFailedLoginAttempt("-1", IPAddress, "unknown");
            //    Response.Redirect("Logout.aspx");
            //}

            //if (((JwtSecurityToken)jwttoken).ValidTo <= DateTime.UtcNow)
            //{
            //    return RedirectToAction("Logout");
            //}

            if (((JwtSecurityToken)jwttoken).Claims.Count() > 1)
            {
                var winAccNameClaim = ((JwtSecurityToken)jwttoken).Claims.Where(x => x.Type.ToLower() == "ipuid").FirstOrDefault();
                var userFullName = ((JwtSecurityToken)jwttoken).Claims.Where(x => x.Type.ToLower() == "name").FirstOrDefault();
                if (((JwtSecurityToken)jwttoken).Claims.Where(x => x.Type.ToLower() == "idp").FirstOrDefault().Value == "local")
                {
                    winAccNameClaim = ((JwtSecurityToken)jwttoken).Claims.Where(x => x.Type.ToLower() == "email").FirstOrDefault();

                }

                HttpContext.Session.SetString(SynapseSession.IsPharamacist,
                    ((JwtSecurityToken)jwttoken).Claims.Any(x => string.Compare(x.Type, "synapseroles", true) == 0
                    && string.Compare(x.Value, "pharmacist", true) == 0).ToString());

                if (winAccNameClaim == null && string.IsNullOrWhiteSpace(winAccNameClaim.Value))
                {
                    //recordFailedLoginAttempt("-1", IPAddress, "Studio");
                    return RedirectToAction("Logout");
                }
                else
                {
                    HttpContext.Session.SetString(SynapseSession.UserID, winAccNameClaim.Value);

                }

                if (userFullName != null)
                    if (userFullName.Value.Split(' ').Length > 1)
                    {
                        string firstName = userFullName.Value.Replace(",", "").Split(' ')[1];
                        string lastName = userFullName.Value.Replace(",", "").Split(' ')[0];
                        HttpContext.Session.SetString(SynapseSession.FullName, firstName + " " + lastName);

                    }
                    else
                    { HttpContext.Session.SetString(SynapseSession.FullName, userFullName.Value); }
                else
                {
                    //recordFailedLoginAttempt("-1", IPAddress, "Studio");
                    return RedirectToAction("Logout");
                }

                return RedirectToAction("Index", "Home");
            }
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Logout");
        }
        [HttpPost]
        public ActionResult LoginUser(LoginModel model)
        {
            string sql = "SELECT * FROM systemsettings.app_user WHERE emailaddress = @email AND userpassword = crypt(@password, userpassword);";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("email", model.UserName),
                new KeyValuePair<string, string>("password", model.Password)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            List<LoginModel> lst = ds.Tables[0].ToList<LoginModel>();
            return RedirectToAction("Index");
        }
        public ActionResult Callback()
        {
            return View();
        }
        public ActionResult SilentRenew()
        {
            return View();
        }

        public async Task Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("cookie");
            await HttpContext.SignOutAsync("oidc");
            //return View();
        }

        public IActionResult Signout()
        {
            return View();
        }
    }

}