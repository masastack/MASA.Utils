namespace MASA.Utils.Development.Dapr.AspNetCore;

public class DaprBackgroundService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDaprProcess _daprProcess;
    private readonly IOptions<DaprOptions> _options;
    private readonly ILogger<DaprBackgroundService>? _logger;

    public DaprBackgroundService(
        IServiceProvider serviceProvider,
        IDaprProcess daprProcess,
        IOptions<DaprOptions> options,
        ILogger<DaprBackgroundService>? logger)
    {
        _serviceProvider = serviceProvider;
        _daprProcess = daprProcess;
        _options = options;
        _logger = logger;
    }

    private ushort GetAppPort(DaprOptions options)
    {
        var server = _serviceProvider.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;
        if (addresses != null && !addresses.IsReadOnly && addresses.Count == 0)
        {
            throw new Exception("Failed to get the startup port, please specify the port manually");
        }

        return addresses!
            .Select(address => new Uri(address))
            .Where(address
                => (options.EnableSsl != null && options.EnableSsl == true &&
                    address.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
                || address.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
            .Select(address => (ushort)address.Port).FirstOrDefault();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger?.LogInformation("{Name} is Starting ...",nameof(DaprBackgroundService));
        System.Timers.Timer timer = new System.Timers.Timer(3000);
        timer.Elapsed += delegate
        {
            var options = _options.Value;
            options.AppPort ??= GetAppPort(options);
            _daprProcess.Start(options, cancellationToken);
        };
        timer.AutoReset = false;
        timer.Enabled = true;
        timer.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger?.LogInformation("{Name} is Stopping...",nameof(DaprBackgroundService));
        _daprProcess.Stop();
        return Task.CompletedTask;
    }
}
