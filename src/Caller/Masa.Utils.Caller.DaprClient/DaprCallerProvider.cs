namespace Masa.Utils.Caller.DaprClient;

public class DaprCallerProvider : AbstractCallerProvider
{
    private readonly string AppId;
    private readonly IRequestMessage _requestMessage;
    private readonly Dapr.Client.DaprClient _daprClient;

    public DaprCallerProvider(string appId, IRequestMessage requestMessage, Dapr.Client.DaprClient daprClient)
    {
        AppId = appId;
        _requestMessage = requestMessage;
        _daprClient = daprClient;
    }

    public override async Task<TResponse?> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken = default) where TResponse : default
    {
        var response = await _daprClient.InvokeMethodWithResponseAsync(request, cancellationToken);
        return await _requestMessage.ProcessResponseAsync<TResponse>(response, cancellationToken);
    }

    public override HttpRequestMessage CreateRequest(HttpMethod method, string? methodName)
        => _daprClient.CreateInvokeMethodRequest(method, AppId, methodName);

    public override HttpRequestMessage CreateRequest<TRequest>(HttpMethod method, string? methodName, TRequest data)
        => _daprClient.CreateInvokeMethodRequest(method, AppId, methodName, data);

    public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        => _daprClient.InvokeMethodWithResponseAsync(request, cancellationToken);

    public override Task SendGrpcAsync(string methodName, CancellationToken cancellationToken = default)
        => _daprClient.InvokeMethodGrpcAsync(AppId, methodName, cancellationToken);

    public override Task<TResponse> SendGrpcAsync<TResponse>(string methodName, CancellationToken cancellationToken = default)
        => _daprClient.InvokeMethodGrpcAsync<TResponse>(AppId, methodName, cancellationToken);

    public override Task SendGrpcAsync<TRequest>(string methodName, TRequest request, CancellationToken cancellationToken = default)
        => _daprClient.InvokeMethodGrpcAsync(AppId, methodName, request, cancellationToken);

    public override Task<TResponse> SendGrpcAsync<TRequest, TResponse>(string methodName, TRequest request, CancellationToken cancellationToken = default)
        => _daprClient.InvokeMethodGrpcAsync<TResponse>(AppId, methodName, cancellationToken);
}
