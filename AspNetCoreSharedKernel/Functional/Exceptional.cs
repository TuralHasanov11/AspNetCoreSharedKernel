using static AspNetCoreSharedKernel.Functional.F;
using Unit = System.ValueTuple;

namespace AspNetCoreSharedKernel.Functional;

internal static partial class F
{
    internal static Exceptional<T> Exceptional<T>(T value) => new Exceptional<T>(value);
}

internal readonly struct Exceptional<T>
{
    internal Exception Ex { get; }
    internal T Value { get; }

    internal bool Success => Ex == null;
    internal bool Exception => Ex != null;

    internal Exceptional(Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);

        Ex = ex;
        Value = default;
    }

    internal Exceptional(T right)
    {
        Value = right;
        Ex = null;
    }

    public static implicit operator Exceptional<T>(Exception left) => new Exceptional<T>(left);
    public static implicit operator Exceptional<T>(T right) => new Exceptional<T>(right);

    internal TR Match<TR>(Func<Exception, TR> Exception, Func<T, TR> Success)
       => this.Exception ? Exception(Ex) : Success(Value);

    internal Unit Match(Action<Exception> Exception, Action<T> Success)
       => Match(Exception.ToFunc(), Success.ToFunc());

    public override string ToString()
       => Match(
          ex => $"Exception({ex.Message})",
          t => $"Success({t})");
}

internal static class Exceptional
{
    // creating a new Exceptional

    internal static Func<T, Exceptional<T>> Return<T>()
       => t => t;

    internal static Exceptional<R> Of<R>(Exception left)
       => new Exceptional<R>(left);

    internal static Exceptional<R> Of<R>(R right)
       => new Exceptional<R>(right);

    // applicative

    internal static Exceptional<R> Apply<T, R>(this Exceptional<Func<T, R>> @this, Exceptional<T> arg)
      => @this.Match(
          Exception: ex => ex,
          Success: func => arg.Match(
             Exception: ex => ex,
             Success: t => new Exceptional<R>(func(t))));

    internal static Exceptional<Func<T2, R>> Apply<T1, T2, R>
       (this Exceptional<Func<T1, T2, R>> @this, Exceptional<T1> arg)
       => Apply(@this.Map(F.Curry), arg);

    internal static Exceptional<Func<T2, T3, R>> Apply<T1, T2, T3, R>
       (this Exceptional<Func<T1, T2, T3, R>> @this, Exceptional<T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    internal static Exceptional<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
       (this Exceptional<Func<T1, T2, T3, T4, R>> @this, Exceptional<T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    internal static Exceptional<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
       (this Exceptional<Func<T1, T2, T3, T4, T5, R>> @this, Exceptional<T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    internal static Exceptional<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
       (this Exceptional<Func<T1, T2, T3, T4, T5, T6, R>> @this, Exceptional<T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    internal static Exceptional<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
       (this Exceptional<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Exceptional<T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    internal static Exceptional<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
       (this Exceptional<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Exceptional<T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    internal static Exceptional<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
       (this Exceptional<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Exceptional<T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    // functor

    internal static Exceptional<RR> Map<R, RR>(this Exceptional<R> @this, Func<R, RR> func)
        => @this.Success ? func(@this.Value) : new Exceptional<RR>(@this.Ex);

    internal static Exceptional<Unit> ForEach<R>(this Exceptional<R> @this, Action<R> act)
       => Map(@this, act.ToFunc());

    internal static Exceptional<RR> Bind<R, RR>(this Exceptional<R> @this, Func<R, Exceptional<RR>> func)
        => @this.Success ? func(@this.Value) : new Exceptional<RR>(@this.Ex);

    // LINQ

    internal static Exceptional<R> Select<T, R>(this Exceptional<T> @this, Func<T, R> map) => @this.Map(map);

    internal static Exceptional<RR> SelectMany<T, R, RR>(
        this Exceptional<T> @this,
        Func<T, Exceptional<R>> bind, Func<T, R, RR> project)
    {
        if (@this.Exception)
        {
            return new Exceptional<RR>(@this.Ex);
        }

        var bound = bind(@this.Value);
        return bound.Exception
           ? new Exceptional<RR>(bound.Ex)
           : project(@this.Value, bound.Value);
    }
}
