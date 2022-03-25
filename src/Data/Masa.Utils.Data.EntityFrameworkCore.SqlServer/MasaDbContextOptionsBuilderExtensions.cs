namespace Masa.Utils.Data.EntityFrameworkCore.SqlServer;

public static class MasaDbContextOptionsBuilderExtensions
{
    public static MasaDbContextOptionsBuilder UseSqlServer(
        this MasaDbContextOptionsBuilder builder,
        Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null)
    {
        var connectionStringProvider = builder.ServiceProvider.GetRequiredService<IConnectionStringProvider>();
        return builder.UseSqlServer(connectionStringProvider.GetConnectionString(), sqlServerOptionsAction);
    }

    public static MasaDbContextOptionsBuilder UseSqlServer(
        this MasaDbContextOptionsBuilder builder,
        string connectionString,
        Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null)
    {
        builder.DbContextOptionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction);
        return builder;
    }

    public static MasaDbContextOptionsBuilder UseSqlServer(
        this MasaDbContextOptionsBuilder builder,
        DbConnection connection,
        Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null)
    {
        builder.DbContextOptionsBuilder.UseSqlServer(connection, sqlServerOptionsAction);
        return builder;
    }
}
