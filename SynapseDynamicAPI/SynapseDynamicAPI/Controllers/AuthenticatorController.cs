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
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SynapseDynamicAPI.Models;
using SynapseDynamicAPI.Services;
using System;
using System.Net;

namespace SynapseDynamicAPI.Controllers
{
    //[Authorize]
    [Route("Authenticator/")]
    public class AuthenticatorController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticatorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("[action]")]
        public void SaveSmartCardToken([FromBody] SmartCardUserModel smartCardUser)
        {
            System.IO.File.AppendAllLines(@".\logs\log.txt", new string[] { DateTime.Now.ToString() + ":" + JsonConvert.SerializeObject(smartCardUser) });

            AuthenticatorServices.SaveSmartCardToken(smartCardUser);

            Response.StatusCode = (int)HttpStatusCode.Created;
        }
        
        [HttpDelete]
        [Route("[action]/{userId?}")]
        public void RemoveSmartCardToken(string userId)
        {
            System.IO.File.AppendAllLines(@".\logs\log.txt", new string[] { DateTime.Now.ToString() + ":" + JsonConvert.SerializeObject(userId) });

            AuthenticatorServices.RemoveSmartCardToken(userId);

            Response.StatusCode = (int)HttpStatusCode.NoContent;
        }

        //[Authorize]
        [HttpGet]
        [Route("[action]/{userId?}")]
        public string GetSmartCardToken(string userId)
        {
            System.IO.File.AppendAllLines(@".\logs\log.txt", new string[] { DateTime.Now.ToString() + ":" + JsonConvert.SerializeObject(userId) });

            var useridClaim = User.FindFirst(_configuration["SynapseCore:Settings:TokenUserIdClaimType"]);

            if (useridClaim != null)
            {
                string[] loggenInUserId = useridClaim.Value.Split('\\', StringSplitOptions.RemoveEmptyEntries);

                if (loggenInUserId.Length == 2 && loggenInUserId[1].Equals(userId, StringComparison.OrdinalIgnoreCase))
                {
                    string token = AuthenticatorServices.GetSmartCardToken(userId);

                    if (string.IsNullOrEmpty(token))
                    {
                        Response.StatusCode = (int)HttpStatusCode.NoContent;
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
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return "Could not extract user from access token";
            }
        }
    }
}
