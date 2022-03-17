[中](README.zh-CN.md) | EN

## Masa.Utils.Data.EntityFrameworkCore

## Example:

```c#
Install-Package Masa.Utils.Data.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
```

#### Basic usage:

Using MasaDbContext

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSoftDelete();//enable soft delete
    optionsBuilder.DbContextOptionsBuilder.UseSqlServer("server=localhost;uid=sa;pwd=P@ssw0rd;database=identity");
});
```

Recommended usage:

[SqlServer](../Masa.Utils.Data.EntityFrameworkCore.SqlServer/README.md)

#### data filter

``` C#
public async Task<string> GetAllAsync([FromServices] IRepository<Users> repository, [FromServices] IDataFilter dataFilter)
{
    // Temporarily disable soft delete filtering
    using (dataFilter.Disable<ISoftDelete>())
    {
        var list = (await repository.GetListAsync()).ToList();
        return System.Text.Json.JsonSerializer.Serialize(list);
    }
}
```