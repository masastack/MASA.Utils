namespace MASA.Utils.Data.Elasticsearch.Options.Alias;

public class BindAliasIndexOptions
{
    public string[]? IndexNames { get; }

    public string Alias { get; }

    private BindAliasIndexOptions(string alias)
    {
        Alias = alias;
    }

    public BindAliasIndexOptions(string alias, string? indexName = null) : this(alias)
    {
        IndexNames = indexName == null ? null : new[] { indexName };
    }

    public BindAliasIndexOptions(string alias, string[] indexNames) : this(alias)
    {
        ArgumentNullException.ThrowIfNull(nameof(indexNames));
        IndexNames = indexNames;
    }
}
