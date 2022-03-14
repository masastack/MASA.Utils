namespace Masa.Utils.Data.EntityFrameworkCore;

public abstract class MasaDbContextOptionsBuilder : DbContextOptionsBuilder
{
    public MasaDbContextOptionsBuilder(DbContextOptions options)
        : base(options)
    {
        ContextLifetime = ServiceLifetime.Scoped;
        OptionsLifetime = ServiceLifetime.Scoped;
    }

    public ServiceLifetime ContextLifetime { get; set; }

    public ServiceLifetime OptionsLifetime { get; set; }

    public abstract MasaDbContextOptionsBuilder UseModelCreatingProvider<TProvider>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TProvider : class, IModelCreatingProvider;

    public abstract MasaDbContextOptionsBuilder UseSaveChangesFilter<TFilter>()
        where TFilter : class, ISaveChangesFilter;
}
