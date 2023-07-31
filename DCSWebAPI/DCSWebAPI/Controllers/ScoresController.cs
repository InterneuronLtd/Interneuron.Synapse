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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DCSWebAPI.Models;
using DCSWebAPI.Models.FormDefinition;
using DCSWebAPI.Services;
using Newtonsoft.Json;
using DCSWebAPI.Services.SynapseDynamicAPI;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DCSWebAPI.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class ScoresController : Controller
    {
        private readonly FormInstanceHelper _formInstanceHelper;
        private readonly FormResponseHelper _formResponseHelper;

        public ScoresController(FormInstanceHelper client, FormResponseHelper formResponseHelper)
        {
            _formInstanceHelper = client;
            _formResponseHelper = formResponseHelper;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("{formInstanceId}")]
        public string CalculateASAScore(string formInstanceId)
        {
            //get all the form responses for this form instance          
            List<FormResponse> responses = _formResponseHelper.GetFormResponse(formInstanceId);

            return FormScoreHelper.CalculateASAScore(responses);    
        }
    }
}
