using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mycroes.Expressions
{
    public static class Lambda
    {
        /// <summary>
        /// Return the body of the given lambda expression as the requested
        /// expression type.
        /// </summary>
        /// <typeparam name="T">The expected body expression type.</typeparam>
        /// <param name="expression">Lambda expression to extract the body from.</param>
        /// <returns>The body of the input expression.</returns>
        /// 
        /// <exception cref="ArgumentException">
        /// The body of <paramref name="expression"/> is not an instance of <typeparamref name="T"/>.
        /// </exception>
        public static T ExtractBody<T>(this LambdaExpression expression) where T : Expression =>
            expression.Body as T ?? throw expression.BodyShouldBeOfType(nameof(expression), typeof(T));

        /// <summary>
        /// Return the body of the given lambda expression as the requested
        /// expression type.
        /// </summary>
        /// <typeparam name="T">The expected body expression type.</typeparam>
        /// <param name="expression">Lambda expression to extract the body from.</param>
        /// <returns>The body of the input expression.</returns>
        /// 
        /// <exception cref="ArgumentException">
        /// The body of <paramref name="expression"/> is not an instance of <typeparamref name="T"/>.
        /// </exception>
        public static T ExtractBody<T>(Expression<Func<object>> expression) where T : Expression =>
            expression.ExtractBody<T>();

        /// <summary>
        /// Return the body of the given lambda expression as the requested
        /// expression type.
        /// </summary>
        /// <typeparam name="T">The expected body expression type.</typeparam>
        /// <param name="expression">Lambda expression to extract the body from.</param>
        /// <returns>The body of the input expression.</returns>
        /// 
        /// <exception cref="ArgumentException">
        /// The body of <paramref name="expression"/> is not an instance of <typeparamref name="T"/>.
        /// </exception>
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