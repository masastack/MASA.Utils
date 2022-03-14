namespace Masa.Utils.Data.EntityFrameworkCore;

public abstract class MasaDbContextOptionsBuilder : DbContextOptionsBuilder
{
    public readonly IServiceCollection Services;

    public ServiceLifetime ContextLifetime { get; set; }

    public ServiceLifetime OptionsLifetime { get; set; }

    public MasaDbContextOptionsBuilder(IServiceCollection services, DbContextOptions options)
        : base(options)
    {
        Services = services;
        ContextLifetime = ServiceLifetime.Scoped;
        OptionsLifetime = ServiceLifetime.Scoped;
    }

    public abstract MasaDbContextOptionsBuilder UseModelCreatingProvider<TProvider>(
        ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TProvider : class, IModelCreatingProvider;

    public abstract MasaDbContextOptionsBuilder UseSaveChangesFilter<TFilter>()
        where TFilter : class, ISaveChangesFilter;
}
