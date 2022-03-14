namespace Masa.Utils.Data.EntityFrameworkCore;

public abstract class MasaDbContextOptionsBuilder : DbContextOptionsBuilder
{
    public readonly IServiceCollection Services;

    protected MasaDbContextOptionsBuilder(IServiceCollection services, DbContextOptions options) : base(options)
        => Services = services;

    public abstract MasaDbContextOptionsBuilder UseQueryFilterProvider<TProvider>()
        where TProvider : class, IQueryFilterProvider;

    public abstract MasaDbContextOptionsBuilder UseSaveChangesFilter<TFilter>()
        where TFilter : class, ISaveChangesFilter;
}
