namespace MASA.Utils.Data.Elasticsearch.Response.Alias;

public class GetAliasResponse : ResponseBase
{
    public string[] Aliases { get; }

    public GetAliasResponse(CatResponse<CatAliasesRecord> ret) : base(ret)
    {
        Aliases = ret.IsValid ? ret.Records.Select(r => r.Alias).ToArray() : Array.Empty<string>();
    }

    // public GetAliasResponse(Nest.GetAliasResponse ret) : base(ret)
    // {
    //     if (ret.IsValid)
    //     {
    //         Aliases = ret.Indices
    //             .Select(item => item.Value)
    //             .SelectMany(indexAlias => indexAlias.Aliases)
    //             .Select(alias => alias.Key).Distinct().ToArray();
    //     }
    //     else
    //     {
    //         Aliases = Array.Empty<string>();
    //     }
    // }
}
