namespace MASA.Utils.Caller.HttpClient;

public class HttpClientCallerProvider : AbstractCallerProvider
{
    private readonly System.Net.Http.HttpClient _httpClient;
    private string _baseAPI;
    private JsonSerializerOptions? _jsonSerializerOptions;

    public HttpClientCallerProvider(System.Net.Http.HttpClient httpClient, string baseAPI, JsonSerializerOptions? jsonSerializerOptions)
    {
        _httpClient = httpClient;
        _baseAPI = baseAPI;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public override async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

        if (typeof(TResponse).GetInterfaces().Any(type => type == typeof(IConvertible)))
        {
            var content = await response.Content.ReadAsStringAsync();
            return (TResponse)Convert.ChangeType(content, typeof(TResponse));
        }
        return await response.Content.ReadFromJsonAsync<TResponse>(this._jsonSerializerOptions, cancellationToken)
            ?? throw new ArgumentException("Response cannot be empty");
    }

    public override HttpRequestMessage CreateRequest(HttpMethod method, string? methodName)
        => new HttpRequestMessage(method, GetRequestUri(methodName));

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

    public override Task<TResponse> SendGrpcAsync<TRequest, TResponse>(string methodName, TRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private string GetRequestUri(string? methodName)
    {
        if (methodName is null)
            return string.Empty;

        if (_baseAPI != string.Empty && !methodName.Split('?')[0].Contains("/"))
        {
            return $"{_baseAPI}{(_baseAPI.Substring(_baseAPI.Length - 1, 1) == "/" ? methodName : $"/{methodName}")}";
        }
        return methodName;
    }
}
