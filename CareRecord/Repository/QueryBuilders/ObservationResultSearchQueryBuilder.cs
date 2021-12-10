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


﻿using Interneuron.CareRecord.Infrastructure.Search;
using Interneuron.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.CareRecord.Repository.QueryBuilders
{
    public class ObservationResultSearchQueryBuilder : QueryBuilder
    {
        public override List<dynamic> Execute(List<SynapseSearchTerm> synapseSearchTerms)
        {
            var baseQuery = (from result in this.dbContext.entitystorematerialised_CoreResult1
                             join person in this.dbContext.entitystorematerialised_CorePerson on
                             result.PersonId equals person.PersonId
                             join personIden in this.dbContext.entitystorematerialised_CorePersonidentifier on
                             person.PersonId equals personIden.PersonId
                             join resultOrder in this.dbContext.entitystorematerialised_CoreOrder1 on
                             result.OrderId equals resultOrder.OrderId into orderTemp
                             from order in orderTemp.DefaultIfEmpty()
                             select new
                             {
                                 resultData = result,
                                 personData = person,
                                 personIdData = personIden,
                                 orderData = order
                             });

            var searchOp = new GenericSearchOpProcessor();

            var withSearchClause = searchOp.Apply(baseQuery, synapseSearchTerms);

            var matResults = withSearchClause
                .OrderByDescending((entity) => entity.resultData.Createdtimestamp)
                .Select(entity => new
                {
                    result = entity.resultData,
                    patientIdentifer = entity.personIdData
                })
                .ToList();

            if (matResults.IsCollectionValid())
            {
                var results = matResults.Select(m => m.result).ToList();
                var patientIdentifier = matResults.Select(m => m.patientIdentifer).FirstOrDefault();

                return new List<dynamic> { results, patientIdentifier };
            }

            return null;
        }
    }
}
