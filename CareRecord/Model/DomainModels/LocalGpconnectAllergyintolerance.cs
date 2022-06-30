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
    public partial class entitystorematerialised_LocalGpconnectAllergyintolerance : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string GpconnectAllergyintoleranceId { get; set; }
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
        public string Clinicalstatus { get; set; }
        public string Verificationstatus { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Criticality { get; set; }
        public string Allergycode { get; set; }
        public string Allergytext { get; set; }
        public DateTime? Onsetdatetime { get; set; }
        public decimal? Onsetage { get; set; }
        public DateTime? Onsetperiodstart { get; set; }
        public DateTime? Onsetperionend { get; set; }
        public string Onsetstring { get; set; }
        public DateTime? Asserteddatetime { get; set; }
        public string Recordedby { get; set; }
        public string Assertedby { get; set; }
        public DateTime? Lastoccurrence { get; set; }
        public string Allergynote { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public DateTime? Enddatetime { get; set; }
        public string Reasonended { get; set; }
    }
}
