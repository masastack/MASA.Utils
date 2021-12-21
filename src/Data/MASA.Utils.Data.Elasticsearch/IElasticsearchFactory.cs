namespace MASA.Utils.Data.Elasticsearch;

public interface IElasticsearchFactory
{
    IElasticClient CreateElasticClient();

    IElasticClient CreateElasticClient(string name);
}
