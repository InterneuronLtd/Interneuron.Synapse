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


﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseDynamicAPI.Models.Meta
{
    public class MetaPersonaBaseview
    {
        public string persona_id { get; set; }
        public string personadispname { get; set; }
        public string personaname { get; set; }
        public int personadisporder { get; set; }
        public string personacontext_id { get; set; }
        public string displayname { get; set; }
        public string contextname { get; set; }
        public int displayorder { get; set; }
        public string useridentifier { get; set; }
    }
}
