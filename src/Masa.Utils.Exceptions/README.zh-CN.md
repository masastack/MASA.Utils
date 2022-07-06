中 | [EN](README.md)

## Masa.Utils.Exceptions

提供了用于处理Web应用程序异常的模型

* 支持自定义处理异常，用于处理非框架类异常
* 接管`UserFriendlyException`异常，并响应状态码为299，返回友好的错误信息
* 默认处理所有异常，并对外输出`An error occur in masa framework`

## 用例:

``` C#
Install-Package Masa.Utils.Exceptions
```

1. 修改`Program.cs`

``` C#
app.UseMasaExceptionHandler();
```

2. 如何使用？

``` C#
app.MapGet("/Test", ()
{
    throw new UserFriendlyException("This method is deprecated");
}
```

3. 错误响应消息，其中Http状态码为299

``` js
This method is deprecated
```

## 如何自定义异常处理？

``` C#
app.UseMasaExceptionHandler(option =>
{
    option.CatchAllException = true;//是否捕获所有异常，默认为true，捕获到的异常默认输出：An error occur in masa framework

    // 自定义处理异常，与ExceptionFilter类似，可根据异常类型处理异常信息，并通过ToResult方法输出响应结果
    option.ExceptionHandler = context =>
    {
        if (context.Exception is ArgumentNullException argumentNullException)
        {
            context.ToResult("参数不能为空");
        }
    };
});
```

## 常见问题

1. 不希望记录异常为UserFriendlyException时的错误信息？

    ``` C#
    builder.Services.Configure<MasaExceptionLogRelationOptions>(options =>
    {
        options.MapLogLevel<UserFriendlyException>(LogLevel.None);
    });
    ```