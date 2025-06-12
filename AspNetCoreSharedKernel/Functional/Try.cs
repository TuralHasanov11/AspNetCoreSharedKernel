namespace AspNetCoreSharedKernel.Functional;

internal delegate Exceptional<T> Try<T>();

internal static partial class F
{
    internal static Try<T> Try<T>(Func<T> f) => () => f();
}

internal static class TryExt
{
    internal static Exceptional<T> Run<T>(this Try<T> @try)
    {
        try { return @try(); }
        catch (Exception ex) { return ex; }
    }

    internal static Try<R> Map<T, R>
       (this Try<T> @try, Func<T, R> f)
       => ()
       => @try.Run()
             .Match<Exceptional<R>>(
                ex => ex,
                t => f(t));

    internal static Try<Func<T2, R>> Map<T1, T2, R>
       (this Try<T1> @try, Func<T1, T2, R> func)
       => @try.Map(func.Curry());

    internal static Try<R> Bind<T, R>
       (this Try<T> @try, Func<T, Try<R>> f)
       => ()
       => @try.Run().Match(
             Exception: ex => ex,
             Success: t => f(t).Run());

    // LINQ

    internal static Try<R> Select<T, R>(this Try<T> @this, Func<T, R> func) => @this.Map(func);

    internal static Try<RR> SelectMany<T, R, RR>
       (this Try<T> @try, Func<T, Try<R>> bind, Func<T, R, RR> project)
       => ()
       => @try.Run().Match(
             ex => ex,
             t => bind(t).Run()
                      .Match<Exceptional<RR>>(
                         ex => ex,
                         r => project(t, r))
                      );
}
