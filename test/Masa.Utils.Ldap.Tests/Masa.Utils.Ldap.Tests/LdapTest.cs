namespace Masa.Utils.Ldap.Tests;

[TestClass]
public class LdapTest
{
    readonly IServiceCollection Services;
    readonly ILdapProvider ldapProvider;

    public LdapTest()
    {
        Services = new ServiceCollection();
        var mockLdapOptions = new Mock<LdapOptions>();

        var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var configuration = configurationBuilder.Build();
        var ldapConfigurationSection = configuration.GetSection(nameof(LdapOptions));
        Services.AddLadpContext(ldapConfigurationSection);
        var serviceProvider = Services.BuildServiceProvider();
        ldapProvider = serviceProvider.GetRequiredService<ILdapProvider>();
    }

    [TestInitialize]
    public void EdgeDriverInitialize()
    {

    }

    [TestMethod]
    public async Task GetAllUser()
    {
        var allUsers = await ldapProvider.GetAllUserAsync().ToListAsync(); ;
        Assert.IsTrue(allUsers.Count > 0);
    }

    [TestMethod]
    public async Task GetPagingUser()
    {
        var pagingUsers = await ldapProvider.GetPagingUserAsync(1);
        Assert.IsTrue(pagingUsers.Count > 0);
    }

    [TestMethod]
    public async Task GetUserByUserName()
    {
        var user = await ldapProvider.GetUserByUserNameAsync("mayue");
        Assert.IsNotNull(user);
    }

    [TestMethod]
    public async Task GetUserByUserEmail()
    {
        var user = await ldapProvider.GetUsersByEmailAddressAsync("mayue@masastack.com");
        Assert.IsNotNull(user);
    }

    [TestMethod]
    public async Task GetGroupAsync()
    {
        var group = await ldapProvider.GetGroupAsync("杭州产品研发部");
        Assert.IsNotNull(group);
    }

    [TestMethod]
    public async Task GetUsersInGroupAsync()
    {
        var users = await ldapProvider.GetUsersInGroupAsync("杭州产品研发部").ToListAsync();
        Assert.IsTrue(users.Count > 0);
    }
}
