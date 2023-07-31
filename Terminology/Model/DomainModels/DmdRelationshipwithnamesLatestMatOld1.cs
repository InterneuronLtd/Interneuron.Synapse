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
using NpgsqlTypes;

namespace Interneuron.Terminology.Model.DomainModels
{
    public partial class DmdRelationshipwithnamesLatestMatOld1 : Interneuron.Terminology.Infrastructure.Domain.EntityBase
    {
        public string Name { get; set; }
        public NpgsqlTsVector NameTokens { get; set; }
        public string Code { get; set; }
        public string Level { get; set; }
        public int? LogicalLevel { get; set; }
        public string ParentCode { get; set; }
        public string ParentLevel { get; set; }
        public int? ParentLogicalLevel { get; set; }
        public string Route { get; set; }
        public string Form { get; set; }
    }
}
