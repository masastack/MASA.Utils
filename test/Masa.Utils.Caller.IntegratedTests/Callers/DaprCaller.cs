namespace Masa.Utils.Caller.IntegratedTest.Callers;

public class DaprCaller : DaprCallerBase
{
    public DaprCaller(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        AppId = "DaprCaller";
    }

    protected override string AppId { get; set; }

    public bool CallerProviderIsNotNull() => CallerProvider != null;
}
