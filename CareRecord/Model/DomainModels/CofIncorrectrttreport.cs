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


using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_CofIncorrectrttreport : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public string Nhsnumber { get; set; }
        public string Mrn { get; set; }
        public string Titletext { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Familyname { get; set; }
        public string Fullname { get; set; }
        public string Preferredname { get; set; }
        public DateTime? Dateofbirth { get; set; }
        public string Dateofbirthformatted { get; set; }
        public double? Ageinyears { get; set; }
        public TimeSpan? Agetext { get; set; }
        public DateTime? Dateofdeath { get; set; }
        public string Ddateofdeathformatted { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        public string Maritalstatus { get; set; }
        public string Religion { get; set; }
        public bool? Deathindicator { get; set; }
        public string EncounterId { get; set; }
        public DateTime? Admitdatetime { get; set; }
        public string Admitdateformatted { get; set; }
        public string Admittimeformatted { get; set; }
        public string Episodestatuscode { get; set; }
        public string Episodestatustext { get; set; }
        public string Admitreasontext { get; set; }
        public string Appointmenttype { get; set; }
        public string Iscompleted { get; set; }
        public string Attendancestatus { get; set; }
        public string Currentrttstatus { get; set; }
        public string Attendanceoutcome { get; set; }
        public string Clinicoutcome { get; set; }
        public string Cliniccode { get; set; }
        public string Clinicdescription { get; set; }
        public string Clinicowner { get; set; }
        public string Leadconsultantcode { get; set; }
        public string Leadconsultantname { get; set; }
        public string Rttpathway { get; set; }
        public DateTime? Rttstartdate { get; set; }
        public string Rttstartdateformatted { get; set; }
        public DateTime? Rttbreachdate { get; set; }
        public string Rttbreachdateformatted { get; set; }
        public string Localrttcode { get; set; }
        public string Nationalrttcode { get; set; }
        public string Specialtycode { get; set; }
        public string Specialtytext { get; set; }
        public string Specialtycategory { get; set; }
        public string CofRttextractId { get; set; }
        public string Cofkey { get; set; }
        public string Rttpathwayid { get; set; }
        public int? Rttcurrentwaittime { get; set; }
        public int? Rttwaittimeatappointment { get; set; }
        public double? Dayssinceappointment { get; set; }
        public DateTime? Appointmentdate { get; set; }
        public string Appointmentdateformatted { get; set; }
        public string Appointmenttime { get; set; }
        public string Manualattendordna { get; set; }
        public string Statusclass { get; set; }
        public bool? Incorrectrtt { get; set; }
        public string Incorrectrttreason { get; set; }
        public DateTime? Cofcompleteddate { get; set; }
        public string Cofcompleteddateformatted { get; set; }
        public string Cofcompletedby { get; set; }
    }
}
