namespace MASA.Utils.Data.Elasticsearch;

public class DefaultMasaElasticClient : IMasaElasticClient
{
    private readonly IElasticClient _elasticClient;

    public DefaultMasaElasticClient(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

}
