using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace MASA.Framework.Exceptions.Handling
{
    /// <summary>
    /// Mvc pipeline exception filter to catch global exception
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;
        private readonly MasaExceptionHandlingOptions _options;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IStringLocalizerFactory stringLocalizerFactory, IOptions<MasaExceptionHandlingOptions> optionsAccesser)
        {
            _logger = logger;
            _stringLocalizerFactory = stringLocalizerFactory;
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
                if (userFriendlyException.LocalizeData != null)
                {
                    var stringLocalizer = _stringLocalizerFactory.Create(userFriendlyException.LocalizeData.ResourceType);
                    message = stringLocalizer[userFriendlyException.LocalizeData.Key];
                }

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
}
