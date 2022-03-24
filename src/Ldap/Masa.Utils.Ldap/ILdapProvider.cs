namespace Masa.Utils.Ldap;

public interface ILdapProvider
{
    Task<LdapEntry?> GetGroupAsync(string groupName);

    IAsyncEnumerable<LdapUser> GetUsersInGroupAsync(string groupName);

    Task<LdapUser?> GetUsersByEmailAddressAsync(string emailAddress);

    Task<LdapUser?> GetUserByUserNameAsync(string userName);

    IAsyncEnumerable<LdapUser> GetAllUserAsync();

    Task<List<LdapEntry>> GetPagingUserAsync(int pageSize);

    Task AddUserAsync(LdapUser user, string password);

    Task DeleteUserAsync(string distinguishedName);

    Task<bool> AuthenticateAsync(string distinguishedName, string password);
}
