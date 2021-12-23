namespace MASA.Utils.Data.Elasticsearch.Response;

public class DeleteMultiResponse : ResponseBase
{
    public List<DeleteRangeResponseItems> Data { get; set; }

    public DeleteMultiResponse(Nest.BulkResponse ret) : base(ret)
    {
        Data = ret.Items.Select(item => new DeleteRangeResponseItems(item.Id, item.IsValid, item.Error?.ToString() ?? "")).ToList();
    }

    public class DeleteRangeResponseItems
    {
        public string Id { get; }

        public bool IsValid { get; }

        public string Message { get; }

        public DeleteRangeResponseItems(string id, bool isValid, string message)
        {
            Id = id;
            IsValid = isValid;
            Message = message;
        }
    }
}
