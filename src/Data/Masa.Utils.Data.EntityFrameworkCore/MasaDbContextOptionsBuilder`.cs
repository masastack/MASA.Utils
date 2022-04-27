// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.EntityFrameworkCore;

public class MasaDbContextOptionsBuilder<TContext> : MasaDbContextOptionsBuilder
    where TContext : MasaDbContext
{
    public MasaDbContextOptionsBuilder(IServiceProvider serviceProvider) : base(serviceProvider, new DbContextOptions<TContext>(), false)
    {
    }
}
