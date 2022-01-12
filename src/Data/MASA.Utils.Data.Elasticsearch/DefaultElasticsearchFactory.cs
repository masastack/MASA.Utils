namespace MASA.Utils.Data.Elasticsearch;

public class DefaultElasticsearchFactory : IElasticsearchFactory
{
    private readonly Dictionary<string, ElasticsearchRelations> _relations;
    private readonly ConcurrentDictionary<string, IElasticClient> _elasticClients;

    public DefaultElasticsearchFactory(ElasticsearchRelationsOptions options)
    {
        _relations = options.Relations;
        _elasticClients = new();
    }

    public IMasaElasticClient CreateClient()
    {
        return new DefaultMasaElasticClient(CreateElasticClient());
    }

    public IMasaElasticClient CreateClient(string name)
    {
        return new DefaultMasaElasticClient(CreateElasticClient(name));
    }

    public IElasticClient CreateElasticClient()
    {
        var elasticsearchRelation = _relations.Values.SingleOrDefault(r => r.IsDefault) ?? _relations.Values.FirstOrDefault();

        if (elasticsearchRelation == null)
            throw new Exception("The default ElasticClient is not found, please check if Elasticsearch is added");

        return GetOrAddElasticClient(elasticsearchRelation.Name);
    }

    public IElasticClient CreateElasticClient(string name)
    {
        if (!_relations.ContainsKey(name))
            throw new NotSupportedException($"The ElasticClient whose name is {name} is not found");

        return GetOrAddElasticClient(name);
    }

    private IElasticClient GetOrAddElasticClient(string name)
        => _elasticClients.GetOrAdd(name, name => Create(name));

    private IElasticClient Create(string name)
    {
        var relation = _relations[name];

        var settings = relation.UseConnectionPool
            ? GetConnectionSettingsConnectionPool(relation)
            : GetConnectionSettingsBySingleNode(relation);

        return new ElasticClient(settings);
    }

    private ConnectionSettings GetConnectionSettingsBySingleNode(ElasticsearchRelations relation) => new(relation.Nodes[0]);

    private ConnectionSettings GetConnectionSettingsConnectionPool(ElasticsearchRelations relation)
    {
        var pool = new StaticConnectionPool(
            relation.Nodes,
            relation.StaticConnectionPoolOptions?.Randomize ?? true,
            relation.StaticConnectionPoolOptions?.DateTimeProvider);

        var settings = new ConnectionSettings(
            pool,
            relation.ConnectionSettingsOptions?.Connection,
            relation.ConnectionSettingsOptions?.SourceSerializerFactory,
            relation.ConnectionSettingsOptions?.PropertyMappingProvider);

        relation.Action?.Invoke(settings);
        return settings;
    }
}
