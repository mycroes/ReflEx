using System;
using System.Text;

namespace ReflEx.Types;

/// <summary>
/// Provides methods for type name serialization.
/// </summary>
public static class TypeNameSerializer
{
    /// <summary>
    /// Serialize a type name.
    /// <para>
    /// Serializes types in <c>System</c> namespace using <c><see cref="Type.FullName"/></c> and
    /// other types to <c>$"{type.FullName}, {type.Assembly.GetName().Name}"</c>. This
    /// avoids type loading issues when switching between runtimes or target frameworks and avoids
    /// matching the assembly for non-System types to the assembly version.
    /// </para>
    /// </summary>
    /// <param name="type">The <see cref="Type"/> for which to serialize the name.</param>
    /// <returns>The serialized type name.</returns>
    public static string Serialize(Type type)
    {
        var sb = new StringBuilder();
        Serialize(type, sb);

        return sb.ToString();
    }

    private static void Serialize(Type type, StringBuilder sb)
    {
        var offset = sb.Length;
        var p = type.DeclaringType;
        while (p != null)
        {
            sb.Insert(offset, '+');
            sb.Insert(offset, p.Name);
            p = p.DeclaringType;
        }

        sb.Insert(offset, '.');
        sb.Insert(offset, type.Namespace);
        sb.Append(type.Name);

        var args = type.GetGenericArguments();
        if (args.Length > 0)
        {
            sb.Append('[');
            foreach (var arg in type.GetGenericArguments())
            {
                sb.Append('[');
                Serialize(arg, sb);
                sb.Append("],");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(']');
        }

        if (type.Assembly == typeof(int).Assembly) return;

        sb.Append(", ");
        sb.Append(type.Assembly.GetName().Name);
    }
}