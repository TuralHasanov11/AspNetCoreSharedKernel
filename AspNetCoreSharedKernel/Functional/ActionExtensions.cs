using static AspNetCoreSharedKernel.Functional.F;
using Unit = System.ValueTuple;

namespace AspNetCoreSharedKernel.Functional;

public static class ActionExtensions
{
    public static Func<Unit> ToFunc(this Action action)
    {
        return () =>
        {
            action();
            return Unit();
        };
    }

    public static Func<T, Unit> ToFunc<T>(this Action<T> action)
    {
        return t =>
        {
            action(t);
            return Unit();
        };
    }

    public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> action)
    {
        return (T1 t1, T2 t2) => { action(t1, t2); return new(); };
    }
}
