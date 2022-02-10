namespace MASA.Utils.Exceptions.Handling;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly MasaExceptionHandlingOptions _options;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IOptions<MasaExceptionHandlingOptions> optionsAccesser)
    {
        _next = next;
        _logger = logger;
        _options = optionsAccesser.Value;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (UserFriendlyException userFriendlyException)
        {
            var message = userFriendlyException.Message;
            _logger.LogError(userFriendlyException, message);
            await httpContext.Response.WriteTextAsync((int) MasaHttpStatusCode.UserFriendlyException, message);
        }
        catch (Exception exception)
        {
            if (exception is MasaException || _options.CatchAllException)
            {
                var message = "An error occur in masa framework";

                _logger.LogError(exception, message);
                await httpContext.Response.WriteTextAsync((int) HttpStatusCode.InternalServerError, message);
            }
        }
    }
}
