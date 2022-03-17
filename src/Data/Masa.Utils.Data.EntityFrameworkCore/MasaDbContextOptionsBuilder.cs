namespace Masa.Utils.Data.EntityFrameworkCore;

public abstract class MasaDbContextOptionsBuilder
{
    public DbContextOptionsBuilder DbContextOptionsBuilder;

    public IServiceProvider ServiceProvider { get; }

    internal bool EnableSoftwareDelete { get; private set; }

    protected MasaDbContextOptionsBuilder(IServiceProvider serviceProvider, DbContextOptions options, bool enableSoftwareDelete)
    {
        DbContextOptionsBuilder = new DbContextOptionsBuilder(options);
        ServiceProvider = serviceProvider;
        EnableSoftwareDelete = enableSoftwareDelete;
    }

    public MasaDbContextOptionsBuilder UseSoftDelete()
    {
        EnableSoftwareDelete = true;
        return this;
    }
}
