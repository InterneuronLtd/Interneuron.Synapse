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


﻿using Interneuron.Terminology.API.AppCode.Validators;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Generic;

namespace Interneuron.Terminology.API.AppCode.DTOs
{
    public partial class FormularyDTO : TerminologyResource
    {
        //public string RowId { get; set; }
        //public DateTime? Createddate { get; set; }
        //public string Createdby { get; set; }
        //public string Tenant { get; set; }
        //public string FormularyId { get; set; }
        //public int? VersionId { get; set; }
        //public string FormularyVersionId { get; set; }
        //public string Code { get; set; }
        //public string Name { get; set; }
        //public string ProductType { get; set; }
        //public string ParentCode { get; set; }
        //public string ParentName { get; set; }
        //public string ParentProductType { get; set; }
        //public int? RecStatusCode { get; set; }
        //public DateTime? RecStatuschangeDate { get; set; }
        //public bool? IsDuplicate { get; set; }
        //public bool? IsLatest { get; set; }
        //public int? RecSource { get; set; }
        //public string VtmId { get; set; }
        //public string VmpId { get; set; }

        public string RowId { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Updatedby { get; set; }
        public string FormularyId { get; set; }
        public int? VersionId { get; set; }
        public string FormularyVersionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public string ParentProductType { get; set; }
        public string RecStatusCode { get; set; }
        public DateTime? RecStatuschangeTs { get; set; }
        public DateTime? RecStatuschangeDate { get; set; }
        public string RecStatuschangeTzname { get; set; }
        public int? RecStatuschangeTzoffset { get; set; }
        public bool? IsDuplicate { get; set; }
        public string RecStatuschangeMsg { get; set; }
        public string DuplicateOfFormularyId { get; set; }

        public bool? IsLatest { get; set; }
        public string RecSource { get; set; }
        public string VtmId { get; set; }
        public string VmpId { get; set; }
        public string CodeSystem { get; set; }

        public List<FormularyAdditionalCodeDTO> FormularyAdditionalCodes { get; set; }
        public FormularyDetailDTO Detail { get; set; }
        //public List<FormularyIndicationDTO> FormularyIndications { get; set; }
        public List<FormularyIngredientDTO> FormularyIngredients { get; set; }
        public List<FormularyRouteDetailDTO> FormularyRouteDetails { get; set; }

        public List<FormularyLocalRouteDetailDTO> FormularyLocalRouteDetails { get; set; }
        public List<FormularyExcipientDTO> FormularyExcipients { get; set; }

        //public List<FormularyOntologyFormDTO> FormularyOntologyForms { get; set; }

        public FormularyRuleboundQueryValidator ValidationResult { get; set; }

        //public List<FormularySupplierDTO> FormularySuppliers { get; set; }
    }
}
