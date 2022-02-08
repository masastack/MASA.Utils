namespace MASA.Utils.Development.Dapr;

public interface IDaprProcess
{
    void Start(DaprOptions options, CancellationToken cancellationToken = default);

    void Stop(CancellationToken cancellationToken = default);
}
