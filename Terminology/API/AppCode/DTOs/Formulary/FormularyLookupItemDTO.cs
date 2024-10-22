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

namespace Interneuron.Terminology.API.AppCode.DTOs
{
    public class FormularyLookupItemDTO : TerminologyResource, ILookupItemDTO
    {
        public string Cd { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }
        public bool? IsDefault { get; set; }
        public short? Recordstatus { get; set; }
        public string Source { get; set; }
        public string AdditionalProperties { get; set; }
    }

    public class FormularyLookupDTO : TerminologyResource
    {
        public List<FormularyLookupItemDTO> Items { get; set; }
    }
}
