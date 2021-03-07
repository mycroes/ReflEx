using System;
using System.Linq.Expressions;

namespace ReflEx.Composition
{
    public static class Placeholder
    {
        public static IPlaceholder<T> Create<T>(Expression<Func<T>> expression) => new Placeholder<T>(expression);

        public static IPlaceholder<TIn, TOut> Create<TIn, TOut>(Expression<Func<TIn, TOut>> expression) =>
            new Placeholder<TIn, TOut>(expression);
    }

    internal class Placeholder<T> : IPlaceholder<T>, ILambdaPlaceholder
    {
        public LambdaExpression Expression { get; }

        public Placeholder(Expression<Func<T>> expression)
        {
            Expression = expression;
        }

        public T Value => throw new NotImplementedException();
    }

    internal class Placeholder<TIn, TOut> : IPlaceholder<TIn, TOut>, ILambdaPlaceholder
    {
        public LambdaExpression Expression { get; }

        public Placeholder(Expression<Func<TIn, TOut>> expression)
        {
            Expression = expression;
        }

        public TOut Invoke(TIn @in) => throw new NotImplementedException();
    }
}
