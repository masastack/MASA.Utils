namespace Masa.Utils.Development.Dapr.Internal;

internal class DefaultOptions
{
    public static string DefaultAppId => ((Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName().Name ??
        throw new NotSupportedException("dapr appid is not empty")).Replace(".", Const.DEFAULT_APPID_DELIMITER);

    /// <summary>
    /// Appid suffix, the default is the current MAC address
    /// </summary>
    public static string DefaultAppidSuffix = NetworkUtils.GetPhysicalAddress();
}
