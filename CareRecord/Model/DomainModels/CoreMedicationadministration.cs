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
    public partial class entitystorematerialised_CoreMedicationadministration : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string MedicationadministrationId { get; set; }
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
        public string Site { get; set; }
        public string Method { get; set; }
        public string PersonId { get; set; }
        public string DoseId { get; set; }
        public string PrescriptionId { get; set; }
        public string PosologyId { get; set; }
        public DateTime? Administrationstartime { get; set; }
        public DateTime? Administrationendtime { get; set; }
        public string Administredby { get; set; }
        public DateTime? Planneddatetime { get; set; }
        public decimal? Administeredstrengthneumerator { get; set; }
        public string Administeredstrengthneumeratorunits { get; set; }
        public decimal? Administeredstrengthdenominator { get; set; }
        public string Administeredstrengthdenominatorunits { get; set; }
        public decimal? Plannedstrengthneumerator { get; set; }
        public string Plannedstrengthneumeratorunits { get; set; }
        public decimal? Plannedstrengthdenominator { get; set; }
        public string Plannedstrengthdenominatorunits { get; set; }
        public string Administreddosesize { get; set; }
        public string Administreddoseunit { get; set; }
        public string Administreddosemeasure { get; set; }
        public string Planneddosesize { get; set; }
        public string Planneddoseunit { get; set; }
        public string Planneddosemeasure { get; set; }
        public string Plannedinfustionrate { get; set; }
        public string Administredinfusionrate { get; set; }
        public string Adminstrationstatus { get; set; }
        public string Adminstrationstatusreason { get; set; }
        public bool? Requestresupply { get; set; }
        public string Doctorsordercomments { get; set; }
        public string Witness { get; set; }
        public bool? Substituted { get; set; }
        public string Administrationdevice { get; set; }
        public string Administrationsite { get; set; }
        public string MedicationId { get; set; }
        public string EncounterId { get; set; }
        public string Logicalid { get; set; }
        public string Batchnumber { get; set; }
        public DateTime? Expirydate { get; set; }
        public string Prescriptionroutesid { get; set; }
        public string Comments { get; set; }
        public string Adminstrationstatusreasontext { get; set; }
        public string Routename { get; set; }
        public string Planneddosesizerangemax { get; set; }
        public string Levelofselfadmin { get; set; }
        public string Administereddescriptivedose { get; set; }
    }
}
