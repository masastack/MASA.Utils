namespace MASA.Utils.Data.Elasticsearch.Response;

public class CreateIndexResponse : ResponseBase
{
    public CreateIndexResponse(Nest.CreateIndexResponse ret) : base(ret)
    {
    }
}
