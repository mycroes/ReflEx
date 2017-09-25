using System;
using System.Linq.Expressions;

namespace Mycroes.Expressions
{
    public static class ExpressionUtils
    {
        public static T EvaluateExpression<T>(Expression expression)
        {
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
            else if (typeof(T) == typeof(object) && expression.Type.IsValueType)
            {
                expression = Expression.Convert(expression, typeof(T));
            }

            var lambda = Expression.Lambda<Func<T>>(expression);
            return lambda.Compile().Invoke();
        }
    }
}
