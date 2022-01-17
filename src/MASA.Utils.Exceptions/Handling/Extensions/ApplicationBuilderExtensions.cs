using MASA.Framework.Exceptions.Handling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
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
}
