namespace MASA.Utils.Data.Elasticsearch.Options.Document.Query;

public class QueryOptions : QueryBaseOptions
{
    public int Skip { get; }

    public int Take { get; }

    public QueryOptions(string[] fields, string query, int skip, int take)
        : this(null, fields, query, skip, take)
    {
    }

    public QueryOptions(string? indexName, string[] fields, string query, int skip, int take)
        : base(indexName, fields, query)
    {
        Skip = skip;
        Take = take;
    }
}
