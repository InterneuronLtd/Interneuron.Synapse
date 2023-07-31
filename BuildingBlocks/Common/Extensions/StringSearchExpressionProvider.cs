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
using System.Reflection;

namespace Interneuron.Common.Extensions
{
    public class StringSearchExpressionProvider : DefaultSearchExpressionProvider
    {
        public const string StartsWithOperator = "sw";
        public const string ContainsOperator = "co";

        private static readonly MethodInfo StartsWithMethod = typeof(string)
            .GetMethods()
            .First(m => m.Name == "StartsWith" && m.GetParameters().Length == 2);

        private static readonly MethodInfo StringEqualsMethod = typeof(string)
            .GetMethods()
            .First(m => m.Name == "Equals" && m.GetParameters().Length == 2);

        private static readonly MethodInfo ContainsMethod = typeof(string)
            .GetMethods()
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 1);

        private static readonly ConstantExpression IgnoreCase = Expression.Constant(StringComparison.InvariantCultureIgnoreCase);//Expression.Constant(StringComparison.OrdinalIgnoreCase);

        public override IEnumerable<string> GetOperators()
            => base.GetOperators()
            .Concat(new[]
            {
                StartsWithOperator,
                ContainsOperator
            });

        public override Expression GetComparison(MemberExpression left, string op, ConstantExpression right)
        {
            Expression newLeft = Expression.Call(left, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));  
            Expression newRight = Expression.Call(right, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));  

            switch (op.ToLower())
            {
                case StartsWithOperator:
                    return Expression.Call(left, StartsWithMethod, right, IgnoreCase);

                // TODO: This may or may not be case-insensitive, depending
                // on how the database translates Contains()
                case ContainsOperator:
                    //return Expression.Call(left, ContainsMethod, right);
                    return Expression.Call(newLeft, ContainsMethod, newRight);

                // Handle the "eq" operator ourselves (with a case-insensitive compare)
                case EqualsOperator:
                    //return Expression.Call(newLeft, StringEqualsMethod, newRight, IgnoreCase);
                    return Expression.Equal(newLeft, newRight);

                default: return base.GetComparison(left, op, right);
            }
        }
    }
}
