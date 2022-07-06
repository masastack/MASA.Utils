[中](README.zh-CN.md) | EN

## Masa.Utils.Exceptions

Provides a model for handling web application exceptions

* Support custom handling exceptions for handling non-framework exceptions
* Take over the `UserFriendlyException` exception and respond with a status code of 299 and return a friendly error message
* Handle all exceptions by default, and output `An error occur in masa framework` externally

## Example:

``` C#
Install-Package Masa.Utils.Exceptions
```

1. Modify `Program.cs`

``` C#
app.UseMasaExceptionHandler();
```

2. How to use?

``` C#
app.MapGet("/Test", ()
{
    throw new UserFriendlyException("This method is deprecated");
}
```

3. Error response message, where Http status code is 299

``` js
This method is deprecated
```

## How to customize exception handling?

``` C#
app.UseMasaExceptionHandler(option =>
{
    option.CatchAllException = true;//Whether to catch all exceptions, the default is true, the default output of caught exceptions: An error occur in masa framework

    // Custom handling exceptions, similar to ExceptionFilter, can handle exception information according to the exception type, and output the response result through the ToResult method
    option.ExceptionHandler = context =>
    {
        if (context.Exception is ArgumentNullException argumentNullException)
        {
            context.ToResult("Parameter cannot be empty");
        }
    };
});
```

## common problem

1. Don't want to record the error message when the exception is UserFriendlyException?

     ```` C#
     builder.Services.Configure<MasaExceptionLogRelationOptions>(options =>
     {
         options.MapLogLevel<UserFriendlyException>(LogLevel.None);
     });
     ````