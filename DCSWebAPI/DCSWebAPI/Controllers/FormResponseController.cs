//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

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
using System.Linq;
using DCSWebAPI.Models;
using DCSWebAPI.Models.FormDefinition;
using DCSWebAPI.Models.WebAPI.SynapseDynamicAPI;
using DCSWebAPI.Services;
using DCSWebAPI.Services.SynapseDynamicAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DCSWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FormResponseController : ControllerBase
    {
        private readonly DataService _dataService = new DataService();

        private readonly SynapseDynamicAPIClient _dynamicAPIClient;
        private readonly FormInstanceHelper _formInstanceHelper;
        private readonly FormResponseHelper _formResponseHelper;

        public FormResponseController(SynapseDynamicAPIClient client, FormInstanceHelper formInstanceHelper, FormResponseHelper formResponseHelper)
        {
            _dynamicAPIClient = client;
            _formInstanceHelper = formInstanceHelper;
            _formResponseHelper = formResponseHelper;
        }

        [HttpPost]
        public List<FormResponse> Post([FromBody] Form formData)
        {
            InitializeDestination(ref formData);

            if (formData != null)
            {
                List<FormResponse> formResponses = new List<FormResponse>();
                string personId = formData.FormParameters.Where(item => item.ParameterName.Equals("Person_Id")).FirstOrDefault().ParameterValue;
                string encounterId = formData.FormParameters.Where(item => item.ParameterName.Equals("Encounter_Id")).FirstOrDefault().ParameterValue;
                string formContext = formData.FormParameters.Where(item => item.ParameterName.Equals("Form_Context")).FirstOrDefault().ParameterKey;
                string formContextId = formData.FormParameters.Where(item => item.ParameterName.Equals("Form_Context")).FirstOrDefault().ParameterValue;

                FormInstance formInstance = _formInstanceHelper.GetFormInstance(formData.FormInstanceId);

                FormScore savedFormScore = _formInstanceHelper.GetFormScore(formInstance.FormInstanceId);

                foreach (var section in formData.Sections)
                {
                    foreach (var field in section.Fields)
                    {
                        foreach (var fieldData in field.FieldData)
                        {
                            var formResponse = new FormResponse
                            {
                                FormResponseId = Guid.NewGuid().ToString(),
                                FormInstanceId = formInstance.FormInstanceId,
                                FormId = formData.FormId,
                                SectionId = section.SectionId,
                                FieldId = field.FieldId,
                                FieldType = field.FieldControlTypeName,
                                FieldQuestion = field.FieldLabelText,
                                ResponseText = fieldData.Text,
                                ResponseValue = fieldData.Value
                            };

                            formResponses.Add(formResponse);
                        }
                    }
                }

                _dynamicAPIClient.DeleteObjectByAttribute("dcs", "formresponse", "forminstance_id", formInstance.FormInstanceId);

                _dynamicAPIClient.PostObjectArray("dcs", "formresponse", formResponses);

                foreach (var destination in formData.DestinationEntities)
                {
                    string value = _dynamicAPIClient.ExecuteNonSQL(new SQLData()
                    {
                        Parameters = destination.SQLParameters,
                        ReturnColumn = "_sequenceid",
                        Query = destination.SQL
                    });
                }

                /* 
                 * Below call need to be replaced by saving form scores based on the scoring model present in JSON. Implemented this way for Pre-op form delivery
                 */
                string score = FormScoreHelper.CalculateASAScore(formResponses);
                dynamic asaScore = JsonConvert.DeserializeObject(score);

                FormScore formScore = new FormScore();

                if (savedFormScore == null)
                {
                    formScore.FormScoreId = Guid.NewGuid().ToString();
                }
                else
                {
                    formScore.FormScoreId = savedFormScore.FormScoreId;
                }
                formScore.FormInstanceId = formInstance.FormInstanceId;
                formScore.FormInstanceScore = asaScore.result.asascore;
                formScore.ReviewType = asaScore.result.reviewtype;
                formScore.ScoreDescription = asaScore.result.desc;
                formScore.ScoreId = "ASAScore";

                _dynamicAPIClient.PostObject("dcs", "formscore", formScore);

                /*
                 * End of comments
                 */

                formInstance.Status = "Completed";
                _dynamicAPIClient.PostObject("dcs", "forminstance", formInstance);

                return formResponses;
            }
            else
                return null;
        }

        private void InitializeDestination(ref Form formData)
        {
            foreach (var destination in formData.DestinationEntities)
            {
                destination.ParentForm = formData;
            }
        }
    }
}