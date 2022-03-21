namespace Masa.Utils.Data.EntityFrameworkCore.InMemory;

public static class MasaDbContextOptionsBuilderExtensions
{
    public static MasaDbContextOptionsBuilder UseInMemoryDatabase(
        this MasaDbContextOptionsBuilder builder,
        Action<InMemoryDbContextOptionsBuilder>? inMemoryOptionsAction = null)
    {
        var connectionStringProvider = builder.ServiceProvider.GetRequiredService<IConnectionStringProvider>();
        return builder.UseInMemoryDatabase(connectionStringProvider.GetConnectionString(), inMemoryOptionsAction);
    }

    public static MasaDbContextOptionsBuilder UseInMemoryDatabase(
        this MasaDbContextOptionsBuilder builder,
        string databaseName,
        Action<InMemoryDbContextOptionsBuilder>? inMemoryOptionsAction = null)
    {
        builder.DbContextOptionsBuilder.UseInMemoryDatabase(databaseName, inMemoryOptionsAction);
        return builder;
    }
}
