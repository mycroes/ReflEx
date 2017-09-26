using System.Linq.Expressions;

namespace Mycroes.Expressions
{
    public class ExpressionReplacementVisitor : ExpressionVisitor
    {
        private readonly Expression oldValue;
        private readonly Expression newValue;

        private ExpressionReplacementVisitor(Expression oldValue, Expression newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            return node == oldValue ? newValue : base.Visit(node);
        }

        public static Expression Replace(Expression expression, Expression oldValue, Expression newValue)
        {
            var visitor = new ExpressionReplacementVisitor(oldValue, newValue);

            return visitor.Visit(expression);
        }
    }
}