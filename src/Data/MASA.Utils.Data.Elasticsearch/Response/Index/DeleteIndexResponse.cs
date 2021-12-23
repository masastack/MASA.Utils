namespace MASA.Utils.Data.Elasticsearch.Response.Index;

public class DeleteIndexResponse : ResponseBase
{
    public DeleteIndexResponse(Nest.DeleteIndexResponse ret) : base(ret)
    {
    }

    public DeleteIndexResponse(Nest.DeleteAliasResponse ret) : base(ret)
    {
    }
}
