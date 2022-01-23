namespace MASA.Utils.Data.Elasticsearch.Response;

public class DeleteMultiResponse : ResponseBase
{
    public List<DeleteRangeResponseItems> Data { get; set; }

    public DeleteMultiResponse(Nest.BulkResponse bulkResponse) : base(bulkResponse)
    {
        Data = bulkResponse.Items.Select(item => new DeleteRangeResponseItems(item.Id, item.IsValid, item.Error?.ToString() ?? string.Empty)).ToList();
    }
}
