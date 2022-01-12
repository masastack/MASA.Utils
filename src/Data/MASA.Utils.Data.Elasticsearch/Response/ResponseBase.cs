namespace MASA.Utils.Data.Elasticsearch.Response;

public class ResponseBase
{
    public bool IsValid { get; }

    public string Message { get; }

    protected ResponseBase(bool isValid, string message)
    {
        IsValid = isValid;
        Message = message;
    }

    public ResponseBase(IResponse ret) : this(ret.IsValid, ret.IsValid ? "success" : ret.ServerError?.ToString() ?? string.Empty)
    {
    }
}
