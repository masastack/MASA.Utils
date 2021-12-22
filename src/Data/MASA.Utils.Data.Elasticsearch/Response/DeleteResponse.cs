namespace MASA.Utils.Data.Elasticsearch.Response;

public class DeleteResponse : ResponseBase
{
    public DeleteResponse(Nest.DeleteResponse ret) : base(ret)
    {
    }
}
