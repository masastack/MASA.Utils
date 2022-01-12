namespace MASA.Utils.Data.Elasticsearch.Options.Alias;

public class UnBindAliasIndexOptions
{
    public string[]? IndexNames { get; }

    public string Alias { get; }

    private UnBindAliasIndexOptions(string alias)
    {
        Alias = alias;
    }

    public UnBindAliasIndexOptions(string alias, string? indexName = null) : this(alias)
    {
        IndexNames = indexName == null ? null : new[] { indexName };
    }

    public UnBindAliasIndexOptions(string alias, string[] indexNames) : this(alias)
    {
        ArgumentNullException.ThrowIfNull(nameof(indexNames));
        IndexNames = indexNames;
    }
}
