namespace Masa.Utils.Caller.Core.Tests.Callers;

public class GithubCaller : HttpClientCallerBase
{
    public GithubCaller(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override string BaseAddress { get; set; } = default!;
}
