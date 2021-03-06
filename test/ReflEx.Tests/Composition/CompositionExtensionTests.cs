using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReflEx.Composition;
using Shouldly;
using Xunit;

namespace ReflEx.Tests.Composition
{
    public static class CompositionExtensionTests
    {
        public class ComposeQueryTests
        {
            [Fact]
            public void Should_Use_Func_Placeholder_Expression_On_ComposeQuery()
            {
                var filter =
                    Placeholder.Create((IQueryable<int> x) => x.Where(i => i > 2).Where(i => i < 5));

                var l = new List<int>
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6
                };

                var res = l.AsQueryable().SelectMany(i => filter.Invoke(l.AsQueryable()).Select(j => new {i, j}))
                    .ComposeQuery().ToList();

                res.Count.ShouldBe(l.Count * 2);
            }

            [Fact]
            public void Should_Use_Value_Placeholder_Expression_On_ComposeQuery()
            {
                var range =
                    Placeholder.Create(() => Enumerable.Range(1, 3));

                var l = new List<int>
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6
                };

                var res = l.AsQueryable().SelectMany(i => range.Value.Select(j => new {i, j}))
                    .ComposeQuery().ToList();

                res.Count.ShouldBe(l.Count * 3);
            }
        }

        public class ComposeExpressionTests
        {
            [Fact]
            public void Should_Use_Func_Placeholder_Expression_On_ComposeExpression()
            {
                var add = Placeholder.Create((int x) => x + 1);
                Expression<Func<int>> expr = () => add.Invoke(1);

                expr.ComposeExpression().Compile().Invoke().ShouldBe(2);
            }

            [Fact]
            public void Should_Use_Value_Placeholder_Expression_On_ComposeExpression()
            {
                var calculated = Placeholder.Create(() => 5);
                Expression<Func<int>> expr = () => 3 + calculated.Value;

                expr.ComposeExpression().Compile().Invoke().ShouldBe(8);
            }
        }
    }
}
