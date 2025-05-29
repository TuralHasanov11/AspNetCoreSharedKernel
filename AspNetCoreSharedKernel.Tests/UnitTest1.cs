using System.Globalization;
using System.Reflection;
using Xunit.v3;

namespace AspNetCoreSharedKernel.Tests;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class UseCultureAttribute : BeforeAfterTestAttribute
{
    private readonly Lazy<CultureInfo> _culture;
    private readonly Lazy<CultureInfo> _uiCulture;

    private CultureInfo _originalCulture;
    private CultureInfo _originalUICulture;

    public UseCultureAttribute(string culture)
        : this(culture, culture) { }

    public UseCultureAttribute(string culture, string uiCulture)
    {
        _culture = new Lazy<CultureInfo>(() => new CultureInfo(culture, false));
        _uiCulture = new Lazy<CultureInfo>(() => new CultureInfo(uiCulture, false));
    }

    public CultureInfo Culture { get { return _culture.Value; } }

    public CultureInfo UICulture { get { return _uiCulture.Value; } }

    public override void Before(MethodInfo methodUnderTest, IXunitTest test)
    {
        _originalCulture = Thread.CurrentThread.CurrentCulture;
        _originalUICulture = Thread.CurrentThread.CurrentUICulture;

        Thread.CurrentThread.CurrentCulture = Culture;
        Thread.CurrentThread.CurrentUICulture = UICulture;

        CultureInfo.CurrentCulture.ClearCachedData();
        CultureInfo.CurrentUICulture.ClearCachedData();
    }

    public override void After(MethodInfo methodUnderTest, IXunitTest test)
    {
        Thread.CurrentThread.CurrentCulture = _originalCulture;
        Thread.CurrentThread.CurrentUICulture = _originalUICulture;

        CultureInfo.CurrentCulture.ClearCachedData();
        CultureInfo.CurrentUICulture.ClearCachedData();
    }
}