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


﻿using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Interneuron.Terminology.Model.DomainModels
{
    public partial class FormularyHeader : Interneuron.Terminology.Infrastructure.Domain.EntityBase,  Interneuron.Terminology.Infrastructure.Domain.IAuditable
    {
        public FormularyHeader()
        {
            FormularyAdditionalCode = new HashSet<FormularyAdditionalCode>();
            FormularyDetail = new HashSet<FormularyDetail>();
            FormularyExcipient = new HashSet<FormularyExcipient>();
            FormularyIndication = new HashSet<FormularyIndication>();
            FormularyIngredient = new HashSet<FormularyIngredient>();
            FormularyLocalRouteDetail = new HashSet<FormularyLocalRouteDetail>();
            FormularyOntologyForm = new HashSet<FormularyOntologyForm>();
            FormularyRouteDetail = new HashSet<FormularyRouteDetail>();
        }

        public string RowId { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public string Timezonename { get; set; }
        public int? Timezoneoffset { get; set; }
        public string Tenant { get; set; }
        public string FormularyId { get; set; }
        public int? VersionId { get; set; }
        public string FormularyVersionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public NpgsqlTsVector NameTokens { get; set; }
        public string ProductType { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public NpgsqlTsVector ParentNameTokens { get; set; }
        public string ParentProductType { get; set; }
        public string RecStatusCode { get; set; }
        public DateTime? RecStatuschangeTs { get; set; }
        public DateTime? RecStatuschangeDate { get; set; }
        public string RecStatuschangeTzname { get; set; }
        public int? RecStatuschangeTzoffset { get; set; }
        public bool? IsDuplicate { get; set; }
        public bool? IsLatest { get; set; }
        public string RecSource { get; set; }
        public string VtmId { get; set; }
        public string VmpId { get; set; }
        public string RecStatuschangeMsg { get; set; }
        public string DuplicateOfFormularyId { get; set; }
        public DateTime? Updatedtimestamp { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Updatedby { get; set; }
        public string CodeSystem { get; set; }

        public virtual ICollection<FormularyAdditionalCode> FormularyAdditionalCode { get; set; }
        public virtual ICollection<FormularyDetail> FormularyDetail { get; set; }
        public virtual ICollection<FormularyExcipient> FormularyExcipient { get; set; }
        public virtual ICollection<FormularyIndication> FormularyIndication { get; set; }
        public virtual ICollection<FormularyIngredient> FormularyIngredient { get; set; }
        public virtual ICollection<FormularyLocalRouteDetail> FormularyLocalRouteDetail { get; set; }
        public virtual ICollection<FormularyOntologyForm> FormularyOntologyForm { get; set; }
        public virtual ICollection<FormularyRouteDetail> FormularyRouteDetail { get; set; }
    }
}
