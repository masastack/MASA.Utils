namespace MASA.Utils.Caller.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaller(this IServiceCollection services, Action<CallerOptions> options)
    {
        if (services.Any(service => service.ImplementationType == typeof(CallerService)))
            return services;

        services.AddSingleton<CallerService>();

        CallerOptions callerOption = new CallerOptions(services);
        options.Invoke(callerOption);

        if (callerOption.Callers.Count == 0)
            throw new ArgumentNullException("Caller provider is not found, check if Caller is used");

        if (callerOption.Callers.Count(c => c.IsDefault) > 1)
            throw new ArgumentNullException("Caller provider can only have one default");

        services.AddSingleton(typeof(ICallerFactory), serviceProvider =>
        {
            return new DefaultCallerFactory(serviceProvider, callerOption.Callers);
        });
        ServiceCollectionDescriptorExtensions.TryAddTransient(services, serviceProvider => serviceProvider.GetRequiredService<ICallerFactory>().CreateClient());
        return services;
    }

    private class CallerService
    {

    }
}
