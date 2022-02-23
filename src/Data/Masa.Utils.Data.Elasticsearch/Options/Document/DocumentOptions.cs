namespace Masa.Utils.Data.Elasticsearch.Options.Document;

public class DocumentOptions
{
    public string IndexName { get; }

    public DocumentOptions(string indexName)
    {
        if (string.IsNullOrEmpty(indexName))
            throw new ArgumentException("indexName cannot be empty",nameof(indexName));

        IndexName = indexName;
    }
}
