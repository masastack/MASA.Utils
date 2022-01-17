namespace MASA.Utils.Exceptions.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Use localizable <see cref="ExceptionHandlingMiddleware"/>
    /// </summary>
    /// <param name="app"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseMasaExceptionHandling(this IApplicationBuilder app, Action<RequestLocalizationOptions> action)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseRequestLocalization(action);

        return app;
    }
}
