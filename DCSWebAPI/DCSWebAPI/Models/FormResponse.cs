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


﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCSWebAPI.Models
{
    public class FormResponse
    {
        [JsonProperty("formresponse_id")]
        public string FormResponseId { get; set; }

        [JsonProperty("forminstance_id")]
        public string FormInstanceId { get; set; }

        [JsonProperty("form_id")]
        public string FormId { get; set; }

        [JsonProperty("sectionid")]
        public string SectionId { get; set; }

        [JsonProperty("fieldid")]
        public string FieldId { get; set; }

        [JsonProperty("fieldtype")]
        public string FieldType { get; set; }

        [JsonProperty("fieldquestion")]
        public string FieldQuestion { get; set; }

        [JsonProperty("responsetext")]
        public string ResponseText { get; set; }

        [JsonProperty("responsevalue")]
        public string ResponseValue { get; set; }
    }
}