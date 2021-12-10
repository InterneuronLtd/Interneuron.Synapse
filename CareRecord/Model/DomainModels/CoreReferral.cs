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
    public partial class entitystorematerialised_CoreReferral : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string ReferralId { get; set; }
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
        public string Type { get; set; }
        public DateTime? Effectivedate { get; set; }
        public DateTime? Processdate { get; set; }
        public DateTime? Expriationdate { get; set; }
        public string Ubrn { get; set; }
        public string Referraltype { get; set; }
        public string Referringproviderlocationtext { get; set; }
        public string Referringproviderlocationcode { get; set; }
        public string Referringprovideridentifier { get; set; }
        public string Referringproviderprefix { get; set; }
        public string Referringprovidergivenname { get; set; }
        public string Referringprovidermiddlename { get; set; }
        public string Referringproviderfamilyname { get; set; }
        public string Referringproviderfullname { get; set; }
        public string Referringprovideraddressline1 { get; set; }
        public string Referringprovideraddressline2 { get; set; }
        public string Referringprovideraddressline3 { get; set; }
        public string Referringprovideraddressline4 { get; set; }
        public string Referringprovideraddresspostcode { get; set; }
        public string Referringprovideraddresscountry { get; set; }
        public string Referredtoproviderlocationtext { get; set; }
        public string Referredtoproviderlocationcode { get; set; }
        public string Referredtoprovideridentifier { get; set; }
        public string Referredtoproviderprefix { get; set; }
        public string Referredtoprovidergivenname { get; set; }
        public string Referredtoprovidermiddlename { get; set; }
        public string Referredtoproviderfamilyname { get; set; }
        public string Referredtoproviderfullname { get; set; }
        public string Referredtoprovideraddressline1 { get; set; }
        public string Referredtoprovideraddressline2 { get; set; }
        public string Referredtoprovideraddressline3 { get; set; }
        public string Referredtoprovideraddressline4 { get; set; }
        public string Referredtoprovideraddresspostcode { get; set; }
        public string Referredtoprovideraddresscountry { get; set; }
        public string Specialty { get; set; }
        public string Priority { get; set; }
        public string Source { get; set; }
    }
}
