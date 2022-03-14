namespace Masa.Utils.Data.EntityFrameworkCore;

public class MasaDbContextOptionsBuilder<TContext> : MasaDbContextOptionsBuilder
    where TContext : MasaDbContext
{
    private readonly IServiceCollection _services;

    public MasaDbContextOptionsBuilder(IServiceCollection services)
        : base(new DbContextOptions<TContext>())
    {
        _services = services;
    }

    public override MasaDbContextOptionsBuilder UseModelCreatingProvider<TProvider>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        _services.TryAddEnumerable(new ServiceDescriptor(typeof(IModelCreatingProvider), typeof(TProvider), serviceLifetime));
        return this;
    }

    public override MasaDbContextOptionsBuilder UseSaveChangesFilter<TFilter>()
    {
        _services.AddScoped<ISaveChangesFilter, TFilter>();
        return this;
    }
}
