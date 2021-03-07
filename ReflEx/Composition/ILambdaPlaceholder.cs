using System.Linq.Expressions;

namespace ReflEx.Composition
{
    internal interface ILambdaPlaceholder
    {
        LambdaExpression Expression { get; }
    }
}