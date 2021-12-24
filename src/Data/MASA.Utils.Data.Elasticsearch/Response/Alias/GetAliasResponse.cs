namespace MASA.Utils.Data.Elasticsearch.Response.Alias;

public class GetAliasResponse : ResponseBase
{
    public string[] Aliases { get; }

    public GetAliasResponse(Nest.GetAliasResponse ret) : base(ret)
    {
        if (ret.IsValid)
        {
            Aliases = ret.Indices
                .Select(item => item.Value)
                .SelectMany(indexAlias => indexAlias.Aliases)
                .Select(alias => alias.Key).ToArray();
        }
    }
}
