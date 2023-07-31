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


﻿using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public partial class FormularyQueries : IFormularyQueries
    {
        public async Task<List<T>> GetLookup<T>(LookupType lookupType) where T : ILookupItemDTO
        {
            var lookupFactory = this._provider.GetService(typeof(LookupFactory)) as LookupFactory;

            var results = await lookupFactory.GetLookupDTO<T>(lookupType);

            if (!results.IsCollectionValid()) { return null; }

            //var lookups = results.Where(res => res.Recordstatus == 1).ToList(); //not too many records in lookup -- otherwise  to be moved to db query
            var lookups = results.ToList(); //not too many records in lookup -- otherwise  to be moved to db query

            if (!lookups.IsCollectionValid()) { return null; }

            return lookups;
        }

        public async Task<Dictionary<T1, T2>> GetLookup<T, T1, T2>(LookupType lookupType, Func<T, T1> keySelector, Func<T, T2> valueSelector) where T : ILookupItemDTO
        {
            var lookupDictionary = new Dictionary<T1, T2>();

            var lookupFactory = this._provider.GetService(typeof(LookupFactory)) as LookupFactory;

            var results = await lookupFactory.GetLookupDTO<T>(lookupType);

            if (!results.IsCollectionValid()) { return lookupDictionary; }

            var lookups = results.ToList();

            if (!lookups.IsCollectionValid()) { return lookupDictionary; }

            lookups.Each(rec =>
            {
                lookupDictionary[keySelector(rec)] = valueSelector(rec);
            });

            return lookupDictionary;
        }
    }
}
