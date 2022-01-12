namespace MASA.Utils.Data.Elasticsearch.Response;

public class SetResponse : BulkResponse
{
    public SetResponse(Nest.BulkResponse ret) : base(ret)
    {
    }
}
