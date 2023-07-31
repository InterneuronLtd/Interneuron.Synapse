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

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_LocalEpmaPrescriptionmedicaitonsupply : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string EpmaPrescriptionmedicaitonsupplyId { get; set; }
        public string RowId { get; set; }
        public int? Sequenceid { get; set; }
        public string Contextkey { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdsource { get; set; }
        public string Createdmessageid { get; set; }
        public string Createdby { get; set; }
        public short? Recordstatus { get; set; }
        public string Timezonename { get; set; }
        public int? Timezoneoffset { get; set; }
        public string Tenant { get; set; }
        public string Prescriptionid { get; set; }
        public string Prescribedmedicationid { get; set; }
        public string Selectedproductcode { get; set; }
        public string Selectproductcodetype { get; set; }
        public string Quantityunits { get; set; }
        public string Complianceaid { get; set; }
        public bool? Ownsupplyathome { get; set; }
        public string Resupplyfrom { get; set; }
        public string Lastmodifiedby { get; set; }
        public string Updatesouce { get; set; }
        public decimal? Noofdays { get; set; }
        public string Availablequantity { get; set; }
    }
}
