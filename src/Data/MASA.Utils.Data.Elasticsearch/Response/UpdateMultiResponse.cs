namespace MASA.Utils.Data.Elasticsearch.Response;

public class UpdateMultiResponse : BulkResponse
{
    public UpdateMultiResponse(Nest.BulkResponse ret) : base(ret)
    {
    }
}
