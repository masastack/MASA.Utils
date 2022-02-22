namespace MASA.Utils.Data.Elasticsearch.Response;

public class ExistsResponse : ResponseBase
{
    public bool Exists { get; }

    public ExistsResponse(Nest.ExistsResponse existsResponse) : base(
        existsResponse.IsValid || existsResponse.ApiCall.HttpStatusCode == 404,
        existsResponse.IsValid || existsResponse.ApiCall.HttpStatusCode == 404 ? "success" : existsResponse.ServerError?.ToString() ?? string.Empty)
    {
        Exists = existsResponse.Exists;
    }
}
