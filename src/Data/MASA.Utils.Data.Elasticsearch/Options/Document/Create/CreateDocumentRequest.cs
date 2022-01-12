namespace MASA.Utils.Data.Elasticsearch.Options.Document.Create;

public class CreateDocumentRequest<TDocument> : DocumentOptions where TDocument : class
{
    public SingleDocumentBaseRequest<TDocument> Request { get; }

    public CreateDocumentRequest(TDocument document, string? documentId = null) : this(null, document, documentId)
    {
    }

    public CreateDocumentRequest(string? indexName, TDocument document, string? documentId = null) : base(indexName)
    {
        Request = new SingleDocumentBaseRequest<TDocument>(document, documentId);
    }
}
