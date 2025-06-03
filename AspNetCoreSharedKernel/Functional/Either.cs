using static AspNetCoreSharedKernel.Functional.F;
using Unit = System.ValueTuple;

namespace AspNetCoreSharedKernel.Functional;

public static partial class F
{
    public static string Render(Either<string, double> val) => val.Match(
        left: l => $"Invalid value: {l}",
        right: r => $"The result is: {r}");

    public static Either.Left<L> Left<L>(L l) => new(l);

    public static Either.Right<R> Right<R>(R r) => new(r);
}

public readonly struct Either<L, R>
{
    public L Left { get; }

    public R Right { get; }

    private bool IsRight { get; }

    private readonly bool IsLeft => !IsRight;

    public Either(L left)
    {
        ArgumentNullException.ThrowIfNull(left);
        Left = left;
        Right = default;
        IsRight = false;
    }

    public Either(R right)
    {
        ArgumentNullException.ThrowIfNull(right);
        Right = right;
        Left = default;
        IsRight = true;
    }

    public static implicit operator Either<L, R>(L left) => new(left);

    public static implicit operator Either<L, R>(R right) => new(right);

    public static implicit operator Either<L, R>(Either.Left<L> left) => new(left.Value);

    public static implicit operator Either<L, R>(Either.Right<R> right) => new(right.Value);

    public TR Match<TR>(Func<L, TR> left, Func<R, TR> right)
    {
        return IsLeft ? left(Left) : right(Right);
    }

    public Unit Match(Action<L> left, Action<R> right)
        => Match(left.ToFunc(), right.ToFunc());

    public override string ToString()
    {
        return Match(
            left: l => $"Left({l})",
            right: r => $"Right({r})");
    }

    public Either<L, R> ToEither()
    {
        throw new NotImplementedException();
    }
}

public static class Either
{
    public readonly struct Left<L>
    {
        public L Value { get; }

        public Left(L value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
        }

        public override string ToString() => $"Left({Value})";
    }

    public readonly struct Right<R>
    {
        public R Value { get; }

        public Right(R value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
        }

        public override string ToString() => $"Right({Value})";
    }
}

public static class EitherExtensions
{
    public static Either<L, RR> Map<L, R, RR>(this Either<L, R> @this, Func<R, RR> f)
        => @this.Match<Either<L, RR>>(
            l => Left(l),
            r => Right(f(r)));

    public static Either<LL, RR> Map<L, LL, R, RR>(this Either<L, R> @this, Func<L, LL> left, Func<R, RR> right)
        => @this.Match<Either<LL, RR>>(
            l => Left(left(l)),
            r => Right(right(r)));

    public static Either<L, Unit> ForEach<L, R>(this Either<L, R> @this, Action<R> act)
        => Map(@this, act.ToFunc());

    public static Either<L, RR> Bind<L, R, RR>(this Either<L, R> @this, Func<R, Either<L, RR>> f)
        => @this.Match(
            l => Left(l),
            r => f(r));

    // Applicative

    public static Either<L, RR> Apply<L, R, RR>
       (this Either<L, Func<R, RR>> @this, Either<L, R> arg)
       => @this.Match(
          left: (errF) => Left(errF),
          right: (f) => arg.Match<Either<L, RR>>(
             left: (err) => Left(err),
             right: (t) => Right(f(t))));

    public static Either<L, Func<T2, R>> Apply<L, T1, T2, R>
       (this Either<L, Func<T1, T2, R>> @this, Either<L, T1> arg)
       => Apply(@this.Map(F.Curry), arg);

    public static Either<L, Func<T2, T3, R>> Apply<L, T1, T2, T3, R>
       (this Either<L, Func<T1, T2, T3, R>> @this, Either<L, T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    public static Either<L, Func<T2, T3, T4, R>> Apply<L, T1, T2, T3, T4, R>
       (this Either<L, Func<T1, T2, T3, T4, R>> @this, Either<L, T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    public static Either<L, Func<T2, T3, T4, T5, R>> Apply<L, T1, T2, T3, T4, T5, R>
       (this Either<L, Func<T1, T2, T3, T4, T5, R>> @this, Either<L, T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    public static Either<L, Func<T2, T3, T4, T5, T6, R>> Apply<L, T1, T2, T3, T4, T5, T6, R>
       (this Either<L, Func<T1, T2, T3, T4, T5, T6, R>> @this, Either<L, T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    public static Either<L, Func<T2, T3, T4, T5, T6, T7, R>> Apply<L, T1, T2, T3, T4, T5, T6, T7, R>
       (this Either<L, Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Either<L, T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    public static Either<L, Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<L, T1, T2, T3, T4, T5, T6, T7, T8, R>
       (this Either<L, Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Either<L, T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    public static Either<L, Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<L, T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
       (this Either<L, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Either<L, T1> arg)
       => Apply(@this.Map(F.CurryFirst), arg);

    // LINQ

    public static Either<L, R> Select<L, T, R>(this Either<L, T> @this
       , Func<T, R> map) => @this.Map(map);

    public static Either<L, RR> SelectMany<L, T, R, RR>(this Either<L, T> @this
       , Func<T, Either<L, R>> bind, Func<T, R, RR> project)
       => @this.Match(
          left: l => Left(l),
          right: t =>
             bind(@this.Right).Match<Either<L, RR>>(
                left: l => Left(l),
                right: r => project(t, r)));
}
