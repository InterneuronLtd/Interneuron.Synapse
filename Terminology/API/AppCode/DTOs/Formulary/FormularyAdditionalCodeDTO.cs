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
﻿using Interneuron.Terminology.API.AppCode.DTOs;
using System;
using System.Collections.Generic;

namespace Interneuron.Terminology.API.AppCode.DTOs
{
    public partial class FormularyAdditionalCodeDTO : TerminologyResource
    {
        public string RowId { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public string FormularyVersionId { get; set; }
        public string AdditionalCode { get; set; }
        public string AdditionalCodeSystem { get; set; }
        public string AdditionalCodeDesc { get; set; }
        public string Attr1 { get; set; }
        public string MetaJson { get; set; }
        public string Source { get; set; }
        public string CodeType { get; set; }
    }
}
