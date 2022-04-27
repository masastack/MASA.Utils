// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.EntityFrameworkCore;

public abstract class MasaDbContextOptionsBuilder
{
    public DbContextOptionsBuilder DbContextOptionsBuilder;

    public IServiceProvider ServiceProvider { get; }

    internal bool EnableSoftDelete { get; private set; }

    protected MasaDbContextOptionsBuilder(IServiceProvider serviceProvider, DbContextOptions options, bool enableSoftDelete)
    {
        DbContextOptionsBuilder = new DbContextOptionsBuilder(options);
        ServiceProvider = serviceProvider;
        EnableSoftDelete = enableSoftDelete;
    }

    public MasaDbContextOptionsBuilder UseSoftDelete()
    {
        EnableSoftDelete = true;
        return this;
    }
}
