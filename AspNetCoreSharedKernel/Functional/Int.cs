using static AspNetCoreSharedKernel.Functional.F;

namespace AspNetCoreSharedKernel.Functional;

internal static class Int
{
    internal static Option<int> Parse(string s)
    {
        return int.TryParse(s, out int result) ? Some(result) : None;
    }

    internal static bool IsOdd(int i) => i % 2 == 1;

    internal static bool IsEven(int i) => i % 2 == 0;
}
