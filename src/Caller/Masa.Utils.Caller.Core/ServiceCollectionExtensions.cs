// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

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

        services.TryAddSingleton<ICallerFactory>(serviceProvider => new DefaultCallerFactory(serviceProvider, callerOption));
        services.TryAddSingleton<IRequestMessage>(_ => new JsonRequestMessage(callerOption.JsonSerializerOptions));
        services.TryAddSingleton<IResponseMessage>(serviceProvider
            => new DefaultResponseMessage(callerOption, serviceProvider.GetService<ILogger<DefaultResponseMessage>>()));
        services.TryAddScoped(serviceProvider => serviceProvider.GetRequiredService<ICallerFactory>().CreateClient());

        services.TryAddSingleton<ITypeConvertProvider, DefaultTypeConvertProvider>();
        services.AddAutomaticCaller(callerOption);
        CheckCallerOptions(callerOption);
        return services;
    }

    private static void CheckCallerOptions(CallerOptions options)
    {
        if (options.Callers.GroupBy(r => r.Name).Any(x => x.Count() > 1))
        {
            var callerName = options.Callers.GroupBy(r => r.Name).Where(x => x.Count() > 1).Select(r => r.Key).FirstOrDefault();
            throw new ArgumentException($"The caller name already exists, please change the name, the repeat name is [{callerName}]");
        }

        if (options.Callers.Where(r => r.IsDefault).GroupBy(r => r.IsDefault).Any(x => x.Count() > 1))
        {
            string errorCallerNames = string.Join("、", options.Callers
                .Where(relation => relation.IsDefault)
                .Select(relation => relation.Name)
                .Concat(options.Callers.Where(relation => relation.IsDefault).Select(relation => relation.Name))
                .Distinct());
            throw new ArgumentException(
                $"There can only be at most one default Caller Provider, and now the following Caller Providers are found to be default: {errorCallerNames}");
        }
    }

    private static void AddAutomaticCaller(this IServiceCollection services, CallerOptions callerOptions)
    {
        var callerTypes = callerOptions.Assemblies.SelectMany(x => x.GetTypes())
            .Where(type => typeof(CallerBase).IsAssignableFrom(type) && !type.IsAbstract).ToList();

        if (callerTypes.Count == 0)
            return;

        callerTypes.Arrangement().ForEach(type =>
        {
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(type, serviceProvider =>
            {
                var constructorInfo = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .MaxBy(constructor => constructor.GetParameters().Length)!;
                List<object> parameters = new();
                foreach (var parameter in constructorInfo.GetParameters())
                {
                    parameters.Add(serviceProvider.GetRequiredService(parameter.ParameterType));
                }
                var callerBase = (constructorInfo.Invoke(parameters.ToArray()) as CallerBase)!;
                callerBase.SetCallerOptions(callerOptions, type.FullName ?? type.Name);
                return callerBase;
            }, callerOptions.CallerLifetime);
            services.TryAdd(serviceDescriptor);
        });

        var serviceProvider = services.BuildServiceProvider();
        callerTypes.ForEach(type =>
        {
            var callerBase = (CallerBase)serviceProvider.GetRequiredService(type);
            callerBase.UseCallerExtension();
        });
    }
}
