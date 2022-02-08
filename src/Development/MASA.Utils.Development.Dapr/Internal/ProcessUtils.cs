namespace MASA.Utils.Development.Dapr.Internal;

internal class ProcessUtils
{
    private System.Diagnostics.Process _daprProcess;
    private readonly ILogger<ProcessUtils>? _logger;

    public ProcessUtils(ILoggerFactory? loggerFactory = null)
    {
        _logger = loggerFactory?.CreateLogger<ProcessUtils>();
    }

    public System.Diagnostics.Process Run(
        string fileName,
        string arguments,
        bool createNoWindow = true,
        bool isWait = false)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = !createNoWindow,
            CreateNoWindow = createNoWindow
        };
        _daprProcess = new System.Diagnostics.Process()
        {
            StartInfo = processStartInfo,
        };
        if (createNoWindow)
        {
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;

            _daprProcess.OutputDataReceived += (sender, args) => OnOutputDataReceived(args);
            _daprProcess.ErrorDataReceived += (sender, args) => OnErrorDataReceived(args);
        }
        _daprProcess.Start();
        if (createNoWindow)
        {
            _daprProcess.BeginOutputReadLine();
            _daprProcess.BeginErrorReadLine();
        }
        _daprProcess.Exited += (sender, args) => OnExited();
        _logger?.LogInformation("Process {ProcessName} PID:{ProcessId} started successfully", _daprProcess.ProcessName, _daprProcess.Id);

        if (isWait)
        {
            _daprProcess.WaitForExit();
        }
        return _daprProcess;
    }

    public event EventHandler<System.Diagnostics.DataReceivedEventArgs> OutputDataReceived = default!;

    public event EventHandler<System.Diagnostics.DataReceivedEventArgs> ErrorDataReceived = default!;

    public event EventHandler Exit = default!;

    protected virtual void OnOutputDataReceived(System.Diagnostics.DataReceivedEventArgs args) => OutputDataReceived?.Invoke(this, args);

    protected virtual void OnErrorDataReceived(System.Diagnostics.DataReceivedEventArgs args) => ErrorDataReceived?.Invoke(this, args);

    protected virtual void OnExited() => Exit(this, EventArgs.Empty);
}
