namespace MASA.Utils.Data.Elasticsearch;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddElasticsearch(this IServiceCollection services)
    {
        if (services.Any(service => service.ImplementationType == typeof(ElasticsearchService)))
            return services;

        services.AddSingleton<ElasticsearchService>();

        AddElasticsearchCore(services);

        services.TryAddElasticsearchRelation(string.Empty, new());

        return services;
    }

    public static IServiceCollection AddElasticsearch(this IServiceCollection services, string[] nodes)
        => services.AddElasticsearch(Const.DEFAULT_CLIENT_NAME, nodes);

    public static IServiceCollection AddElasticsearch(this IServiceCollection services, string name, params string[] nodes)
        => services.AddElasticsearch(name, options => options.UseNodes(nodes));

    public static IServiceCollection AddElasticsearch(this IServiceCollection services, string name, Action<ElasticsearchOptions> action)
    {
        return services.AddElasticsearch(name, () =>
        {
            ElasticsearchOptions options = new();
            action.Invoke(options);
            return options;
        });
    }

    public static IServiceCollection AddElasticsearch(this IServiceCollection services, string name, Func<ElasticsearchOptions> func)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        AddElasticsearchCore(services);

        services.TryAddElasticsearchRelation(name, func.Invoke());

        return services;
    }

    private static IServiceCollection AddElasticsearchCore(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IElasticsearchFactory, DefaultElasticsearchFactory>();

        services.TryAddSingleton(serviceProvider =>
            serviceProvider.GetRequiredService<IElasticsearchFactory>().CreateElasticClient());

        services.TryAddSingleton<IMasaElasticClient>(serviceProvider =>
            new DefaultMasaElasticClient(serviceProvider.GetRequiredService<IElasticClient>()));

        services.TryAddSingleton(new ElasticsearchRelationsOptions());

        return services;
    }

    private static void TryAddElasticsearchRelation(this IServiceCollection services, string name, ElasticsearchOptions options)
    {
        var serviceProvider = services.BuildServiceProvider();
        var relationsOptions = serviceProvider.GetRequiredService<ElasticsearchRelationsOptions>();

        if (relationsOptions.Relations.Any(r => r.Name == name))
            throw new ArgumentException($"The ElasticClient whose name is {name} is exist");

        if (options.IsDefault && relationsOptions.Relations.Any(r => r.IsDefault))
            throw new ArgumentNullException("ElasticClient can only have one default");

        relationsOptions.AddRelation(name, options);
    }

    private class ElasticsearchService
    {
    }
}
