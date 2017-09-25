using System.Linq.Expressions;

namespace Mycroes.Expressions
{
    public static class Extensions
    {
        public static T Evaluate<T>(this Expression expression) => ExpressionUtils.EvaluateExpression<T>(expression);
    }
}