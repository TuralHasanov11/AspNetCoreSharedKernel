using static AspNetCoreSharedKernel.Functional.F;
using Unit = System.ValueTuple;

namespace AspNetCoreSharedKernel.Functional;

internal static class EnumerableExtensions
{
    internal static Func<T, IEnumerable<T>> Return<T>() => t => List(t);

    internal static IEnumerable<T> Append<T>(this IEnumerable<T> source
       , params T[] ts) => source.Concat(ts);

    internal static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T val)
    {
        yield return val;
        foreach (T t in source)
        {
            yield return t;
        }
    }

    internal static Option<T> Find<T>(this IEnumerable<T> source, Func<T, bool> predicate)
       => source.Where(predicate).Head();

    internal static T FirstOr<T>(this IEnumerable<T> source, T defaultValue)
       => source.Head().Match(
          () => defaultValue,
          t => t);

    internal static IEnumerable<Unit> ForEach<T>
       (this IEnumerable<T> ts, Action<T> action)
       => [.. ts.Map(action.ToFunc())];

    internal static IEnumerable<R> Map_InTermsOfFold<T, R>
       (this IEnumerable<T> ts, Func<T, R> f)
        => ts.Aggregate(List<R>()
           , (rs, t) => rs.Append(f(t)));

    internal static IEnumerable<T> Where_InTermsOfFold<T>
       (this IEnumerable<T> @this, Func<T, bool> predicate)
        => @this.Aggregate(List<T>()
           , (ts, t) => predicate(t) ? ts.Append(t) : ts);

    internal static IEnumerable<R> Bind_InTermsOfFold<T, R>
       (this IEnumerable<T> ts, Func<T, IEnumerable<R>> f)
    {
        return ts.Aggregate(List<R>(), (rs, t) => rs.Concat(f(t)));
    }

    internal static IEnumerable<R> Map<T, R>
       (this IEnumerable<T> list, Func<T, R> func)
        => list.Select(func);

    internal static R Match<T, R>(this IEnumerable<T> list
       , Func<R> Empty, Func<T, IEnumerable<T>, R> Otherwise)
       => list.Head().Match(
          None: Empty,
          Some: head => Otherwise(head, list.Skip(1)));

    internal static Option<T> Head<T>(this IEnumerable<T> list)
    {
        if (list == null)
        {
            return None;
        }
        var enumerator = list.GetEnumerator();
        return enumerator.MoveNext() ? Some(enumerator.Current) : None;
    }

    internal static IEnumerable<R> _Map<T, R>(this IEnumerable<T> list, Func<T, R> func)
    {
        foreach (var item in list)
        {
            yield return func(item);
        }
    }

    internal static IEnumerable<T> Where<T>(this IEnumerable<T> list, Func<T, bool> predicate)
    {
        foreach (var item in list)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }

    internal static IEnumerable<Func<T2, R>> Map<T1, T2, R>(this IEnumerable<T1> list
       , Func<T1, T2, R> func)
       => list.Map(func.Curry());

    internal static IEnumerable<Func<T2, Func<T3, R>>> Map<T1, T2, T3, R>(
        this IEnumerable<T1> opt, Func<T1, T2, T3, R> func)
            => opt.Map(func.Curry());

    internal static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, IEnumerable<R>> func)
        => list.SelectMany(func);

    internal static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> list)
        => list.SelectMany(x => x);

    internal static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func)
        => list.Bind(t => func(t).AsEnumerable());

    // LINQ

    internal static IEnumerable<RR> SelectMany<T, R, RR>
       (this IEnumerable<T> source
       , Func<T, Option<R>> bind
       , Func<T, R, RR> project)
       => from t in source
          let opt = bind(t)
          where opt.IsSome()
          select project(t, opt.ValueUnsafe());

    internal static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> @this, Func<T, bool> pred)
    {
        foreach (var item in @this)
        {
            if (pred(item))
            {
                yield return item;
            }
            else
            {
                yield break;
            }
        }
    }

    internal static IEnumerable<T> DropWhile<T>(this IEnumerable<T> @this, Func<T, bool> pred)
    {
        bool clean = true;
        foreach (var item in @this)
        {
            if (!clean || !pred(item))
            {
                yield return item;
                clean = false;
            }
        }
    }
}
