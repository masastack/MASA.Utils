namespace Masa.Utils.Data.Elasticsearch.Options.Document.Delete;

public class DeleteMultiDocumentRequest : DocumentOptions
{
    public IEnumerable<string> DocumentIds { get; }

    public DeleteMultiDocumentRequest(string indexName, params string[] documentIds) : base(indexName)
        => DocumentIds = documentIds;

    public DeleteMultiDocumentRequest(string indexName, IEnumerable<string> documentIds) : base(indexName)
        => DocumentIds = documentIds;
}
