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

    [TestMethod]
    public void TestMultiDefaultCaller()
    {
        IServiceCollection services = new ServiceCollection();
        Assert.ThrowsException<ArgumentException>(() =>
        {
            services.AddCaller(opt =>
            {
                opt.UseHttpClient(builder =>
                {
                    builder.Name = "github";
                    builder.BaseApi = "https://github.com/masastack";
                    builder.IsDefault = true;
                });
                opt.UseHttpClient(builder =>
                {
                    builder.Name = "gitee";
                    builder.BaseApi = "https://gitee.com/masastack";
                    builder.IsDefault = true;
                });
            });
        });
    }

    [TestMethod]
    public void TestMultiDefaultCaller2()
    {
        IServiceCollection services = new ServiceCollection();
        Assert.ThrowsException<ArgumentException>(() =>
        {
            services.AddCaller(opt =>
            {
                opt.UseHttpClient(builder =>
                {
                    builder.Name = "gitee";
                    builder.BaseApi = "https://gitee.com/masastack";
                    builder.IsDefault = true;
                });
            });
            services.AddCaller(opt =>
            {
                opt.UseHttpClient(builder =>
                {
                    builder.Name = "github";
                    builder.BaseApi = "https://github.com/masastack";
                    builder.IsDefault = true;
                });
            });
        });
    }

    [TestMethod]
    public void TestRepeatCallerName()
    {
        IServiceCollection services = new ServiceCollection();
        Assert.ThrowsException<ArgumentException>(() =>
        {
            services.AddCaller(opt =>
            {
                opt.UseHttpClient(builder =>
                {
                    builder.Name = "github";
                    builder.BaseApi = "https://github.com/masastack";
                    builder.IsDefault = true;
                });
                opt.UseHttpClient(builder =>
                {
                    builder.Name = "github";
                    builder.BaseApi = "https://github.com/masastack";
                    builder.IsDefault = true;
                });
            });
        });
    }

    [TestMethod]
    public void TestRepeatCallerName2()
    {
        IServiceCollection services = new ServiceCollection();
        Assert.ThrowsException<ArgumentException>(() =>
        {
            services.AddCaller(opt =>
            {
                opt.UseHttpClient(builder =>
                {
                    builder.Name = "github";
                    builder.BaseApi = "https://github.com/masastack";
                    builder.IsDefault = true;
                });
            });

            services.AddCaller(opt =>
            {
                opt.UseHttpClient(builder =>
                {
                    builder.Name = "github";
                    builder.BaseApi = "https://github.com/masastack";
                    builder.IsDefault = true;
                });
            });
        });
    }

    [TestMethod]
    public void TestRepeatCallerName3()
    {
        IServiceCollection services = new ServiceCollection();
        Assert.ThrowsException<ArgumentException>(() =>
        {
            services.AddCaller(opt =>
            {
                opt.UseHttpClient(builder =>
                {
                    builder.Name = nameof(CustomCaller);
                    builder.BaseApi = "https://*.com/masastack";
                    builder.IsDefault = true;
                });
            });

            services.AddCaller(opt =>
            {
                opt.UseHttpClient(builder =>
                {
                    builder.Name = "gitee";
                    builder.BaseApi = "https://github.com/masastack";
                    builder.IsDefault = true;
                });
            });
        });
    }
}
