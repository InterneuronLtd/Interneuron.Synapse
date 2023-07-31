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
    public partial class SnomedctExtendedmaprefsetF : Interneuron.Terminology.Infrastructure.Domain.EntityBase
    {
        public Guid Id { get; set; }
        public string Effectivetime { get; set; }
        public char Active { get; set; }
        public string Moduleid { get; set; }
        public string Refsetid { get; set; }
        public string Referencedcomponentid { get; set; }
        public short Mapgroup { get; set; }
        public short Mappriority { get; set; }
        public string Maprule { get; set; }
        public string Mapadvice { get; set; }
        public string Maptarget { get; set; }
        public string Correlationid { get; set; }
        public string Mapcategoryid { get; set; }
    }
}
