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

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_LocalEpmaSupplyrequest : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string EpmaSupplyrequestId { get; set; }
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
        public string PrescriptionId { get; set; }
        public string MedicationId { get; set; }
        public string Selectedproductcode { get; set; }
        public string Selectproductcodetype { get; set; }
        public string Requeststatus { get; set; }
        public string Requestedby { get; set; }
        public string Lastmodifiedby { get; set; }
        public decimal? Requestednoofdays { get; set; }
        public string Requestquantity { get; set; }
        public string Requestedquantityunits { get; set; }
        public DateTime? Requestedon { get; set; }
        public DateTime? Daterequired { get; set; }
        public bool? Labelinstructiosrequired { get; set; }
        public string Additionaldirections { get; set; }
        public bool? Isformulary { get; set; }
        public string Othercommentsnf { get; set; }
        public string Ordermessage { get; set; }
        public string Suppliedquantity { get; set; }
        public string Suppliedquantityunits { get; set; }
        public DateTime? Lastmodifiedon { get; set; }
        public DateTime? Fulfilledon { get; set; }
        public string Selectedproductname { get; set; }
        public string Route { get; set; }
        public string Indication { get; set; }
        public string Otherindication { get; set; }
        public string Licenseauthority { get; set; }
        public string Marketingauthorisation { get; set; }
        public string Othercountry { get; set; }
        public string Othercountrytext { get; set; }
        public string Personid { get; set; }
        public string Encounterid { get; set; }
        public string Durationoftreatment { get; set; }
        public string Indicationinuk { get; set; }
        public string Patientownsupply { get; set; }
        public string Specifyformulary { get; set; }
        public string Reasonforprescribingnonformulary { get; set; }
        public string Reasonfornotprescribingformulary { get; set; }
        public string Costofmedicine { get; set; }
        public string Patientownsupplyother { get; set; }
    }
}
