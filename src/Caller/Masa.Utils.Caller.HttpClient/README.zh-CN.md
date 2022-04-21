中 | [EN](README.md)

## Masa.Utils.Caller.HttpClient

## 用例:

```c#
Install-Package Masa.Utils.Caller.HttpClient
```

### 基本用法:

1. 修改`Program.cs`

    ``` C#
    builder.Services.AddCaller(options =>
    {
        options.UseHttpClient(httpClientBuilder =>
        {
            httpClientBuilder.Name = "UserCaller";//仅存在一个HttpClient时，可以不对Name赋值
            httpClientBuilder.BaseAddress = "http://localhost:5000" ;
        });
    });
    ```

2. 如何使用:

    ``` C#
    app.MapGet("/Test/User/Check/Healthy", ([FromServices] ICallerProvider userCallerProvider)
        => userCallerProvider.GetAsync<string>("/Check/Healthy"));
    ```

    > 完整请求的接口地址是：http://localhost:5000/Check/Healthy

3. 当存在多个HttpClient时，则修改`Program.cs`为

    ``` C#
    builder.Services.AddCaller(options =>
    {
        options.UseHttpClient(httpClientBuilder =>
        {
            httpClientBuilder.Name = "UserCaller";
            httpClientBuilder.BaseAddress = "http://localhost:5000" ;
        });
        options.UseHttpClient(httpClientBuilder =>
        {
            httpClientBuilder.Name = "OrderCaller";
            httpClientBuilder.BaseAddress = "http://localhost:6000" ;
        });
    });
    ```

4. 如何使用UserCaller或OrderCaller

    ``` C#
    app.MapGet("/Test/User/Check/Healthy", ([FromServices] ICallerProvider userCallerProvider)
        => userCallerProvider.GetAsync<string>("/Check/Healthy"));


    app.MapGet("/Test/Order/Check/Healthy", ([FromServices] ICallerFactory callerFactory) =>
    {
        var callerProvider = callerFactory.CreateClient("OrderCaller");
        return callerProvider.GetAsync<string>("/Check/Healthy");
    });
    ```

#### 推荐用法

1. 修改`Program.cs`

    ``` C#
    builder.Services.AddCaller();
    ```

2. 新增加类`UserServiceCaller`

    ``` C#
    public class UserCaller: HttpClientCallerBase
    {
        protected override string BaseAddress { get; set; } = "http://localhost:5000";

        public HttpCaller(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<string> GetHello1Async() => CallerProvider.GetStringAsync("/Check/Hello1");

        /// <summary>
        /// 默认不需要重载，对httpClient有特殊需求时可重载
        /// </summary>
        /// <param name="httpClient"></param>
        protected override void ConfigureHttpClient(System.Net.Http.HttpClient httpClient)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(5);
        }
    }
    ```

3. 如何使用UserCaller

    ``` C#
    app.MapGet("/Test/User/Check/Healthy", ([FromServices] UserCaller userCaller)
        => userCaller.GetAsync<string>("/Check/Healthy"));
    ```