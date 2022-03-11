namespace Masa.Utils.Data.EntityFrameworkCore;

public class MasaDbContextOptionsBuilder<TContext> : MasaDbContextOptionsBuilder
    where TContext : MasaDbContext
{
    public MasaDbContextOptionsBuilder(IServiceCollection services)
        : base(services, new DbContextOptions<TContext>())
    {

    }

    public override MasaDbContextOptionsBuilder UseQueryFilterProvider<TProvider>()
    {
        Services.AddScoped<IQueryFilterProvider, TProvider>();
        return this;
    }

    public override MasaDbContextOptionsBuilder UseSaveChangesFilter<TFilter>()
    {
        Services.AddScoped<ISaveChangesFilter, TFilter>();
        return this;
    }
}
