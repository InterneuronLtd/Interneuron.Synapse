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
﻿using Interneuron.Common.Extensions;
using Interneuron.Terminology.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Core
{
    public class APIRequestContextProvider
    {
        private IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;

        public APIRequestContextProvider(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            //APIRequestContext.APIRequestContextProvider = CreateAPIContext;
        }

        public APIRequestContext CreateAPIContext()
        {
            if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null) return new APIRequestContext();

            var configSection = _configuration.GetSection("TerminologyConfig");
            var userIdConfig  = configSection.GetValue<string>("TokenUserIdClaimType");
            var userRolesConfig = configSection.GetValue<string>("TokenUserRolesClaimType");
            var userScopesConfig = configSection.GetValue<string>("TokenUserScopesClaimType");

            var requestContext = new APIRequestContext();

            var terminologyAPIUser = new TerminologyAPIUser
            {
                UserId = "UNKNOWN"
            };
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(userIdConfig);
            var clientIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("client_id");
            var authToken =  _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (userIdClaim != null && userIdClaim.Value.IsNotEmpty())
            {
                terminologyAPIUser.UserId = userIdClaim.Value;
            }
            if (clientIdClaim != null && clientIdClaim.Value.IsNotEmpty())
            {
                requestContext.ClientId = clientIdClaim.Value;
            }
            if (authToken.IsNotEmpty())
            {
                requestContext.AuthToken = authToken.Replace("Bearer ","");
            }
            var roleClaims = _httpContextAccessor.HttpContext.User.FindAll(userRolesConfig);
            var scopeClaims = _httpContextAccessor.HttpContext.User.FindAll(userScopesConfig);

            if (roleClaims.IsCollectionValid())
            {
                terminologyAPIUser.UserRoles = roleClaims.Select(r => r.Value).Distinct().ToHashSet<string>();
            }

            if (scopeClaims.IsCollectionValid())
            {
                terminologyAPIUser.UserScopes = scopeClaims.Select(r => r.Value).Distinct().ToHashSet();
            }

            requestContext.TerminologyAPIUser = terminologyAPIUser;

            return requestContext;
        }

        /*
         #region set_createdby         
            var useridClaim = User.FindFirst(_configuration["SynapseCore:Settings:TokenUserIdClaimType"]);
            var userid = "unknown";

            if (useridClaim != null)
            {
                userid = useridClaim.Value;
            }
            else if (dataDict.ContainsKey("_createdby"))
            {
                userid = Convert.ToString(dataDict["_createdby"]);
            }

            //check if dataDict has _createdby key
            if (dataDict.ContainsKey("_createdby"))
            {
                //update they key value to userid from toke               
                dataDict["_createdby"] = userid;
            }
            else
            {
                //add _createdby key and set value to userid from token 
                dataDict.Add("_createdby", userid);
            }
            #endregion
         */
    }

}
