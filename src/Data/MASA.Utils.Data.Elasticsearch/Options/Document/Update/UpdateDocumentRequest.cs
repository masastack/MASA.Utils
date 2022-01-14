namespace MASA.Utils.Data.Elasticsearch.Options.Document.Update;

public class UpdateDocumentRequest<TDocument> : DocumentOptions where TDocument : class
{
    public UpdateDocumentBaseRequest<TDocument> Request { get; }

    public UpdateDocumentRequest(string indexName, TDocument document, string? documentId = null) : base(indexName)
    {
        Request = new UpdateDocumentBaseRequest<TDocument>(document, documentId);
    }

    public UpdateDocumentRequest(string indexName, object partialDocument, string? documentId = null) : base(indexName)
    {
        Request = new UpdateDocumentBaseRequest<TDocument>(partialDocument, documentId);
    }
}
