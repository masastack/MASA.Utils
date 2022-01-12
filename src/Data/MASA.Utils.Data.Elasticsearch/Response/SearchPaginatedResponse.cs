namespace MASA.Utils.Data.Elasticsearch.Response;

public class SearchPaginatedResponse<TDocument> : SearchResponse<TDocument>
    where TDocument : class
{
    public long Total { get; set; }

    public int TotalPages { get; set; }

    public SearchPaginatedResponse(ISearchResponse<TDocument> ret) : base(ret)
    {
        Total = ret.Hits.Count;
    }

    public SearchPaginatedResponse(int pageSize, ISearchResponse<TDocument> ret) : this(ret)
    {
        TotalPages = (int)Math.Ceiling(Total / (decimal)pageSize);
    }
}
