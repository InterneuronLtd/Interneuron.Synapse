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
    public partial class baseview_EpmaOrdersetprescriptiondetails : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PrescriptionId { get; set; }
        public string Name { get; set; }
        public string Form { get; set; }
        public string Frequency { get; set; }
        public decimal? Frequencysize { get; set; }
        public string Dosesize { get; set; }
        public string Doseunit { get; set; }
        public string Route { get; set; }
        public string Indication { get; set; }
        public string OrdersetprescriptionId { get; set; }
        public string OrdersetId { get; set; }
    }
}