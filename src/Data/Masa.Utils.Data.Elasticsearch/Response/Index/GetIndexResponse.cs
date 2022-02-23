namespace Masa.Utils.Data.Elasticsearch.Response.Index;

public class GetIndexResponse : ResponseBase
{
    public string[] IndexNames { get; set; }

    public GetIndexResponse(CatResponse<CatIndicesRecord> catResponse) : base(catResponse)
    {
        IndexNames = catResponse.IsValid ? catResponse.Records.Select(r => r.Index).ToArray() : Array.Empty<string>();
    }
}
