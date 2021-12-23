namespace MASA.Utils.Data.Elasticsearch.Options.Document;

public class DocumentOptions
{
    public string? IndexName { get; }

    public DocumentOptions(string? indexName = null)
    {
        IndexName = indexName;
    }
}
