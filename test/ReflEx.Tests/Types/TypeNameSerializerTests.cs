using ReflEx.Types;
using Shouldly;
using Xunit;

namespace ReflEx.Tests.Types
{
    public class TypeNameSerializerTests
    {
        [Fact]
        public void Should_Serialize_System_Int_Without_Assembly()
        {
            TypeNameSerializer.Serialize(typeof(int)).ShouldBe("System.Int32");
        }

        [Fact]
        public void Should_Serialize_Non_System_Type_With_Assembly()
        {
            TypeNameSerializer.Serialize(typeof(TypeNameSerializer)).ShouldBe($"{typeof(TypeNameSerializer).Namespace}.{nameof(TypeNameSerializer)}, {typeof(TypeNameSerializer).Assembly.GetName().Name}");
        }

        [Fact]
        public void Should_Serialize_Nested_Type()
        {
            TypeNameSerializer.Serialize(typeof(Nested)).ShouldBe(
                $"{GetType().Namespace}.{GetType().Name}+{nameof(Nested)}, {GetType().Assembly.GetName().Name}");
        }

        [Fact]
        public void Should_Serialize_NestedGeneric_Type()
        {
            TypeNameSerializer.Serialize(typeof(NestedGeneric<int>)).ShouldBe(
                $"{GetType().Namespace}.{GetType().Name}+{nameof(NestedGeneric<int>)}`1[[System.Int32]], {GetType().Assembly.GetName().Name}");
        }

        [Fact]
        public void Should_Serialize_NestedGeneric_NestedNested_Type()
        {
            TypeNameSerializer.Serialize(typeof(NestedGeneric<int>.NestedNested)).ShouldBe(
                $"{GetType().Namespace}.{GetType().Name}+{nameof(NestedGeneric<int>)}`1+{nameof(NestedGeneric<int>.NestedNested)}[[System.Int32]], {GetType().Assembly.GetName().Name}");
        }

        [Fact]
        public void Should_Serialize_NestedGeneric_NestedNestedGeneric_Type()
        {
            TypeNameSerializer.Serialize(typeof(NestedGeneric<int>.NestedNestedGeneric<string>)).ShouldBe(
                $"{GetType().Namespace}.{GetType().Name}+{nameof(NestedGeneric<int>)}`1+{nameof(NestedGeneric<int>.NestedNestedGeneric<string>)}`1[[System.Int32],[System.String]], {GetType().Assembly.GetName().Name}");
        }

        private class Nested
        {
        }

        private class NestedGeneric<TNestedGeneric>
        {
            public class NestedNested
            {
            }

            public class NestedNestedGeneric<TNestedNestedGeneric>
            {
            }
        }
    }
}
