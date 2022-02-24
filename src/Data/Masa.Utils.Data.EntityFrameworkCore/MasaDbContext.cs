namespace Masa.Utils.Data.EntityFrameworkCore;

public class MasaDbContext : DbContext
{
    private readonly MasaDbContextOptions? _options;

    public MasaDbContext() { }

    public MasaDbContext(MasaDbContextOptions options)
        : base(options)
    {
        _options = options;
    }

    /// <summary>
    /// Automatic filter soft delete data.
    /// When you override this method,you should call base.<see cref="OnModelCreating(ModelBuilder)"/>.
    /// <inheritdoc/>
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingExecuting(modelBuilder);

        // null when run dotnet ef cli
        if (_options == null)
        {
            base.OnModelCreating(modelBuilder);
            return;
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var provider in _options.QueryFilterProviders)
            {
                try
                {
                    var lambda = provider.OnExecuting(entityType);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occured when QueryFilterProvider executing", ex);
                }
            }
        }
    }

    /// <summary>
    /// Use this method instead of OnModelCreating
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected virtual void OnModelCreatingExecuting(ModelBuilder modelBuilder)
    {

    }

    /// <summary>
    /// Automatic soft delete.
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public override int SaveChanges()
    {
        return SaveChanges(true);
    }

    /// <summary>
    /// Automatic soft delete.
    /// <inheritdoc/>
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <returns></returns>
    public sealed override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnFilterExecuting();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private void OnFilterExecuting()
    {
        if (_options != null)
        {
            foreach (var filter in _options.SaveChangesFilters)
            {
                try
                {
                    filter.OnExecuting(ChangeTracker);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occured when intercept SaveChanges(Async)", ex);
                }
            }
        }
    }

    /// <summary>
    /// Automatic soft delete.
    /// <inheritdoc/>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(true, cancellationToken);
    }

    /// <summary>
    /// Automatic soft delete.
    /// <inheritdoc/>
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public sealed override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnFilterExecuting();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
