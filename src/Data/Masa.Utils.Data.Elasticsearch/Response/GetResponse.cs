namespace Masa.Utils.Data.Elasticsearch.Response;

public class GetResponse<TDocument> : ResponseBase
    where TDocument : class
{
    public TDocument Document { get; set; }

    public GetResponse(IGetResponse<TDocument> getResponse) : base(getResponse)
    {
        Document = getResponse.Source;
    }
}
