using MASA.Utils.Development.Dapr.Internal;

namespace MASA.Utils.Development.Dapr;

/// <summary>
/// dapr startup configuration information
/// When the specified attribute is configured as null, the default value of the parameter is subject to the default value of dapr of the current version
/// </summary>
public class DaprOptions
{
    /// <summary>
    /// The id for your application, used for service discovery
    /// </summary>
    public string AppId { get; }

    private string _appIdDelimiter = "-";

    /// <summary>
    /// Separator used to splice AppId and AppIdSuffix
    /// default:- , AppIdDelimiter not support .
    /// </summary>
    public string AppIdDelimiter
    {
        get => _appIdDelimiter;
        set
        {
            if (value == ".")
            {
                throw new NotSupportedException("AppIdDelimiter does not support assignment.");
            }
        }
    }

    /// <summary>
    /// Appid suffix, the default is the current MAC address
    /// </summary>
    public string? AppIdSuffix { get; }

    /// <summary>
    /// The concurrency level of the application, otherwise is unlimited
    /// default: unlimited
    /// </summary>
    public string? MaxConcurrency { get; }

    /// <summary>
    /// The port your application is listening on
    /// </summary>
    public string AppPort { get; }

    /// <summary>
    /// The protocol (gRPC or HTTP) Dapr uses to talk to the application. Valid values are: http or grpc
    /// default: http
    /// </summary>
    public Protocol? AppProtocol { get; }

    /// <summary>
    /// Enable https when Dapr invokes the application
    /// default: false
    /// </summary>
    public bool? EnableSsl { get; }

    /// <summary>
    /// Dapr configuration file
    /// default:
    /// Linux & Mac: $HOME/.dapr/config.yaml
    /// Windows: %USERPROFILE%\.dapr\config.yaml
    /// </summary>
    public string? Config { get; }

    /// <summary>
    /// The path for components directory
    /// default:
    /// Linux & Mac: $HOME/.dapr/components
    /// Windows: %USERPROFILE%\.dapr\components
    /// </summary>
    public string? ComponentPath { get; }

    /// <summary>
    /// The gRPC port for Dapr to listen on
    /// default: 50001
    /// </summary>
    public int? DaprGrpcPort { get; }

    /// <summary>
    /// The HTTP port for Dapr to listen on
    /// default: 3500
    /// </summary>
    public int? DaprHttpPort { get; }

    /// <summary>
    /// Enable pprof profiling via an HTTP endpoint
    /// default: false
    /// </summary>
    public bool? EnableProfiling { get; }

    /// <summary>
    /// The log verbosity. Valid values are: debug, info, warn, error, fatal, or panic
    /// default: info
    /// </summary>
    public LogLevel? LogLevel { get; }

    /// <summary>
    /// default: localhost
    /// </summary>
    public string? PlacementHostAddress { get; }

    /// <summary>
    /// The port that Dapr sends its metrics information to
    /// default: DAPR_METRICS_PORT
    /// </summary>
    public string? MetricsPort { get; }

    /// <summary>
    /// The port for the profile server to listen on
    /// default: 7777
    /// </summary>
    public int? ProfilePort { get; }

    /// <summary>
    /// Path to a unix domain socket dir mount. If specified
    /// communication with the Dapr sidecar uses unix domain sockets for lower latency and greater throughput when compared to using TCP ports
    /// Not available on Windows OS
    /// </summary>
    public string UnixDomainSocket { get; }

    /// <summary>
    /// Max size of request body in MB.
    /// default: 4
    /// </summary>
    public int? DaprMaxRequestSize { get; }

    public DaprOptions(string appId, string appPort)
    {
        AppId = appId;
        AppPort = appPort;
    }

    private string GetAppId()
    {
        var appid = AppId;
        var appIdSuffix = AppIdSuffix ?? NetworkUtils.GetPhysicalAddress();
        if (string.IsNullOrEmpty(appIdSuffix))
        {
            return appid;
        }
        return $"{appid}{AppIdDelimiter}{appIdSuffix}";
    }

    private string AppendMaxConcurrency()
        => MaxConcurrency != null ? $" --app-max-concurrency {MaxConcurrency}" : "";

    private string AppendAppProtocol()
        => AppProtocol != null ? $" --app-protocol {AppProtocol.Value.ToString().ToLower()}" : "";

    private string AppendAppSsl()
        => EnableSsl != null ? $" --app-ssl {EnableSsl.Value.ToString().ToLower()}" : "";

    private string AppendComponentsPath()
        => ComponentPath != null ? $" --components-path {ComponentPath!}" : "";

    private string AppendConfig()
        => Config != null ? $" --config {Config!}" : "";

    private string AppendGrpcPort()
        => DaprGrpcPort != null ? $" --dapr-grpc-port {DaprGrpcPort.Value}" : "";

    private string AppendHttpPort()
        => DaprHttpPort != null ? $" --dapr-http-port {DaprHttpPort.Value}" : "";

    private string AppendLogLevel()
        => LogLevel != null ? $" --log-level {LogLevel.Value.ToString().ToLower()}" : "";

    private string AppendPlacementHostAddress()
        => PlacementHostAddress != null ? $" --placement-host-address {PlacementHostAddress}" : "";

    private string AppendProfiling()
        => EnableProfiling != null ? $" --enable-profiling {EnableProfiling.Value.ToString().ToLower()}" : "";

    private string AppendProfilePort()
        => ProfilePort != null ? $" --profile-port {ProfilePort.Value}" : "";

    private string AppendMetricsPort()
        => MetricsPort != null ? $" --metrics-port {MetricsPort}" : "";

    private string AppendHttpMaxRequestSize() =>
        DaprMaxRequestSize != null ? $" --dapr-http-max-request-size {DaprMaxRequestSize.Value}" : "";

    public override string ToString()
    {
        return $"--app-id {GetAppId()} --app-port {AppPort}{AppendMaxConcurrency()}";
    }
}

public enum Protocol
{
    Http = 1,
    GRpc
}

public enum LogLevel
{
    Debug = 1,
    Info,
    Warn,
    Error,
    Fatal,
    Panic
}
