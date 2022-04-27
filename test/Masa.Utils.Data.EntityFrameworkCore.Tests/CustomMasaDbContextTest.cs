// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.EntityFrameworkCore.Test;

[TestClass]
public class TestCustomMasaDbContextTest : TestBase
{
    [TestMethod]
    public void TestCustomDatabase()
    {
        Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
        var dbContext = CreateDbContext(false, out IServiceProvider serviceProvider);
        Assert.IsTrue(GetDatabaseName(dbContext) == "custom");
    }

    [TestMethod]
    public void TestCustomDatabase2()
    {
        Services.Configure<MasaDbConnectionOptions>(options =>
        {
            options.DefaultConnection = "user";
        });
        var dbContext = CreateDbContext(false, out IServiceProvider serviceProvider);
        Assert.IsTrue(GetDatabaseName(dbContext) == "user");
    }

    private string GetDatabaseName(DbContext dbContext)
    {
        FieldInfo fieldInfo =
            dbContext.GetType().GetField("Options", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
        MasaDbContextOptions masaDbContextOptions = (fieldInfo.GetValue(dbContext) as MasaDbContextOptions)!;

        foreach (var dbContextOptionsExtension in masaDbContextOptions.Extensions)
        {
            if (dbContextOptionsExtension is InMemoryOptionsExtension inMemoryOptionsExtension)
            {
                return inMemoryOptionsExtension.StoreName;
            }
        }
        return string.Empty;
    }
}
