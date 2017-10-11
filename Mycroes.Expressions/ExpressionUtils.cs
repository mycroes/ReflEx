using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Mycroes.Expressions
{
    public static class ExpressionUtils
    {
        public static T EvaluateExpression<T>(Expression expression)
        {
            expression = MakeAssignableExpression<T>(expression);
            var lambda = Expression.Lambda<Func<T>>(expression);
            return lambda.Compile().Invoke();
        }

        public static Expression MakeAssignableExpression<T>(Expression expression)
        {
            if (typeof(T) == expression.Type) return expression;

            if (!typeof(T).IsAssignableFrom(expression.Type))
                throw new ArgumentException(
                    $"Expression return type {expression.Type} is not assignable to {typeof(T)}.");

            if (Nullable.GetUnderlyingType(typeof(T)) == expression.Type)
            {
                var ctor = typeof(T).GetConstructor(new[] {expression.Type}) ??
                    throw new Exception(
                        $"Missing single argument constructor with parameter of type {expression.Type} on type {typeof(T)}");
                expression = Expression.New(ctor, expression);
            }
            else
            {
                expression = Expression.Convert(expression, typeof(T));
            }

            return expression;
        }

        public static Expression<Func<TInNew, TOut>> TranslateExpression<TInOld, TInNew, TOut>(
            Expression<Func<TInOld, TOut>> original, Expression<Func<TInNew, TInOld>> translation)
        {
            return Expression.Lambda<Func<TInNew, TOut>>(
                original.Body.Replace(original.Parameters[0], translation.Body), translation.Parameters[0]);
        }
    }
}
