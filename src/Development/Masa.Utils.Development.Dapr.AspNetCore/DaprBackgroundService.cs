// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Development.Dapr.AspNetCore;

public class DaprBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDaprProcess _daprProcess;
    private readonly DaprOptions _options;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<DaprBackgroundService>? _logger;

    public DaprBackgroundService(
        IServiceProvider serviceProvider,
        IDaprProcess daprProcess,
        IOptionsMonitor<DaprOptions> options,
        IHostApplicationLifetime hostApplicationLifetime,
        ILogger<DaprBackgroundService>? logger)
    {
        _serviceProvider = serviceProvider;
        _daprProcess = daprProcess;
        _options = options.CurrentValue;
        options.OnChange(daprOptions =>
        {
            daprOptions.AppPort ??= GetAppPort(daprOptions);
            _daprProcess.Refresh(daprOptions);
        });
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
    }

    private ushort GetAppPort(DaprOptions options)
    {
        var server = _serviceProvider.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;
        if (addresses is { IsReadOnly: false, Count: 0 })
            throw new Exception("Failed to get the startup port, please specify the port manually");

        return addresses!
            .Select(address => new Uri(address))
            .Where(address
                => (options.EnableSsl is true && address.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
                || address.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
            .Select(address => (ushort)address.Port).FirstOrDefault();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!await WaitForAppStartup(_hostApplicationLifetime, stoppingToken))
            return;

        // if cancellation was requested, stop
        if (stoppingToken.IsCancellationRequested)
        {
            _logger?.LogInformation("{Name} is Stopping...", nameof(DaprBackgroundService));
            _daprProcess.Stop(stoppingToken);
        }
        else
        {
            _logger?.LogInformation("{Name} is Starting ...", nameof(DaprBackgroundService));
            _options.AppPort ??= GetAppPort(_options);
            _daprProcess.Start(_options, stoppingToken);
        }
    }

    static async Task<bool> WaitForAppStartup(IHostApplicationLifetime hostApplicationLifetime, CancellationToken stoppingToken)
    {
        var startedSource = new TaskCompletionSource();
        var cancelledSource = new TaskCompletionSource();

        await using var startedCancellationTokenRegistration = hostApplicationLifetime.ApplicationStarted.Register(() => startedSource.SetResult());
        await using var cancellationTokenRegistration = stoppingToken.Register(() => cancelledSource.SetResult());

        Task completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(false);

        // If the completed tasks was the "app started" task, return true, otherwise false
        return completedTask == startedSource.Task;
    }
}
