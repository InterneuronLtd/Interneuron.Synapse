using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Interneuron.Common.Extensions
{
    public class DefaultSearchExpressionProvider : ISearchExpressionProvider
    {
        protected const string EqualsOperator = "eq";

        public virtual IEnumerable<string> GetOperators()
        {
            yield return EqualsOperator;
        }

        public virtual Expression GetComparison(
            MemberExpression left, string op, ConstantExpression right)
        {
            switch (op.ToLower())
            {
                case EqualsOperator: return Expression.Equal(left, right);
                default: throw new ArgumentException($"Invalid operator '{op}'.");
            }

        }

        public virtual ConstantExpression GetValue(string input)
            => Expression.Constant(input);
    }
}
