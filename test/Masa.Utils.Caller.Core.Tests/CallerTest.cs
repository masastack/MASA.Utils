using Masa.Utils.Caller.DaprClient;

namespace Masa.Utils.Caller.Core.Tests;

[TestClass]
public class CallerTest
{
    [TestMethod]
    public void TestAutomaticCaller()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddCaller();
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var caller = serviceProvider.GetRequiredService<CustomCaller>();
        Assert.IsNotNull(caller);
    }

    [TestMethod]
    public void TestCaller()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddCaller(opt =>
        {
            opt.UseHttpClient(clientBuilder =>
            {
                clientBuilder.Name = "http";
                clientBuilder.IsDefault = true;
                clientBuilder.BaseApi = "https://github.com/masastack/MASA.Contrib";
            });
            opt.UseDapr(clientBuilder =>
            {
                clientBuilder.Name = "dapr";
                clientBuilder.IsDefault = false;
            });
        });
        var serviceProvider = services.BuildServiceProvider();
        var callerProvider = serviceProvider.GetRequiredService<ICallerProvider>();
        Assert.IsNotNull(callerProvider);

        var caller = serviceProvider.GetRequiredService<ICallerFactory>().CreateClient();
        var daprCaller = serviceProvider.GetRequiredService<ICallerFactory>().CreateClient("dapr");
        var httpCaller = serviceProvider.GetRequiredService<ICallerFactory>().CreateClient("http");

        Assert.IsTrue(caller.GetType().FullName != daprCaller.GetType().FullName);
        Assert.IsTrue(caller.GetType().FullName == httpCaller.GetType().FullName);
    }
}
