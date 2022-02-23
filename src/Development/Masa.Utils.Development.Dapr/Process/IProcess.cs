namespace Masa.Utils.Development.Dapr.Process;

public interface IProcess
{
    int PId { get; }

    public string Name { get; }

    void Kill();

    bool Start();

    void WaitForExit(int? milliseconds = null);
}
