namespace MASA.Utils.Caller.Core;

public interface IRequestMessage
{
    Task<TResponse?> ProcessResponseAsync<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken = default);
}
