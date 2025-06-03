using System.Diagnostics.CodeAnalysis;
using static AspNetCoreSharedKernel.Functional.F;
using Unit = System.ValueTuple;
namespace AspNetCoreSharedKernel.Functional
{
    public static partial class F
    {
        public static Option<T> Some<T>(T value) => new Option.Some<T>(value);
        public static Option.None None => Option.None.Default;
    }

    public static class OptionExamples
    {
        public static void Run()
        {
            static string greet(Option<string> greete)
            {
                return greete.Match(None: () => "Hello", Some: (g) => $"Hello {g}");
            }

            greet(None);
            greet(Some("Jon"));

            // Map
            Func<string, string> greet2 = name => $"hello, {name}";
            Option<string> _ = None;
            Option<string> optJohn = Some("John");

            _.Map(greet2); // None
            optJohn.Map(greet2); // Some("hello, John")

            // As Enumerable
            Console.WriteLine(Some("thing").AsEnumerable().Count()); // => 1


            // Either
            Render(Right(12d));
            // => "The result is: 12"
            Render(Left("oops"));
            // => "Invalid value: oops"
        }
    }


    public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<Option.None>
    {
        public T Value { get; }

        public bool IsSome { get; }

        [MemberNotNullWhen(false, nameof(Value))]
        public readonly bool IsNone => !IsSome;

        private Option(T value)
        {
            ArgumentNullException.ThrowIfNull(value);

            IsSome = true;
            Value = value;
        }

        public static implicit operator Option<T>(Option.None _) => new();
        public static implicit operator Option<T>(Option.Some<T> some) => new(some.Value);
        public static implicit operator Option<T>(T value) => value is null ? None : Some(value);

        public readonly R Match<R>(Func<R> None, Func<T, R> Some)
        {
            return IsSome ? Some(Value) : None();
        }

        public readonly bool Equals(Option<T> other) => IsSome == other.IsSome && (IsNone || Value.Equals(other.Value));

        public readonly bool Equals(Option.None _) => IsNone;

        public override readonly bool Equals(object? obj)
        {
            return obj is Option<T> option && Equals(option);
        }

        public override readonly int GetHashCode()
        {
            return !IsNone ? Value.GetHashCode() : 0;
        }

        public static bool operator ==(Option<T> current, Option<T> other) => current.Equals(other);
        public static bool operator !=(Option<T> current, Option<T> other) => !(current == other);

        public override readonly string ToString()
        {
            return IsSome ? $"Some({Value})" : "None";
        }

        public readonly IEnumerable<T> AsEnumerable()
        {
            if (IsSome)
            {
                yield return Value;
            }
        }

        public Option<T> ToOption()
        {
            throw new NotImplementedException();
        }
    }

    namespace Option
    {
        public readonly struct None
        {
            internal static readonly None Default = new();
        }

        public readonly struct Some<T>
        {
            internal T Value { get; }

            internal Some(T value)
            {
                ArgumentNullException.ThrowIfNull(value, "Cannot wrap a null value in a 'Some'; use 'None' instead");
                Value = value;
            }
        }
    }

    public static class OptionExtensions
    {
        public static Option<R> Apply<T, R>
           (this Option<Func<T, R>> @this, Option<T> arg)
           => @this.Match(
              () => None,
              (func) => arg.Match(
                 () => None,
                 (val) => Some(func(val))));

        public static Option<Func<T2, R>> Apply<T1, T2, R>
           (this Option<Func<T1, T2, R>> @this, Option<T1> arg)
           => Apply(@this.Map(F.Curry), arg);

