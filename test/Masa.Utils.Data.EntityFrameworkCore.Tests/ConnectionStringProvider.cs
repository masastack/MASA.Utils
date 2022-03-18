namespace Masa.Utils.Data.EntityFrameworkCore.Test;

public class ConnectionStringProvider : IConnectionStringProvider
{
    public Task<string> GetConnectionStringAsync() => Task.FromResult(GetConnectionString());

    public string GetConnectionString() => "custom";
}
