using System;
using System.Collections.Generic;
using System.Linq;
using ReflEx.Composition;
using Shouldly;
using Xunit;

namespace ReflEx.Tests.Composition
{
    public static class PlaceholderTests
    {
        public class InvokeTests
        {
            [Fact]
            public void Should_Throw_On_Func_Placeholder_Invocation()
            {
                var filter =
                    Placeholder.Create<IQueryable<int>, IQueryable<int>>(x => x.Where(i => i > 2).Where(i => i < 5));

                var l = new List<int>
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6
                };

                Should.Throw<NotImplementedException>(() =>
                    l.AsQueryable().SelectMany(i => filter.Invoke(l.AsQueryable()).Select(j => new {i, j})).ToList());
            }

            [Fact]
            public void Should_Throw_On_Value_Placeholder_Value_Access()
            {
                var placeholder = Placeholder.Create(() => 5);
                Should.Throw<NotImplementedException>(() => placeholder.Value);
            }
        }
    }
}