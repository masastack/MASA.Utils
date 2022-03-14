namespace Masa.Utils.Caller.DaprClient;

public static class CallerOptionsExtensions
{
    public static CallerOptions UseDapr(this CallerOptions callerOptions, Func<MasaDaprClientBuilder> clientBuilder)
    {
        if (clientBuilder == null)
            throw new ArgumentNullException(nameof(clientBuilder));

        MasaDaprClientBuilder builder = clientBuilder.Invoke();
        if (clientBuilder == null)
            throw new ArgumentNullException(nameof(clientBuilder));

        callerOptions.Services.AddDaprClient(builder.Configure);
        AddCallerExtensions.AddCaller(callerOptions, builder.Name, builder.IsDefault, (serviceProvider) =>
        {
            var daprClient = serviceProvider.GetRequiredService<Dapr.Client.DaprClient>();
            var requestMessage = serviceProvider.GetRequiredService<IRequestMessage>();
            return new DaprCallerProvider(builder.AppId, requestMessage, daprClient);
        });
        return callerOptions;
    }

    public static CallerOptions UseDapr(this CallerOptions callerOptions, Action<MasaDaprClientBuilder> clientBuilder)
    {
        if (clientBuilder == null)
            throw new ArgumentNullException(nameof(clientBuilder));

        MasaDaprClientBuilder builder = new MasaDaprClientBuilder();
        clientBuilder.Invoke(builder);

        return callerOptions.UseDapr(() => builder);
    }
}
