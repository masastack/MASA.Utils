namespace Masa.Utils.Data.Elasticsearch.Response.Document;

public class ClearDocumentResponse : ResponseBase
{
    public ClearDocumentResponse(DeleteByQueryResponse response) : base(response)
    {
    }
}
