namespace MASA.Utils.Data.Elasticsearch.Options.Document.Set;

public class SetDocumentRequest<TDocument> : DocumentOptions where TDocument : class
{
    public List<SingleDocumentBaseRequest<TDocument>> Items { get; set; }

    public SetDocumentRequest(string indexName) : base(indexName)
    {
        Items = new();
    }

    public SetDocumentRequest(string indexName, TDocument document, string? documentId = null) : this(indexName)
    {
        AddDocument(document, documentId);
    }

    public SetDocumentRequest<TDocument> AddDocument(TDocument document, string? documentId = null)
    {
        return AddDocument(new SingleDocumentBaseRequest<TDocument>(document, documentId));
    }

    public SetDocumentRequest<TDocument> AddDocument(SingleDocumentBaseRequest<TDocument> item)
    {
        Items.Add(item);
        return this;
    }
}
