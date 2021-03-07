using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ReflEx.Evaluation
{
    public static class ExpressionEvaluator
    {
        public static bool TryEvaluate(Expression expression, out object value)
        {
            switch (expression)
            {
                case DefaultExpression de:
                    value = de.Type.IsValueType ? Activator.CreateInstance(de.Type) : null;
                    return true;
                case ConstantExpression ce:
                    value = ce.Value;
                    return true;
                case MemberExpression me:
                    object instance = null;
                    if (me.Expression != null && !TryEvaluate(me.Expression, out instance)) break;

                    switch (me.Member)
                    {
                        case FieldInfo fi:
                            value = fi.GetValue(instance);
                            return true;
                        case PropertyInfo pi:
                            value = pi.GetValue(instance);
                            return true;
                    }

                    break;
            }

            value = null;
            return false;
        }
    }
}
