using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Mycroes.Expressions
{
    public static class Extract
    {
        public static MethodInfo GenericMethodDefinition(Expression<Action> expression) => MethodImpl(expression)
            .GetGenericMethodDefinition();

        public static MethodInfo GenericMethodDefinition<T>(Expression<Func<T>> expression) => MethodImpl(expression)
            .GetGenericMethodDefinition();

        public static MethodInfo Method(Expression<Action> expression) => MethodImpl(expression);

        public static MethodInfo Method<T>(Expression<Func<T>> expression) => MethodImpl(expression);

        public static PropertyInfo Property<T>(Expression<Func<T>> expression)
        {
            return ExtractImpl(expression, (MemberExpression e) => e.Member as PropertyInfo) ??
                throw BodyIsNot(expression, "property access");
        }

        private static MethodInfo MethodImpl(LambdaExpression expression) => ExtractImpl(expression,
            (MethodCallExpression e) => e.Method);

        private static T ExtractImpl<TExpr, T>(LambdaExpression expression, Func<TExpr, T> convert)
            where TExpr : Expression
        {
            var expr = expression.Body as TExpr ?? throw BodyIsNot(expression, typeof(TExpr).Name);

            return convert(expr);
        }

        private static ArgumentException BodyIsNot(LambdaExpression expression, string what) => new ArgumentException(
            $"Body of {expression} is not a {what}");
    }
}
