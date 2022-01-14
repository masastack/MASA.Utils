namespace MASA.Utils.Data.Elasticsearch.Response;

public class SearchResponse<TDocument> : ResponseBase
    where TDocument : class
{
    public List<TDocument> Data { get; }

    public SearchResponse(Nest.ISearchResponse<TDocument> searchResponse) : base(searchResponse)
    {
        Data = searchResponse.Hits.Select(hit => hit.Source).ToList();
    }
}
