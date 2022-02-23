namespace Masa.Utils.Data.Elasticsearch;

public interface IElasticsearchFactory
{
    IMasaElasticClient CreateClient();

    IMasaElasticClient CreateClient(string name);

    IElasticClient CreateElasticClient();

    IElasticClient CreateElasticClient(string name);
}
