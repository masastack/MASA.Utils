namespace MASA.Utils.Data.Elasticsearch.Options.Document.Delete;

public class DeleteMultiDocumentRequest : DocumentOptions
{
    public string[] DocumentIds { get; }

    public DeleteMultiDocumentRequest(string[] documentIds, string? indexName = null) : base(indexName)
    {
        DocumentIds = documentIds;
    }
}
