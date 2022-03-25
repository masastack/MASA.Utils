namespace Masa.Utils.Data.Elasticsearch.Options.Document.Query;

public class PaginatedOptions<TDocument> : QueryBaseOptions<TDocument>
    where TDocument : class
{
    public int Page { get; }

    public int PageSize { get; }

    public PaginatedOptions(
        string indexName,
        string query,
        string defaultField,
        int page,
        int pageSize,
        Operator @operator = Operator.Or)
        : base(indexName, query, defaultField, @operator)
    {
        Page = page;
        PageSize = pageSize;
    }
}
