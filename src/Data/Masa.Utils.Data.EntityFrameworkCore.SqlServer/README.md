[中](README.zh-CN.md) | EN

## Masa.Utils.Data.EntityFrameworkCore.SqlServer

## Example:

```c#
Install-Package Masa.Utils.Data.EntityFrameworkCore.SqlServer
```

##### Usage 1:

1. Configure appsettings.json

``` appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;uid=sa;pwd=P@ssw0rd;database=identity"
  }
}
```

2. Using MasaDbContext

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder => optionsBuilder.UseSoftDelete().UseSqlServer());
```

##### Usage 2:

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder => optionsBuilder.UseSoftDelete().UseSqlServer("server=localhost;uid=sa;pwd=P@ssw0rd;database=identity"));
```