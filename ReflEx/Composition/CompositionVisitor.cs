using System.Linq.Expressions;

namespace ReflEx.Composition
{
    internal class CompositionVisitor : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression.Type.IsGenericType &&
                node.Expression.Type.GetGenericTypeDefinition() == typeof(IPlaceholder<>) &
                node.Member.Name == nameof(IPlaceholder<object>.Value))
            {
                return Visit(((ILambdaPlaceholder) node.Expression.Evaluate()).Expression.Body)!;
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Object is { } instance && instance.Type.IsGenericType &&
                instance.Type.GetGenericTypeDefinition() == typeof(IPlaceholder<,>) &&
                node.Method.Name == nameof(IPlaceholder<object, object>.Invoke))
            {
                var lambda = ((ILambdaPlaceholder) instance.Evaluate()).Expression;

                return lambda.Body.Replace(Visit(lambda.Parameters[0]), Visit(node.Arguments[0]));
            }

            return base.VisitMethodCall(node);
        }
    }
}