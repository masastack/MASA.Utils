namespace Masa.Utils.Data.Elasticsearch.Options.Document.Get;

public class GetDocumentRequest : DocumentOptions
{
    public string Id { get; }

    public GetDocumentRequest(string indexName, string id) : base(indexName) => Id = id;
}
