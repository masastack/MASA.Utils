namespace MASA.Utils.Data.Elasticsearch.Options.Document.Delete;

public class DeleteDocumentRequest : DocumentOptions
{
    public string DocumentId { get; }

    public DeleteDocumentRequest(string documentId, string? indexName = null) : base(indexName)
    {
        DocumentId = documentId;
    }
}
