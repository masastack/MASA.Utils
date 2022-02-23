namespace Masa.Utils.Data.Elasticsearch.Response;

public class UpdateMultiResponse : BulkResponse
{
    public UpdateMultiResponse(Nest.BulkResponse bulkResponse) : base(bulkResponse)
    {
    }
}
