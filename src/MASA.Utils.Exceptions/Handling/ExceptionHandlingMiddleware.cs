using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MASA.Framework.Exceptions.Handling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;
        private readonly MasaExceptionHandlingOptions _options;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IStringLocalizerFactory stringLocalizerFactory, IOptions<MasaExceptionHandlingOptions> optionsAccesser)
        {
            _next = next;
            _logger = logger;
            _stringLocalizerFactory = stringLocalizerFactory;
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
                if (userFriendlyException.LocalizeData != null)
                {
                    var stringLocalizer = _stringLocalizerFactory.Create(userFriendlyException.LocalizeData.ResourceType);
                    message = stringLocalizer[userFriendlyException.LocalizeData.Key];
                }

                _logger.LogError(userFriendlyException, message);
                await httpContext.Response.WriteTextAsync((int)MasaHttpStatusCode.UserFriendlyException, message);
            }
            catch (Exception exception)
            {
                if (exception is MasaException || _options.CatchAllException)
                {
                    var message = "An error occur in masa framework";

                    _logger.LogError(exception, message);
                    await httpContext.Response.WriteTextAsync((int)HttpStatusCode.InternalServerError, message);
                }
            }
        }
    }
}
