using System.Security.Cryptography;
using Ardalis.Result;
using Microsoft.AspNetCore.DataProtection;

namespace AspNetCoreSharedKernel;


public static class TimeLimitedDataProtector
{
    private const int DefaultLifetime = 30;

    public static Result<string> ProtectedData(
        string name,
        string data,
        IDataProtectionProvider dataProtectionProvider,
        int lifetime = DefaultLifetime)
    {
        var protector = dataProtectionProvider.CreateProtector(name)
            .ToTimeLimitedDataProtector();

        return Result.Success(protector.Protect(data, TimeSpan.FromSeconds(lifetime)));
    }

    public static Result<string> UnprotectData(
        string name,
        string data,
        IDataProtectionProvider dataProtectionProvider)
    {
        var protector = dataProtectionProvider.CreateProtector(name)
            .ToTimeLimitedDataProtector();

        try
        {
            return protector.Unprotect(data);
        }
        catch (CryptographicException)
        {
            return Result.Conflict();
        }
    }
}
