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
    public partial class entitystorematerialised_LocalPoaAssessmentsummary : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PoaAssessmentsummaryId { get; set; }
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
        public string PoaPreopassessmentId { get; set; }
        public string Akistatus { get; set; }
        public decimal? Akirisk { get; set; }
        public string Akiguidance { get; set; }
        public string Sortstatus { get; set; }
        public decimal? Sortrisk { get; set; }
        public string Sortnotes { get; set; }
        public string Frailtyscore { get; set; }
        public string Cognitivecriteria { get; set; }
        public string Cognitivedementiadiagnosis { get; set; }
        public string Cognitivecitscore { get; set; }
        public string Cognitivecitguidance { get; set; }
        public string Cognitiveformalcarer { get; set; }
        public string Cognitivepointofcontact { get; set; }
        public string Malnutritionscore { get; set; }
        public string Malnutritionguidance { get; set; }
        public decimal? Akiadditionalriskfactors { get; set; }
        public bool? Taskrefercomplexcase { get; set; }
        public bool? Taskrefergp { get; set; }
        public bool? Taskreferdieteticteam { get; set; }
        public bool? Taskreferdiabetesteam { get; set; }
        public bool? Taskprovideakiinfo { get; set; }
        public bool? Taskcspinexray { get; set; }
        public bool? Taskreferacutepainteam { get; set; }
        public bool? Taskreferanaesthetist { get; set; }
        public bool? Taskinsertdeleriumproforma { get; set; }
        public bool? Taskbedmanagementsideroom { get; set; }
        public bool? Taskreferpharmacy { get; set; }
        public bool? Taskbookhdubed { get; set; }
        public bool? Taskconsiderreferanaesthetist { get; set; }
        public bool? Taskflaghaem { get; set; }
        public string Cognitiverequired { get; set; }
        public string Frailtyrequired { get; set; }
        public string Frailtysixtyplus { get; set; }
    }
}
