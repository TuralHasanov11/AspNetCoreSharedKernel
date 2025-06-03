namespace AspNetCoreSharedKernel;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }

    DateTime DeletedOnUtc { get; set; }

    void Delete()
    {
        IsDeleted = true;
        DeletedOnUtc = DateTime.UtcNow;
    }
}
