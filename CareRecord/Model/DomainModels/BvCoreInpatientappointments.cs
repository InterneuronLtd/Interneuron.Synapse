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
    public partial class baseview_BvCoreInpatientappointments : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Familyname { get; set; }
        public string Fullname { get; set; }
        public string Preferredname { get; set; }
        public string Titlecode { get; set; }
        public string Titletext { get; set; }
        public DateTime? Dateofbirth { get; set; }
        public DateTime? Dateofbirthts { get; set; }
        public DateTime? Dateofdeath { get; set; }
        public DateTime? Dateofdeathts { get; set; }
        public string Gendercode { get; set; }
        public string Gendertext { get; set; }
        public string Ethnicitycode { get; set; }
        public string Ethnicitytext { get; set; }
        public string Maritalstatuscode { get; set; }
        public string Maritalstatustext { get; set; }
        public string Religioncode { get; set; }
        public string Religiontext { get; set; }
        public bool? Deathindicator { get; set; }
        public string EncounterId { get; set; }
        public string Assignedpatientlocationidentifier { get; set; }
        public string Assignedpatientlocationfacility { get; set; }
        public string Assignedpatientlocationbuilding { get; set; }
        public string Assignedpatientlocationfloor { get; set; }
        public string Assignedpatientlocationpointofcare { get; set; }
        public string Assignedpatientlocationroom { get; set; }
        public string Assignedpatientlocationbay { get; set; }
        public string Assignedpatientlocationbed { get; set; }
        public string Assignedpatientlocationlocationtypecode { get; set; }
        public string Assignedpatientlocationlocationtypetext { get; set; }
        public DateTime? Admitdatetime { get; set; }
        public DateTime? Admitdatetimets { get; set; }
        public string Admitreasoncode { get; set; }
        public string Admitreasontext { get; set; }
        public string Admittingdoctorcode { get; set; }
        public string Admittingdoctortext { get; set; }
        public string Attendingdoctorcode { get; set; }
        public string Attendingdoctortext { get; set; }
        public DateTime? Dischargedatetime { get; set; }
        public DateTime? Dischargedatetimets { get; set; }
        public string Dischargecode { get; set; }
        public string Dischargetext { get; set; }
        public string Dischargedisposition { get; set; }
        public string Accountnumber { get; set; }
        public string Encounterid1 { get; set; }
        public string Admissionsourcecode { get; set; }
        public string Admissionsourcetext { get; set; }
        public string Patientclasscode { get; set; }
        public string Patientclasstext { get; set; }
        public string Patienttypecode { get; set; }
        public string Patienttypetext { get; set; }
        public string Visitnumber { get; set; }
        public string Consultingdoctorid { get; set; }
        public string Consultingdoctortext { get; set; }
        public string Referringdoctorid { get; set; }
        public string Referringdoctortext { get; set; }
        public string Accountstatuscode { get; set; }
        public string Accountstatuscodetext { get; set; }
        public string Episodestatuscode { get; set; }
        public string Episodestatustext { get; set; }
    }
}
