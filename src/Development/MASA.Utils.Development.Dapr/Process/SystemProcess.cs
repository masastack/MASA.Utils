namespace MASA.Utils.Development.Dapr.Process;

public class SystemProcess : IProcess
{
    private readonly System.Diagnostics.Process _process;

    public SystemProcess(System.Diagnostics.Process process)
    {
        _process = process;
    }

    public void Kill() => _process.Kill();

    public bool Start() => _process.Start();

    public void WaitForExit(int? milliseconds = null)
    {
        if (milliseconds.HasValue && milliseconds > 0)
        {
            _process.WaitForExit(milliseconds.Value);
        }
        else if (!_process.HasExited)
        {
            _process.WaitForExit();
        }
        else
        {
            _process.Kill();
        }
    }
}
