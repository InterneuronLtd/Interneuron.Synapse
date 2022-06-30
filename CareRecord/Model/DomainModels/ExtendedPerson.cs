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
    public partial class entitystorematerialised_ExtendedPerson : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
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
        public string Organisationname { get; set; }
        public string Organisationtypecode { get; set; }
        public string Organisationtypetext { get; set; }
        public string Organisationnationalcode { get; set; }
        public string Organisationcontactinfo { get; set; }
        public string Organisationlocalcode { get; set; }
        public string Organisationemail { get; set; }
        public string Pcpnationalcode { get; set; }
        public string Pcpfamilyname { get; set; }
        public string Pcpgivenname { get; set; }
        public string Pcpprefix { get; set; }
        public string Pcpaddressline1 { get; set; }
        public string Pcpaddressline2 { get; set; }
        public string Pcpaddressline3 { get; set; }
        public string Pcpaddressline4 { get; set; }
        public string Pcpaddresspostcode { get; set; }
        public string Pcpprovidertype { get; set; }
    }
}
