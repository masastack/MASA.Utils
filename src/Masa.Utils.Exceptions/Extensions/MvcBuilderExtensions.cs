namespace Masa.Utils.Exceptions.Extensions;

public static class MvcBuilderExtensions
{
    /// <summary>
    /// Register <see cref="GlobalExceptionFilter"/> to <see cref="MvcOptions"/>
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IMvcBuilder AddMasaExceptionHandling(this IMvcBuilder builder)
    {
        return builder.AddMasaExceptionHandling(_ => { });
    }

    /// <summary>
    /// Register <see cref="GlobalExceptionFilter"/> to <see cref="MvcOptions"/>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="action">Configure handling options</param>
    /// <returns></returns>
    public static IMvcBuilder AddMasaExceptionHandling(this IMvcBuilder builder, Action<MasaExceptionHandlingOptions> action)
    {
        builder.Services.AddLocalization();

        builder.Services.Configure<MvcOptions>(options => { options.Filters.Add<GlobalExceptionFilter>(); });

        builder.Services.Configure(action);

        return builder;
    }
}
