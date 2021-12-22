namespace MASA.Utils.Data.Elasticsearch;

public static class ServiceCollectionExtensions
{
    internal static List<ElasticsearchRelations> ElasticsearchRelations = new();

    public static IServiceCollection AddElasticsearch(this IServiceCollection services, string[] nodes)
        => services.AddElasticsearch(Const.DEFAULT_CLIENT_NAME, nodes);

    public static IServiceCollection AddElasticsearch(this IServiceCollection services, string name, params string[] nodes)
        => services.AddElasticsearch(name, options => options.UseNodes(nodes));

    public static IServiceCollection AddElasticsearch(this IServiceCollection services)
    {
        AddElasticsearchCore(services);

        string name = string.Empty;
        if (ElasticsearchRelations.All(r => r.Name != name))
        {
            AddElasticsearchRelation(name, new ElasticsearchOptions());
        }

        return services;
    }

    public static IServiceCollection AddElasticsearch(this IServiceCollection services, string name, Action<ElasticsearchOptions> action)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        AddElasticsearchCore(services);

        ElasticsearchOptions options = new();
        action.Invoke(options);
        TryAddElasticsearchRelation(name, options);

        return services;
    }

    public static IServiceCollection AddElasticsearchCore(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.TryAddSingleton<IElasticsearchFactory, DefaultElasticsearchFactory>();

        services.TryAddSingleton(serviceProvider =>
            serviceProvider.GetRequiredService<IElasticsearchFactory>().CreateElasticClient());

        services.TryAddSingleton<IMasaElasticClient>(serviceProvider =>
            new DefaultMasaElasticClient(serviceProvider.GetRequiredService<IElasticClient>()));

        return services;
    }

    private static void TryAddElasticsearchRelation(string name, ElasticsearchOptions options)
    {
        if (ElasticsearchRelations.Any(r => r.Name == name))
            throw new ArgumentException($"The ElasticClient whose name is {name} is exist");

        if (options.IsDefault && ElasticsearchRelations.Any(r => r.IsDefault))
            throw new ArgumentNullException("ElasticClient can only have one default");

        AddElasticsearchRelation(name, options);
    }

    private static void AddElasticsearchRelation(string name, ElasticsearchOptions options)
    {
        Uri[] nodes = options.Nodes.Select(uriString => new Uri(uriString)).ToArray();
        ElasticsearchRelations relation = new ElasticsearchRelations(name, options.UseConnectionPool, nodes)
            .UseStaticConnectionPoolOptions(options.StaticConnectionPoolOptions)
            .UseConnectionSettingsOptions(options.ConnectionSettingsOptions)
            .UseConnectionSettings(options.Action);
        ElasticsearchRelations.Add(relation);
    }
}
