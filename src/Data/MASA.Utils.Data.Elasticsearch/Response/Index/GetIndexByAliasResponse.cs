namespace MASA.Utils.Data.Elasticsearch.Response.Index;

public class GetIndexByAliasResponse : Response.ResponseBase
{
    public string[] IndexNames { get; }

    public GetIndexByAliasResponse(CatResponse<CatIndicesRecord> ret) : base(ret)
    {
        IndexNames = ret.IsValid ? ret.Records.Select(r => r.Index).ToArray() : Array.Empty<string>();
        IndexNames = ret.IsValid ? ret.Records.Select(r => r.Index).ToArray() : Array.Empty<string>();
    }
}
