namespace Masa.Utils.Data.Elasticsearch.Response;

public class SetResponse : BulkResponse
{
    public SetResponse(Nest.BulkResponse bulkResponse) : base(bulkResponse)
    {
    }
}
