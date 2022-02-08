namespace MASA.Utils.Development.Dapr.Process;

public interface IProcess
{
    void Kill();

    bool Start();

    void WaitForExit(int? milliseconds = null);
}
