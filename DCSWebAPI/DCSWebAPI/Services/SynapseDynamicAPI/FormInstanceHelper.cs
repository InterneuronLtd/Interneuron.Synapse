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
﻿using DCSWebAPI.Models;
using DCSWebAPI.Models.FormDefinition;
using DCSWebAPI.Models.WebAPI.SynapseDynamicAPI;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace DCSWebAPI.Services.SynapseDynamicAPI
{
    public class FormInstanceHelper : SynapseDynamicAPIClient
    {
        public string AccessToken { get; set; }

        public FormInstanceHelper(HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
        {
            
        }

        public FormInstance GetFormInstance(string formInstanceId)
        {
            string data = GetListByAttribute("dcs", "forminstance", "forminstance_id", formInstanceId, false);

            return (JsonConvert.DeserializeObject<FormInstance[]>(data))[0];
        }

        public FormInstance GetFormInstance(string formId, string personId, string encounterId, string formContext, string formContextId)
        {
            FormInstance[] instance = new FormInstance[1];

            string query = "SELECT forminstance_id, form_id, person_id, encounter_id, formcontext, formcontextid, status, lastupdatedby, lastupdatedatetime, lastupdatedatetimets " +
                           "from entitystorematerialised.dcs_forminstance " +
                           "WHERE form_id = @FormId AND " +
                           "person_id = @PersonId AND " +
                           (!string.IsNullOrWhiteSpace(encounterId) ? "encounter_id = @EncounterId AND " : "encounter_id is null AND ") +
                           (!string.IsNullOrWhiteSpace(formContext) ? "formcontext = @FormContext AND " : "formcontext is null AND ") +
                           (!string.IsNullOrWhiteSpace(formContextId) ? "formcontextid = @FormContextId " : "formcontextid is null ");

            string queryResult = null;

            List<SQLParameter> paramList = new List<SQLParameter>();

            paramList.Add(new SQLParameter()
            {
                Name = "@FormId",
                Value = formId
            });
            paramList.Add(new SQLParameter()
            {
                Name = "@PersonId",
                Value = personId
            });

            if (!string.IsNullOrEmpty(encounterId))
            {
                paramList.Add(new SQLParameter()
                {
                    Name = "@EncounterId",
                    Value = encounterId
                });
            }
            if (!string.IsNullOrEmpty(formContext))
            {
                paramList.Add(new SQLParameter()
                {
                    Name = "@FormContext",
                    Value = formContext
                });
            }
            if (!string.IsNullOrEmpty(formContextId))
            {
                paramList.Add(new SQLParameter()
                {
                    Name = "@FormContextId",
                    Value = formContextId
                });
            }

            queryResult = ExecuteSQL(new SQLData()
            {
                Parameters = paramList,
                Query = query
            });

            if (string.IsNullOrEmpty(queryResult) || queryResult == "[]")
            {
                instance[0] = CreateFormInstance(formId, personId, encounterId, formContext, formContextId);
            }
            else
            {
                instance = JsonConvert.DeserializeObject<FormInstance[]>(queryResult);
            }

            return instance[0];
        }

        internal FormScore GetFormScore(string formInstanceId)
        {
            string returnValue = GetListByAttribute("dcs", "formscore", "forminstance_id", formInstanceId, false);

            List<FormScore> formScores = JsonConvert.DeserializeObject<List<FormScore>>(returnValue);

            return formScores.Count == 0 ? null : formScores[0];
        }

        public FormInstance CreateFormInstance(string formId, string personId, string encounterId, string formContext, string formContextId)
        {
            FormInstance formInstance = new FormInstance()
            {
                FormInstanceId = Guid.NewGuid().ToString(),
                FormId = formId,
                PersonId = personId,
                EncounterId = encounterId,
                FormContext = formContext,
                FormContextId = formContextId,
                LastUpdateDateTime = DateTime.Now,
                Status = "InProgress"
            };

            string formInstanceJSON = PostObject("dcs", "forminstance", formInstance);

            return formInstance;
        }
    }
}
