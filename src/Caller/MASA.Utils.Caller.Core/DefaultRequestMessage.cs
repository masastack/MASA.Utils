namespace MASA.Utils.Caller.Core;

public class DefaultRequestMessage : IRequestMessage
{
    private readonly CallerOptions _callerOptions;

    public DefaultRequestMessage(CallerOptions callerOptions)
    {
        _callerOptions = callerOptions;
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
                    if (typeof(TResponse).GetInterfaces().Any(type => type == typeof(IConvertible)))
                    {
                        var content = await response.Content.ReadAsStringAsync(cancellationToken);
                        return (TResponse)Convert.ChangeType(content, typeof(TResponse));
                    }
                    return await response.Content.ReadFromJsonAsync<TResponse>(_callerOptions.JsonSerializerOptions, cancellationToken)
                        ?? throw new ArgumentException("Response cannot be empty");
            }
        }
        throw new Exception(await response.Content.ReadAsStringAsync(cancellationToken));
    }
}
