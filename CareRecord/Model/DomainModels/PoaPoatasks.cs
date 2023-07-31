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
    public partial class baseview_PoaPoatasks : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PoaTaskId { get; set; }
        public string PersonId { get; set; }
        public string Notes { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public string PoaStaffgroupId { get; set; }
        public string PoaPreopassessmentId { get; set; }
        public string Taskdetails { get; set; }
        public string PoaTaskstatusId { get; set; }
        public string Taskcreatedby { get; set; }
        public DateTime? Taskcreateddatetime { get; set; }
        public string Assignedto { get; set; }
        public string Taskname { get; set; }
        public decimal? Statusorder { get; set; }
        public decimal? Displayorder { get; set; }
        public string Referredto { get; set; }
    }
}
