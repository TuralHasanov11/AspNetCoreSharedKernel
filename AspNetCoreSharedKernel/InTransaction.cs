namespace AspNetCoreSharedKernel;


public interface IInTransaction
{
    Task<T> Run<T>(Func<T> action);
}

public abstract class InTransaction(DbContext dbContext) : IInTransaction
{
    public async Task<T> Run<T>(Func<T> action)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var result = action.Invoke();

            if (dbContext.ChangeTracker.HasChanges())
            {
                await dbContext.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
