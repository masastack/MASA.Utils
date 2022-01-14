namespace MASA.Utils.Data.Elasticsearch.Options.Document.Query;

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
        int pageSize)
        : base(indexName, query, defaultField)
    {
        Page = page;
        PageSize = pageSize;
    }
}
