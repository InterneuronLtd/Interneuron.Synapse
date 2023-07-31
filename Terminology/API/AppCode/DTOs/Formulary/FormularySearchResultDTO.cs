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
﻿using System;
using System.Collections.Generic;

namespace Interneuron.Terminology.API.AppCode.DTOs.Formulary
{
    public class FormularySearchResultDTO
    {
        public string RowId { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public string FormularyId { get; set; }
        public int? VersionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public int? LogicalLevel { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public string ParentProductType { get; set; }
        public int? ParentProductLogicalLevel { get; set; }
        public bool? IsDuplicate { get; set; }
        public bool? IsLatest { get; set; }
        public string FormularyVersionId { get; set; }
        public string RecStatusCode { get; internal set; }
        public string RecSource { get; internal set; }
        public string RnohFormularyStatuscd { get; set; }
        public bool Prescribable { get; set; }
    }

    public class FormularySearchResultsWithHierarchyDTO : TerminologyResource
    {
        public FormularySearchFilterRequest FilterCriteria { get; set; }

        public List<FormularySearchResultWithTreeDTO> Data { get; set; }
    }

    public class FormularySearchResultWithTreeDTO : FormularySearchResultDTO
    {
        public List<FormularySearchResultWithTreeDTO> Parents { get; set; }

        public List<FormularySearchResultWithTreeDTO> Children { get; set; }
       
    }
}
