namespace Masa.Utils.Caller.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaller(this IServiceCollection services)
        => services.AddCaller(AppDomain.CurrentDomain.GetAssemblies());

    public static IServiceCollection AddCaller(this IServiceCollection services, params Assembly[] assemblies)
        => services.AddCaller(options => options.Assemblies = assemblies);

    private static IServiceCollection AddCaller(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        params Assembly[] assemblies)
        => services.AddCaller(options =>
        {
            options.Assemblies = assemblies;
            options.CallerLifetime = lifetime;
        });

    public static IServiceCollection AddCaller(this IServiceCollection services, Action<CallerOptions> options)
    {
        CallerOptions callerOption = new CallerOptions(services);
        options.Invoke(callerOption);

        services.AddAutomaticCaller(callerOption);
        services.TryOrUpdateCallerOptions(callerOption);
        services.TryAddSingleton<ICallerFactory, DefaultCallerFactory>();
        services.TryAddSingleton<IRequestMessage, DefaultRequestMessage>();
        services.TryAddTransient(serviceProvider => serviceProvider.GetRequiredService<ICallerFactory>().CreateClient());

        if (callerOption.Callers.Count == 0)
            throw new ArgumentException("Caller provider is not found, check if Caller is used");

        if (callerOption.Callers.Count(c => c.IsDefault) > 1)
            throw new ArgumentException("Caller provider can only have one default");

        return services;
    }

    private static IServiceCollection TryOrUpdateCallerOptions(this IServiceCollection services, CallerOptions options)
    {
        services.TryAddSingleton(new CallerOptions(options.Services));
        var serviceProvider = services.BuildServiceProvider();
        var callerOptions = serviceProvider.GetRequiredService<CallerOptions>();

        options.Callers.ForEach(caller =>
        {
            if (callerOptions.Callers.Any(relation => relation.Name == caller.Name))
                throw new ArgumentException(
                    $"The current name already exists, please change the name, the repeat caller name is {caller.Name}");

            if (callerOptions.Callers.Any(relation => relation.IsDefault && caller.IsDefault))
                throw new ArgumentException($"Caller provider can only have one default, the caller name is {caller.Name}");

            callerOptions.Callers.Add(caller);
        });

        return services;
    }

    private static void AddAutomaticCaller(this IServiceCollection services, CallerOptions callerOptions)
    {
        var callerTypes = callerOptions.Assemblies.SelectMany(x => x.GetTypes())
            .Where(type => typeof(CallerBase).IsAssignableFrom(type) && !type.IsAbstract).ToList();
        callerTypes.ForEach(type =>
        {
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(type, type, callerOptions.CallerLifetime);
            services.TryAdd(serviceDescriptor);
        });

        callerTypes.ForEach(type =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var callerBase = (CallerBase)serviceProvider.GetRequiredService(type);
            callerBase.SetCallerOptions(callerOptions);
            callerBase.UseCallerExtension();
        });
    }
}
