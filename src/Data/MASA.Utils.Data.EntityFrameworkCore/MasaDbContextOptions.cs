namespace MASA.Utils.Data.EntityFrameworkCore;

public abstract class MasaDbContextOptions : DbContextOptions
{
    public MasaDbContextOptions()
        : base(new Dictionary<Type, IDbContextOptionsExtension>())
    {

    }

    public MasaDbContextOptions([NotNull] IReadOnlyDictionary<Type, IDbContextOptionsExtension> extensions)
        : base(extensions)
    {

    }

    /// <summary>
    /// Can be used to filter data
    /// </summary>
    public abstract IEnumerable<IQueryFilterProvider> QueryFilterProviders { get; }

    /// <summary>
    /// Can be used to intercept SaveChanges(Async) method
    /// </summary>
    public abstract IEnumerable<ISaveChangesFilter> SaveChangesFilters { get; }
}
