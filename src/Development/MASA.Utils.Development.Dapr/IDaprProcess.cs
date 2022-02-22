namespace MASA.Utils.Development.Dapr;

public interface IDaprProcess
{
    void Start(DaprOptions options, CancellationToken cancellationToken = default);

    void Stop(CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh the dapr configuration, the source dapr process will be killed and the new dapr process will be restarted
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    void Refresh(DaprOptions options, CancellationToken cancellationToken = default);
}
