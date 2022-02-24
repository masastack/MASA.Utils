namespace Masa.Utils.Data.Elasticsearch.Response;

public class GetMultiResponseItems<TDocument>
    where TDocument : class
{
    public string Id { get; }

    public TDocument Document { get; }

    public GetMultiResponseItems(string id, TDocument document)
    {
        Id = id;
        Document = document;
    }
}
