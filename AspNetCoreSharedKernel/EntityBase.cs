namespace AspNetCoreSharedKernel;

public abstract class EntityBase : HasDomainEventsBase
{
    protected EntityBase()
    {
    }

    public int Id { get; }

    public byte[]? RowVersion { get; }
}

public abstract class EntityBase<TId> : HasDomainEventsBase
  where TId : class, IEquatable<TId>
{
    protected EntityBase()
    {
    }

    protected EntityBase(TId id)
    {
        Id = id;
    }

    public TId Id { get; }

    public byte[]? RowVersion { get; }

    public string RowVersionValue => RowVersion is not null ? Convert.ToBase64String(RowVersion) : string.Empty;
}
