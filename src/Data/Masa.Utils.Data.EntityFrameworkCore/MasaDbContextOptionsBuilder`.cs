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

    public override MasaDbContextOptionsBuilder UseQueryFilterProvider<TProvider>()
    {
        _services.AddScoped<IQueryFilterProvider, TProvider>();
        return this;
    }

    public override MasaDbContextOptionsBuilder UseSaveChangesFilter<TFilter>()
    {
        _services.AddScoped<ISaveChangesFilter, TFilter>();
        return this;
    }
}
