namespace MASA.Utils.Data.Elasticsearch.Options.Document.Update;

public class UpdateMultiDocumentRequest<TDocument> : DocumentOptions where TDocument : class
{
    public List<UpdateDocumentBaseRequest<TDocument>> Items { get; set; }

    public UpdateMultiDocumentRequest(string? indexName = null) : base(indexName)
    {
        Items = new();
    }

    public UpdateMultiDocumentRequest<TDocument> AddDocument(UpdateDocumentBaseRequest<TDocument> item)
    {
        Items.Add(item);
        return this;
    }
}
