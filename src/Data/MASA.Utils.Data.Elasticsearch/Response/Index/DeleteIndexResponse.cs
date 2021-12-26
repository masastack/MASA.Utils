namespace MASA.Utils.Data.Elasticsearch.Response.Index;

public class DeleteIndexResponse : ResponseBase
{
    public DeleteIndexResponse(Nest.DeleteIndexResponse ret) : base(ret)
    {
    }

    public DeleteIndexResponse(Nest.BulkAliasResponse ret) : base(ret)
    {
    }

    public DeleteIndexResponse(string message) : base(false, message)
    {
    }
}
