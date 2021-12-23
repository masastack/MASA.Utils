namespace MASA.Utils.Data.Elasticsearch.Options.Document.Exist;

public class ExistDocumentRequest : DocumentOptions
{
    public string DocumentId { get; set; }

    public ExistDocumentRequest(string documentId, string? indexName = null) : base(indexName)
    {
        DocumentId = documentId;
    }
}
