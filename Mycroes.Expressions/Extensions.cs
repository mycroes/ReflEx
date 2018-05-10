using System;
using System.Linq.Expressions;

namespace Mycroes.Expressions
{
    public static class Extensions
    {
        public static T Evaluate<T>(this Expression expression) => ExpressionUtils.EvaluateExpression<T>(expression);

        public static object Evaluate(this Expression expression) => ExpressionUtils.EvaluateExpression(expression);

        public static Expression Replace(this Expression expression, Expression oldValue,
            Expression newValue) => ExpressionReplacementVisitor.Replace(expression, oldValue, newValue);

        public static Expression<Func<TInNew, TOut>> Translate<TInOld, TInNew, TOut>(
            this Expression<Func<TInOld, TOut>> expression,
            Expression<Func<TInNew, TInOld>> translation) => ExpressionUtils.TranslateExpression(expression,
            translation);
    }
}