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
    public partial class baseview_BvassessmentRedandamberreadjson : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string AssessmentId { get; set; }
        public string Sectionname { get; set; }
        public string Selectionone { get; set; }
        public string Section1Recenttraumasurgeryinvasiveprocedure { get; set; }
        public string Section1Indwellinglinesbrokenskin { get; set; }
        public string Section1Impairedimmunity { get; set; }
        public string Assessmentid1 { get; set; }
        public string Sectionname2 { get; set; }
        public string Selectiontwo { get; set; }
        public string Section2Impairedimmunity { get; set; }
        public string Section2Respiratory { get; set; }
        public string Section2Urine { get; set; }
        public string Section2Skinjointwound { get; set; }
        public string Section2Indwellingdevice { get; set; }
        public string Section2Brain { get; set; }
        public string Section2Surgical { get; set; }
        public string Section2Other { get; set; }
    }
}
