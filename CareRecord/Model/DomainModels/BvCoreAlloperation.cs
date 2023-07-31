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
    public partial class baseview_BvCoreAlloperation : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string OperationId { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public string LocationId { get; set; }
        public string Statuscode { get; set; }
        public string Statustext { get; set; }
        public string Operationtypecode { get; set; }
        public string Operationtypetext { get; set; }
        public string Scheduleidentifiercode { get; set; }
        public string Scheduleidentifiertext { get; set; }
        public string Reasoncode { get; set; }
        public string Reasontext { get; set; }
        public decimal? Operationduration { get; set; }
        public string Operationdurationunit { get; set; }
        public DateTime? Plannedstart { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
        public string Operationsidecode { get; set; }
        public string Operationsidetext { get; set; }
        public string Operationnotestatuscode { get; set; }
        public string Operationnotestatustext { get; set; }
        public string Operationqualifiercode { get; set; }
        public string Operationqualifiertext { get; set; }
        public bool? Isretrospectivedata { get; set; }
        public string Diagnoses { get; set; }
        public string Procedures { get; set; }
        public string Operationproviders { get; set; }
        public string Hendersonoutcome { get; set; }
    }
}
