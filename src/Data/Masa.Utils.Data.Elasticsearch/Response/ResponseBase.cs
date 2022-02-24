namespace Masa.Utils.Data.Elasticsearch.Response;

public class ResponseBase
{
    public bool IsValid { get; }

    public string Message { get; }

    protected ResponseBase(bool isValid, string message)
    {
        IsValid = isValid;
        Message = message;
    }

    public ResponseBase(IResponse response) : this(response.IsValid, response.IsValid ? "success" : response.ServerError?.ToString() ?? string.Empty)
    {
    }
}
