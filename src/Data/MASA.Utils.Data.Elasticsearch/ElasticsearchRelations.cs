﻿namespace MASA.Utils.Data.Elasticsearch;

public class ElasticsearchRelations
{
    public bool IsDefault { get; private set; }

    public string? Name { get; }

    public Uri[] Nodes { get; }

    public bool UseConnectionPool { get; }

    internal StaticConnectionPoolOptions StaticConnectionPoolOptions { get; private set; }

    internal ConnectionSettingsOptions ConnectionSettingsOptions { get; private set; }

    public ElasticsearchRelations(string? name, bool useConnectionPool, Uri[] nodes)
    {
        IsDefault = false;
        Name = name;
        UseConnectionPool = useConnectionPool;
        Nodes = nodes;
    }

    public ElasticsearchRelations UseStaticConnectionPoolOptions(StaticConnectionPoolOptions staticConnectionPoolOptions)
    {
        StaticConnectionPoolOptions = staticConnectionPoolOptions;
        return this;
    }

    public ElasticsearchRelations UseConnectionSettingsOptions(ConnectionSettingsOptions connectionSettingsOptions)
    {
        ConnectionSettingsOptions = connectionSettingsOptions;
        return this;
    }
}
