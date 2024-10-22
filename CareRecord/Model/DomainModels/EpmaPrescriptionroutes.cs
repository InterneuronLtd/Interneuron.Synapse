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
    public partial class baseview_EpmaPrescriptionroutes : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PrescriptionroutesId { get; set; }
        public string Routecode { get; set; }
        public string Route { get; set; }
        public string Routetype { get; set; }
        public bool? Isunlicensed { get; set; }
        public bool? Isdefault { get; set; }
        public string PrescriptionId { get; set; }
        public string MedicationId { get; set; }
    }
}
