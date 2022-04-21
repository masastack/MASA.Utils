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
    [DataRow("https://github.com", "", "")]
    [DataRow("http://github.com", "", "")]
    [DataRow("/v1/check", "healthy", "/v1/check/healthy")]
    [DataRow("/v1/check/", "healthy", "/v1/check/healthy")]
    [DataRow("/v1/check/", "/healthy", "/v1/check/healthy")]
    [DataRow("/v1/check/", "/healthy", "/v1/check/healthy")]
    [DataRow("/v1/check/", "https://github.com/check/healthy?date=1650465417", "https://github.com/check/healthy?date=1650465417")]
    [DataRow("", "healthy", "healthy")]
    [DataRow("", "/healthy?id=1", "/healthy?id=1")]
    public void TestGetRequestUri(string prefix, string methods, string result)
    {
        var services = new ServiceCollection();
        services.AddCaller(opt => opt.UseHttpClient());
        var serviceProvider = services.BuildServiceProvider();
        var provider = new CustomHttpClientCallerProvider(serviceProvider, string.Empty, prefix);
        Assert.IsTrue(provider.GetResult(methods) == result);
    }
}
