using System;
using System.Linq.Expressions;
using ReflEx.Evaluation;
using Shouldly;
using Xunit;

namespace ReflEx.Tests.Evaluation
{
    public static class ExpressionEvaluatorTests
    {
        public class TryEvaluateTests
        {
            [Fact]
            public void Should_Evaluate_Const()
            {
                Evaluate(() => Helper.ConstOne, out var value).ShouldBeTrue();
                value.ShouldBe(1);
            }

            [Fact]
            public void Should_Evaluate_Captured_Field()
            {
                var number = 1;
                Evaluate(() => number, out var value).ShouldBeTrue();
                value.ShouldBe(1);
            }

            [Fact]
            public void Should_Evaluate_Default_Object()
            {
                ExpressionEvaluator.TryEvaluate(Expression.Default(typeof(object)), out var value).ShouldBeTrue();
                value.ShouldBeNull();
            }

            [Fact]
            public void Should_Evaluate_Default_Int()
            {
                ExpressionEvaluator.TryEvaluate(Expression.Default(typeof(int)), out var value).ShouldBeTrue();
                value.ShouldBe(0);
            }

            [Fact]
            public void Should_Evaluate_Default_Nullable_Int()
            {
                ExpressionEvaluator.TryEvaluate(Expression.Default(typeof(int?)), out var value).ShouldBeTrue();
                value.ShouldBeNull();
            }

            [Fact]
            public void Should_Evaluate_Instance_Field()
            {
                var helper = new Helper();
                Evaluate(() => helper.InstanceFieldOne, out var value).ShouldBeTrue();
                value.ShouldBe(1);
            }

            [Fact]
            public void Should_Evaluate_Instance_Property()
            {
                var helper = new Helper();
                Evaluate(() => helper.InstancePropertyOne, out var value).ShouldBeTrue();
                value.ShouldBe(1);
            }

            [Fact]
            public void Should_Evaluate_Static_Field()
            {
                Evaluate(() => Helper.StaticFieldOne, out var value).ShouldBeTrue();
                value.ShouldBe(1);

            }

            [Fact]
            public void Should_Evaluate_Static_Property()
            {
                Evaluate(() => Helper.StaticPropertyOne, out var value).ShouldBeTrue();
                value.ShouldBe(1);
            }

            [Fact]
            public void Should_Evaluate_Nested_Field()
            {
                var helper = new Helper();
                Evaluate(() => helper.Nested.InstanceFieldOne, out var value).ShouldBeTrue();
                value.ShouldBe(1);
            }

            [Fact]
            public void Should_Evaluate_Nested_Property()
            {
                var helper = new Helper();
                Evaluate(() => helper.Nested.InstancePropertyOne, out var value).ShouldBeTrue();
                value.ShouldBe(1);
            }

            private bool Evaluate<T>(Expression<Func<T>> lambda, out object value)
            {
                return ExpressionEvaluator.TryEvaluate(lambda.Body, out value);
            }
        }

        private class Helper
        {
            public const int ConstOne = 1;

            public int InstanceFieldOne = 1;

            public int InstancePropertyOne => 1;

            public static int StaticFieldOne = 1;

            public static int StaticPropertyOne => 1;

            public Helper Nested { get; }

            public Helper()
            {
                Nested = this;
            }
        }
    }
}
