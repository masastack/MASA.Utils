namespace MASA.Utils.Caller.HttpClient;

public abstract class HttpClientCallerBase : CallerBase
{
    protected abstract string BaseAddress { get; set; }

    protected HttpClientCallerBase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override void UseCallerExtension() => UseHttpClient();

    protected virtual IHttpClientBuilder UseHttpClient()
    {
        return CallerOptions.UseHttpClient(opt =>
        {
            opt.Name = Name;
            opt.Configure = client => { client.BaseAddress = new Uri(BaseAddress); };
        });
    }
}
