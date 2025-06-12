namespace AspNetCoreSharedKernel.Functional;

internal static partial class F
{
    internal static Error Error(string message) => new(message);
}

internal class Error
{
    internal virtual string Message { get; }
    public override string ToString() => Message;
    protected Error() { }
    internal Error(string Message) { this.Message = Message; }

    public static implicit operator Error(string m) => new Error(m);
}
