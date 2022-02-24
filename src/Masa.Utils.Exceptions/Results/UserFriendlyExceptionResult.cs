using Masa.Utils.Exceptions.Internal;

namespace Masa.Utils.Exceptions.Results;

public class UserFriendlyExceptionResult : IActionResult
{
    public string Message { get; set; }

    public UserFriendlyExceptionResult(string message)
    {
        Message = message;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        await context.HttpContext.Response.WriteTextAsync((int)MasaHttpStatusCode.UserFriendlyException, Message);
    }
}
