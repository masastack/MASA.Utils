// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Exceptions.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Use localizable <see cref="ExceptionHandlingMiddleware"/>
    /// </summary>
    /// <param name="app"></param>
    /// <param name="action"></param>
    /// <param name="exceptionHandlingOptions"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseMasaExceptionHandling(
        this IApplicationBuilder app,
        Action<MasaExceptionHandlingOptions>? exceptionHandlingOptions = null)
    {
        return app.UseMasaExceptionHandling(_ =>
        {
        }, exceptionHandlingOptions);
    }

    /// <summary>
    /// Use localizable <see cref="ExceptionHandlingMiddleware"/>
    /// </summary>
    /// <param name="app"></param>
    /// <param name="action"></param>
    /// <param name="exceptionHandlingOptions"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseMasaExceptionHandling(
        this IApplicationBuilder app,
        Action<RequestLocalizationOptions> action,
        Action<MasaExceptionHandlingOptions>? exceptionHandlingOptions)
    {
        var option = new MasaExceptionHandlingOptions();
        exceptionHandlingOptions?.Invoke(option);

        var options = app.ApplicationServices.GetRequiredService<IOptions<MasaExceptionHandlingOptions>>();
        options.Value.CatchAllException = option.CatchAllException;
        options.Value.CustomExceptionHandler = option.CustomExceptionHandler;

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseRequestLocalization(action);
        return app;
    }
}
