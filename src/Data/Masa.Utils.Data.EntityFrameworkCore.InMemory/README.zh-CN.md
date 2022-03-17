中 | [EN](README.md)

## Masa.Utils.Data.EntityFrameworkCore.InMemory

## 用例:

```c#
Install-Package Masa.Utils.Data.EntityFrameworkCore.InMemory
```

#### 用法1:

1. 配置appsettings.json

``` appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "identity"
  }
}
```

2. 使用MasaDbContext

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder => optionsBuilder.UseSoftDelete().UseInMemoryDatabase());
```

#### 用法2:

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder => optionsBuilder.UseSoftDelete().UseInMemoryDatabase("identity"));
```