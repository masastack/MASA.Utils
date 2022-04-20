namespace Masa.Utils.Caller.HttpClient;

public class HttpClientCallerProvider : AbstractCallerProvider
{
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly IRequestMessage _requestMessage;
    private readonly bool baseApiIsNullOrEmpty;
    private readonly string _baseApi;

    public HttpClientCallerProvider(IServiceProvider serviceProvider, string name, string baseApi)
        : base(serviceProvider)
    {
        _httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(name);
        _requestMessage = serviceProvider.GetRequiredService<IRequestMessage>();
        _baseApi = baseApi;
        baseApiIsNullOrEmpty = string.IsNullOrEmpty(_baseApi);
    }

    public override async Task<TResponse?> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken = default)
        where TResponse : default
    {
        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        return await _requestMessage.ProcessResponseAsync<TResponse>(response, cancellationToken);
    }

    public override HttpRequestMessage CreateRequest(HttpMethod method, string? methodName) => new(method, GetRequestUri(methodName));

    public override HttpRequestMessage CreateRequest<TRequest>(HttpMethod method, string? methodName, TRequest data)
    {
        HttpRequestMessage request = CreateRequest(method, methodName);
        request.Content = JsonContent.Create(data);
        return request;
    }

    public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        => _httpClient.SendAsync(request, cancellationToken);

    public override Task SendGrpcAsync(string methodName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<TResponse> SendGrpcAsync<TResponse>(string methodName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task SendGrpcAsync<TRequest>(string methodName, TRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<TResponse> SendGrpcAsync<TRequest, TResponse>(string methodName, TRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    protected virtual string GetRequestUri(string? methodName)
    {
        if (string.IsNullOrEmpty(methodName))
            return baseApiIsNullOrEmpty ? string.Empty : _baseApi;

        if (baseApiIsNullOrEmpty || Uri.IsWellFormedUriString(methodName, UriKind.Absolute))
            return methodName;

        if (_baseApi.EndsWith("/"))
        {
            return $"{_baseApi}{(methodName.StartsWith("/") ? methodName.Substring(1) : methodName)}";
        }
        return $"{_baseApi}{(methodName.StartsWith("/") ? methodName : "/" + methodName)}";
    }
}
