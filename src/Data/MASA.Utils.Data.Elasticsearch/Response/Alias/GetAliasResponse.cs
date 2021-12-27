namespace MASA.Utils.Data.Elasticsearch.Response.Alias;

public class GetAliasResponse : ResponseBase
{
    public string[] Aliases { get; }

    public GetAliasResponse(CatResponse<CatAliasesRecord> ret) : base(ret)
    {
        Aliases = ret.IsValid ? ret.Records.Select(r => r.Alias).ToArray() : Array.Empty<string>();
    }

    public GetAliasResponse(Nest.GetAliasResponse ret) : base(ret)
    {
        Aliases = ret.IsValid
            ? ret.Indices
                .Select(item => item.Value)
                .SelectMany(indexAlias => indexAlias.Aliases)
                .Select(alias => alias.Key).Distinct().ToArray()
            : Array.Empty<string>();
    }
}
