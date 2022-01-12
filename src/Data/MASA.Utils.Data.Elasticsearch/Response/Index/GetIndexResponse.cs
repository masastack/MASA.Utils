namespace MASA.Utils.Data.Elasticsearch.Response.Index;

public class GetIndexResponse : ResponseBase
{
    public string[] IndexNames { get; set; }

    public GetIndexResponse(CatResponse<CatIndicesRecord> ret) : base(ret)
    {
        IndexNames = ret.IsValid ? ret.Records.Select(r => r.Index).ToArray() : Array.Empty<string>();
    }
}
