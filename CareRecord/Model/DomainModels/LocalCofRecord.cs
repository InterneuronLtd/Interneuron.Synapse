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
using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_LocalCofRecord : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string CofRecordId { get; set; }
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
        public string EncounterId { get; set; }
        public bool? Attendanceoutcometreatmentgiven { get; set; }
        public string Weeksmonths { get; set; }
        public string Attendanceoutcomecomments { get; set; }
        public bool? Tcitaskcreated { get; set; }
        public string Appointmentcomments { get; set; }
        public string Scheduleappointmentin { get; set; }
        public string Dischargereasoncomments { get; set; }
        public string Overbookingallowed { get; set; }
        public string Canbewithregistrar { get; set; }
        public bool? Riskflagcontinue { get; set; }
        public string Appointmentreason { get; set; }
        public bool? Attendanceoutcomefollowup { get; set; }
        public bool? Attendanceoutcomeaddtowl { get; set; }
        public string Cofrecordlastupdatedby { get; set; }
        public string Rttpathwayid { get; set; }
        public string Clinicoutcomecomments { get; set; }
        public string Priority { get; set; }
        public bool? Attendanceoutcomedischargepatient { get; set; }
        public string Attendanceoutcome { get; set; }
        public bool? Iscompleted { get; set; }
        public bool? Attendanceoutcomerefer { get; set; }
        public string Clinicoutcome { get; set; }
        public string Tcitasktype { get; set; }
        public DateTime? Tcitaskcreateddate { get; set; }
        public bool? Manualattend { get; set; }
        public string Incorrectrttreason { get; set; }
        public bool? Attendanceoutcomediagnostics { get; set; }
        public bool? Attendanceoutcomeadmitnow { get; set; }
        public string Decisiontotreatreason { get; set; }
        public bool? Incorrectrtt { get; set; }
        public string Dischargereason { get; set; }
        public bool? Manualdna { get; set; }
        public bool? Attendanceoutcomereschedule { get; set; }
        public string Appointmentconsultant { get; set; }
        public string Supporteddischarge { get; set; }
    }
}
