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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.DataService.APIModel
{
    public class FormularyListAPIModel
    {
        public List<FormularyListAPIModel> Parents { get; set; }
        public List<FormularyListAPIModel> Children { get; set; }
        public string RowId { get; set; }
        public DateTimeOffset? Createdtimestamp { get; set; }
        public DateTimeOffset? Createddate { get; set; }
        public string Createdby { get; set; }
        public string FormularyId { get; set; }
        public long? VersionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public long? LogicalLevel { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public string ParentProductType { get; set; }
        public long? ParentProductLogicalLevel { get; set; }
        public bool? IsDuplicate { get; set; }
        public bool? IsLatest { get; set; }
        public string FormularyVersionId { get; set; }
        public string RecStatusCode { get; set; }
        public string RecSource { get; set; }
    }

    
}
