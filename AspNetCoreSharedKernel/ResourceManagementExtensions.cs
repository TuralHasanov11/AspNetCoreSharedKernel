namespace AspNetCoreSharedKernel;

public static class ResourceManagementExtensions
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

    public static void Using<TDisposable>(
        TDisposable disposable,
        Action<TDisposable> func)
        where TDisposable : IDisposable
    {
        using (disposable)
        {
            func(disposable);
        }
    }
}
