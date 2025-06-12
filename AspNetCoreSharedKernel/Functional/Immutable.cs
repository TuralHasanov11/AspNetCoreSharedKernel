using System.Linq.Expressions;
using System.Reflection;

namespace AspNetCoreSharedKernel.Functional;

internal static class Immutable
{
    internal static T With<T>(this T source, string propertyName, object newValue)
       where T : class
    {
        T @new = source.ShallowCopy();

        typeof(T).GetBackingField(propertyName).SetValue(@new, newValue);

        return @new;
    }

    internal static T With<T, P>(this T source, Expression<Func<T, P>> exp, object newValue)
       where T : class
       => source.With(exp.MemberName(), newValue);

    private static string MemberName<T, P>(this Expression<Func<T, P>> e)
    {
        return ((MemberExpression)e.Body).Member.Name;
    }

    private static T ShallowCopy<T>(this T source)
    {
        return (T)source.GetType()
            .GetTypeInfo()
            .GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)
            .Invoke(source, null);
    }

    private static string BackingFieldName(string propertyName)
    {
        return string.Format("<{0}>k__BackingField", propertyName);
    }

    private static FieldInfo GetBackingField(this Type t, string propertyName)
    {
        return t.GetTypeInfo()
            .GetField(BackingFieldName(propertyName), BindingFlags.Instance | BindingFlags.NonPublic);
    }
}
