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


﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SynapseDynamicAPI.Models;
using SynapseDynamicAPI.Services;
using System.Net;

namespace SynapseDynamicAPI.Controllers
{
    [Authorize]
    [Route("Authenticator/")]
    public class AuthenticatorController : ControllerBase
    {
        private IConfiguration _configuration { get; }

        [HttpPost]
        [Route("[action]")]
        public void SaveSmartCardToken([FromForm] SmartCardUserModel smartCardUser)
        {
            AuthenticatorServices.SaveSmartCardToken(smartCardUser);

            Response.StatusCode = (int)HttpStatusCode.Created;
        }
        
        [HttpDelete]
        [Route("[action]/{userId?}")]
        public void RemoveSmartCardToken(string userId)
        {
            AuthenticatorServices.RemoveSmartCardToken(userId);

            Response.StatusCode = (int)HttpStatusCode.NoContent;
        }

        [HttpGet]
        [Route("[action]/{userId?}")]
        public string GetSmartCardToken(string userId)
        {
            var useridClaim = User.FindFirst(_configuration["SynapseCore:Settings:TokenUserIdClaimType"]);

            if (useridClaim != null && useridClaim.Value == userId)
            {
                string token = AuthenticatorServices.GetSmartCardToken(userId);

                if (string.IsNullOrEmpty(token))
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                }

                return token;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return "Not authorised to access token for the requested User";
            }
        }
    }
}
