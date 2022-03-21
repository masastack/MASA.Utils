namespace Masa.Utils.Data.EntityFrameworkCore;

public interface IConnectionStringProvider
{
    Task<string> GetConnectionStringAsync();

    string GetConnectionString();
}
