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
﻿

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseDynamicAPI.Models.Meta
{
    public class PersonaContext
    {
        [JsonProperty("personacontext_id")]
        public string PersonaContextId { get; set; }

        [JsonProperty("persona_id")]
        public string PersonaId { get; set; }

        [JsonProperty("displayname")]
        public string DisplayName { get; set; }

        [JsonProperty("contextname")]
        public string ContextName { get; set; }

        [JsonProperty("displayorder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("mappingentityname")]
        public string MappingEntityName { get; set; }

        [JsonProperty("mappingentitycolumn")]
        public string MappingEntityColumn { get; set; }

        [JsonProperty("mappingentitycontext")]
        public string MappingEntityContext { get; set; }

        [JsonProperty("useridentifier")]
        public string UserIdentifier { get; set; }
    }
}
