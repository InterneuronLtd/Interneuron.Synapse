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
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.DTO.FluidBalance
{
    public class FluidBalanceSessionDTO
    {
        [JsonProperty("fluidbalancesession_id")]
        public string FluidbalancesessionId { get; set; }

        [JsonProperty("startdate")]
        public string Startdate { get; set; }

        [JsonProperty("stopdate")]
        public string Stopdate { get; set; }

        [JsonProperty("person_id")]
        public string PersonId { get; set; }

        [JsonProperty("encounter_id")]
        public string EncounterId { get; set; }

        [JsonProperty("addedby")]
        public string Addedby { get; set; }

        [JsonProperty("modifiedby")]
        public string Modifiedby { get; set; }
        
        [JsonProperty("initialexpectedurineoutput")]
        public float InitialExpectedUrineOutput { get; set; }
        
        [JsonProperty("initialweight")]
        public float InitialWeight { get; set; }

        [JsonProperty("initialage")]
        public float InitialAge { get; set; }
    }
}
