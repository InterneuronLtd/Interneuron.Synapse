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
using Interneuron.CareRecord.Repository.Infrastructure;
using Interneuron.CareRecord.Repository.QueryBuilders;
using Interneuron.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.CareRecord.Repository
{
    public class GenericSearchRepository : IGenericSearchRepository
    {
        private IServiceProvider _provider;
        private SynapseDBContext _dbContext;

        public GenericSearchRepository(IServiceProvider provider, SynapseDBContext dBContext)
        {
            this._provider = provider;
            this._dbContext = dBContext;
        }

        public List<dynamic> Search(string searchEntityIdentifier, List<SynapseSearchTerm> synapseSearchTerms)
        {
            var builderFactory = this._provider.GetService(typeof(QueryBuilderFactory)) as QueryBuilderFactory;

            var queryBuilder = builderFactory.GetQueryBuilder(searchEntityIdentifier);
            queryBuilder.dbContext = this._dbContext;

            return queryBuilder.Execute(synapseSearchTerms);

//            var baseQuery = (from encounter in this._dbContext.entitystorematerialised_CoreEncounter
//                             join person in this._dbContext.entitystorematerialised_CorePerson on
//                             encounter.PersonId equals person.PersonId
//                             join personId in this._dbContext.entitystorematerialised_CorePersonidentifier on
//                             person.PersonId equals personId.PersonId
//                             select new
//                             {
//                                 encounterData = encounter,
//                                 personData = person,
//                                 personIdData = personId
//                             });

//            var searchOp = new GenericSearchOpProcessor();

//            var withSearchClause = searchOp.Apply(baseQuery, synapseSearchTerms);

//#if DEBUG 
//            var stringQuery = withSearchClause.ToSql();
//#endif
//            var matEncounters = withSearchClause
//                .OrderByDescending((entity) => entity.encounterData.Createdtimestamp)
//                .Select(entity => new
//                {
//                    encounter = entity.encounterData,
//                    patientIdentifer = entity.personIdData
//                })
//                .ToList();

//            if (matEncounters.IsCollectionValid())
//            {
//                var encounters = matEncounters.Select(m => m.encounter).ToList(); // make sure this is the root entity - by convention
//                var patientIdentifier = matEncounters.Select(m => m.patientIdentifer).FirstOrDefault();

//                return new List<dynamic> { encounters, patientIdentifier };
//            }

//            return null;
        }
    }
}
