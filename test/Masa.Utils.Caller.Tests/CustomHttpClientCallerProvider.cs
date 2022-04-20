namespace Masa.Utils.Caller.Tests;

public class CustomHttpClientCallerProvider : HttpClientCallerProvider
{
    public CustomHttpClientCallerProvider(IServiceProvider serviceProvider, string name, string baseApi)
        : base(serviceProvider, name, baseApi)
    {

    }

    public string GetFullPath(string? methodName) => base.GetRequestUri(methodName);
}
