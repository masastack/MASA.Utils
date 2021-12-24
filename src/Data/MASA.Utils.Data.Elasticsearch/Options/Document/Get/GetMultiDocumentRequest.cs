namespace MASA.Utils.Data.Elasticsearch.Options.Document.Get;

public class GetMultiDocumentRequest : DocumentOptions
{
    public string[] Id { get; }

    public GetMultiDocumentRequest(string[] id, string? indexName = null) : base(indexName)
    {
        Id = id;
    }
}
