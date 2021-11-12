namespace MASA.Utils.Caller.Core;

public abstract class AbstractCallerProvider : ICallerProvider
{
    public abstract Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken = default);

    public abstract HttpRequestMessage CreateRequest(HttpMethod method, string? methodName);

    public abstract HttpRequestMessage CreateRequest<TRequest>(HttpMethod method, string? methodName, TRequest data);

    public abstract Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);

    public virtual Task<HttpResponseMessage> SendAsync(HttpMethod method, string? methodName, HttpContent? content, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage request = CreateRequest(method, methodName);
        request.Content = content;
        return SendAsync(request, cancellationToken);
    }

    public virtual Task<HttpResponseMessage> SendAsync<TRequest>(HttpMethod method, string? methodName, TRequest data, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage request = CreateRequest(method, methodName, data);
        return SendAsync(request);
    }

    public virtual Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string? methodName, TRequest data, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage request = CreateRequest(method, methodName, data);
        return SendAsync<TResponse>(request, cancellationToken);
    }

    public abstract Task SendGrpcAsync(string methodName, CancellationToken cancellationToken = default);

    public abstract Task<TResponse> SendGrpcAsync<TResponse>(string methodName, CancellationToken cancellationToken = default)
        where TResponse : IMessage, new();

    public abstract Task SendGrpcAsync<TRequest>(string methodName, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IMessage;

    public abstract Task<TResponse> SendGrpcAsync<TRequest, TResponse>(string methodName, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IMessage
        where TResponse : IMessage, new();

    public virtual async Task<string> GetStringAsync(string? methodName, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage request = CreateRequest(HttpMethod.Get, methodName);
        HttpResponseMessage content = await SendAsync(request, cancellationToken);
        return await content.Content.ReadAsStringAsync();
    }

    public virtual async Task<byte[]> GetByteArrayAsync(string? methodName, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = CreateRequest(HttpMethod.Get, methodName);
        HttpResponseMessage content = await SendAsync(request, cancellationToken);
        return await content.Content.ReadAsByteArrayAsync();
    }

    public virtual async Task<Stream> GetStreamAsync(string? methodName, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = CreateRequest(HttpMethod.Get, methodName);
        HttpResponseMessage content = await SendAsync(request, cancellationToken);
        return await content.Content.ReadAsStreamAsync();
    }

    public virtual Task<HttpResponseMessage> GetAsync(string? methodName, CancellationToken cancellationToken)
        => SendAsync(HttpMethod.Get, methodName, null, cancellationToken);

    public virtual Task<HttpResponseMessage> GetAsync(string? methodName, Dictionary<string, string> data, CancellationToken cancellationToken)
    {
        methodName = GetUrl(methodName ?? String.Empty, data);
        return GetAsync(methodName, cancellationToken);
    }

    public Task<TResponse> GetAsync<TResponse>(string? methodName, Dictionary<string, string> data, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = CreateRequest(HttpMethod.Get, methodName, data);
        return SendAsync<TResponse>(request, cancellationToken);
    }

    protected virtual string GetUrl(string url, Dictionary<string, string> properties)
    {
        foreach (var property in properties)
        {
            string value = property.Value ?? "";

            url = !url.Contains("?") ?
                $"{url}?{property.Key}={value}" :
                $"{url}&{property.Key}={value}";
        }

        return url;
    }

    public virtual Task<HttpResponseMessage> PostAsync(string? methodName, HttpContent? content, CancellationToken cancellationToken)
        => SendAsync(HttpMethod.Post, methodName, content, cancellationToken);

    public virtual Task<HttpResponseMessage> PostAsync<TRequest>(string? methodName, TRequest data, CancellationToken cancellationToken)
    {
        var request = CreateRequest(HttpMethod.Post, methodName, data);
        return SendAsync(request, cancellationToken);
    }

    public virtual Task<TResponse> PostAsync<TRequest, TResponse>(string? methodName, TRequest data, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = CreateRequest(HttpMethod.Post, methodName, data);
        return SendAsync<TResponse>(request, cancellationToken);
    }

    public virtual Task<HttpResponseMessage> PatchAsync(string? methodName, HttpContent? content, CancellationToken cancellationToken)
        => SendAsync(HttpMethod.Patch, methodName, content, cancellationToken);

    public virtual Task<HttpResponseMessage> PatchAsync<TRequest>(string? methodName, TRequest data, CancellationToken cancellationToken)
    {
        var request = CreateRequest(HttpMethod.Patch, methodName, data);
        return SendAsync(request, cancellationToken);
    }

    public virtual Task<TResponse> PatchAsync<TRequest, TResponse>(string? methodName, TRequest data, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = CreateRequest(HttpMethod.Post, methodName, data);
        return SendAsync<TResponse>(request, cancellationToken);
    }

    public virtual Task<HttpResponseMessage> PutAsync(string? methodName, HttpContent? content, CancellationToken cancellationToken)
        => SendAsync(HttpMethod.Put, methodName, content, cancellationToken);

    public virtual Task<HttpResponseMessage> PutAsync<TRequest>(string? methodName, TRequest data, CancellationToken cancellationToken)
    {
        var request = CreateRequest(HttpMethod.Put, methodName, data);
        return SendAsync(request, cancellationToken);
    }

    public virtual Task<TResponse> PutAsync<TRequest, TResponse>(string? methodName, TRequest data, CancellationToken cancellationToken)
    {
        var request = CreateRequest(HttpMethod.Put, methodName, data);
        return SendAsync<TResponse>(request, cancellationToken);
    }

    public virtual Task<HttpResponseMessage> DeleteAsync(string? methodName, HttpContent? content, CancellationToken cancellationToken)
        => SendAsync(HttpMethod.Delete, methodName, content, cancellationToken);

    public virtual Task<HttpResponseMessage> DeleteAsync<TRequest>(string? methodName, TRequest data, CancellationToken cancellationToken)
    {
        var request = CreateRequest(HttpMethod.Delete, methodName, data);
        return SendAsync(request, cancellationToken);
    }

    public virtual Task<TResponse> DeleteAsync<TRequest, TResponse>(string? methodName, TRequest data, CancellationToken cancellationToken)
    {
        var request = CreateRequest(HttpMethod.Delete, methodName, data);
        return SendAsync<TResponse>(request, cancellationToken);
    }
}
