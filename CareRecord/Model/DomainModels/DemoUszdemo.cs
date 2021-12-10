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
    public partial class baseview_DemoUszdemo : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public string Nhsnumber { get; set; }
        public string Mrn { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Familyname { get; set; }
        public string Fullname { get; set; }
        public string Preferredname { get; set; }
        public DateTime? Dateofbirth { get; set; }
        public double? Ageinyears { get; set; }
        public DateTime? Dateofdeath { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        public string Maritalstatus { get; set; }
        public string Religion { get; set; }
        public bool? Deathindicator { get; set; }
        public string EncounterId { get; set; }
        public string Assignedpatientlocationfacility { get; set; }
        public string Ward { get; set; }
        public string Assignedpatientlocationbay { get; set; }
        public string Assignedpatientlocationbed { get; set; }
        public string Bed { get; set; }
        public string Bedcode { get; set; }
        public DateTime? Admitdatetime { get; set; }
        public string Attendingdoctorcode { get; set; }
        public string Attendingdoctortext { get; set; }
        public string Wardinformationdetailed { get; set; }
        public string Wardinformationsimple { get; set; }
        public string Wardinformationformatted { get; set; }
        public string Patientinformationdetailed { get; set; }
        public string Patientinformationsimple { get; set; }
        public string Admissiondetails { get; set; }
        public string Ews { get; set; }
        public string Ewssmall { get; set; }
        public string Nilbymouth { get; set; }
        public string Locationdisplayname { get; set; }
        public string Locationwardinformation { get; set; }
        public string Locationwardinformationright { get; set; }
        public string Otstatus { get; set; }
        public string Questionflag { get; set; }
        public string Allergies { get; set; }
    }
}
