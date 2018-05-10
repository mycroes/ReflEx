using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mycroes.Expressions
{
    public static class Lambda
    {
        public static T ExtractBody<T>(this LambdaExpression expression) where T : Expression =>
            expression.Body as T ?? throw expression.BodyShouldBeOfType(nameof(expression), typeof(T));

        public static T ExtractBody<T>(Expression<Func<object>> expression) where T : Expression =>
            expression.ExtractBody<T>();

        public static T ExtractBody<T>(Expression<Action> expression) where T : Expression =>
            expression.ExtractBody<T>();

        internal static ArgumentException BodyShouldBeOfType(this LambdaExpression expression, string paramName,
            params Type[] allowedTypes)
        {
            switch (allowedTypes.Length)
            {
                case 0:
                    throw new ArgumentException("At least one type needs to be allowed.", nameof(allowedTypes));
                case 1:
                    return expression.BodyShouldBeOfType(paramName, allowedTypes[0]);
                default:
                    var sb = new StringBuilder();
                    foreach (var type in allowedTypes.Take(allowedTypes.Length - 2))
                    {
                        sb.Append(type.Name).Append(", ");
                    }

                    sb.Append(allowedTypes[allowedTypes.Length - 2]).Append(" or ")
                        .Append(allowedTypes[allowedTypes.Length - 1]);

                    return expression.BodyShouldBeOfTypeImpl(paramName, sb.ToString());
            }
        }

        internal static ArgumentException BodyShouldBeOfType(this LambdaExpression expression, string paramName,
            Type allowedType)
        {
            return expression.BodyShouldBeOfTypeImpl(paramName, allowedType.Name);
        }

        private static ArgumentException BodyShouldBeOfTypeImpl(this LambdaExpression expression, string paramName,
            string allowedTypesString)
        {
            return new ArgumentException(
                $"Expression body needs to be of type {allowedTypesString}, but is of type {expression.Body.GetType().Name}.",
                paramName);
        }
    }
}