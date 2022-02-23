namespace Masa.Utils.Data.Elasticsearch.Options.Document.Delete;

public class DeleteMultiDocumentRequest : DocumentOptions
{
    public string[] DocumentIds { get; }

    public DeleteMultiDocumentRequest(string indexName, string[] documentIds) : base(indexName)
    {
        DocumentIds = documentIds;
    }
}
