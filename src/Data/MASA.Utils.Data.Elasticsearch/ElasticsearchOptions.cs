using MASA.Utils.Data.Elasticsearch.Options;

namespace MASA.Utils.Data.Elasticsearch;

public class ElasticsearchOptions
{
    public bool IsDefault { get; set; }

    public bool UseConnectionPool { get; private set; }

    internal string[] Nodes { get; set; }

    internal StaticConnectionPoolOptions StaticConnectionPoolOptions { get; set; }

    internal ConnectionSettingsOptions ConnectionSettingsOptions { get; set; }

    public static ElasticsearchOptions Default = new ElasticsearchOptions();

    public ElasticsearchOptions(params string[] nodes)
    {
        if (nodes.Length == 0)
        {
            nodes = new[]
            {
                "http://localhost:9200"
            };
        }

        this.IsDefault = false;
        this.Nodes = nodes;
        this.UseConnectionPool = nodes.Length > 1;
        this.ConnectionSettingsOptions = new();
        this.StaticConnectionPoolOptions = new();
    }

    public ElasticsearchOptions UseNodes(params string[] nodes)
    {
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException("Please enter the Elasticsearch node address");

        this.Nodes = nodes;
        this.UseConnectionPool = nodes.Length > 1;
        return this;
    }

    public ElasticsearchOptions UseRandomize(bool randomize)
    {
        this.StaticConnectionPoolOptions.UseRandomize(randomize);
        return this;
    }

    public ElasticsearchOptions UseDateTimeProvider(IDateTimeProvider dateTimeProvider)
    {
        this.StaticConnectionPoolOptions.UseDateTimeProvider(dateTimeProvider);
        return this;
    }
}
