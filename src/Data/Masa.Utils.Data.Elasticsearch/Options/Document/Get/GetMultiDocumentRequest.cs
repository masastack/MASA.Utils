namespace Masa.Utils.Data.Elasticsearch.Options.Document.Get;

public class GetMultiDocumentRequest : DocumentOptions
{
    public IEnumerable<string> Ids { get; }

    public GetMultiDocumentRequest(string indexName, string[] ids) : base(indexName)
        => Ids = ids;

    public GetMultiDocumentRequest(string indexName, IEnumerable<string> ids) : base(indexName)
        => Ids = ids;
}