        public static Option<Func<T2, T3, R>> Apply<T1, T2, T3, R>
           (this Option<Func<T1, T2, T3, R>> @this, Option<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Option<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
           (this Option<Func<T1, T2, T3, T4, R>> @this, Option<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Option<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
           (this Option<Func<T1, T2, T3, T4, T5, R>> @this, Option<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Option<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
           (this Option<Func<T1, T2, T3, T4, T5, T6, R>> @this, Option<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Option<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
           (this Option<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Option<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Option<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
           (this Option<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Option<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Option<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
           (this Option<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Option<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Option<R> Bind<T, R>
           (this Option<T> optT, Func<T, Option<R>> f)
            => optT.Match(
               () => None,
               (t) => f(t));

        public static IEnumerable<R> Bind<T, R>
           (this Option<T> @this, Func<T, IEnumerable<R>> func)
            => @this.AsEnumerable().Bind(func);

        public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action)
           => Map(@this, action.ToFunc());

        public static Option<R> Map<T, R>
           (this Option.None _, Func<T, R> __)
           => None;

        public static Option<R> Map<T, R>
           (this Option.Some<T> some, Func<T, R> f)
           => Some(f(some.Value));

        public static Option<R> Map<T, R>
           (this Option<T> optT, Func<T, R> f)
           => optT.Match(
              () => None,
              (t) => Some(f(t)));

        public static Option<Func<T2, R>> Map<T1, T2, R>
           (this Option<T1> @this, Func<T1, T2, R> func)
            => @this.Map(func.Curry());

        public static Option<Func<T2, T3, R>> Map<T1, T2, T3, R>
           (this Option<T1> @this, Func<T1, T2, T3, R> func)
            => @this.Map(func.CurryFirst());

        //public static IEnumerable<Option<R>> Traverse<T, R>(this Option<T> @this
        //   , Func<T, IEnumerable<R>> func)
        //   => @this.Match(
        //      () => List((Option<R>)None),
        //      (t) => func(t).Map(r => Some(r)));

        // utilities

        public static Unit Match<T>(this Option<T> @this, Action None, Action<T> Some)
            => @this.Match(None.ToFunc(), Some.ToFunc());

        internal static bool IsSome<T>(this Option<T> @this)
           => @this.Match(
              () => false,
              (_) => true);

        internal static T ValueUnsafe<T>(this Option<T> @this)
           => @this.Match(
              () => throw new InvalidOperationException(),
              (t) => t);

        public static T GetOrElse<T>(this Option<T> opt, T defaultValue)
           => opt.Match(
              () => defaultValue,
              (t) => t);

        public static T GetOrElse<T>(this Option<T> opt, Func<T> fallback)
           => opt.Match(
              () => fallback(),
              (t) => t);

        //public static Task<T> GetOrElse<T>(this Option<T> opt, Func<Task<T>> fallback)
        //   => opt.Match(
        //      () => fallback(),
        //      (t) => Async(t));

        public static Option<T> OrElse<T>(this Option<T> left, Option<T> right)
           => left.Match(
              () => right,
              (_) => left);

        public static Option<T> OrElse<T>(this Option<T> left, Func<Option<T>> right)
           => left.Match(
              () => right(),
              (_) => left);


        //public static Validation<T> ToValidation<T>(this Option<T> opt, Func<Error> error)
        //   => opt.Match(
        //      () => Invalid(error()),
        //      (t) => Valid(t));

        // LINQ

        public static Option<R> Select<T, R>(this Option<T> @this, Func<T, R> func)
           => @this.Map(func);

        public static Option<T> Where<T>
           (this Option<T> optT, Func<T, bool> predicate)
           => optT.Match(
              () => None,
              (t) => predicate(t) ? optT : None);

        public static Option<RR> SelectMany<T, R, RR>
           (this Option<T> opt, Func<T, Option<R>> bind, Func<T, R, RR> project)
           => opt.Match(
              () => None,
              (t) => bind(t).Match(
                 () => None,
                 (r) => Some(project(t, r))));
    }

    public readonly struct Email
    {
        public string Value { get; }

        public static Option<Email> Of(string value)
        {
            return value is not null
                ? Some(new Email(value))
                : None;
        }
        public Email(string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (!value.Contains('@'))
            {
                throw new ArgumentException("Email must contain an '@' character.", nameof(value));
            }

            Value = value;
        }
    }
}

