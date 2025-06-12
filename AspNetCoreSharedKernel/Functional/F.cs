using System.Collections.Immutable;
using Unit = System.ValueTuple;

namespace AspNetCoreSharedKernel.Functional;

internal static partial class F
{
    internal static Unit Unit() => default;

    // function manipulation

    internal static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> func)
        => t1 => t2 => func(t1, t2);

    internal static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(this Func<T1, T2, T3, R> func)
        => t1 => t2 => t3 => func(t1, t2, t3);

    internal static Func<T1, Func<T2, T3, R>> CurryFirst<T1, T2, T3, R>
       (this Func<T1, T2, T3, R> @this) => t1 => (t2, t3) => @this(t1, t2, t3);

    internal static Func<T1, Func<T2, T3, T4, R>> CurryFirst<T1, T2, T3, T4, R>
       (this Func<T1, T2, T3, T4, R> @this) => t1 => (t2, t3, t4) => @this(t1, t2, t3, t4);

    internal static Func<T1, Func<T2, T3, T4, T5, R>> CurryFirst<T1, T2, T3, T4, T5, R>
       (this Func<T1, T2, T3, T4, T5, R> @this) => t1 => (t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);

    internal static Func<T1, Func<T2, T3, T4, T5, T6, R>> CurryFirst<T1, T2, T3, T4, T5, T6, R>
       (this Func<T1, T2, T3, T4, T5, T6, R> @this) => t1 => (t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6);

    internal static Func<T1, Func<T2, T3, T4, T5, T6, T7, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, R>
       (this Func<T1, T2, T3, T4, T5, T6, T7, R> @this) => t1 => (t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7);

    internal static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, R>
       (this Func<T1, T2, T3, T4, T5, T6, T7, T8, R> @this) => t1 => (t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8);

    internal static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
       (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> @this) => t1 => (t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);

    internal static Func<T, T> Tap<T>(Action<T> act)
       => x => { act(x); return x; };

    internal static R Pipe<T, R>(this T @this, Func<T, R> func) => func(@this);

    /// <summary>
    /// Pipes the input value in the given Action, i.e. invokes the given Action on the given value.
    /// returning the input value. Not really a genuine implementation of pipe, since it combines pipe with Tap.
    /// </summary>
    internal static T Pipe<T>(this T input, Action<T> func) => Tap(func)(input);

    // DATA STRUCTURES

    internal static KeyValuePair<K, T> Pair<K, T>(K key, T value)
       => new KeyValuePair<K, T>(key, value);

    internal static IEnumerable<T> List<T>(params T[] items) => [.. items];

    internal static Func<T, IEnumerable<T>> SingletonList<T>() => (item) => [item];

    internal static IEnumerable<T> Cons<T>(this T t, IEnumerable<T> ts)
       => List(t).Concat(ts);

    internal static Func<T, IEnumerable<T>, IEnumerable<T>> Cons<T>()
       => (t, ts) => t.Cons(ts);

    internal static IDictionary<K, T> Map<K, T>(params KeyValuePair<K, T>[] pairs) where K : notnull
    {
        return pairs.ToImmutableDictionary();
    }

    // misc

    // Using
    internal static R Using<TDisposable, R>(TDisposable disposable, Func<TDisposable, R> func)
        where TDisposable : IDisposable
    {
        using var disp = disposable;
        return func(disp);
    }

    internal static Unit Using<TDisposable>(TDisposable disposable, Action<TDisposable> act)
        where TDisposable : IDisposable
        => Using(disposable, act.ToFunc());

    internal static R Using<TDisposable, R>(Func<TDisposable> createDisposable, Func<TDisposable, R> func)
        where TDisposable : IDisposable
    {
        using var disp = createDisposable();
        return func(disp);
    }

    internal static Unit Using<TDisposable>(Func<TDisposable> createDisposable, Action<TDisposable> action)
        where TDisposable : IDisposable
        => Using(createDisposable, action.ToFunc());

    // Range
    internal static IEnumerable<char> Range(char from, char to)
    {
        for (var i = from; i <= to; i++)
        {
            yield return i;
        }
    }

    internal static IEnumerable<int> Range(int from, int to)
    {
        for (var i = from; i <= to; i++)
        {
            yield return i;
        }
    }
}
