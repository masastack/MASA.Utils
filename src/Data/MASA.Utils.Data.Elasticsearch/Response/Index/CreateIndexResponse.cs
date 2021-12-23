namespace MASA.Utils.Data.Elasticsearch.Response.Index;

public class CreateIndexResponse : ResponseBase
{
    public CreateIndexResponse(Nest.CreateIndexResponse ret) : base(ret)
    {
    }
}
