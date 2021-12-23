namespace MASA.Utils.Data.Elasticsearch.Options.Document.Set;

public class SetDocumentRequest<TDocument> : DocumentOptions where TDocument : class
{
    public List<SingleDocumentBaseRequest<TDocument>> Items { get; set; }

    public SetDocumentRequest(string? indexName = null) : base(indexName)
    {
        Items = new();
    }

    public SetDocumentRequest(TDocument document, string? documentId = null, string? indexName = null) : this(indexName)
    {
        this.AddDocument(document, documentId);
    }

    public SetDocumentRequest<TDocument> AddDocument(TDocument document, string? documentId = null)
    {
        return this.AddDocument(new SingleDocumentBaseRequest<TDocument>(document, documentId));
    }

    public SetDocumentRequest<TDocument> AddDocument(SingleDocumentBaseRequest<TDocument> item)
    {
        Items.Add(item);
        return this;
    }
}
