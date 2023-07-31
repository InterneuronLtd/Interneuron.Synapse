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


using Interneuron.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Interneuron.CareRecord.Infrastructure.Search
{
    public class GenericSearchOpProcessor
    {
        public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, List<SynapseSearchTerm> terms)
        {
            if (!terms.Any()) return query;

            var modifiedQuery = query;

            foreach (var term in terms)
            {
                if (term.Name.Contains("."))
                {
                    var termNames = term.Name.Split(".");

                    ParameterExpression pe = Expression.Parameter(typeof(TEntity), "entity");

                    MemberExpression left = Expression.Property(pe, termNames[0]);

                    for (int typeIndex = 1; typeIndex < termNames.Length; typeIndex++)
                    {
                        left = Expression.Property(left, termNames[typeIndex]);
                    }

                    // "Value"
                    var right = term.ExpressionProvider.GetValue(term.Value);

                    // x.Property == "Value"
                    var comparisonExpression = term.ExpressionProvider.GetComparison(left, term.Op, right);

                    // x => x.Property == "Value"
                    var lambdaExpression = ExpressionHelper.GetLambda<TEntity, bool>(pe, comparisonExpression);

                    // query = query.Where...
                    modifiedQuery = ExpressionHelper.CallWhere(modifiedQuery, lambdaExpression);
                }
                else
                {
                    var propertyInfo = ExpressionHelper.GetPropertyInfo<TEntity>(term.Name);

                    var obj = ExpressionHelper.Parameter<TEntity>();

                    // x.Property
                    var left = ExpressionHelper.GetPropertyExpression(obj, propertyInfo);
                    // "Value"
                    var right = term.ExpressionProvider.GetValue(term.Value);

                    // x.Property == "Value"
                    var comparisonExpression = term.ExpressionProvider.GetComparison(left, term.Op, right);

                    // x => x.Property == "Value"
                    var lambdaExpression = ExpressionHelper.GetLambda<TEntity, bool>(obj, comparisonExpression);

                    // query = query.Where...
                    modifiedQuery = ExpressionHelper.CallWhere(modifiedQuery, lambdaExpression);
                }
            }

            return modifiedQuery;
        }
    }
}
