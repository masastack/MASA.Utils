namespace MASA.Utils.Data.Elasticsearch.Response;

public class ExistsResponse : ResponseBase
{
    public bool Exists { get; }

    public ExistsResponse(Nest.ExistsResponse ret) : base(
        ret.IsValid || ret.ApiCall.HttpStatusCode == 404,
        ret.IsValid || ret.ApiCall.HttpStatusCode == 404 ? "success" : ret.ServerError?.ToString() ?? "")
    {
        Exists = ret.Exists;
    }
}
