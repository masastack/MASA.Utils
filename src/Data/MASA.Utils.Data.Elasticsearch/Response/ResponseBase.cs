namespace MASA.Utils.Data.Elasticsearch.Response;

public class ResponseBase
{
    public bool IsValid { get;  }

    public string Message { get; }

    private ResponseBase(bool isValid, string message)
    {
        IsValid = isValid;
        Message = message;
    }

    public ResponseBase(Nest.ResponseBase ret) : this(ret.IsValid, ret.IsValid ? "success" : ret.ServerError?.ToString() ?? "")
    {
    }
}
