﻿namespace Masa.Utils.Data.EntityFrameworkCore.Test;

[TestClass]
public class MasaDbContextTests : TestBase
{
    [TestMethod]
    public async Task TestAddAsync()
    {
        await using var dbContext = CreateDbContext(true, out _);
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
        Services.Configure<MasaDbConnectionOptions>(options =>
        {
            options.DefaultConnection = "soft-delete-db";
        });
        await using var dbContext = CreateDbContext(true, out IServiceProvider serviceProvider);
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
    public async Task TestDisabledSoftDelete()
    {
        Services.AddMasaDbContext<TestDbContext>(options => options.UseInMemoryDatabase("disabled-soft-delete-db"));
        var serviceProvider = Services.BuildServiceProvider();
        var dbContext =  serviceProvider.GetRequiredService<TestDbContext>();
        var student = new Student
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
}