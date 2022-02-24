namespace Masa.Utils.Caller.HttpClient;

public static class CallerOptionsExtensions
{
    public static IHttpClientBuilder UseHttpClient(this CallerOptions callerOptions, Func<MasaHttpClientBuilder>? clientBuilder = null)
    {
        var builder = clientBuilder == null ? new MasaHttpClientBuilder() : clientBuilder.Invoke();

        IHttpClientBuilder httpClientBuilder = builder.Configure == null ?
            callerOptions.Services.AddHttpClient(builder.Name) :
            callerOptions.Services.AddHttpClient(builder.Name, builder.Configure);

        callerOptions.AddCaller(builder.Name, builder.IsDefault, (serviceProvider) =>
        {
            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(builder.Name);
            var requestMessage = serviceProvider.GetRequiredService<IRequestMessage>();
            return new HttpClientCallerProvider(
                httpClient,
                requestMessage,
                builder.BaseApi);
        });
        return httpClientBuilder;
    }

    public static IHttpClientBuilder UseHttpClient(this CallerOptions callerOptions, Action<MasaHttpClientBuilder>? clientBuilder)
    {
        MasaHttpClientBuilder builder = new MasaHttpClientBuilder();
        clientBuilder?.Invoke(builder);

        return callerOptions.UseHttpClient(() => builder);
    }
}
