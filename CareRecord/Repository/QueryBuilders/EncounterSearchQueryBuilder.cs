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


﻿using Interneuron.CareRecord.Infrastructure.Search;
using Interneuron.CareRecord.Repository.Infrastructure;
using Interneuron.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.CareRecord.Repository.QueryBuilders
{
    public class EncounterSearchQueryBuilder : QueryBuilder
    {
        public override List<dynamic> Execute(List<SynapseSearchTerm> synapseSearchTerms)
        {
            var baseQuery = (from encounter in this.dbContext.entitystorematerialised_CoreEncounter
                             join person in this.dbContext.entitystorematerialised_CorePerson on
                             encounter.PersonId equals person.PersonId
                             join personId in this.dbContext.entitystorematerialised_CorePersonidentifier on
                             person.PersonId equals personId.PersonId
                             select new
                             {
                                 encounterData = encounter,
                                 personData = person,
                                 personIdData = personId
                             });

            var searchOp = new GenericSearchOpProcessor();

            var withSearchClause = searchOp.Apply(baseQuery, synapseSearchTerms);

#if DEBUG 
            var stringQuery = withSearchClause.ToSql();
#endif
            var matEncounters = withSearchClause
                .OrderByDescending((entity) => entity.encounterData.Createdtimestamp)
                .Select(entity => new
                {
                    encounter = entity.encounterData,
                    patientIdentifer = entity.personIdData
                })
                .ToList();

            if (matEncounters.IsCollectionValid())
            {
                var encounters = matEncounters.Select(m => m.encounter).ToList(); // make sure this is the root entity
                var patientIdentifier = matEncounters.Select(m => m.patientIdentifer).FirstOrDefault();

                return new List<dynamic> { encounters, patientIdentifier };
            }

            return null;
        }
    }
}
