namespace AspNetCoreSharedKernel;

public static class FunctionalExtensions
{
    public static TOutput Using<TOutput, TDisposable>(
        TDisposable disposable,
        Func<TDisposable, TOutput> func)
        where TDisposable : IDisposable
    {
        using (disposable)
        {
            return func(disposable);
        }
    }
}
