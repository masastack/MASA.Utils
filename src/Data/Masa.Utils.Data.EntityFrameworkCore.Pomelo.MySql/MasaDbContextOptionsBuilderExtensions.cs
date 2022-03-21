namespace Masa.Utils.Data.EntityFrameworkCore.Pomelo.MySql;

public static class MasaDbContextOptionsBuilderExtensions
{
    public static MasaDbContextOptionsBuilder UseMySql(
        this MasaDbContextOptionsBuilder builder,
        ServerVersion serverVersion,
        Action<MySqlDbContextOptionsBuilder>? mySqlOptionsAction = null)
    {
        var connectionStringProvider = builder.ServiceProvider.GetRequiredService<IConnectionStringProvider>();
        return builder.UseMySql(connectionStringProvider.GetConnectionString(), serverVersion, mySqlOptionsAction);
    }

    public static MasaDbContextOptionsBuilder UseMySql(
        this MasaDbContextOptionsBuilder builder,
        string connectionString,
        ServerVersion serverVersion,
        Action<MySqlDbContextOptionsBuilder>? mySqlOptionsAction = null)
    {
        builder.DbContextOptionsBuilder.UseMySql(connectionString, serverVersion, mySqlOptionsAction);
        return builder;
    }
}
