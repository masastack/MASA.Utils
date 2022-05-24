// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Development.Dapr.Internal;

internal class ProcessUtils
{
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
        _logger?.LogDebug("FileName: {FileName}, Arguments: {Arguments}", fileName, arguments);
        var processStartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = !createNoWindow,
            CreateNoWindow = createNoWindow
        };
        var daprProcess = new System.Diagnostics.Process()
        {
            StartInfo = processStartInfo,
        };
        if (createNoWindow)
        {
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;

            daprProcess.OutputDataReceived += (_, args) => OnOutputDataReceived(args);
            daprProcess.ErrorDataReceived += (_, args) => OnErrorDataReceived(args);
        }
        daprProcess.Start();
        if (createNoWindow)
        {
            daprProcess.BeginOutputReadLine();
            daprProcess.BeginErrorReadLine();
        }
        daprProcess.Exited += (_, _) => OnExited();
        string command = daprProcess.ProcessName + arguments;
        _logger?.LogInformation("Process: {ProcessName}, Command: {Command}, PID: {ProcessId} executed successfully", daprProcess.ProcessName, command, daprProcess.Id);

        if (isWait)
        {
            daprProcess.WaitForExit();
        }
        return daprProcess;
    }

    public event EventHandler<DataReceivedEventArgs> OutputDataReceived = default!;

    public event EventHandler<DataReceivedEventArgs>? ErrorDataReceived;

    public event EventHandler Exit = default!;

    protected virtual void OnOutputDataReceived(DataReceivedEventArgs args)
    {
        try
        {
            OutputDataReceived(this, args);
        }
        catch (Exception ex)
        {
            _logger?.LogError("ProcessUtils: error in output information ", ex);
        }
    }

    protected virtual void OnErrorDataReceived(DataReceivedEventArgs args)
    {
        try
        {
            ErrorDataReceived?.Invoke(this, args);
        }
        catch (Exception ex)
        {
            _logger?.LogError("execution error", ex);
        }
    }

    protected virtual void OnExited() => Exit(this, EventArgs.Empty);
}
