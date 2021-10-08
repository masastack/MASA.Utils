namespace MASA.Utils.Data.EntityFrameworkCore;

public class MasaDbContextOptions<TContext> : MasaDbContextOptions
    where TContext : MasaDbContext
{
    private readonly DbContextOptions _originOptions;

    public MasaDbContextOptions(
        DbContextOptions originOptions,
        IEnumerable<IQueryFilterProvider> queryFilterProviders,
        IEnumerable<ISaveChangesFilter> saveChangesFilters)
    {
        _originOptions = originOptions;
        QueryFilterProviders = queryFilterProviders;
        SaveChangesFilters = saveChangesFilters;
    }

    /// <summary>
    /// Can be used to filter data
    /// </summary>
    public override IEnumerable<IQueryFilterProvider> QueryFilterProviders { get; }

    /// <summary>
    /// Can be used to intercept SaveChanges(Async) method
    /// </summary>
    public override IEnumerable<ISaveChangesFilter> SaveChangesFilters { get; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override Type ContextType => _originOptions.ContextType;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override bool IsFrozen => _originOptions.IsFrozen;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<IDbContextOptionsExtension> Extensions => _originOptions.Extensions;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="TExtension"></typeparam>
    /// <param name="extension"></param>
    /// <returns></returns>
    public override DbContextOptions WithExtension<TExtension>(TExtension extension)
    {
        return _originOptions.WithExtension(extension);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="TExtension"></typeparam>
    /// <returns></returns>
    public override TExtension FindExtension<TExtension>()
    {
        return _originOptions.GetExtension<TExtension>();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void Freeze()
    {
        _originOptions.Freeze();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="TExtension"></typeparam>
    /// <returns></returns>
    public override TExtension GetExtension<TExtension>()
    {
        return _originOptions.GetExtension<TExtension>();
    }
}

