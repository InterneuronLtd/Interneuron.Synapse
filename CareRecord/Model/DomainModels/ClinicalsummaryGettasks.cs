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


﻿using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_ClinicalsummaryGettasks : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public DateTime? Createdtimestamp { get; set; }
        public string TaskId { get; set; }
        public string Createdby { get; set; }
        public string Correlationid { get; set; }
        public string Correlationtype { get; set; }
        public string PersonId { get; set; }
        public string Tasktype { get; set; }
        public string Taskdetails { get; set; }
        public string Taskcreatedby { get; set; }
        public DateTime? Taskcreateddatetime { get; set; }
        public string Taskname { get; set; }
        public string Allocatedto { get; set; }
        public string Notes { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
        public string EncounterId { get; set; }
        public DateTime? Duedate { get; set; }
        public DateTime? Allocateddatetime { get; set; }
        public DateTime? Ownerassigneddatetime { get; set; }
        public string ClinicalsummaryId { get; set; }
        public int? PriorityNum { get; set; }
    }
}
