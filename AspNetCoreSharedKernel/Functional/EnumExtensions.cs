using static AspNetCoreSharedKernel.Functional.F;

namespace AspNetCoreSharedKernel.Functional;

internal static class EnumExtensions
{
    internal static Option<T> Parse<T>(this string s) where T : struct
        => Enum.TryParse(s, out T t) ? Some(t) : None;
}
