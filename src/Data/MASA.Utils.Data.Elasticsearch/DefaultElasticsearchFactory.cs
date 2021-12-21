namespace MASA.Utils.Data.Elasticsearch;

public class DefaultElasticsearchFactory : IElasticsearchFactory
{
    private readonly List<ElasticsearchRelations> _relations;
    private ConcurrentDictionary<string, IElasticClient> _elasticClients;

    public DefaultElasticsearchFactory()
    {
        _relations = ServiceCollectionExtensions.ElasticsearchRelations;
        _elasticClients = new();
    }

    public IElasticClient CreateElasticClient()
    {
        var elasticsearchRelation = _relations.SingleOrDefault(r => r.IsDefault) ?? _relations.FirstOrDefault();
        if (elasticsearchRelation == null)
            throw new Exception("The default ElasticClient is not found, please check if Elasticsearch is added");

        return GetOrAddElasticClient(elasticsearchRelation.Name);
    }

    public IElasticClient CreateElasticClient(string name)
    {
        if (_relations.All(r => r.Name != name))
            throw new NotSupportedException($"The ElasticClient whose name is {name} is not found");

        return GetOrAddElasticClient(name);
    }

    private IElasticClient GetOrAddElasticClient(string name)
    {
        if (_elasticClients.ContainsKey(name))
        {
            return _elasticClients[name];
        }

        var client = Create(name);
        _elasticClients.TryAdd(name, client);
        return client;
    }

    private IElasticClient Create(string name)
    {
        var relation = _relations.Single(r => r.Name == name);

        var settings = relation.UseConnectionPool
            ? GetConnectionSettingsConnectionPool(relation)
            : GetConnectionSettingsBySingleNode(relation);

        return new ElasticClient(settings);
    }

    private ConnectionSettings GetConnectionSettingsBySingleNode(ElasticsearchRelations relation) => new(relation.Nodes[0]);

    private ConnectionSettings GetConnectionSettingsConnectionPool(ElasticsearchRelations relation)
    {
        var pool = new StaticConnectionPool(relation.Nodes, relation.StaticConnectionPoolOptions.Randomize,
            relation.StaticConnectionPoolOptions.DateTimeProvider);
        var settings = new ConnectionSettings(pool, relation.ConnectionSettingsOptions.Connection,
            relation.ConnectionSettingsOptions.SourceSerializerFactory, relation.ConnectionSettingsOptions.PropertyMappingProvider);
        return settings;
    }
}
