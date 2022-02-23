namespace Masa.Utils.Data.Elasticsearch.Response;

public class UpdateResponse : ResponseBase
{
    public UpdateResponse(IUpdateResponse<object> updateResponse) : base(updateResponse)
    {
    }
}
