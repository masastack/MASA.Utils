namespace Masa.Utils.Data.Elasticsearch.Response.Index;

public class CreateIndexResponse : ResponseBase
{
    public CreateIndexResponse(Nest.CreateIndexResponse createIndexResponse) : base(createIndexResponse)
    {
    }
}
