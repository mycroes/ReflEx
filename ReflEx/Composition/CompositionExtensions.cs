using System.Linq;
using System.Linq.Expressions;

namespace ReflEx.Composition
{
    public static class CompositionExtensions
    {
        private static readonly ExpressionVisitor Visitor = new CompositionVisitor();

        public static T ComposeQuery<T>(this T queryable) where T : IQueryable
        {
            return (T) queryable.Provider.CreateQuery(Visitor.VisitAndConvert(queryable.Expression, nameof(ComposeQuery))!);
        }

        public static T ComposeExpression<T>(this T expression) where T : Expression
        {
            return Visitor.VisitAndConvert(expression, nameof(ComposeExpression));
        }
    }
}