namespace MASA.Utils.Exceptions.Handling;

/// <summary>
/// Mvc pipeline exception filter to catch global exception
/// </summary>
public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly MasaExceptionHandlingOptions _options;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger,
        IOptions<MasaExceptionHandlingOptions> optionsAccesser)
    {
        _logger = logger;
        _options = optionsAccesser.Value;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        if (_options.CustomExceptionHandler is not null)
        {
            var handlerResult = _options.CustomExceptionHandler.Invoke(exception);

            if (handlerResult.ExceptionHandled) return;

            if (handlerResult.OverrideException is not null) exception = handlerResult.OverrideException;
        }

        if (exception is UserFriendlyException userFriendlyException)
        {
            var message = userFriendlyException.Message;
            _logger.LogError(userFriendlyException, message);

            context.ExceptionHandled = true;
            context.Result = new UserFriendlyExceptionResult(message);

            return;
        }

        if (exception is MasaException || _options.CatchAllException)
        {
            var message = "An error occur in masa framework";
            _logger.LogError(exception, message);

            context.ExceptionHandled = true;
            context.Result = new InternalServerErrorObjectResult(message);
        }
    }
}
