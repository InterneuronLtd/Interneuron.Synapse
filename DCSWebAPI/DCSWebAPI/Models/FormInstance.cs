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


﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DCSWebAPI.Models
{
    public class FormInstance
    {
        [JsonProperty("forminstance_id")]
        public string FormInstanceId { get; set; }

        [JsonProperty("person_id")]
        public string PersonId { get; set; }

        [JsonProperty("encounter_id", NullValueHandling = NullValueHandling.Ignore)]
        public string EncounterId { get; set; }

        [JsonProperty("formcontext", NullValueHandling = NullValueHandling.Ignore)]
        public string FormContext { get; set; }

        [JsonProperty("formcontextid", NullValueHandling = NullValueHandling.Ignore)]
        public string FormContextId { get; set; }

        [JsonProperty("form_id")]
        public string FormId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("lastupdatedatetime")]
        public DateTime? LastUpdateDateTime { get; set; }

        [JsonProperty("lastupdatedatetimets")]
        public DateTimeOffset? LastUpdateDateTimeTs { get; set; }

        [JsonProperty("lastupdatedby")]
        public string LastUpdatedBy { get; set; }

        internal List<FormResponse> FormInstanceResponse { get; set; }
    }
}