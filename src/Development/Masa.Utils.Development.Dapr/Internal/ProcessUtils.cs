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
        => Run(fileName, arguments, out string _, createNoWindow, isWait);

    public System.Diagnostics.Process Run(
        string fileName,
        string arguments,
        out string response,
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
        }
        daprProcess.Start();
        daprProcess.Exited += (_, _) => OnExited();
        string command = daprProcess.ProcessName + arguments;
        _logger?.LogDebug("Process: {ProcessName}, Command: {Command}, PID: {ProcessId} executed successfully", daprProcess.ProcessName,
            command, daprProcess.Id);

        response = daprProcess.StandardOutput.ReadToEnd();
        if (isWait)
        {
            daprProcess.WaitForExit();
        }

        return daprProcess;
    }

    public event EventHandler Exit = default!;

    protected virtual void OnExited() => Exit(this, EventArgs.Empty);
}
