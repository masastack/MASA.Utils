namespace Masa.Utils.Caller.Core.Tests.Callers;

public class BlogCaller : HttpClientCallerBase
{
    public readonly CustomCaller CustomCaller;
    public readonly GithubCaller GithubCaller;

    public BlogCaller(CustomCaller customCaller, GithubCaller githubCaller,IServiceProvider serviceProvider) : base(serviceProvider)
    {
        CustomCaller = customCaller;
        GithubCaller = githubCaller;
    }

    protected override string BaseAddress { get; set; }
}
