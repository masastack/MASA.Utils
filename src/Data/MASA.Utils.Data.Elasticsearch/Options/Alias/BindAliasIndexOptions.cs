namespace MASA.Utils.Data.Elasticsearch.Options.Alias;

public class BindAliasIndexOptions
{
    public string[] IndexNames { get; } = default!;

    public string Alias { get; }

    private BindAliasIndexOptions(string alias)
    {
        Alias = alias;
    }

    public BindAliasIndexOptions(string alias, string indexName) : this(alias)
    {
        if (string.IsNullOrEmpty(indexName))
            throw new ArgumentException("indexName cannot be empty",nameof(indexName));

        IndexNames = new[] { indexName };
    }

    public BindAliasIndexOptions(string alias, string[] indexNames) : this(alias)
    {
        ArgumentNullException.ThrowIfNull(nameof(indexNames));
        IndexNames = indexNames;
    }
}
