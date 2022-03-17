namespace Masa.Utils.Caller.IntegratedTest;

[TestClass]
public class AutomaticCallerTest
{
    private WebApplicationBuilder _builder = default!;

    [TestInitialize]
    public void EdgeDriverInitialize()
    {
        _builder = WebApplication.CreateBuilder();
    }

    [TestMethod]
    public async Task TestGetAsync()
    {
        _builder.Services.AddCaller();
        _ = _builder.Build();
        var serviceProvider = _builder.Services.BuildServiceProvider();
        var githubCaller = serviceProvider.GetRequiredService<GithubCaller>();
        Assert.IsTrue(await githubCaller.GetAsync());
    }

    [TestMethod]
    public void TestRepeatAddCaller()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            _builder.Services.AddCaller().AddCaller();
        });
    }
}