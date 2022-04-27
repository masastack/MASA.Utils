// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Security.Authentication;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register <see cref="MasaAuthorizationFilter"/> to <see cref="MvcOptions"/>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="systemCode">Configure your system code</param>
    /// <returns></returns>
    public static IServiceCollection AddMasaAuth(this IServiceCollection services, string systemCode)
        => AddMasaAuthCore(services, systemCode);

    /// <summary>
    /// Register MasaAuth
    /// </summary>
    /// <param name="services"></param>
    /// <param name="systemCode"></param>
    /// <param name="authServerUrl"></param>
    /// <param name="clinetId"></param>
    /// <param name="clientSecret"></param>
    /// <returns></returns>
    public static IServiceCollection AddMasaAuth(
        this IServiceCollection services,
        string systemCode,
        string authServerUrl,
        string clinetId,
        string clientSecret)
    {
        return services.AddMasaAuthCore(systemCode, () =>
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = "Cookies";
                })
                .AddCookie("Cookies", options => { options.ExpireTimeSpan = TimeSpan.FromSeconds(3600); })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = authServerUrl;
                    options.RequireHttpsMetadata = false;
                    options.ClientId = clinetId;
                    options.ClientSecret = clientSecret;
                    options.ResponseType = OpenIdConnectResponseType.Code;

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.UseTokenLifetime = true;

                    options.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(3600);
                    options.TokenValidationParameters.RequireExpirationTime = true;
                    options.TokenValidationParameters.ValidateLifetime = true;

                    options.NonceCookie.SameSite = SameSiteMode.Unspecified;
                    options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
                });
        });
    }

    private static IServiceCollection AddMasaAuthCore(this IServiceCollection services, string systemCode, Action? action = null)
    {
        if (services.Any(service => service.ServiceType == typeof(AuthenticationService)))
        {
            return services;
        }

        services.AddSingleton<AuthenticationService>();

        if (services.All(s => s.ServiceType != typeof(IHttpContextAccessor)))
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        services.Configure<MvcOptions>(options => { options.Filters.Add<MasaAuthorizationFilter>(); });

        MasaAuthOptions masaAuthOptions = new MasaAuthOptions
        {
            SystemCode = systemCode
        };
        services.TryAddSingleton(typeof(IOptions<MasaAuthOptions>), _ => Options.Create(masaAuthOptions));
        action?.Invoke();
        return services;
    }

    private class AuthenticationService
    {
    }
}
