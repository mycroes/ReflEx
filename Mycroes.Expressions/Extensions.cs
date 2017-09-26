using System.Linq.Expressions;

namespace Mycroes.Expressions
{
    public static class Extensions
    {
        public static T Evaluate<T>(this Expression expression) => ExpressionUtils.EvaluateExpression<T>(expression);

        public static Expression Replace(this Expression expression, Expression oldValue,
            Expression newValue) => ExpressionReplacementVisitor.Replace(expression, oldValue, newValue);
    }
}