namespace MASA.Utils.Data.Elasticsearch.Response;

public class UpdateResponse : ResponseBase
{
    public UpdateResponse(Nest.IUpdateResponse<object> ret) : base(ret)
    {
    }
}
