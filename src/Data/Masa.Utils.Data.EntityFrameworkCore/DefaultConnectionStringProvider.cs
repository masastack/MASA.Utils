// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.EntityFrameworkCore;

public class DefaultConnectionStringProvider : IConnectionStringProvider
{
    private readonly IOptionsSnapshot<MasaDbConnectionOptions> _options;

    public DefaultConnectionStringProvider(IOptionsSnapshot<MasaDbConnectionOptions> options) => _options = options;

    public Task<string> GetConnectionStringAsync() => Task.FromResult(GetConnectionString());

    public string GetConnectionString() => _options.Value.DefaultConnection;
}
