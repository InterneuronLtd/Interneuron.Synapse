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

namespace Interneuron.Terminology.Model.DomainModels
{
    public partial class DmdSyncLog : Interneuron.Terminology.Infrastructure.Domain.EntityBase
    {
        public string DmdId { get; set; }
        public string SyncProcessId { get; set; }
        public string DmdEntityName { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string RowAction { get; set; }
        public long SerialNum { get; set; }
        public bool? IsDmdUpdated { get; set; }
        public DateTime? DmdUpdateDt { get; set; }
        public bool? IsFormularyUpdated { get; set; }
        public DateTime? FormularyUpdateDt { get; set; }
        public string DmdVersion { get; set; }
        public string SlId { get; set; }
    }
}
