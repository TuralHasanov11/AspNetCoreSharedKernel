using System.Collections.Specialized;
using System.Text.RegularExpressions;

//using System.Configuration;
using static AspNetCoreSharedKernel.Functional.F;
namespace AspNetCoreSharedKernel.Functional.Exercises.Chapter03;


internal static partial class Exercises
{
    // 1 Write a generic function that takes a string and parses it as a value of an enum. It
    // should be usable as follows:

    // Enum.Parse<DayOfWeek>("Friday") // => Some(DayOfWeek.Friday)
    // Enum.Parse<DayOfWeek>("Freeday") // => None

    // 2 Write a Lookup function that will take an IEnumerable and a predicate, and
    // return the first element in the IEnumerable that matches the predicate, or None
    // if no matching element is found. Write its signature in arrow notation:

    // bool isOdd(int i) => i % 2 == 1;
    // new List<int>().Lookup(isOdd) // => None
    // new List<int> { 1 }.Lookup(isOdd) // => Some(1)

    internal static Option<TSource> Lookup<TSource>(this IEnumerable<TSource> source,
        Func<TSource, bool> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item))
            {
                return Some(item);
            }
        }
        return (Option<TSource>)None;
    }

    // 3 Write a type Email that wraps an underlying string, enforcing that it’s in a valid
    // format. Ensure that you include the following:
    // - A smart constructor
    // - Implicit conversion to string, so that it can easily be used with the typical API
    // for sending emails

    internal partial class Email
    {
        private string Value { get; }

        private Email(string value) => Value = value;

        internal static Option<Email> Create(string s)
           => Regex().IsMatch(s)
              ? Some(new Email(s))
              : None;

        public static implicit operator string(Email e)
           => e.Value;

        [GeneratedRegex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        private static partial Regex Regex();
    }

    // 4 Take a look at the extension methods defined on IEnumerable inSystem.LINQ.Enumerable.
    // Which ones could potentially return nothing, or throw some
    // kind of not-found exception, and would therefore be good candidates for
    // returning an Option<T> instead?
}

// 5.  Write implementations for the methods in the `AppConfig` class
// below. (For both methods, a reasonable one-line method body is possible.
// Assume settings are of type string, numeric or date.) Can this
// implementation help you to test code that relies on settings in a
// `.config` file?
internal class AppConfig
{
    private readonly NameValueCollection _source;

    //internal AppConfig() : this(ConfigurationManager.AppSettings) { }

    internal AppConfig(NameValueCollection source)
    {
        this._source = source;
    }

    internal Option<T> Get<T>(string name)
    {
        var value = _source[name];

        if (value == null)
        {
            return None;
        }

        return Some((T)(object)value);
    }

    internal T Get<T>(string name, T defaultValue)
    {
        Option<T> value = Get<T>(name);

        return value.Match(
            None: () => defaultValue,
            Some: (s) => s);
    }
}
