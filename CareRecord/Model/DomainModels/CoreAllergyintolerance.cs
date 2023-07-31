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
    public partial class entitystorematerialised_CoreAllergyintolerance : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string AllergyintoleranceId { get; set; }
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
        public string Causativeagentcodesystem { get; set; }
        public string Causativeagentcode { get; set; }
        public string Causativeagentdescription { get; set; }
        public string Clinicalstatusvalue { get; set; }
        public string Clinicalstatusby { get; set; }
        public DateTime? Cliinicialstatusdatetime { get; set; }
        public string Category { get; set; }
        public string Criticality { get; set; }
        public string Reportedbyname { get; set; }
        public DateTime? Reportedbydatetime { get; set; }
        public string Verificationstatus { get; set; }
        public string Assertedby { get; set; }
        public DateTime? Asserteddatetime { get; set; }
        public string Allergynotes { get; set; }
        public string Manifestationnotes { get; set; }
        public DateTime? Onsetdate { get; set; }
        public DateTime? Enddate { get; set; }
        public DateTime? Lastoccurencedate { get; set; }
        public string Recordedby { get; set; }
        public DateTime? Recordeddatetime { get; set; }
        public string Displaywarning { get; set; }
        public string Allergyconcept { get; set; }
        public string Reactionconcepts { get; set; }
        public string Reportedbygroup { get; set; }
        public string Reactiontext { get; set; }
        public DateTime? Lastupdatedrecorddatetime { get; set; }
        public string Allergentype { get; set; }
    }
}
