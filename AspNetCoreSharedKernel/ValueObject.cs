namespace AspNetCoreSharedKernel;

public abstract record ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public bool ValueEquals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
}
