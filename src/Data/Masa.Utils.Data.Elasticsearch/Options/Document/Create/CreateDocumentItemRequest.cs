namespace Masa.Utils.Data.Elasticsearch.Options.Document.Create;

public class CreateDocumentItemRequest<TDocument> : SingleDocumentBaseRequest<TDocument>
    where TDocument : class
{
    public CreateDocumentItemRequest(TDocument document, string? documentId) : base(document, documentId)
    {
    }
}
