namespace MASA.Utils.Data.Elasticsearch.Response.Index;

public class DeleteIndexResponse : ResponseBase
{
    public DeleteIndexResponse(Nest.DeleteIndexResponse deleteIndexResponse) : base(deleteIndexResponse)
    {
    }

    public DeleteIndexResponse(Nest.BulkAliasResponse bulkAliasResponse) : base(bulkAliasResponse)
    {
    }

    public DeleteIndexResponse(string message) : base(false, message)
    {
    }
}
