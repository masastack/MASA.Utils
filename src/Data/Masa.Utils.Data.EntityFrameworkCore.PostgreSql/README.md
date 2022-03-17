[中](README.zh-CN.md) | EN

## Masa.Utils.Data.EntityFrameworkCore.PostgreSql

## Example:

```c#
Install-Package Masa.Utils.Data.EntityFrameworkCore.PostgreSql
```

##### Usage 1:

1. Configure appsettings.json

``` appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=myserver;Username=sa;Password=P@ssw0rd;Database=identity"
  }
}
```

2. Using MasaDbContext

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder => optionsBuilder.UseSoftDelete().UseNpgsql());
```

##### Usage 2:

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder => optionsBuilder.UseSoftDelete().UseNpgsql("Host=myserver;Username=sa;Password=P@ssw0rd;Database=identity"));
```