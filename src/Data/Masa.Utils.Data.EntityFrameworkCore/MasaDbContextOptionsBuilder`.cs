namespace Masa.Utils.Data.EntityFrameworkCore;

public class MasaDbContextOptionsBuilder<TContext> : MasaDbContextOptionsBuilder
    where TContext : MasaDbContext
{
    public MasaDbContextOptionsBuilder(IServiceProvider serviceProvider) : base(serviceProvider, new DbContextOptions<TContext>(), false)
    {
    }
}
