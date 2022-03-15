using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        var customCaller = serviceProvider.GetRequiredService<CustomCaller>();
        Assert.IsNotNull(customCaller);
        Assert.IsTrue(customCaller.Name == nameof(CustomCaller));

        var githubCaller = serviceProvider.GetRequiredService<GithubCaller>();
        Assert.IsNotNull(githubCaller);
        Assert.IsTrue(githubCaller.Name == typeof(GithubCaller).FullName);

        var blogCaller = serviceProvider.GetRequiredService<BlogCaller>();
        Assert.IsTrue(blogCaller.Name == typeof(BlogCaller).FullName);

        Assert.IsTrue(customCaller.Equals(blogCaller.CustomCaller));
        Assert.IsTrue(githubCaller.Equals(blogCaller.GithubCaller));
    }

    [TestMethod]
    public void TestCallerProviderServiceLifetime()
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
        });
        var serviceProvider = services.BuildServiceProvider();
        var callerProvider1 = serviceProvider.GetRequiredService<ICallerProvider>();
        var callerProvider2 = serviceProvider.GetRequiredService<ICallerProvider>();
        Assert.IsTrue(callerProvider1 == callerProvider2);
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
                    builder.Name = "github";
                    builder.BaseApi = "https://github.com/masastack";
                    builder.IsDefault = true;
                });
            });
        });
    }
}
