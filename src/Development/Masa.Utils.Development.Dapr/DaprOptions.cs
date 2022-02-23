namespace Masa.Utils.Development.Dapr;

/// <summary>
/// dapr startup configuration information
/// When the specified attribute is configured as null, the default value of the parameter is subject to the default value of dapr of the current version
/// </summary>
public class DaprOptions
{
    private string _appid = DefaultOptions.DefaultAppId;

    /// <summary>
    /// The id for your application, used for service discovery
    /// Required, no blanks allowed
    /// </summary>
    public string AppId
    {
        get => _appid;
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(AppId));

            _appid = value;
        }
    }

    private string _appIdDelimiter = Const.DEFAULT_APPID_DELIMITER;

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
                throw new NotSupportedException("AppIdDelimiter is not supported as .");
            }

            _appIdDelimiter = value;
        }
    }

    /// <summary>
    /// Appid suffix
    /// optional. the default is the current MAC address
    /// </summary>
    public string AppIdSuffix { get; set; } = DefaultOptions.DefaultAppidSuffix;

    /// <summary>
    /// The concurrency level of the application, otherwise is unlimited
    /// </summary>
    public string? MaxConcurrency { get; set; }

    /// <summary>
    /// The port your application is listening on
    /// </summary>
    public ushort? AppPort { get; set; }

    /// <summary>
    /// The protocol (gRPC or HTTP) Dapr uses to talk to the application. Valid values are: http or grpc
    /// </summary>
    public Protocol? AppProtocol { get; set; }

    /// <summary>
    /// Enable https when Dapr invokes the application
    /// </summary>
    public bool? EnableSsl { get; set; }

    /// <summary>
    /// Dapr configuration file
    /// default:
    /// Linux & Mac: $HOME/.dapr/config.yaml
    /// Windows: %USERPROFILE%\.dapr\config.yaml
    /// </summary>
    public string? Config { get; set; }

    /// <summary>
    /// The path for components directory
    /// default:
    /// Linux & Mac: $HOME/.dapr/components
    /// Windows: %USERPROFILE%\.dapr\components
    /// </summary>
    public string? ComponentPath { get; set; }

    /// <summary>
    /// The gRPC port for Dapr to listen on
    /// </summary>
    public ushort? DaprGrpcPort { get; set; }

    /// <summary>
    /// The HTTP port for Dapr to listen on
    /// </summary>
    public ushort? DaprHttpPort { get; set; }

    /// <summary>
    /// Enable pprof profiling via an HTTP endpoint
    /// </summary>
    public bool? EnableProfiling { get; set; }

    /// <summary>
    /// The image to build the code in. Input is: repository/image
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// The log verbosity. Valid values are: debug, info, warn, error, fatal, or panic
    /// default: info
    /// </summary>
    public LogLevel? LogLevel { get; set; }

    /// <summary>
    /// default: localhost
    /// </summary>
    public string? PlacementHostAddress { get; set; }

    /// <summary>
    /// The port that Dapr sends its metrics information to
    /// </summary>
    public string? MetricsPort { get; set; }

    /// <summary>
    /// The port for the profile server to listen on
    /// </summary>
    public int? ProfilePort { get; set; }

    /// <summary>
    /// Path to a unix domain socket dir mount. If specified
    /// communication with the Dapr sidecar uses unix domain sockets for lower latency and greater throughput when compared to using TCP ports
    /// Not available on Windows OS
    /// </summary>
    public string? UnixDomainSocket { get; set; }

    /// <summary>
    /// Max size of request body in MB.
    /// </summary>
    public int? DaprMaxRequestSize { get; set; }

    /// <summary>
    /// Heartbeat detection interval, used to detect dapr status
    /// default: 5000 ms
    /// </summary>
    public int? HeartBeatInterval { get; set; }

    public bool CreateNoWindow { get; set; } = true;
}
