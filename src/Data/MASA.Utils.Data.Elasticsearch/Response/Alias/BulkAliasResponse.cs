namespace MASA.Utils.Data.Elasticsearch.Response.Alias;

public class BulkAliasResponse : Response.ResponseBase
{
    public BulkAliasResponse(Nest.BulkAliasResponse bulkAliasResponse) : base(bulkAliasResponse)
    {
    }
}
