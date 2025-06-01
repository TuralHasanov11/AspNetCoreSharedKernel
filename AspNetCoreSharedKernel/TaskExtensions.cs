namespace AspNetCoreSharedKernel;

public static class TaskExtensions
{
    public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource();

        // Create the timeout task (don't await it)
        var timeoutTask = Task<TResult>.Delay(timeout, cts.Token);

        // Run the task and timeout in parallel, return the Task that completes first
        var completedTask = await Task<TResult>.WhenAny(task, timeoutTask).ConfigureAwait(false);

        if (completedTask == task)
        {
            cts.Cancel();
            return await task.ConfigureAwait(false);
        }
        else
        {
            throw new TimeoutException($"Task timed out after {timeout}");
        }
    }
}