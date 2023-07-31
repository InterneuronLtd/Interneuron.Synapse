//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

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
    public partial class entitystorematerialised_ExtendedEncounter : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string EncounterId { get; set; }
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
        public DateTime? Intendeddischargedatetime { get; set; }
        public DateTime? Intendeddischargedatetimets { get; set; }
        public string Specialtycode { get; set; }
        public string Specialtytext { get; set; }
        public string Consultingdoctorgmccode { get; set; }
        public string Consultingdoctorpasid { get; set; }
        public string ReferralId { get; set; }
        public DateTime? Intendedadmissiondate { get; set; }
        public DateTime? Intendedadmissiondatets { get; set; }
        public string Intendedward { get; set; }
        public string Intendedbay { get; set; }
        public string Intendedbed { get; set; }
        public DateTime? Originaledd { get; set; }
        public DateTime? Originaleddts { get; set; }
        public DateTime? Expectedleaveofabsencereturndate { get; set; }
        public DateTime? Decisiontoadmitdate { get; set; }
        public string Managementintention { get; set; }
        public DateTime? Consultantepisodestartdate { get; set; }
        public DateTime? Consultantepisodeenddate { get; set; }
        public string Specialtytreatmentfunctioncode { get; set; }
        public string Specialtytreatmentfunctiontext { get; set; }
        public string Specialtytreatmentfunctioncodingsystem { get; set; }
        public string Specialtytreatmentfunctionaltcodingsystem { get; set; }
        public string Referralsource { get; set; }
        public string Servicetyperequestedcode { get; set; }
        public string Servicetyperequestedtext { get; set; }
        public string Servicetyperequestedcodingsystem { get; set; }
        public string Servicetyperequestedaltcode { get; set; }
        public string Servicetyperequestedalttext { get; set; }
        public string Servicetyperequestedaltcodingsystem { get; set; }
        public DateTime? Referralreceiveddate { get; set; }
        public DateTime? Referraldate { get; set; }
        public string Referralmethodcode { get; set; }
        public string Referralmethodtext { get; set; }
        public string Writtenreferralindicator { get; set; }
        public DateTime? Readyfordischargedate { get; set; }
        public string Nhsnumbertracingstatus { get; set; }
        public string Delayeddischargereasoncode { get; set; }
        public string Delayeddischargereasontext { get; set; }
        public string Outcomeofattendancecode { get; set; }
        public string Outcomeofattendancetext { get; set; }
        public string Firstattendanceindicatorcode { get; set; }
        public string Firstattendanceindicatortext { get; set; }
        public string Firstattendanceindicatorcodingsystem { get; set; }
        public string Firstattendanceindicatoraltcode { get; set; }
        public string Firstattendanceindicatoralttext { get; set; }
        public string Firstattendanceindicatoraltcodingsystem { get; set; }
        public string Cancelledbycode { get; set; }
        public string Cancelledbytext { get; set; }
        public string Cancellationreasoncode { get; set; }
        public string Cancellationreasontext { get; set; }
        public DateTime? Datethisprovider { get; set; }
        public DateTime? Originaldateonlist { get; set; }
        public DateTime? Guaranteedadmissiondate { get; set; }
        public DateTime? Suspensionstartdate { get; set; }
        public DateTime? Suspensionenddate { get; set; }
        public DateTime? Tcidate { get; set; }
        public string Reasonforremoval { get; set; }
        public DateTime? Removaldate { get; set; }
        public string Outcomeofofferofadmission { get; set; }
        public string Waitinglisttypecode { get; set; }
        public string Waitinglisttypetext { get; set; }
        public string Reasonforexceedingguaranteedate { get; set; }
        public DateTime? Lastreviewdate { get; set; }
    }
}
