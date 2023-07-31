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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Security.Claims;

namespace InterneuronFDBAPI.Infrastructure
{
    public class FDBAuthorization : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //Taking the parameter from the header  
            string authToken = actionContext.Request.Headers.Authorization.Parameter?? actionContext.Request.Headers.Authorization.Scheme;
           
            //decode the parameter  
            var jwttoken = DecodeJWTToken(authToken);

            if(jwttoken == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }

            if (((JwtSecurityToken)jwttoken).Claims.Count() > 1)
            {
                List<Claim> userScopes = ((JwtSecurityToken)jwttoken).Claims.Where(x => x.Type.ToLower() == "scope").ToList();

                if (!(userScopes.Where(s => s.Value.CompareTo("dynamicapi.read") == 0)).Any() &&
                    !(userScopes.Where(s => s.Value.CompareTo("dynamicapi.write") == 0)).Any() &&
                    !(userScopes.Where(s => s.Value.CompareTo("terminologyapi.read") == 0)).Any() &&
                    !(userScopes.Where(s => s.Value.CompareTo("terminologyapi.write") == 0)).Any())
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return;
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }
            base.OnAuthorization(actionContext);
        }

        private object DecodeJWTToken(string tokenString)
        {
            if (tokenString == null || tokenString.Trim() == string.Empty) return null;

            JwtSecurityTokenHandler j = new JwtSecurityTokenHandler();
            JwtSecurityToken jwttoken;
            try
            {
                jwttoken = j.ReadJwtToken(tokenString);
            }
            catch
            {
                return null;
            }

            return jwttoken;
        }
    }
}