namespace Masa.Utils.Data.Elasticsearch.Response.Document;

public class CountDocumentResponse : ResponseBase
{
    public long Count { get; }

    public CountDocumentResponse(CountResponse response) : base(response)
    {
        Count = response.Count;
    }
}
