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
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.DataService.APIModel
{
    public  class TerminologyConfigurationAPIModel
    {
        public Guid RowId { get; set; }
        public long Sequenceid { get; set; }
        public string Contextkey { get; set; }
        public DateTimeOffset Createddate { get; set; }
        public string Createdsource { get; set; }
        public string Createdby { get; set; }
        public long Recordstatus { get; set; }
        public string Name { get; set; }
        public string ConfigJson { get; set; }
    }
}
