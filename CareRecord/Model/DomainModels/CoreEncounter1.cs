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
    public partial class entitystore_CoreEncounter1 : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string EncounterId { get; set; }
        public string RowId { get; set; }
        public int Sequenceid { get; set; }
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
