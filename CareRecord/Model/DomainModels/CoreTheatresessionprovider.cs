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
    public partial class entitystorematerialised_CoreTheatresessionprovider : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string TheatresessionproviderId { get; set; }
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
        public string TheatresessionId { get; set; }
        public int? Setid { get; set; }
        public string Actioncode { get; set; }
        public string Actiontext { get; set; }
        public string Provideridnumber { get; set; }
        public string Providerdegree { get; set; }
        public string Prefix { get; set; }
        public string Firstname { get; set; }
        public string Familyname { get; set; }
        public string Fullname { get; set; }
        public string Providerroletext { get; set; }
        public string Providerolecode { get; set; }
        public string Teamidentifiercode { get; set; }
        public string Teamidentifiertext { get; set; }
    }
}
