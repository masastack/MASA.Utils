namespace MASA.Utils.Data.Elasticsearch.Response;

public class CreateMultiResponse : BulkResponse
{
    public CreateMultiResponse(Nest.BulkResponse ret) : base(ret)
    {
    }
}
