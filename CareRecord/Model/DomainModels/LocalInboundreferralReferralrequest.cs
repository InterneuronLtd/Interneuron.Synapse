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
    public partial class entitystorematerialised_LocalInboundreferralReferralrequest : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string InboundreferralReferralrequestId { get; set; }
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
        public string Referralsessionid { get; set; }
        public string Referringorganisationcode { get; set; }
        public string Referringorganisationname { get; set; }
        public string Referrername { get; set; }
        public string Referrerdesignation { get; set; }
        public string Referrerdepartment { get; set; }
        public string Referrertel1 { get; set; }
        public string Referrertel2 { get; set; }
        public string Referreremail { get; set; }
        public string Patienttitle { get; set; }
        public string Patientfirstname { get; set; }
        public string Patientlastname { get; set; }
        public string Patientgender { get; set; }
        public string Patientdateofbirth { get; set; }
        public string Patientnhsnumber { get; set; }
        public string Patientaddress { get; set; }
        public string Patientcontacttel1 { get; set; }
        public string Patientcontacttel2 { get; set; }
        public string Patientemail { get; set; }
        public string Patientcontactinstructions { get; set; }
        public string Patientgpnameandaddress { get; set; }
        public string Patientgptel { get; set; }
        public string Servicerequested { get; set; }
        public string Medicalhistory { get; set; }
        public bool? Referralcreated { get; set; }
        public string Patientpostcode { get; set; }
        public string Referralstatus { get; set; }
        public string Matchedhospitalnumber { get; set; }
        public string Adminnotes { get; set; }
        public DateTime? Lastupdated { get; set; }
        public string Referralcategory { get; set; }
        public bool? Senttonoteon { get; set; }
        public DateTime? Senttonoteondate { get; set; }
        public string Senttonoteonfilename { get; set; }
    }
}
