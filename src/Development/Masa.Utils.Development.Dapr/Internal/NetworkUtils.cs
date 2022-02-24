namespace Masa.Utils.Development.Dapr.Internal;

internal class NetworkUtils
{
    public static string GetPhysicalAddress()
    {
        var firstMacAddress = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault();

        return firstMacAddress ?? string.Empty;
    }
}
