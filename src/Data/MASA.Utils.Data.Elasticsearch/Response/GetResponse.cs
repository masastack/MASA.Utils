namespace MASA.Utils.Data.Elasticsearch.Response;

public class GetResponse<TDocument> : ResponseBase
    where TDocument : class
{
    public TDocument Document { get; set; }

    public GetResponse(IGetResponse<TDocument> ret) : base(ret)
    {
        Document = ret.Source;
    }
}
