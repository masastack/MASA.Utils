namespace MASA.Utils.Caller.Core;

public interface ICallerProvider
{
    Task<TResponse?> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> SendAsync(HttpMethod method, string? methodName, HttpContent? content, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> SendAsync<TRequest>(HttpMethod method, string? methodName, TRequest data, CancellationToken cancellationToken = default);

    Task<TResponse?> SendAsync<TRequest, TResponse>(HttpMethod method, string? methodName, TRequest data, CancellationToken cancellationToken = default);

    Task SendGrpcAsync(string methodName, CancellationToken cancellationToken = default);

    Task<TResponse> SendGrpcAsync<TResponse>(string methodName, CancellationToken cancellationToken = default)
        where TResponse : IMessage, new();

    Task SendGrpcAsync<TRequest>(string methodName, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IMessage;

    Task<TResponse> SendGrpcAsync<TRequest, TResponse>(string methodName, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IMessage
        where TResponse : IMessage, new();

    Task<string> GetStringAsync(string? methodName, CancellationToken cancellationToken = default);

    Task<byte[]> GetByteArrayAsync(string? methodName, CancellationToken cancellationToken = default);

    Task<Stream> GetStreamAsync(string? methodName, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> GetAsync(string? methodName, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> GetAsync(string? methodName, Dictionary<string, string> data, CancellationToken cancellationToken = default);

    Task<TResponse?> GetAsync<TResponse>(string? methodName, CancellationToken cancellationToken = default);

    Task<TResponse?> GetAsync<TResponse>(string? methodName, Dictionary<string, string> data, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> PostAsync(string? methodName, HttpContent? content, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> PostAsync<TRequest>(string? methodName, TRequest data, CancellationToken cancellationToken = default);

    Task<TResponse?> PostAsync<TRequest, TResponse>(string? methodName, TRequest data, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> PatchAsync(string? methodName, HttpContent? content, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> PatchAsync<TRequest>(string? methodName, TRequest data, CancellationToken cancellationToken = default);

    Task<TResponse?> PatchAsync<TRequest, TResponse>(string? methodName, TRequest data, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> PutAsync(string? methodName, HttpContent? content, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> PutAsync<TRequest>(string? methodName, TRequest data, CancellationToken cancellationToken = default);

    Task<TResponse?> PutAsync<TRequest, TResponse>(string? methodName, TRequest data, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> DeleteAsync(string? methodName, HttpContent? content, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> DeleteAsync<TRequest>(string? methodName, TRequest data, CancellationToken cancellationToken = default);

    Task<TResponse?> DeleteAsync<TRequest, TResponse>(string? methodName, TRequest data, CancellationToken cancellationToken = default);
}
