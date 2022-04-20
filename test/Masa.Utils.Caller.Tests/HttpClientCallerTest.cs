namespace Masa.Utils.Caller.Tests;

[TestClass]
public class HttpClientCallerTest
{
    [DataTestMethod]
    [DataRow("https://github.com/", "/check/healthy", "https://github.com/check/healthy")]
    [DataRow("https://github.com", "/check/healthy", "https://github.com/check/healthy")]
    [DataRow("https://github.com", "check/healthy", "https://github.com/check/healthy")]
    [DataRow("https://github.com/check", "healthy", "https://github.com/check/healthy")]
    [DataRow("https://github.com/check/", "healthy", "https://github.com/check/healthy")]
    [DataRow("https://github.com/check/", "/healthy", "https://github.com/check/healthy")]
    [DataRow("https://github.com/check/", "/healthy?date=1650465417", "https://github.com/check/healthy?date=1650465417")]
    [DataRow("https://github.com/check/", "healthy?date=1650465417", "https://github.com/check/healthy?date=1650465417")]
    [DataRow("https://github.com/check", "healthy?date=1650465417", "https://github.com/check/healthy?date=1650465417")]
    [DataRow("https://github.com", "https://github.com/check/healthy?date=1650465417", "https://github.com/check/healthy?date=1650465417")]
    [DataRow("https://github.com", "", "https://github.com")]
    [DataRow("http://github.com", "", "http://github.com")]
    [DataRow("", "", "")]
    public void TestGetRequestUri(string baseApi, string methods, string fullPath)
    {
        var services = new ServiceCollection();
        services.AddCaller(opt => opt.UseHttpClient());
        var serviceProvider = services.BuildServiceProvider();
        var provider = new CustomHttpClientCallerProvider(serviceProvider, string.Empty, baseApi);
        Assert.IsTrue(provider.GetFullPath(methods) == fullPath);
    }
}
