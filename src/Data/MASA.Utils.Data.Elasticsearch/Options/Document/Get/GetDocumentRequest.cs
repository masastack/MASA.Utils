namespace MASA.Utils.Data.Elasticsearch.Options.Document.Get;

public class GetDocumentRequest : DocumentOptions
{
    public string Id { get; }

    public GetDocumentRequest(string id, string? indexName = null) : base(indexName)
    {
        Id = id;
    }
}
