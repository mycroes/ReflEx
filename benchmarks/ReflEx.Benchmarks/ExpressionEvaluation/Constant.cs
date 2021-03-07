using System;
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;

namespace ReflEx.Benchmarks.ExpressionEvaluation
{
    public class Constant
    {
        private readonly Expression expression = Expression.Constant(1);

        [Benchmark(Baseline = true)]
        public int CompileValue()
        {
            var lambda = Expression.Lambda<Func<int>>(expression);
            var func = lambda.Compile();

            return func.Invoke();
        }

        [Benchmark]
        public object CompileObject()
        {
            var typed = Expression.TypeAs(expression, typeof(object));
            var lambda = Expression.Lambda<Func<object>>(typed);
            var func = lambda.Compile();

            return func.Invoke();
        }

        [Benchmark]
        public int AccessValue()
        {
            if (expression is ConstantExpression ce)
            {
                return (int) ce.Value;
            }

            throw new Exception();
        }

        [Benchmark]
        public object AccessObject()
        {
            if (expression is ConstantExpression ce)
            {
                return ce.Value;
            }

            throw new Exception();
        }
    }
}
