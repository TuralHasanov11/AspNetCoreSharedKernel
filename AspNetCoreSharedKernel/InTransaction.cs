namespace AspNetCoreSharedKernel;


public interface IInTransaction
{
    Task<T> Run<T>(Func<T> action);

    Task Run(Action action);
}

public abstract class InTransaction(DbContext dbContext) : IInTransaction
{
    public async Task<T> Run<T>(Func<T> action)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var result = action();

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

    public async Task Run(Action action)
    {
        await Run(action);
    }
}
