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
using Interneuron.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.CareRecord.Repository.QueryBuilders
{
    internal class ReportSearchQueryBuilder : QueryBuilder
    {
        public override List<dynamic> Execute(List<SynapseSearchTerm> synapseSearchTerms)
        {
            var baseQuery = (from report in this.dbContext.entitystorematerialised_CoreReport
                             join person in this.dbContext.entitystorematerialised_CorePerson on
                             report.PersonId equals person.PersonId
                             join personIden in this.dbContext.entitystorematerialised_CorePersonidentifier on
                             person.PersonId equals personIden.PersonId
                             join resultOrder in this.dbContext.entitystorematerialised_CoreOrder on
                             report.OrderId equals resultOrder.OrderId into orderTemp
                             from order in orderTemp.DefaultIfEmpty()
                             select new
                             {
                                 reportData = report,
                                 personData = person,
                                 personIdData = personIden,
                                 orderData = order
                             });

            var searchOp = new GenericSearchOpProcessor();

            var withSearchClause = searchOp.Apply(baseQuery, synapseSearchTerms);

            var matResults = withSearchClause
                .OrderByDescending((entity) => entity.orderData.Createdtimestamp)
                .Select(entity => new
                {
                    order = entity.orderData,
                    report = entity.reportData,
                    patientIdentifer = entity.personIdData
                })
                .ToList();

            if (matResults.IsCollectionValid())
            {
                var orders = matResults.Select(m => m.order).Distinct().ToList();
                var reports = matResults.Select(m => m.report).Distinct().ToList();
                var patientIdentifier = matResults.Select(m => m.patientIdentifer).FirstOrDefault();

                return new List<dynamic> { orders, reports, patientIdentifier };
            }

            return null;
        }
    }
}
