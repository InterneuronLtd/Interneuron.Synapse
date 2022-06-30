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
    public partial class entitystorematerialised_CoreTheatresession : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string TheatresessionId { get; set; }
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
        public string Theatresessioncode { get; set; }
        public string Bookingsequence { get; set; }
        public DateTime? Theatresessiondate { get; set; }
        public DateTime? Theatresessiondatets { get; set; }
        public string Profilecode { get; set; }
        public string Theatresessiondescription { get; set; }
        public string Messagetriggercode { get; set; }
        public string Operationstatus { get; set; }
        public string Operationtypecode { get; set; }
        public string Operationtypetext { get; set; }
        public decimal? Appointmentduration { get; set; }
        public string Appointmentdurationunit { get; set; }
        public string Enteredbyusercode { get; set; }
        public string Enteredbyusername { get; set; }
        public string Enteredbyuserphonenumber { get; set; }
        public string Enteredbyuserlocation { get; set; }
        public DateTime? Deceisiontoadmitdate { get; set; }
        public DateTime? Deceisiontoadmitdatets { get; set; }
        public string Managementintention { get; set; }
        public DateTime? Readyfordischargedate { get; set; }
        public DateTime? Readyfordischargedatets { get; set; }
        public string Nhsnumbertracingstatus { get; set; }
        public string Cancelledbypersontypecode { get; set; }
        public string Cancelledbypersontypetext { get; set; }
        public string Cancelledreasoncode { get; set; }
        public string Cancelledreasontext { get; set; }
        public string Nationalspecialtycode { get; set; }
        public string Nationalspecialtytext { get; set; }
        public string Localspecialtycode { get; set; }
        public string Localspecialtytext { get; set; }
        public string Theatrelocationcode { get; set; }
        public string Theatrelocationtext { get; set; }
        public string Hospitallocation { get; set; }
    }
}
