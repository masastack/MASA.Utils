namespace Masa.Utils.Data.Elasticsearch.Options.Document.Delete;

public class DeleteDocumentRequest : DocumentOptions
{
    public string DocumentId { get; }

    public DeleteDocumentRequest(string indexName, string documentId) : base(indexName)
    {
        DocumentId = documentId;
    }
}
