namespace MASA.Utils.Development.Dapr;

public class DaprProvider : IDaprProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DaprProvider>? _logger = null;
    private readonly ProcessUtils _processUtils;

    public DaprProvider(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        _serviceProvider = serviceProvider;
        _logger = loggerFactory?.CreateLogger<DaprProvider>();
        _processUtils = new ProcessUtils(loggerFactory);
    }

    public List<DaprRuntimeOptions> GetDaprList(string appId)
    {
        var stringBuilder = new StringBuilder();
        _processUtils.OutputDataReceived += delegate (object? sender, DataReceivedEventArgs args)
            {
                if (args.Data != null)
                {
                    lock (stringBuilder)
                    {
                        stringBuilder.AppendLine(args.Data);
                    }
                }
            };
        _processUtils.Exit += delegate
        {

        };
        _processUtils.Run(Const.DEFAULT_FILE_NAME, "list -o json", true, true);
        string response = stringBuilder.ToString().Trim();
        List<DaprRuntimeOptions> daprList = new();
        if (response.StartsWith("["))
        {
            daprList = System.Text.Json.JsonSerializer.Deserialize<List<DaprRuntimeOptions>>(response) ?? new();
        }
        else if (response.StartsWith("{"))
        {
            var option = System.Text.Json.JsonSerializer.Deserialize<DaprRuntimeOptions>(response);
            if (option != null)
            {
                daprList.Add(option);
            }
        }
        _logger?.LogWarning("----- Failed to get currently running dapr collection");
        return daprList.Where(dapr => dapr.AppId == appId).ToList();
    }
}
