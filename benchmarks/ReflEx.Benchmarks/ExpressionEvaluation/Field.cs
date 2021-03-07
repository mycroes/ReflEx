using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace ReflEx.Benchmarks.ExpressionEvaluation
{
    public class Field
    {
        private readonly Expression expression;

        public Field()
        {
            var value = 1;
            Expression<Func<int>> lambda = () => value;
            expression = lambda.Body;
        }

        [Benchmark(Baseline = true)]
        public int CompileTyped()
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
        public int AccessTyped()
        {
            if (expression is MemberExpression me && me.Member is FieldInfo fi &&
                me.Expression is ConstantExpression ce)
            {
                return (int) fi.GetValue(ce.Value);
            }

            throw new Exception();
        }

        [Benchmark]
        public object AccessObject()
        {
            if (expression is MemberExpression me && me.Member is FieldInfo fi &&
                me.Expression is ConstantExpression ce)
            {
                return fi.GetValue(ce.Value);
            }

            throw new Exception();
        }
    }
}
