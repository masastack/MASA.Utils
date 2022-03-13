namespace Masa.Utils.Caller.Core.Tests.Callers;

public class CustomCaller: HttpClientCallerBase
{
    public CustomCaller(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override string Name { get; set; } = nameof(CustomCaller);

    protected override string BaseAddress { get; set; }
}
