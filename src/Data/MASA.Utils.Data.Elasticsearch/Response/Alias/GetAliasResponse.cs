namespace MASA.Utils.Data.Elasticsearch.Response.Alias;

public class GetAliasResponse : ResponseBase
{
    public string[] Aliases { get; }

    public GetAliasResponse(CatResponse<CatAliasesRecord> catResponse) : base(catResponse)
    {
        Aliases = catResponse.IsValid ? catResponse.Records.Select(r => r.Alias).ToArray() : Array.Empty<string>();
    }

    public GetAliasResponse(Nest.GetAliasResponse getAliasResponse) : base(getAliasResponse)
    {
        Aliases = getAliasResponse.IsValid
            ? getAliasResponse.Indices
                .Select(item => item.Value)
                .SelectMany(indexAlias => indexAlias.Aliases)
                .Select(alias => alias.Key).Distinct().ToArray()
            : Array.Empty<string>();
    }
}
