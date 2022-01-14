namespace MASA.Utils.Data.Elasticsearch;

public class ElasticsearchOptions
{
    public bool IsDefault { get; set; }

    public bool UseConnectionPool { get; private set; }

    internal string[] Nodes { get; private set; }

    internal StaticConnectionPoolOptions StaticConnectionPoolOptions { get; }

    internal ConnectionSettingsOptions ConnectionSettingsOptions { get; }

    internal Action<ConnectionSettings>? Action { get; private set; }

    public ElasticsearchOptions(params string[] nodes)
    {
        if (nodes.Length == 0)
            throw new ArgumentException("Please specify the Elasticsearch node address");

        this.IsDefault = false;
        this.Nodes = nodes;
        this.UseConnectionPool = nodes.Length > 1;
        this.ConnectionSettingsOptions = new();
        this.StaticConnectionPoolOptions = new();
        this.Action = null;
    }

    public ElasticsearchOptions UseDefault()
    {
        this.IsDefault = true;
        return this;
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

    public ElasticsearchOptions UseConnectionSettings(Action<ConnectionSettings> action)
    {
        this.Action = action;
        return this;
    }
}
