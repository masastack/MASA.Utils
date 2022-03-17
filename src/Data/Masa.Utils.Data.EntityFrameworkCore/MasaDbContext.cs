namespace Masa.Utils.Data.EntityFrameworkCore;

public abstract class MasaDbContext : DbContext
{
    protected readonly IDataFilter? DataFilter;
    protected readonly MasaDbContextOptions? Options;

    public MasaDbContext(MasaDbContextOptions options) : base(options)
    {
        Options = options;
        DataFilter = options.ServiceProvider.GetService<IDataFilter>();
    }

    /// <summary>
    /// Automatic filter soft delete data.
    /// When you override this method,you should call base.<see cref="OnModelCreating(ModelBuilder)"/>.
    /// <inheritdoc/>
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        GlobalSoftwareFilters(modelBuilder);

        OnModelCreatingExecuting(modelBuilder);

        // null when run dotnet ef cli
        if (Options == null)
        {
            base.OnModelCreating(modelBuilder);
            return;
        }

        foreach (var provider in Options.ModelCreatingProviders)
            provider.Configure(modelBuilder);
    }

    /// <summary>
    /// Use this method instead of OnModelCreating
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected virtual void OnModelCreatingExecuting(ModelBuilder modelBuilder)
    {

    }

    protected virtual void GlobalSoftwareFilters(ModelBuilder modelBuilder)
    {
        var methodInfo = typeof(MasaDbContext).GetMethod(nameof(ConfigureGlobalSoftwareFilters),
            BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            methodInfo!.MakeGenericMethod(entityType.ClrType).Invoke(this, new object?[] { modelBuilder, entityType });
        }
    }

    private void ConfigureGlobalSoftwareFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        if (mutableEntityType.BaseType == null)
        {
            var filterExpression = CreateFilterExpression<TEntity>();
            if (filterExpression != null)
                modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
        }
    }

    private Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>()
        where TEntity : class
    {
        Expression<Func<TEntity, bool>>? expression = null;

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            expression = entity => !IsSoftDeleteFilterEnabled ||
                !EF.Property<bool>(entity, nameof(ISoftDelete.IsDeleted));
        }

        return expression;
    }

    protected virtual bool IsSoftDeleteFilterEnabled => (Options?.EnableSoftware ?? false) && (DataFilter?.IsEnabled<ISoftDelete>() ?? false);

    /// <summary>
    /// Automatic soft delete.
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public override int SaveChanges() => SaveChanges(true);

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
        if (Options != null)
        {
            foreach (var filter in Options.SaveChangesFilters)
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
        => SaveChangesAsync(true, cancellationToken);

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
