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
    public partial class baseview_EpmaDoseevents : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string DoseeventsId { get; set; }
        public string DoseId { get; set; }
        public string PosologyId { get; set; }
        public DateTime? Startdatetime { get; set; }
        public string Eventtype { get; set; }
        public DateTime? Dosedatetime { get; set; }
        public bool? Iscancelled { get; set; }
        public string Logicalid { get; set; }
        public int? Sequenceid { get; set; }
        public string Comments { get; set; }
        public string Titrateddosesize { get; set; }
        public string Titrateddoseunit { get; set; }
        public decimal? Titratedstrengthneumerator { get; set; }
        public string Titratedstrengthneumeratorunits { get; set; }
        public decimal? Titratedstrengthdenominator { get; set; }
        public string Titratedstrengthdenominatorunits { get; set; }
        public bool? Grouptitration { get; set; }
        public string Titrateddescriptivedose { get; set; }
        public DateTime? Titrateduntildatetime { get; set; }
    }
}
