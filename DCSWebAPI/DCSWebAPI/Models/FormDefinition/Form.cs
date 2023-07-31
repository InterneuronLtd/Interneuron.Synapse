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
using System.Linq;

namespace DCSWebAPI.Models.FormDefinition
{    
    public class Form
    {
        [JsonProperty("forminstance_id")]
        public string FormInstanceId { get; internal set; }

        [JsonProperty("form_id")]
        public string FormId { get; set; }

        [JsonProperty("formname")]
        public string FormName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("formparameters")]
        public List<FormParameter> FormParameters { get; set; }

        [JsonProperty("destinationentities")]
        public List<DestinationEntity> DestinationEntities { get; set; }

        [JsonProperty("formsections")]
        public List<FormSection> Sections { get; set; }

        internal void InitializeSavedFormResponse(List<FormResponse> formResponse)
        {
            if (formResponse.Count > 0)
            {
                foreach (var section in Sections)
                {
                    foreach (var field in section.Fields)
                    {
                        var savedResponse = formResponse.Where(fr => fr.FieldId == field.FieldId);                        
                        if (savedResponse != null)
                        {                            
                            field.FieldValue = new List<string>();
                            foreach (var response in savedResponse)
                            {
                                field.FieldValue.Add(response.ResponseValue);
                            }
                        }
                    }
                }
            }
        }
    }
}