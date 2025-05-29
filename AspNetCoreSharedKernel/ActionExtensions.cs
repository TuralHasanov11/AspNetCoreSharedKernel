using Unit = System.ValueTuple;

namespace AspNetCoreSharedKernel;

public static class ActionExtensions
{
    public static Func<Unit> ToFunc(this Action action)
    {
        return () =>
        {
            action();

            return default;
        };
    }

    public static Func<T, Unit> ToFunc<T>(this Action<T> action)
    {
        return (x) =>
        {
            action(x);

            return default;
        };
    }
}
