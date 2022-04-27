// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.EntityFrameworkCore.Test;

public class TestDbContext : MasaDbContext
{
    public TestDbContext(MasaDbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreatingExecuting(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().OwnsOne(x => x.Address);
        modelBuilder.Entity<Student>().OwnsMany(t => t.Hobbies);
    }
}
