namespace MASA.Utils.Data.Elasticsearch.Response;

public class BulkResponse : ResponseBase
{
    public List<BulkResponseItems> Items { get; set; }

    public BulkResponse(Nest.BulkResponse ret) : base(ret)
    {
        Items = ret.Items.Select(item => new BulkResponseItems(item.Id, item.IsValid, item.Error?.ToString() ?? "")).ToList();
    }

    public class BulkResponseItems
    {
        public string Id { get; }

        public bool IsValid { get; }

        public string Message { get; }

        public BulkResponseItems(string id, bool isValid, string message)
        {
            Id = id;
            IsValid = isValid;
            Message = message;
        }
    }
}
