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
            var baseQuery = (from result in this.dbContext.entitystorematerialised_CoreResult
                             join person in this.dbContext.entitystorematerialised_CorePerson on
                             result.PersonId equals person.PersonId
                             join personIden in this.dbContext.entitystorematerialised_CorePersonidentifier on
                             person.PersonId equals personIden.PersonId
                             join resultOrder in this.dbContext.entitystorematerialised_CoreOrder on
                             result.OrderId equals resultOrder.OrderId into orderTemp
                             from order in orderTemp.DefaultIfEmpty()
                             join resultNotes in this.dbContext.entitystorematerialised_CoreNote on
                             result.ResultId equals resultNotes.Parentid into notetemp
                             from note in notetemp.DefaultIfEmpty()
                             select new
                             {
                                 orderData = order,
                                 resultData = result,
                                 personData = person,
                                 personIdData = personIden,
                                 noteData = note,
                             });

            var searchOp = new GenericSearchOpProcessor();

            var withSearchClause = searchOp.Apply(baseQuery, synapseSearchTerms);

            var matResults = withSearchClause
                .OrderByDescending((entity) => entity.resultData.Createdtimestamp)
                .Select(entity => new
                {
                    orders = entity.orderData,
                    result = entity.resultData,
                    patientIdentifer = entity.personIdData,
                    notes = entity.noteData,
                })
                .ToList();

            if (matResults.IsCollectionValid())
            {
                var orders = matResults.Select(m => m.orders).Distinct().ToList();
                var results = matResults.Select(m => m.result).Distinct().ToList();
                var patientIdentifier = matResults.Select(m => m.patientIdentifer).FirstOrDefault();
                var notes = matResults.Select(m => m.notes).Distinct().ToList();

                return new List<dynamic> { 
                    orders, 
                    results, 
                    patientIdentifier, 
                    notes
                };
            }

            return null;
        }
    }
}
