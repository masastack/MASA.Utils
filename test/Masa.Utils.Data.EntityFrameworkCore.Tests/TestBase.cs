namespace Masa.Utils.Data.EntityFrameworkCore.Test;

public class TestBase
{
    protected readonly IServiceCollection Services;

    public TestBase()
    {
        Services = new ServiceCollection();
        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        Services.AddSingleton<IConfiguration>(configurationRoot);
    }

    protected TestDbContext CreateDbContext(bool enableSoftDelete, out IServiceProvider serviceProvider)
    {
        Services.AddMasaDbContext<TestDbContext>(options =>
        {
            if (enableSoftDelete)
                options.UseSoftDelete();

            options.UseInMemoryDatabase();
        });
        serviceProvider = Services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<TestDbContext>();
    }
}
