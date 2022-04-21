[中](README.zh-CN.md) | EN

## Masa.Utils.Caller.HttpClient

## Example:

````c#
Install-Package Masa.Utils.Caller.HttpClient
````

### Basic usage:

1. Modify `Program.cs`

    ```` C#
    builder.Services.AddCaller(options =>
    {
        options.UseHttpClient(httpClientBuilder =>
        {
            httpClientBuilder.Name = "UserCaller";//When there is only one HttpClient, you can not assign a value to Name
            httpClientBuilder.BaseAddress = "http://localhost:5000" ;
        });
    });
    ````

2. How to use:

    ```` C#
    app.MapGet("/Test/User/Check/Healthy", ([FromServices] ICallerProvider userCallerProvider)
        => userCallerProvider.GetAsync<string>("/Check/Healthy"));
    ````

    > The interface address of the complete request is: http://localhost:5000/Check/Healthy

3. When there are multiple HttpClients, modify `Program.cs` as

    ```` C#
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
    ````

4. How to use UserCaller or OrderCaller

    ```` C#
    app.MapGet("/Test/User/Check/Healthy", ([FromServices] ICallerProvider userCallerProvider)
        => userCallerProvider.GetAsync<string>("/Check/Healthy"));


    app.MapGet("/Test/Order/Check/Healthy", ([FromServices] ICallerFactory callerFactory) =>
    {
        var callerProvider = callerFactory.CreateClient("OrderCaller");
        return callerProvider.GetAsync<string>("/Check/Healthy");
    });
    ````

#### Recommended usage

1. Modify `Program.cs`

    ```` C#
    builder.Services.AddCaller();
    ````

2. Add a new class `UserServiceCaller`

    ```` C#
    public class UserCaller: HttpClientCallerBase
    {
        protected override string BaseAddress { get; set; } = "http://localhost:5000";

        public HttpCaller(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<string> GetHello1Async() => CallerProvider.GetStringAsync("/Check/Hello1");

        /// <summary>
        /// There is no need to overload by default, and it can be overloaded when there are special requirements for httpClient
        /// </summary>
        /// <param name="httpClient"></param>
        protected override void ConfigureHttpClient(System.Net.Http.HttpClient httpClient)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(5);
        }
    }
    ````

3. How to use UserCaller

    ```` C#
    app.MapGet("/Test/User/Check/Healthy", ([FromServices] UserCaller userCaller)
        => userCaller.GetAsync<string>("/Check/Healthy"));
    ````
