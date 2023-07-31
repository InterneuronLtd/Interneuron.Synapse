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
using Interneuron.CareRecord.Repository.Infrastructure;
using Interneuron.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interneuron.CareRecord.Repository.QueryBuilders
{
    internal class ProcedureSearchQueryBuilder : QueryBuilder
    {
        public override List<dynamic> Execute(List<SynapseSearchTerm> synapseSearchTerms)
        {
            var baseQuery = from procedure in this.dbContext.entitystorematerialised_CoreProcedure
                             join person in this.dbContext.entitystorematerialised_CorePerson on
                             procedure.PersonId equals person.PersonId
                             join personIdentifier in this.dbContext.entitystorematerialised_CorePersonidentifier on
                             person.PersonId equals personIdentifier.PersonId
                             select new
                             {
                                 procedureData = procedure,
                                 personData = person,
                                 personIdData = personIdentifier
                             };

            var searchOp = new GenericSearchOpProcessor();

            var withSearchClause = searchOp.Apply(baseQuery, synapseSearchTerms);

#if DEBUG 
            var stringQuery = withSearchClause.ToSql();
#endif
            var matProcedures = withSearchClause
                .OrderByDescending((entity) => entity.procedureData.Createdtimestamp)
                .Select(entity => new
                {
                    procedure = entity.procedureData,
                    patientIdentifer = entity.personIdData
                })
                .ToList();

            if (matProcedures.IsCollectionValid())
            {
                var proocedures = matProcedures.Select(m => m.procedure).Distinct().ToList(); // make sure this is the root entity
                var patientIdentifier = matProcedures.Select(m => m.patientIdentifer).FirstOrDefault();

                return new List<dynamic> { proocedures, patientIdentifier };
            }

            return null;
        }
    }
}
