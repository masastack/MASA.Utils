namespace Masa.Utils.Data.EntityFrameworkCore.PostgreSql;

public static class MasaDbContextOptionsBuilderExtensions
{
    public static MasaDbContextOptionsBuilder UseNpgsql(
        this MasaDbContextOptionsBuilder builder,
        Action<NpgsqlDbContextOptionsBuilder>? npgsqlOptionsAction = null)
    {
        var connectionStringProvider = builder.ServiceProvider.GetRequiredService<IConnectionStringProvider>();
        return builder.UseNpgsql(connectionStringProvider.GetConnectionString(), npgsqlOptionsAction);
    }

    public static MasaDbContextOptionsBuilder UseNpgsql(
        this MasaDbContextOptionsBuilder builder,
        string connectionString,
        Action<NpgsqlDbContextOptionsBuilder>? npgsqlOptionsAction = null)
    {
        builder.DbContextOptionsBuilder.UseNpgsql(connectionString, npgsqlOptionsAction);
        return builder;
    }

    public static MasaDbContextOptionsBuilder UseNpgsql(
        this MasaDbContextOptionsBuilder builder,
        DbConnection connection,
        Action<NpgsqlDbContextOptionsBuilder>? npgsqlOptionsAction = null)
    {
        builder.DbContextOptionsBuilder.UseNpgsql(connection, npgsqlOptionsAction);
        return builder;
    }
}
