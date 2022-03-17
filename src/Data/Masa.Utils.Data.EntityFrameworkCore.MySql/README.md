[中](README.zh-CN.md) | EN

## Masa.Utils.Data.EntityFrameworkCore.MySql

## Example:

```c#
Install-Package Masa.Utils.Data.EntityFrameworkCore.MySql
```

##### Usage 1:

1. Configure appsettings.json

``` appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=identity;Uid=myUsername;Pwd=P@ssw0rd;"
  }
}
```

2. Using MasaDbContext

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder => optionsBuilder.UseSoftDelete().UseMySQL());
```

##### Usage 2:

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder => optionsBuilder.UseSoftDelete().UseMySQL("Server=localhost;Database=identity;Uid=myUsername;Pwd=P@ssw0rd;"));
```