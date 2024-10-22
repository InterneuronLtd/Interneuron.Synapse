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
﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace DCSWebAPI.Models.FormDefinition
{
    public class FormSection
    {
        [JsonProperty("formsection_id")]
        public string SectionId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("banner")]
        public string Banner { get; set; }

        [JsonProperty("desciption")]
        public string Desciption { get; set; }

        [JsonProperty("displayorder")]
        public int? DisplayOrder { get; set; }

        [JsonProperty("sectionfields")]
        public List<SectionFields> Fields { get; set; }
    }
}