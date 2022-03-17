中 | [EN](README.md)

## Masa.Utils.Data.EntityFrameworkCore

## 用例:

```c#
Install-Package Masa.Utils.Data.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
```

#### 基本用法:

使用MasaDbContext

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSoftDelete();//启用软删除
    optionsBuilder.DbContextOptionsBuilder.UseSqlServer("server=localhost;uid=sa;pwd=P@ssw0rd;database=identity");
});
```

推荐用法:

[SqlServer](../Masa.Utils.Data.EntityFrameworkCore.SqlServer/README.zh-CN.md)

#### 数据过滤器

``` C#
public async Task<string> GetAllAsync([FromServices] IRepository<Users> repository, [FromServices] IDataFilter dataFilter)
{
    // 临时禁用软删除过滤
    using (dataFilter.Disable<ISoftDelete>())
    {
        var list = (await repository.GetListAsync()).ToList();
        return System.Text.Json.JsonSerializer.Serialize(list);
    }
}
```