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
    public partial class baseview_BvassessmentAlltopversionassessments : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string Createdby { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public short? Recordstatus { get; set; }
        public string RowId { get; set; }
        public string AssessmentId { get; set; }
        public string AssessmenttypeId { get; set; }
        public string EncounterId { get; set; }
        public string FormtypeId { get; set; }
        public bool? Isamended { get; set; }
        public bool? Isdraft { get; set; }
        public string ObservationeventId { get; set; }
        public string PersonId { get; set; }
        public decimal? Versionid { get; set; }
        public string Taskformsectionid { get; set; }
        public string Sourceofinvocation { get; set; }
        public string Assessmenttypename { get; set; }
        public string Assessmenttypedisplayname { get; set; }
        public string Formtypename { get; set; }
        public string Formtypeheadertext { get; set; }
    }
}
