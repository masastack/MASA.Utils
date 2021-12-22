namespace MASA.Utils.Data.Elasticsearch.Options.Document.Query;

public class PaginatedOptions : QueryBaseOptions
{
    public int Page { get; }

    public int PageSize { get; }

    public PaginatedOptions(
        string[] fields,
        string query,
        int page,
        int pageSize)
        : this(null, fields, query, page, pageSize)
    {
    }

    public PaginatedOptions(
        string? indexName,
        string[] fields,
        string query,
        int page,
        int pageSize)
        : base(indexName, fields, query)
    {
        Page = page;
        PageSize = pageSize;
    }
}
