namespace Masa.Utils.Data.EntityFrameworkCore;

public abstract class MasaDbContextOptionsBuilder : DbContextOptionsBuilder
{
    public MasaDbContextOptionsBuilder(DbContextOptions options)
        : base(options)
    {

    }

    public abstract MasaDbContextOptionsBuilder UseQueryFilterProvider<TProvider>()
        where TProvider : class, IQueryFilterProvider;

    public abstract MasaDbContextOptionsBuilder UseSaveChangesFilter<TFilter>()
        where TFilter : class, ISaveChangesFilter;
}
