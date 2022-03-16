namespace Masa.Utils.Caller.IntegratedTest.Callers;

public class GithubCaller : HttpClientCallerBase
{
    public GithubCaller(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        BaseAddress = "https://github.com/masastack";
    }

    protected override string BaseAddress { get; set; }

    public async Task<bool> GetAsync()
    {
        var res = await CallerProvider.GetAsync("");
        return res.IsSuccessStatusCode && res.StatusCode == HttpStatusCode.OK;
    }
}
