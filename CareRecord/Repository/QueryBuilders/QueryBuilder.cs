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
﻿using Interneuron.CareRecord.Infrastructure.Search;
using Interneuron.CareRecord.Repository.DBModelsContext;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Repository.QueryBuilders
{
    public abstract class QueryBuilder
    {
        public SynapseDBContext dbContext { get; set; } // This can be optional and hence property
        public abstract List<dynamic> Execute(List<SynapseSearchTerm> synapseSearchTerms);
    }
}
