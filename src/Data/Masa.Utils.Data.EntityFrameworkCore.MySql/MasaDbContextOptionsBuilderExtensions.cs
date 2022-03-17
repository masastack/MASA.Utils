namespace Masa.Utils.Data.EntityFrameworkCore.MySql;

public static class MasaDbContextOptionsBuilderExtensions
{
    public static MasaDbContextOptionsBuilder UseMySQL(
        this MasaDbContextOptionsBuilder builder,
        Action<MySQLDbContextOptionsBuilder>? mySqlOptionsAction = null)
    {
        var connectionStringProvider = builder.ServiceProvider.GetRequiredService<IConnectionStringProvider>();
        return builder.UseMySQL(connectionStringProvider.GetConnectionString(), mySqlOptionsAction);
    }

    public static MasaDbContextOptionsBuilder UseMySQL(
        this MasaDbContextOptionsBuilder builder,
        string connectionString,
        Action<MySQLDbContextOptionsBuilder>? mySqlOptionsAction = null)
    {
        builder.DbContextOptionsBuilder.UseMySQL(connectionString, mySqlOptionsAction);
        return builder;
    }
}
