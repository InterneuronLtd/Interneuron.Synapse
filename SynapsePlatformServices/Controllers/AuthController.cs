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


﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Diagnostics;
using SynapsePlatformServices.Models;
using Microsoft.Extensions.Configuration;
using SynapsePlatformServices.Services;
using SynapsePlatformServices.Models.STS;

namespace SynapsePlatformServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISynapseIdentityServerClient _identityServerClient;
        private readonly IConfiguration configuration;
        string domain = string.Empty;
        bool testmode = false;

        public AuthController(IConfiguration iConfig, ISynapseIdentityServerClient client)
        {
            configuration = iConfig;
            _identityServerClient = client;
            domain = configuration.GetValue<string>("Settings:AuthController:LDAPDomainName");
            testmode = configuration.GetValue<bool>("Settings:AuthController:testmode");
        }

        [HttpGet]
        [Route("AuthenticateUser")]
        public ADUser AuthenticateUser(string userName, string password)
        {
            ADUser user = new ADUser();

            if (testmode == true)
            {
                if (userName == "test-fail")
                {
                    user = null;
                }
                else
                {
                    user.accountname = userName;
                    user.email = userName;
                    user.firstname = userName;
                    user.lastname = userName;
                    user.displayname = userName;
                    user.memberof = new List<string>();
                    user.memberof.Add("CN=" + userName + ",OU=Security Groups,OU=Groups,OU=Users & Groups,DC=rnoht,DC=local");

                }
            }
            else
            {
                try
                {

                    DirectoryEntry de = new DirectoryEntry("LDAP://" + domain, userName, password);
                    DirectorySearcher dsearch = new DirectorySearcher(de);
                    SearchResult results = null;

                    dsearch.PropertiesToLoad.Add("samaccountname");
                    dsearch.PropertiesToLoad.Add("userprincipalname");
                    dsearch.PropertiesToLoad.Add("givenname");
                    dsearch.PropertiesToLoad.Add("sn");
                    dsearch.PropertiesToLoad.Add("displayname");
                    dsearch.PropertiesToLoad.Add("name");
                    dsearch.PropertiesToLoad.Add("memberof");

                    dsearch.Filter = "(&(objectCategory=User)(objectClass=person)(samaccountname=" + userName + "))";

                    results = dsearch.FindOne();

                    user.accountname = (string)results.Properties["samaccountname"][0];
                    user.email = (string)results.Properties["userprincipalname"][0];
                    user.firstname = (string)results.Properties["givenname"][0];
                    user.lastname = (string)results.Properties["sn"][0];
                    user.displayname = (string)results.Properties["displayname"][0];
                    user.memberof = new List<string>();
                    foreach (var group in results.Properties["memberof"])
                    {
                        user.memberof.Add(group.ToString());
                    }
                }
                catch (Exception e)
                {
                    user = null;
                }

            }
            return user;
        }

        [HttpPost]
        [Route("GetAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IIdentityServerResponse GetAccessToken([FromBody] TokenParameter parameters)
        {
            var identityServerResponse = _identityServerClient.GetAccessToken(parameters).Result;
            if (identityServerResponse.GetType() == typeof(IdentityServerError))
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            return identityServerResponse;
        }


        //private void GetAllGroups()
        //{
        //    SearchResultCollection results;
        //    DirectorySearcher ds = null;
        //    DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());

        //    ds = new DirectorySearcher(de);
        //    // Sort by name
        //    ds.Sort = new SortOption("name", SortDirection.Ascending);
        //    ds.PropertiesToLoad.Add("name");
        //    ds.PropertiesToLoad.Add("memberof");
        //    ds.PropertiesToLoad.Add("member");

        //    ds.Filter = "(&(objectCategory=Group))";

        //    results = ds.FindAll();

        //    foreach (SearchResult sr in results)
        //    {
        //        if (sr.Properties["name"].Count > 0)
        //            Debug.WriteLine(sr.Properties["name"][0].ToString());

        //        if (sr.Properties["memberof"].Count > 0)
        //        {
        //            Debug.WriteLine("  Member of...");
        //            foreach (string item in sr.Properties["memberof"])
        //            {
        //                Debug.WriteLine("    " + item);
        //            }
        //        }
        //        if (sr.Properties["member"].Count > 0)
        //        {
        //            Debug.WriteLine("  Members");
        //            foreach (string item in sr.Properties["member"])
        //            {
        //                Debug.WriteLine("    " + item);
        //            }
        //        }
        //    }
        //}

    }
}
