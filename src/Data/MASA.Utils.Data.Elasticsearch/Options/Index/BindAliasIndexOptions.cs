namespace MASA.Utils.Data.Elasticsearch.Options.Index;

public class BindAliasIndexOptions
{
    public string? IndexName { get; }

    public string[] Aliases { get; }

    public BindAliasIndexOptions(string alias, string? indexName = null)
        : this(new[] {alias}, indexName)
    {
    }

    public BindAliasIndexOptions(string[] aliases, string? indexName = null)
    {
        if (indexName != null && aliases.Contains(indexName))
        {
            throw new ArgumentException("an index or data stream exists with the same name as the alias",nameof(aliases));
        }
        IndexName = indexName;
        Aliases = aliases;
    }
}
