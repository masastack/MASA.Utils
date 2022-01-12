namespace MASA.Utils.Data.Elasticsearch.Response;

public class GetMultiResponse<TDocument> : ResponseBase
    where TDocument : class
{
    public List<GetMultiResponseItems> Data { get; set; }

    public GetMultiResponse(bool isValid, string message, List<IMultiGetHit<TDocument>>? ret = null) : base(isValid, message)
    {
        Data = ret?.Select(res => new GetMultiResponseItems(res.Id, res.Source)).ToList() ?? new();
    }

    public class GetMultiResponseItems
    {
        public string Id { get; }

        public TDocument Document { get; }

        public GetMultiResponseItems(string id, TDocument document)
        {
            Id = id;
            Document = document;
        }
    }
}
