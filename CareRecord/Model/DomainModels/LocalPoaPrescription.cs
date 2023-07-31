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
    public partial class entitystorematerialised_LocalPoaPrescription : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PoaPrescriptionId { get; set; }
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
        public string Indication { get; set; }
        public string Comments { get; set; }
        public string Titrationtype { get; set; }
        public string Targetinr { get; set; }
        public string Targetsaturation { get; set; }
        public string Orderformtype { get; set; }
        public bool? Isinfusion { get; set; }
        public bool? Ismedicinalgas { get; set; }
        public string Prescriptionsourceid { get; set; }
        public bool? Hasbeenmodified { get; set; }
        public string Reasonforediting { get; set; }
        public string Lastmodifiedby { get; set; }
        public string Epmaprescriptioneventid { get; set; }
        public string Reasonforstopping { get; set; }
        public string Reasonforsuspending { get; set; }
        public string Allowsubstitution { get; set; }
        public string Substitutioncomments { get; set; }
        public DateTime? Createdon { get; set; }
        public string Createdby1 { get; set; }
        public DateTime? Lastmodifiedon { get; set; }
        public decimal? Titrationtargetmin { get; set; }
        public decimal? Titrationtargetmax { get; set; }
        public string Titrationtargetunits { get; set; }
        public string Titrationtypecode { get; set; }
        public string Oxygenadditionalinfo { get; set; }
        public string Correlationid { get; set; }
        public string PersonId { get; set; }
        public string ReferralId { get; set; }
        public string PoaPreopassessmentId { get; set; }
        public string PrescriptioncontextId { get; set; }
        public string PrescriptionadditionalconditionsId { get; set; }
        public string OxygendevicesId { get; set; }
        public string InfusiontypeId { get; set; }
        public string PrescriptionsourceId1 { get; set; }
        public string PrescriptionstatusId { get; set; }
        public string EpmaPrescriptioneventId1 { get; set; }
        public bool? Titration { get; set; }
        public string Poacomments { get; set; }
    }
}
