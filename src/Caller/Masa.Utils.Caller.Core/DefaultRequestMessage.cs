namespace Masa.Utils.Caller.Core;

public class DefaultRequestMessage : IRequestMessage
{
    private readonly ILogger<DefaultRequestMessage>? _logger;
    private readonly CallerOptions _callerOptions;

    public DefaultRequestMessage(CallerOptions callerOptions, ILogger<DefaultRequestMessage>? logger)
    {
        _callerOptions = callerOptions;
        _logger = logger;
    }

    public async Task<TResponse?> ProcessResponseAsync<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        if (response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Accepted:
                case HttpStatusCode.NoContent:
                    return default;
                case (HttpStatusCode)MasaHttpStatusCode.UserFriendlyException:
                    throw new UserFriendlyException(await response.Content.ReadAsStringAsync(cancellationToken));
                default:
                    if (typeof(TResponse) == typeof(Guid) || typeof(TResponse) == typeof(Guid?))
                    {
                        var content = await response.Content.ReadAsStringAsync(cancellationToken);
                        if (string.IsNullOrEmpty(content))
                            return (TResponse)(object?)null!;
                        return (TResponse?)(object)Guid.Parse(content);
                    }
                    if (typeof(TResponse).GetInterfaces().Any(type => type == typeof(IConvertible)))
                    {
                        var content = await response.Content.ReadAsStringAsync(cancellationToken);
                        return (TResponse)Convert.ChangeType(content, typeof(TResponse));
                    }
                    try
                    {
                        return await response.Content.ReadFromJsonAsync<TResponse>(_callerOptions.JsonSerializerOptions, cancellationToken)
                            ?? throw new ArgumentException("The response cannot be empty or there is an error in deserialization");
                    }
                    catch (Exception exception)
                    {
                        _logger?.LogWarning(exception, exception.Message ?? string.Empty);
                        ExceptionDispatchInfo.Capture(exception).Throw();
                        return default;//This will never be executed, the previous line has already thrown an exception
                    }
            }
        }
        throw new Exception(await response.Content.ReadAsStringAsync(cancellationToken));
    }

    public async Task ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        if (response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case (HttpStatusCode)MasaHttpStatusCode.UserFriendlyException:
                    throw new UserFriendlyException(await response.Content.ReadAsStringAsync(cancellationToken));
                default:
                    return;
            }
        }
        throw new Exception(await response.Content.ReadAsStringAsync(cancellationToken));
    }
}
