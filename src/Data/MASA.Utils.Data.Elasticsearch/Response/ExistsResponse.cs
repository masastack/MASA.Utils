namespace MASA.Utils.Data.Elasticsearch.Response;

public class ExistsResponse : ResponseBase
{
    public bool Exists { get; }

    public ExistsResponse(Nest.ExistsResponse ret) : base(ret)
    {
        Exists = ret.Exists;
    }
}
