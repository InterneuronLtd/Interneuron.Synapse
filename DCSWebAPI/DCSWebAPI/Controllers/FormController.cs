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
using System.Net.Http;
using DCSWebAPI.Models;
using DCSWebAPI.Models.FormDefinition;
using DCSWebAPI.Models.FormDefinition.Enumerations;
using DCSWebAPI.Models.WebAPI.SynapseDynamicAPI;
using DCSWebAPI.Services;
using DCSWebAPI.Services.SynapseDynamicAPI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DCSWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FormController : ControllerBase
    {
        private readonly DataService _dataService = new DataService();

        private readonly FormInstanceHelper _formInstanceHelper;
        private readonly FormResponseHelper _formResponseHelper;

        public FormController(FormInstanceHelper formInstanceHelper, FormResponseHelper formResponseHelper)
        {
            _formInstanceHelper = formInstanceHelper;
            _formResponseHelper = formResponseHelper;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<Form[]> Get()
        {
            return new Form[] { new Form(), new Form()};
        }

        // GET api/values/5
        [HttpGet("{id}/{personId}/{encounterId?}/{formContext?}/{formContextId?}")]
        public ActionResult<string> Get(string id, string personId, string encounterId, string formContext, string formContextId)
        {
            FormInstance formInstance = _formInstanceHelper.GetFormInstance(id, personId, encounterId, formContext, formContextId);

            string formJson = System.IO.File.ReadAllText(string.Format(@".\JSONStore\form{0}.json", formInstance.FormId));

            List<FormParameter> formParams = new List<FormParameter>();
            formParams.Add(new FormParameter
            {
                ParameterName = "Person_Id",
                ParameterKey = "personId",
                ParameterValue = formInstance.PersonId
            });
            formParams.Add(new FormParameter
            {
                ParameterName = "Encounter_Id",
                ParameterKey = "encounterId",
                ParameterValue = formInstance.EncounterId
            });
            formParams.Add(new FormParameter
            {
                ParameterName = "Form_Context",
                ParameterKey = formInstance.FormContext,
                ParameterValue = formInstance.FormContextId
            });

            Form form = JsonConvert.DeserializeObject<Form>(formJson);
            form.FormParameters = formParams;
            form.FormId = formInstance.FormId;
            form.FormInstanceId = formInstance.FormInstanceId;
            InitializeDynamicFields(form);
            form.InitializeSavedFormResponse(_formResponseHelper.GetFormResponse(formInstance.FormInstanceId));

            return "{ \"form\": " + JsonConvert.SerializeObject(form, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) + " } ";
        }

        private void InitializeDynamicFields(Form form)
        {
            foreach (var section in form.Sections)
            {
                foreach (var field in section.Fields)
                {
                    if (!string.IsNullOrWhiteSpace(field.OptionListType) && field.OptionListType.Equals(OptionType.SQL))
                    {
                        string sql = field.OptionListStatement;
                        string optionList = _formInstanceHelper.ExecuteSQL(new SQLData
                        {
                            Parameters = null,
                            Query = sql,
                            ReturnColumn = "_sequenceid"
                        });

                        if (!string.IsNullOrWhiteSpace(optionList))
                        {
                            field.OptionList = JsonConvert.DeserializeObject<List<Option>>(optionList);
                        }
                        else
                        {
                            field.OptionList = new List<Option>();
                        }

                        field.OptionListType = null;
                        field.OptionListStatement = null;
                    }
                }
            }
        }

        // POST api/forms
        [HttpPost]
        public void Post([FromBody] Form formData)
        {
            throw new NotImplementedException();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }
    }
}