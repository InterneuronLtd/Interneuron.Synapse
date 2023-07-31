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
using System.Collections.Generic;

namespace SynapseDynamicAPI.Models.Meta
{
    public class Persona
    {
        [JsonProperty("persona_id")]
        public string PersonaId { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("personaName")]
        public string PersonaName { get; set; }

        [JsonProperty("displayOrder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("personaContext")]
        public List<PersonaContext> PersonaContexts { get; set; }
    }
}