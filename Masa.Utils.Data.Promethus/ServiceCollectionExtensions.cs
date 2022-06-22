// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Promethus;

public static class ServiceCollectionExtensions
{
    private const string PROMETHUS_HTTP_CLIENT_NAME = "promethus_client_name";

    public static IServiceCollection AddPromethusClient(this IServiceCollection services, string uri)
    {
        ArgumentNullException.ThrowIfNull(uri, nameof(uri));

        if (services.Any(service => service.GetType() == typeof(IMasaPromethusClient)))
            return services;

        services.AddCaller(builder =>
        {
            builder.UseHttpClient(options =>
            {
                options.BaseAddress = uri;
                options.Name = PROMETHUS_HTTP_CLIENT_NAME;
            });
        });

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        jsonOptions.Converters.Add(new JsonStringEnumConverter());

        services.AddScoped<IMasaPromethusClient>(ServiceProvider =>
        {
            var caller = ServiceProvider.GetRequiredService<ICallerFactory>().CreateClient(PROMETHUS_HTTP_CLIENT_NAME);
            return new MasaPromethusClient(caller, jsonOptions);
        });
        return services;
    }
}
