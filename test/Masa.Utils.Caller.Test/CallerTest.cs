using Masa.Utils.Caller.Core;
using Masa.Utils.Caller.HttpClient;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Masa.Utils.Caller.IntegratedTest;

[TestClass]
public class CallerTest
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
        _builder.Services.AddCaller(callerOptions =>
        {
            callerOptions.UseHttpClient(httpClientBuilder =>
            {
                httpClientBuilder.BaseApi = "https://github.com/masastack";
            });
        });
        _ = _builder.Build();
        var serviceProvider = _builder.Services.BuildServiceProvider();
        var githubCaller = serviceProvider.GetRequiredService<ICallerProvider>();
        Assert.IsTrue(await GetAsync(githubCaller));
    }

    public async Task<bool> GetAsync(ICallerProvider callerProvider)
    {
        var res = await callerProvider.GetAsync("");
        return res.IsSuccessStatusCode && res.StatusCode == HttpStatusCode.OK;
    }
}
