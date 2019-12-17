//Interneuron Synapse
//Copyright(C) 2019  Interneuron CIC

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

using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Synapse.STS.Data;
using Synapse.STS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Synapse.STS
{
    public class CustomClaims : IProfileService
    {
        private readonly UserManager<SynapseUser> _userManager;
        private readonly SynapseRoleBasedClaimsContext _claimsContext;
        private readonly IConfiguration _configuration;

        public CustomClaims(UserManager<SynapseUser> userManager, SynapseRoleBasedClaimsContext claimsContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _claimsContext = claimsContext;

        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context.Caller == "ClaimsProviderAccessToken")
            {
                //current Subject   
                var sub = context.Subject.FindFirst("sub")?.Value;
                if (sub != null)
                {
                    //Add any claims from external identiy provider as set in the external login callback
                    context.IssuedClaims.AddRange(context.Subject.Claims.Where(x => context.RequestedClaimTypes.Contains<string>(x.Type)));
                    // context.IssuedClaims.AddRange(context.Subject.Claims);

                    AddRequetedClaimsFromDB(context);


                    //force add winAccName from ADFS subject claim 
                    if (context.Subject.HasClaim("Idp", _configuration["Settings:ADFSSchemeName"]))
                    {
                        var winAccName = context.Subject.FindFirst(_configuration["Settings:WindowsAccountNameClaimType"]).Value;
                        context.IssuedClaims.Add(new Claim("IPUId", winAccName));
                    }
                    else if (context.Subject.HasClaim("Idp", _configuration["Settings:AADSchemeName"]))
                    {
                        var winAccName = context.Subject.FindFirst(_configuration["Settings:AADIPUIdClaimType"]).Value;
                        context.IssuedClaims.Add(new Claim("IPUId", winAccName));
                    }
                    else if (context.Subject.FindFirst("email") != null)
                    {
                        var email = context.Subject.FindFirst("email").Value;
                        context.IssuedClaims.Add(new Claim("IPUId", email));
                    }
                    else context.IssuedClaims.Add(new Claim("IPUId", sub));

                }
            }
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }

        public void AddRequetedClaimsFromDB(ProfileDataRequestContext context)
        {

            //Issue Synapse Role Claim


            //for local logins, select user roles mapping from aspnetroles 
            var roles = _userManager.GetRolesAsync(_userManager.FindByIdAsync(context.Subject.FindFirst("sub")?.Value).Result);

            if (roles != null)
            {
                foreach (var item in roles.Result)
                {
                    context.IssuedClaims.Add(new Claim(_configuration["Settings:SynapseRolesClaimType"], item));
                }
            }
            

            //if idp = ADFS
            //access winAccName from ADFS subject claim then map the value to external providers user roles table to get role names
            //for each /synapse/adgroups claim map the value to external providers user roles table to get role names
            if (context.Subject.HasClaim("Idp", _configuration["Settings:ADFSSchemeName"]))
            {
                var winAccName = context.Subject.FindFirst(_configuration["Settings:WindowsAccountNameClaimType"]).Value;

                List<string> adgroups = context.Subject.FindAll(_configuration["Settings:ADGroupsClaimType"]).Select(x => x.Value).ToList();


                List<string> rolesExternalIdp = _claimsContext.AspNetRoles.
                                                Where(x => (_claimsContext.UserRolesExternalProviders.
                                                    Where(u => u.Idp == _configuration["Settings:ADFSSchemeName"] && (u.ExternalSubjectId.ToLower() == winAccName.ToLower() || adgroups.Contains(u.ExternalSubjectId))).
                                                        Select(r => r.RoleId)).Contains(x.Id)).Select(rn => rn.Name).ToList<string>();

                if (rolesExternalIdp != null)
                {
                    foreach (var item in rolesExternalIdp)
                    {
                        context.IssuedClaims.Add(new Claim(_configuration["Settings:SynapseRolesClaimType"], item));
                    }
                }


            }
        }


    }
}