// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Exceptions.Handlers;

/// <summary>
/// Mvc pipeline exception filter to catch global exception
/// </summary>
public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly MasaExceptionHandlerOptions _options;
    private readonly MasaExceptionLogRelationOptions _logRelationOptions;
    private readonly ILogger<GlobalExceptionFilter>? _logger;

    public GlobalExceptionFilter(IOptions<MasaExceptionHandlerOptions> options,
        IOptions<MasaExceptionLogRelationOptions> logRelationOptions,
        ILogger<GlobalExceptionFilter>? logger = null)
    {
        _options = options.Value;
        _logRelationOptions = logRelationOptions.Value;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var masaExceptionContext = new MasaExceptionContext(context.Exception, context.HttpContext);
        _options.ExceptionHandler?.Invoke(masaExceptionContext);

        if (masaExceptionContext.HttpContext.Response.HasStarted)
            return;

        if (masaExceptionContext.ExceptionHandled)
        {
            context.ExceptionHandled = true;
            context.Result = new DefaultExceptionResult(
                masaExceptionContext.Message,
                masaExceptionContext.StatusCode,
                masaExceptionContext.ContentType);
            return;
        }

        _logger?.WriteLog(masaExceptionContext.Exception, LogLevel.Error, _logRelationOptions);

        if (masaExceptionContext.Exception is UserFriendlyException userFriendlyException)
        {
            context.ExceptionHandled = true;
            context.Result = new UserFriendlyExceptionResult(userFriendlyException.Message);
            return;
        }
        if (masaExceptionContext.Exception is MasaException || _options.CatchAllException)
        {
            context.ExceptionHandled = true;
            context.Result = new InternalServerErrorObjectResult(Constant.DEFAULT_EXCEPTION_MESSAGE);
            return;
        }
    }
}
