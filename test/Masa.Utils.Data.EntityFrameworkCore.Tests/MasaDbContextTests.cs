namespace Masa.Utils.Data.EntityFrameworkCore.Test;

[TestClass]
public class MasaDbContextTests
{
    private readonly IServiceCollection _services;

    public MasaDbContextTests()
    {
        _services = new ServiceCollection();
        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        _services.AddSingleton<IConfiguration>(configurationRoot);
    }

    [TestMethod]
    public async Task TestAddAsync()
    {
        var dbContext = CreateDbContext(true, out _);
        await dbContext.Set<Student>().AddAsync(new Student()
        {
            Id = 1,
            Name = "Jim",
            Age = 18,
        });
        await dbContext.SaveChangesAsync();
        Assert.IsTrue(await dbContext.Set<Student>().CountAsync() == 1);
    }

    [TestMethod]
    public async Task TestSoftDeleteAsync()
    {
        var dbContext = CreateDbContext(true, out IServiceProvider serviceProvider);
        var student = new Student()
        {
            Id = 1,
            Name = "Jim",
            Age = 18,
        };
        await dbContext.Set<Student>().AddAsync(student);
        await dbContext.SaveChangesAsync();
        Assert.IsTrue(await dbContext.Set<Student>().CountAsync() == 1);

        dbContext.Set<Student>().Remove(student);
        await dbContext.SaveChangesAsync();

        Assert.IsTrue(await dbContext.Set<Student>().CountAsync() == 0);

        var dataFilter = serviceProvider.GetRequiredService<IDataFilter>();
        using (dataFilter.Disable<ISoftDelete>())
        {
            Assert.IsTrue(await dbContext.Set<Student>().CountAsync() == 1);

            student = (await dbContext.Set<Student>().FirstOrDefaultAsync())!;
            Assert.IsTrue(student.Id == 1);
            Assert.IsTrue(student.Name == "Jim");
            Assert.IsTrue(student.Age == 18);
            Assert.IsTrue(student.IsDeleted);
        }
    }

    [TestMethod]
    public async Task TestDisabledSoftware()
    {
        var dbContext = CreateDbContext(false, out IServiceProvider serviceProvider);
        var student = new Student()
        {
            Id = 1,
            Name = "Jim",
            Age = 18,
        };
        await dbContext.Set<Student>().AddAsync(student);
        await dbContext.SaveChangesAsync();
        Assert.IsTrue(await dbContext.Set<Student>().CountAsync() == 1);

        dbContext.Set<Student>().Remove(student);
        await dbContext.SaveChangesAsync();

        Assert.IsTrue(await dbContext.Set<Student>().CountAsync() == 0);

        var dataFilter = serviceProvider.GetRequiredService<IDataFilter>();
        using (dataFilter.Disable<ISoftDelete>())
            Assert.IsTrue(await dbContext.Set<Student>().CountAsync() == 0);
    }

    private TestDbContext CreateDbContext(bool enableSoftware, out IServiceProvider serviceProvider)
    {
        _services.AddMasaDbContext<TestDbContext>(options =>
        {
            if (enableSoftware)
                options.UseSoftDelete();

            options.UseInMemoryDatabase();
        });
        serviceProvider = _services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<TestDbContext>();
    }
}
