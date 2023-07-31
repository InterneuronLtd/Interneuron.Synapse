//Interneuron Synapse

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
    public partial class baseview_PatientbannerMainallergies : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public string Allergies { get; set; }
        public string Adversedrugreactions { get; set; }
        public string Sensitityintolerances { get; set; }
        public string Cautions { get; set; }
        public bool? Noallergiesrecorded { get; set; }
        public bool? Noknownallergies { get; set; }
        public bool? Unabletoacsertain { get; set; }
        public long? Allergycount { get; set; }
        public long? Adversereactioncount { get; set; }
        public long? Sensitivityintolerancecount { get; set; }
        public long? Cautioncount { get; set; }
        public long? Lifethreateningcount { get; set; }
    }
}
