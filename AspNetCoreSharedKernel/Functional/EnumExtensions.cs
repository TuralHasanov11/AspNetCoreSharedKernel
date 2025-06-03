using static AspNetCoreSharedKernel.Functional.F;

namespace AspNetCoreSharedKernel.Functional;

public static class EnumExtensions
{
    public static Option<T> Parse<T>(this string s) where T : struct
        => Enum.TryParse(s, out T t) ? Some(t) : None;
}
