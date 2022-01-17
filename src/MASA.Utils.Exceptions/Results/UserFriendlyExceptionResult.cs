namespace MASA.Utils.Exceptions.Results;

public class UserFriendlyExceptionResult : IActionResult
{
    public int StatusCode { get; set; }

    public string Message { get; set; }

    public UserFriendlyExceptionResult(string message)
    {
        StatusCode = (int)MasaHttpStatusCode.UserFriendlyException;
        Message = message;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        await context.HttpContext.Response.WriteTextAsync(StatusCode, Message);
    }
}
