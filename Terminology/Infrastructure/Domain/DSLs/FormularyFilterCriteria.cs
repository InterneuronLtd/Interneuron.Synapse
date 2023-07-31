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
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.Infrastructure.Domain.DSLs
{
    public class FormularyFilterCriteria
    {
        public string SearchTerm { get; set; }

        public bool? HideArchived { get; set; } = false;

        public List<string> RecStatusCds { get; set; }

        public List<string> FormularyStatusCd { get; set; }

        public bool? ShowOnlyDuplicate { get; set; }

        public bool? IncludeDeleted { get; set; } = false;
    }
}
