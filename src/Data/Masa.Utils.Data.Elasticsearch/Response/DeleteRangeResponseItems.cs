namespace Masa.Utils.Data.Elasticsearch.Response;

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
