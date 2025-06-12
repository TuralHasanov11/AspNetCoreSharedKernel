namespace AspNetCoreSharedKernel.Functional.Exercises.Chapter04;

internal class Person
{
    internal string FirstName { get; }
    internal string LastName { get; }

    internal decimal Earnings { get; set; }
    internal Option<int> Age { get; set; }

    internal Person() { }

    internal Person(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
