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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Interneuron.Common.Extensions
{
    public class DefaultSearchExpressionProvider : ISearchExpressionProvider
    {
        public const string EqualsOperator = "eq";

        public virtual IEnumerable<string> GetOperators()
        {
            yield return EqualsOperator;
        }

        public virtual Expression GetComparison(
            MemberExpression left, string op, ConstantExpression right)
        {
            switch (op.ToLower())
            {
                case EqualsOperator:
                    //return Expression.Equal(left, right);
                    return NullableHandler(left, right, (l, r) =>
                    {
                        var convertedR = r.Type != l.Type ? (Expression)Expression.Convert(r, l.Type) : (Expression)r;
                        return Expression.Equal(l, convertedR);
                    });
                default: throw new ArgumentException($"Invalid operator '{op}'.");
            }

        }

        public virtual ConstantExpression GetValue(string input)
            => Expression.Constant(input);

        public Expression NullableHandler(Expression e1, Expression e2, Func<Expression, Expression, Expression> op)
        {
            if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
                e2 = Expression.Convert(e2, e1.Type);
            else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
                e1 = Expression.Convert(e1, e2.Type);
            return op?.Invoke(e1, e2);//Expression.GreaterThan(e1, e2);
        }

        public bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
