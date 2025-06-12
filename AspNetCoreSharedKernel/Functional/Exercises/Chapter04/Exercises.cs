using static AspNetCoreSharedKernel.Functional.F;

namespace AspNetCoreSharedKernel.Functional.Exercises.Chapter04;

internal static class Exercises
{
    // 1 Implement Map for ISet<T> and IDictionary<K, T>. (Tip: start by writing down
    // the signature in arrow notation.)

    internal static ISet<R> Map<T, R>(this ISet<T> ts, Func<T, R> f)
    {
        var rs = new HashSet<R>();
        foreach (var t in ts)
        {
            rs.Add(f(t));
        }

        return rs;
    }

    internal static IDictionary<K, R> Map<K, T, R>(this IDictionary<K, T> dict, Func<T, R> f)
    {
        var rs = new Dictionary<K, R>();
        foreach (var pair in dict)
        {
            rs[pair.Key] = f(pair.Value);
        }

        return rs;
    }

    // 2 Implement Map for Option and IEnumerable in terms of Bind and Return.

    internal static Option<R> Map<T, R>(this Option<T> opt, Func<T, R> f)
         => opt.Bind(t => Some(f(t)));

    internal static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f)
       => ts.Bind(t => List(f(t)));

    // 3 Use Bind and an Option-returning Lookup function (such as the one we defined
    // in chapter 3) to implement GetWorkPermit, shown below.

    //internal static Option<WorkPermit> GetValidWorkPermit(Dictionary<string, Employee> employees, string employeeId)
    //     => employees
    //        .Lookup(employeeId)
    //        .Bind(e => e.WorkPermit)
    //        .Where(HasExpired.Negate());

    //// Then enrich the implementation so that `GetWorkPermit`
    //// returns `None` if the work permit has expired.

    //internal static Option<WorkPermit> GetWorkPermit(Dictionary<string, Employee> employees, string employeeId)
    //     => employees.Lookup(employeeId).Bind(e => e.WorkPermit);

    // 4 Use Bind to implement AverageYearsWorkedAtTheCompany, shown below (only
    // employees who have left should be included).

    internal static double AverageYearsWorkedAtTheCompany(List<Employee> employees)
         => employees
            .Bind(e => e.LeftOn.Map(leftOn => YearsBetween(e.JoinedOn, leftOn)))
            .Average();

    // a more elegant solution, which will become clear in Chapter 9
    internal static double AverageYearsWorkedAtTheCompany_LINQ(List<Employee> employees)
       => (from e in employees
           from leftOn in e.LeftOn
           select YearsBetween(e.JoinedOn, leftOn)
          ).Average();

    internal static double YearsBetween(DateTime start, DateTime end)
       => (end - start).Days / 365d;
}

internal struct WorkPermit
{
    internal string Number { get; set; }
    internal DateTime Expiry { get; set; }
}

internal class Employee
{
    internal string Id { get; set; }
    internal Option<WorkPermit> WorkPermit { get; set; }

    internal DateTime JoinedOn { get; }
    internal Option<DateTime> LeftOn { get; }
}
