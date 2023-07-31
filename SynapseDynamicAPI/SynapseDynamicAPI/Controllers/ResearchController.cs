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


﻿using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SynapseDynamicAPI.Controllers
{
    [Authorize]
    [Route("ResearchAPI/")]
    public class ResearchController : ControllerBase
    {
        private static Random randomGenerator = new Random();

        private BaseViewController _baseViewController;
        private IConfiguration _configuration { get; }

        public ResearchController(BaseViewController baseViewController, IConfiguration configration)
        {
            _baseViewController = baseViewController;
            _configuration = configration;
        }

        [HttpGet] 
        [Route("")]
        [Route("[action]/{baseviewname?}/{orderby?}/{limit?}/{offset?}/{filter?}")]
        public string GetBaseViewList(string baseviewname, string orderby, string limit, string offset, string filter)
        {
            string returnData = _baseViewController.GetBaseViewList(baseviewname, orderby, limit, offset, filter);
            return AnonymiseBaseviewData(returnData);
        }

        [HttpGet]
        [Route("")]
        [Route("[action]/{baseviewname?}/{synapseattributename?}/{attributevalue?}/{orderby?}/{limit?}/{offset?}/{filter?}")]
        public string GetBaseViewListByAttribute(string baseviewname, string synapseattributename, string attributevalue, string orderby, string limit, string offset, string filter)
        {
            string returnData = _baseViewController.GetBaseViewListByAttribute(baseviewname, synapseattributename, attributevalue, orderby, limit, offset, filter);
            return AnonymiseBaseviewData(returnData);
        }

        [HttpGet]
        [Route("")]
        [Route("[action]/{baseviewname?}/{synapseattributename?}/{attributevalue?}/{filter?}")]
        public string GetBaseViewListObjectByAttribute(string baseviewname, string synapseattributename, string attributevalue, string filter)
        {
            string returnData = _baseViewController.GetBaseViewListObjectByAttribute(baseviewname, synapseattributename, attributevalue, filter);
            return AnonymiseBaseviewData(returnData);
        }

        [HttpPost]
        [Route("")]
        [Route("[action]/{baseviewname?}/{orderby?}/{limit?}/{offset?}/{filter?}")]
        public string GetBaseViewListByPost(string baseviewname, string orderby, string limit, string offset, string filter, [FromBody] string data)
        {
            string returnData = _baseViewController.GetBaseViewListByPost(baseviewname, orderby, limit, offset, filter, data);
            return AnonymiseBaseviewData(returnData);
        }

        private string AnonymiseBaseviewData(string baseviewData)
        {
            bool isPHIUser = IsPHIUser();

            List<Dictionary<string, object>> outputData = new List<Dictionary<string, object>>();

            dynamic data = JsonConvert.DeserializeObject(baseviewData);

            int yearSugar = randomGenerator.Next(1, 1000);

            foreach (var row in data)
            {
                Dictionary<string, object> newRow = new Dictionary<string, object>();

                foreach (var item in row)
                {
                    if (item.Name.ToLower().StartsWith("anondate_"))
                    {
                        DateTime date;
                        string sDate = item.Value;
                        DateTime.TryParseExact(sDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                        if (date == DateTime.MinValue)
                        {                            
                            throw new InterneuronBusinessException(errorCode: 400, errorMessage: "Invalid date in anonymise column.", "Client Error");
                        }
                        else
                        {
                            //DateTime tomorrow = DateTime.Now.Date.AddDays(1);

                            //int startYear = (tomorrow.Day == 1 && tomorrow.Month == 1) ? dob.Year : dob.Year - 1;

                            //DateTime start = new DateTime(startYear, tomorrow.Month, tomorrow.Day);
                            DateTime randomisedDate = date.AddYears(yearSugar); //start.AddDays(randomGenerator.Next(364));

                            string colName = item.Name.Replace("anondate_", "", StringComparison.CurrentCultureIgnoreCase);
                            if (isPHIUser)
                            {
                                newRow.Add(colName, date); // Original data if PHI user
                            }
                            else
                            {
                                newRow.Add(colName, randomisedDate); // Modified date if NON PHI User
                            }
                        }

                    }
                    else if (item.Name.ToLower().StartsWith("anonstring_"))
                    {
                        string value = item.Value;
                        string maskedValue = new String(value.Select(r => r == ' ' ? ' ' : 'X').ToArray());
                        string colName = item.Name.Replace("anonstring_", "", StringComparison.CurrentCultureIgnoreCase);
                        if (isPHIUser)
                        {
                            newRow.Add(colName, value); // Original data if PHI user
                        }
                        else
                        {
                            newRow.Add(colName, maskedValue); // Masked data if NON PHI User
                        }
                    }
                    else if (item.Name.ToLower().StartsWith("anonhash_"))
                    {
                        string value = item.Value;

                        byte[] salt = Convert.FromBase64String(_configuration["SynapseCore:Settings:ResearchAPIHashSalt"]);
                        Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

                        // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                        string hashedValue = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: value,
                            salt: salt,
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8));

                        string colName = item.Name.Replace("anonhash_", "", StringComparison.CurrentCultureIgnoreCase);
                        newRow.Add(colName, hashedValue);
                    }
                    else
                    {
                        newRow.Add(item.Name, item.Value);
                    }

                }

                outputData.Add(newRow);
            }

            return JsonConvert.SerializeObject(outputData);
        }


        private bool IsPHIUser()
        {
            bool isPHIUser = false;

            var userClaims = User.Claims.Where(x => x.Type == _configuration["SynapseCore:Settings:SynapseRolesClaimType"]);

            var hasPHIRole = userClaims.FirstOrDefault(role => role.Value == _configuration["SynapseCore:Settings:PHIUserRole"]);

            isPHIUser = hasPHIRole != null;

            return isPHIUser;
        }
    }
}
