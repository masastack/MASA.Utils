namespace Masa.Utils.Data.EntityFrameworkCore;

public abstract class MasaDbContextOptions : DbContextOptions
{
    public readonly IServiceProvider ServiceProvider;

    public abstract IEnumerable<IModelCreatingProvider> ModelCreatingProviders { get; }

    /// <summary>
    /// Can be used to intercept SaveChanges(Async) method
    /// </summary>
    public abstract IEnumerable<ISaveChangesFilter> SaveChangesFilters { get; }

    public bool EnableSoftware { get; }

    protected MasaDbContextOptions(IServiceProvider serviceProvider, bool enableSoftware)
    {
        ServiceProvider = serviceProvider;
        EnableSoftware = enableSoftware;
    }
}
