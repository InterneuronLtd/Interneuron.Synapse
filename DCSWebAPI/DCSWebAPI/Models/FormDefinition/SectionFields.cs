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


﻿using DCSWebAPI.Models.FormDefinition.Enumerations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DCSWebAPI.Models.FormDefinition
{
    public class SectionFields
    {
        [JsonProperty("field_id")]
        public string FieldId { get; set; }

        [JsonProperty("fieldcontroltypename")]
        public string FieldControlTypeName { get; set; }

        [JsonProperty("fieldlabeltext")]
        public string FieldLabelText { get; set; }

        [JsonProperty("fieldvalue")]
        public List<string> FieldValue { get; set; }

        [JsonProperty("defaultvalue")]
        public string DefaultValue { get; set; }
        
        [JsonProperty("fielddisplayorder")]
        public int? FieldDisplayOrder { get; set; }

        [JsonProperty("fieldvalidations")]
        public List<FieldValidations> FieldValidations { get; set; }

        [JsonProperty("optionlisttype")]
        public string OptionListType { get; set; }

        [JsonProperty("optionliststatement")]
        public string OptionListStatement { get; set; }

        [JsonProperty("optionlist")]
        public List<Option> OptionList { get; set; }

        [JsonProperty("fielddata")]
        public List<FieldValue> FieldData { get; set; }

        [JsonProperty("displayrules")]
        public DisplayRules Displayrules { get; set; }
    }
}