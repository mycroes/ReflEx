using System;
using System.Linq.Expressions;
using Shouldly;
using Xunit;
using ReflEx;

namespace ReflEx.Tests
{
    public static class ExtensionsTests
    {
        private static readonly Expression<Func<Parent, Child>> ChildSelector = p => p.Child;
        private static readonly Expression<Func<Child, int>> AgeSelector = c => c.Age;

        public class Pick
        {
            [Fact]
            public void ShouldNotThrow()
            {
                Extensions.Pick(ChildSelector, AgeSelector);
            }

            [Fact]
            public void ShouldNotBeNull()
            {
                Extensions.Pick(ChildSelector, AgeSelector).ShouldNotBeNull();
            }

            [Fact]
            public void ShouldCompile()
            {
                Extensions.Pick(ChildSelector, AgeSelector).Compile();
            }

            [Fact]
            public void ShouldEvaluate()
            {
                Extensions.Pick(ChildSelector, AgeSelector).Compile().Invoke(new Parent { Child = new Child { Age = 1}}).ShouldBe(1);
            }

            [Fact]
            public void ShouldAcceptAnonymousTOutNew()
            {
                var res = Extensions
                    .Pick(ChildSelector, c => new { c.Age, c.Name.Length })
                    .Compile()
                    .Invoke(new Parent { Child = new Child { Age = 1, Name = "Nice child"}});

                res.Age.ShouldBe(1);
                res.Length.ShouldBe("Nice child".Length);
            }
        }

        private class Parent
        {
            public Child Child { get; set; }
        }

        private class Child
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
