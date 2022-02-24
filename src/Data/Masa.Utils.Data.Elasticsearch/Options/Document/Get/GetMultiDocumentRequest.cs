namespace Masa.Utils.Data.Elasticsearch.Options.Document.Get;

public class GetMultiDocumentRequest : DocumentOptions
{
    public string[] Id { get; }

    public GetMultiDocumentRequest(string indexName, string[] id) : base(indexName)
    {
        Id = id;
    }
}
