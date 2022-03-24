namespace Masa.Utils.Data.Elasticsearch.Options.Document;

public class SingleDocumentBaseRequest<TDocument> where TDocument : class
{
    public TDocument Document { get; }

    public string? DocumentId { get; }

    public SingleDocumentBaseRequest(TDocument document, string? documentId)
    {
        Document = document;
        DocumentId = documentId ?? Guid.NewGuid().ToString();
    }
}
