using System;
using System.Linq.Expressions;

namespace ReflEx
{
    public static class ExpressionUtils
    {
        /// <summary>
        /// Evaluates the given expression to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to evaluate to.</typeparam>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The evaluated expression.</returns>
        /// 
        /// <remarks>
        /// <see cref="MakeAssignableExpression{T}" /> is used to match
        /// <paramref name="expression"/> to type <typeparamref name="T"/>.
        /// </remarks>
        public static T EvaluateExpression<T>(Expression expression)
        {
            expression = MakeAssignableExpression<T>(expression);
            var lambda = Expression.Lambda<Func<T>>(expression);
            return lambda.Compile().Invoke();
        }

        /// <summary>
        /// Evaluates the given expression to an object.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The evaluated expression.</returns>
        public static object EvaluateExpression(Expression expression)
        {
            if (expression.Type != typeof(object)) expression = Expression.TypeAs(expression, typeof(object));

            var lambda = Expression.Lambda<Func<object>>(expression);
            return lambda.Compile().Invoke();
        }

        /// <summary>
        /// Creates an expression with Type <typeparamref name="T"/> from <paramref name="expression"/>.
        /// </summary>
        /// <typeparam name="T">The requested type.</typeparam>
        /// <param name="expression">The input expression.</param>
        /// <returns>An expression that converts <paramref name="expression"/> to <typeparamref name="T"/>.</returns>
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

        /// <summary>
        /// Create a <see cref="T:System.Linq.Expressions.Expression`1"/> that acts on
        /// <typeparamref name="TInNew"/> of <paramref name="translation"/> instead of
        /// <typeparamref name="TInOld"/> of <paramref name="original"/>.
        /// </summary>
        /// <typeparam name="TInOld">The input type of the original expression.</typeparam>
        /// <typeparam name="TInNew">The input type of the translated expression.</typeparam>
        /// <typeparam name="TOut">The output type of <paramref name="original"/> and the returned expression.</typeparam>
        /// <param name="original">The expression to translate.</param>
        /// <param name="translation">The translation to apply to the original expression.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.Expression`1"/> that takes <typeparamref name="TInNew"/> as input.</returns>
        public static Expression<Func<TInNew, TOut>> TranslateExpression<TInOld, TInNew, TOut>(
            Expression<Func<TInOld, TOut>> original, Expression<Func<TInNew, TInOld>> translation)
        {
            return Expression.Lambda<Func<TInNew, TOut>>(
                original.Body.Replace(original.Parameters[0], translation.Body), translation.Parameters[0]);
        }
    }
}
