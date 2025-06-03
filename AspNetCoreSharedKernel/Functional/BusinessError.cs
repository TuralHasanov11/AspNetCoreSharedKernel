namespace AspNetCoreSharedKernel.Functional;

public static partial class F
{
    public static BusinessError Error(string message) => new BusinessError(message);
}

public class BusinessError
{
    public virtual string Message { get; }

    public override string ToString() => Message;

    protected BusinessError() { }

    internal BusinessError(string Message) { this.Message = Message; }


    public static implicit operator BusinessError(string m) => new BusinessError(m);
}
