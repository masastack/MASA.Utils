namespace Masa.Utils.Data.EntityFrameworkCore;

public class MasaDbConnectionOptions
{
    public MasaDbConnectionOptions(string defaultConnection)
    {
        DefaultConnection = defaultConnection;
    }

    public string DefaultConnection { get; private set; }
}
