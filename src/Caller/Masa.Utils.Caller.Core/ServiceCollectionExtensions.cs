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
                    $"The caller name already exists, please change the name, the repeat name is {caller.Name}");

            if (callerOptions.Callers.Any(relation => relation.IsDefault && caller.IsDefault))
            {
                string errorCallerNames = string.Join("、", callerOptions.Callers
                    .Where(relation => relation.IsDefault)
                    .Select(relation => relation.Name)
                    .Concat(options.Callers.Where(relation => relation.IsDefault).Select(relation => relation.Name))
                    .Distinct());
                throw new ArgumentException($"There can only be at most one default Caller Provider, and now the following Caller Providers are found to be default: {errorCallerNames}");
            }

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
            callerBase.SetCallerOptions(callerOptions, type.FullName ?? type.Name);
            callerBase.UseCallerExtension();
        });
    }
}
