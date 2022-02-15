namespace MASA.Utils.Data.Elasticsearch.Options.Index;

public class CreateIndexOptions
{
    public IIndexSettings? IndexSettings { get; set; } = null;

    public IAliases? Aliases { get; set; } = null;

    public ITypeMapping? Mappings { get; set; } = null;
}
