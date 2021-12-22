namespace MASA.Utils.Data.Elasticsearch.Response;

public class CreateResponse : ResponseBase
{
    public CreateResponse(Nest.CreateResponse ret) : base(ret)
    {
    }
}
