namespace Masa.Utils.Data.Elasticsearch;

public static partial class ServiceCollectionExtensions
{
    private static IElasticClient CreateElasticsearchClient(this IServiceCollection services, string name)
        => services.BuildServiceProvider().GetRequiredService<IElasticsearchFactory>().CreateElasticClient(name);

    public static IElasticClient AddElasticsearchClient(this IServiceCollection services)
        => services.AddElasticsearch().CreateElasticsearchClient(Const.DEFAULT_CLIENT_NAME);

    public static IElasticClient AddElasticsearchClient(this IServiceCollection services, string[]? nodes)
        => services
            .AddElasticsearch(Const.DEFAULT_CLIENT_NAME, nodes == null || nodes.Length == 0 ? new[] {"http://localhost:9200"} : nodes)
            .CreateElasticsearchClient(Const.DEFAULT_CLIENT_NAME);

    public static IElasticClient AddElasticsearchClient(this IServiceCollection services, string name, params string[] nodes)
        => services.AddElasticsearch(name, nodes).CreateElasticsearchClient(name);

    public static IElasticClient AddElasticsearchClient(this IServiceCollection services, string name, Action<ElasticsearchOptions> action)
        => services.AddElasticsearch(name, action).CreateElasticsearchClient(name);

    public static IElasticClient AddElasticsearchClient(this IServiceCollection services, string name, Func<ElasticsearchOptions> func)
        => services.AddElasticsearch(name, func).CreateElasticsearchClient(name);
}
