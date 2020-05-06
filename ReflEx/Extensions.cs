using System;
using System.Linq.Expressions;

namespace ReflEx
{
    public static class Extensions
    {
        /// <summary>
        /// Evaluates the given expression to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to evaluate to.</typeparam>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The evaluated expression.</returns>
        ///
        /// <remarks>
        /// <see cref="ExpressionUtils.MakeAssignableExpression{T}" /> is used to match
        /// <paramref name="expression"/> to type <typeparamref name="T"/>.
        /// </remarks>
        public static T Evaluate<T>(this Expression expression) => ExpressionUtils.EvaluateExpression<T>(expression);

        /// <summary>
        /// Evaluates the given expression to an object.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The evaluated expression.</returns>
        public static object Evaluate(this Expression expression) => ExpressionUtils.EvaluateExpression(expression);

        /// <summary>
        /// Returns a new expression in which all occurrences of a specified expression
        /// in the input expression are replaced with another specified expression.
        /// </summary>
        /// <param name="expression">The input expression.</param>
        /// <param name="oldValue">The expression to be replaced.</param>
        /// <param name="newValue">The expression to replace all occurrences of <paramref name="oldValue"/>.</param>
        /// <returns></returns>
        public static Expression Replace(this Expression expression, Expression oldValue,
            Expression newValue) => ExpressionReplacementVisitor.Replace(expression, oldValue, newValue);

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
        public static Expression<Func<TInNew, TOut>> Translate<TInOld, TInNew, TOut>(
            this Expression<Func<TInOld, TOut>> original,
            Expression<Func<TInNew, TInOld>> translation) => ExpressionUtils.TranslateExpression(original,
            translation);

        /// <summary>
        /// Create a <see cref="T:System.Linq.Expressions.Expression`1"/> that applies
        /// <paramref name="translation"/> on <paramref name="original"/>. This is similar
        /// to how <see cref="T:Enumerable.Select"/> applies to collections, but then applied to
        /// a single instance to allow composition of expressions.
        /// </summary>
        /// <typeparam name="TIn">The input type of the original expression.</typeparam>
        /// <typeparam name="TOutOld">The output type of the original expression.</typeparam>
        /// <typeparam name="TOutNew">The output type of the returned expression.</typeparam>
        /// <param name="original">The expression to translate.</param>
        /// <param name="translation">The translation to apply to the output of the original expression.</param>
        /// <returns>
        /// A <see cref="T:System.Linq.Expressions.Expression`1"/> that takes <typeparamref name="TIn"/>
        /// as input and returns <typeparamref name="TOutNew"/>.
        /// </returns>
        public static Expression<Func<TIn, TOutNew>> Pick<TIn, TOutOld, TOutNew>(
            this Expression<Func<TIn, TOutOld>> original,
            Expression<Func<TOutOld, TOutNew>> translation) => ExpressionUtils.TranslateExpression(translation, original);
    }
}