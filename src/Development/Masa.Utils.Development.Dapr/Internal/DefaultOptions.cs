namespace Masa.Utils.Development.Dapr.Internal;

internal class DefaultOptions
{
    public static string DefaultAppId => ((Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName().Name ??
        throw new NotSupportedException("dapr appid is not empty")).Replace(".", Const.DEFAULT_APPID_DELIMITER);

    /// <summary>
    /// Appid suffix, the default is the current MAC address
    /// </summary>
    public static string DefaultAppidSuffix = NetworkUtils.GetPhysicalAddress();

    ///// <summary>
    ///// Dapr configuration file
    ///// default:
    ///// Linux & Mac: $HOME/.dapr/config.yaml
    ///// Windows: %USERPROFILE%\.dapr\config.yaml
    ///// </summary>
    //public static string Config = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "%USERPROFILE%\\.dapr\\config.yaml" :
    //    RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "$HOME/.dapr/config.yaml" :
    //    throw new NotSupportedException("Unsupported platform");

    ///// <summary>
    ///// The path for components directory
    ///// default:
    ///// Linux & Mac: $HOME/.dapr/components
    ///// Windows: %USERPROFILE%\.dapr\components
    ///// </summary>
    //public static string ComponentPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "%USERPROFILE%\\.dapr\\components" :
    //    RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "$HOME/.dapr/components" :
    //    throw new NotSupportedException("Unsupported platform");
}
