namespace MASA.Utils.Development.Dapr;

public class DaprProcess : IDaprProcess
{
    private readonly object _lock = new object();

    private readonly CommandLineBuilder _commandLineBuilder;
    private readonly IDaprProvider _daprProvider;
    private readonly IProcessProvider _processProvider;
    private readonly ILoggerFactory? _loggerFactory;
    private IProcess _process = default!;
    private DaprOptions _daprOptions;
    private DaprProcessStatus Status { get; set; }

    public DaprProcess(IDaprProvider daprProvider, IProcessProvider processProvider, ILoggerFactory? loggerFactory)
    {
        _commandLineBuilder = new(Const.DEFAULT_ARGUMENT_PREFIX);
        _daprProvider = daprProvider;
        _processProvider = processProvider;
        _loggerFactory = loggerFactory;
    }

    public void Start(DaprOptions options, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            Initialize(options);
            string appId = GetAppId(options);

            List<DaprRuntimeOptions> daprList = _daprProvider.GetDaprList(appId);
            if (daprList.Count > 0)
            {
                foreach (var dapr in daprList)
                {
                    _process = _processProvider.GetProcess(dapr.PId);
                    if (dapr.AppPort == GetAppPort(options) &&
                        (options.DaprHttpPort == null || options.DaprHttpPort == dapr.HttpPort) &&
                        (options.DaprGrpcPort == null || options.DaprGrpcPort == dapr.GrpcPort))
                    {
                        CompleteDaprEnvironment(dapr.HttpPort.ToString(), dapr.GrpcPort.ToString());
                        return;
                    }
                    _process.Kill();
                }
            }

            var utils = new ProcessUtils(_loggerFactory);
            utils.OutputDataReceived += DaprProcess_OutputDataReceived;
            utils.ErrorDataReceived += DaprProcess_ErrorDataReceived;
            utils.Exit += delegate
            {

            };
            var process = utils.Run(Const.DEFAULT_FILE_NAME, $"run {_commandLineBuilder}", options.CreateNoWindow);
            _process = new SystemProcess(process);

            daprList = _daprProvider.GetDaprList(appId);
            if (daprList.Count > 0)
            {
                var currentDapr = daprList.FirstOrDefault(dapr => dapr.AppId == GetAppId(options));
                if (currentDapr != null)
                {
                    CompleteDaprEnvironment(currentDapr.HttpPort.ToString(), currentDapr.GrpcPort.ToString());
                }
            }
        }
    }

    private static void DaprProcess_OutputDataReceived(object? sender, DataReceivedEventArgs e)
    {
        if (e.Data == null) return;

        var dataSpan = e.Data.AsSpan();
        var levelStartIndex = e.Data.IndexOf("level=", StringComparison.Ordinal) + 6;
        var level = "information";
        if (levelStartIndex > 5)
        {
            var levelLength = dataSpan.Slice(levelStartIndex).IndexOf(' ');
            level = dataSpan.Slice(levelStartIndex, levelLength).ToString();
        }

        var color = Console.ForegroundColor;
        switch (level)
        {
            case "warning":
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case "error":
            case "critical":
            case "fatal":
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            default:
                break;
        }

        Console.WriteLine(e.Data);
        Console.ForegroundColor = color;
    }

    private static void DaprProcess_ErrorDataReceived(object? sender, DataReceivedEventArgs e)
    {
        if (e.Data == null) return;

        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.Data);
        Console.ForegroundColor = color;
    }

    public void Stop(CancellationToken cancellationToken = default)
    {
        _process.WaitForExit();
    }

    private bool Restart(CancellationToken cancellationToken)
    {
        Stop(cancellationToken);

        return true;
    }

    private void Initialize(DaprOptions options)
    {
        options.AppId = GetAppId(options);
        options.AppPort = GetAppPort(options);

        _commandLineBuilder
            .Add("app-id", options.AppId!)
            .Add("app-port", options.AppPort.Value.ToString())
            .Add("app-protocol", options.AppProtocol?.ToString().ToLower() ?? string.Empty, options.AppProtocol == null)
            .Add("app-ssl", options.EnableSsl?.ToString().ToLower() ?? "", options.EnableSsl == null)
            .Add("components-path", options.ComponentPath ?? string.Empty, options.ComponentPath == null)
            .Add("config", options.Config ?? string.Empty, options.Config == null)
            .Add("dapr-grpc-port", options.DaprGrpcPort?.ToString() ?? string.Empty, options.DaprGrpcPort == null)
            .Add("dapr-http-port", options.DaprHttpPort?.ToString() ?? string.Empty, options.DaprHttpPort == null)
            .Add("enable-profiling", options.EnableProfiling?.ToString().ToLower() ?? string.Empty, options.EnableProfiling == null)
            .Add("image", options.Image ?? string.Empty, options.Image == null)
            .Add("log-level", options.LogLevel?.ToString().ToLower() ?? string.Empty, options.LogLevel == null)
            .Add("metrics-port", options.MetricsPort ?? string.Empty, options.MetricsPort == null)
            .Add("profile-port", options.ProfilePort?.ToString() ?? string.Empty, options.ProfilePort == null)
            .Add("unix-domain-socket", options.UnixDomainSocket ?? string.Empty, options.UnixDomainSocket == null)
            .Add("dapr-http-max-request-size", options.DaprMaxRequestSize?.ToString() ?? string.Empty, options.DaprMaxRequestSize == null);

        _daprOptions = options;
    }

    private string GetAppId(DaprOptions options) =>
        string.IsNullOrEmpty(options.AppIdSuffix) ? options.AppId : $"{options.AppId}{options.AppIdDelimiter}{options.AppIdSuffix}";

    private int GetAppPort(DaprOptions options) =>
        options.AppPort ?? throw new ArgumentNullException(nameof(options.AppPort));

    public void UpdateStatus(DaprProcessStatus status)
    {
        if (status != Status)
        {
            _loggerFactory?.CreateLogger<DaprProcess>().LogInformation("Dapr Process Status Change: {DaprProcessPreviousStatus} -> {DaprProcessStatus}", Status, status);
            Status = status;
        }
    }

    private static void CompleteDaprEnvironment(string daprHttpPort, string daprGrpcPort)
    {
        EnvironmentUtils.TryAdd("DAPR_GRPC_PORT", () => daprHttpPort);
        EnvironmentUtils.TryAdd("DAPR_HTTP_PORT", () => daprGrpcPort);
    }
}
