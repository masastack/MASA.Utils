namespace Masa.Utils.Data.Elasticsearch.Response.Alias;

public class BulkAliasResponse : ResponseBase
{
    public BulkAliasResponse(Nest.BulkAliasResponse bulkAliasResponse) : base(bulkAliasResponse)
    {
    }
}
