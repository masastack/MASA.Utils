namespace MASA.Utils.Data.Elasticsearch.Options.Alias;

public class UnBindAliasIndexOptions
{
    public string[] IndexNames { get; } = default!;

    public string Alias { get; }

    private UnBindAliasIndexOptions(string alias)
    {
        Alias = alias;
    }

    public UnBindAliasIndexOptions(string alias, string indexName) : this(alias)
    {
        if (string.IsNullOrEmpty(indexName))
            throw new ArgumentException("indexName cannot be empty", nameof(indexName));

        IndexNames = new[] {indexName};
    }

    public UnBindAliasIndexOptions(string alias, string[] indexNames) : this(alias)
    {
        ArgumentNullException.ThrowIfNull(nameof(indexNames));
        IndexNames = indexNames;
    }
}
