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
