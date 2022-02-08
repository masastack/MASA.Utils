namespace MASA.Utils.Development.Dapr.Process;

public class ProcessProvider : IProcessProvider
{
    /// <summary>
    /// Get process collection based on process name
    /// </summary>
    /// <param name="processName"></param>
    /// <returns></returns>
    public IEnumerable<IProcess> GetProcesses(string processName)
        => System.Diagnostics.Process.GetProcessesByName(processName).Select(process => new SystemProcess(process));

    public IProcess GetProcess(int pId)
        => new SystemProcess(System.Diagnostics.Process.GetProcessById(pId));

    /// <summary>
    /// get available ports
    /// </summary>
    /// <param name="minPort">Minimum port (includes minimum port), default: 0</param>
    /// <param name="maxPort">Maximum ports (including maximum ports), default: 65535</param>
    /// <returns></returns>
    public int GetAvailablePorts(ushort? minPort = null, ushort? maxPort = null)
    {
        minPort = minPort ?? ushort.MinValue;
        maxPort = maxPort ?? ushort.MaxValue;
        var usePorts = GetPortsByUsed();

        var effectivePorts = Enumerable.Range(minPort.Value, maxPort.Value).Except(usePorts).ToList();
        if (effectivePorts.Count == 0)
            throw new Exception("... No port available exception");

        return effectivePorts.FirstOrDefault();
    }

    /// <summary>
    /// get the currently used port
    /// </summary>
    /// <returns>Port set that has been used</returns>
    private IEnumerable<int> GetPortsByUsed()
    {
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        var connectionEndPoints = ipGlobalProperties.GetActiveTcpConnections().Select(information => information.LocalEndPoint);
        var tcpListenerEndPoints = ipGlobalProperties.GetActiveTcpListeners();
        var udpListenerEndPoints = ipGlobalProperties.GetActiveUdpListeners();
        return connectionEndPoints
            .Concat(tcpListenerEndPoints)
            .Concat(udpListenerEndPoints)
            .Select(endPoint => endPoint.Port);
    }
}
