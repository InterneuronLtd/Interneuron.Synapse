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

namespace Interneuron.Terminology.Model.DomainModels
{
    public partial class DmdVmpTemp : Interneuron.Terminology.Infrastructure.Domain.EntityBase
    {
        public string VmpId { get; set; }
        public string RowId { get; set; }
        public int Sequenceid { get; set; }
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
        public string Vpid { get; set; }
        public DateTime? Vpiddt { get; set; }
        public string Vpidprev { get; set; }
        public string Vtmid { get; set; }
        public short? Invalid { get; set; }
        public string Nm { get; set; }
        public string Abbrevnm { get; set; }
        public long? Basiscd { get; set; }
        public DateTime? Nmdt { get; set; }
        public string Nmprev { get; set; }
        public long? BasisPrevcd { get; set; }
        public long? Nmchangecd { get; set; }
        public long? Comprodcd { get; set; }
        public long? PresStatcd { get; set; }
        public int? SugF { get; set; }
        public int? GluF { get; set; }
        public int? PresF { get; set; }
        public int? CfcF { get; set; }
        public int? NonAvailcd { get; set; }
        public DateTime? NonAvaildt { get; set; }
        public long? DfIndcd { get; set; }
        public decimal? Udfs { get; set; }
        public string UdfsUomcd { get; set; }
        public string UnitDoseUomcd { get; set; }
        public Guid? ColValHash { get; set; }
    }
}
