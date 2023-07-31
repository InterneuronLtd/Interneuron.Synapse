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
    public partial class baseview_PoaAssessmentlist : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PoaPreopassessmentId { get; set; }
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
        public string PersonId { get; set; }
        public bool? Iscompletedpastmedicalhistory { get; set; }
        public bool? Iscompletedbaselineobservations { get; set; }
        public bool? Iscompletedallergies { get; set; }
        public bool? Iscompletedmedicationhistory { get; set; }
        public bool? Iscompletedsurgicalhistory { get; set; }
        public bool? Iscompletedphysicalexamination { get; set; }
        public bool? Iscompletedfamilyhistory { get; set; }
        public bool? Iscompletedsocialhistory { get; set; }
        public bool? Iscompletedinfectioncontrol { get; set; }
        public bool? Iscompletedinformationprovided { get; set; }
        public bool? Iscompletednursingassessment { get; set; }
        public decimal? Bmi { get; set; }
        public bool? Reviewedbypharmacist { get; set; }
        public bool? Iscompletedassessments { get; set; }
        public string Poastatus { get; set; }
        public bool? Hdurequired { get; set; }
        public string Proaction { get; set; }
        public string Poatype { get; set; }
        public DateTime? Poadate { get; set; }
        public string Surgeon { get; set; }
        public string Operation { get; set; }
        public DateTime? Admissiondate { get; set; }
        public decimal? Losdays { get; set; }
        public decimal? Losnights { get; set; }
        public DateTime? Dischargedate { get; set; }
        public bool? Revalidated { get; set; }
        public string Revalidationtype { get; set; }
        public DateTime? Revalidateddate { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string Obervationeventid { get; set; }
        public string Heightobservationid { get; set; }
        public string Weightobservationid { get; set; }
        public string Eventcorrelationid { get; set; }
        public DateTime? Reviewedbypharmacistdate { get; set; }
        public string Hdurequiredreason { get; set; }
        public string Reviewedbypharmacistuser { get; set; }
        public bool? Iscompletedgeneral { get; set; }
        public bool? Islocked { get; set; }
        public string Linkedencounterid { get; set; }
        public bool? Lockedonadmission { get; set; }
        public long? Tasks { get; set; }
        public string Completedby { get; set; }
        public string Validatedby { get; set; }
    }
}
