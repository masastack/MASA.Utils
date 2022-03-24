namespace Masa.Utils.Ldap;

public class LdapProvider : ILdapProvider, IDisposable
{
    ILdapConnection ldapConnection = null!;
    LdapOptions _ldapOptions;

    private readonly string[] _attributes =
    {
        "objectSid", "objectGUID", "objectCategory", "objectClass", "memberOf", "name", "cn", "distinguishedName",
        "sAMAccountName", "userPrincipalName", "displayName", "givenName", "sn", "description",
        "telephoneNumber", "mail", "streetAddress", "postalCode", "l", "st", "co", "c"
    };

    public LdapProvider(IOptionsSnapshot<LdapOptions> options)
    {
        _ldapOptions = options.Value;
    }

    private async Task<ILdapConnection> GetConnectionAsync()
    {
        if (ldapConnection != null && ldapConnection.Connected)
        {
            return ldapConnection;
        }
        ldapConnection = new LdapConnection() { SecureSocketLayer = _ldapOptions.ServerPortSsl != 0 };
        //Connect function will create a socket connection to the server - Port 389 for insecure and 3269 for secure    
        await ldapConnection.ConnectAsync(_ldapOptions.ServerAddress,
            _ldapOptions.ServerPortSsl != 0 ? _ldapOptions.ServerPortSsl : _ldapOptions.ServerPort);
        //Bind function with null user dn and password value will perform anonymous bind to LDAP server 
        await ldapConnection.BindAsync(_ldapOptions.RootUserDn, _ldapOptions.RootUserPassword);

        return ldapConnection;
    }

    public async Task<bool> AuthenticateAsync(string distinguishedName, string password)
    {
        using var ldapConnection = new LdapConnection() { SecureSocketLayer = _ldapOptions.ServerPortSsl != 0 };
        await ldapConnection.ConnectAsync(_ldapOptions.ServerAddress,
            ldapConnection.SecureSocketLayer ? _ldapOptions.ServerPortSsl : _ldapOptions.ServerPort);
        try
        {
            await ldapConnection.BindAsync(distinguishedName, password);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task DeleteUserAsync(string distinguishedName)
    {
        using (var ldapConnection = await GetConnectionAsync())
        {
            await ldapConnection.DeleteAsync(distinguishedName);
        }
    }

    public async Task AddUserAsync(LdapUser user, string password)
    {
        var dn = $"CN={user.FirstName} {user.LastName},{_ldapOptions.UserSearchBaseDn}";

        var attributeSet = new LdapAttributeSet
        {
            new LdapAttribute("instanceType", "4"),
            new LdapAttribute("objectCategory", $"CN=Users,{_ldapOptions.UserSearchBaseDn}"),
            new LdapAttribute("objectClass", new[] {"top", "person", "organizationalPerson", "user"}),
            new LdapAttribute("name", user.Name),
            new LdapAttribute("cn", $"{user.FirstName} {user.LastName}"),
            new LdapAttribute("sAMAccountName", user.SamAccountName),
            new LdapAttribute("userPrincipalName", user.UserPrincipalName),
            new LdapAttribute("unicodePwd", Convert.ToBase64String(Encoding.Unicode.GetBytes($"\"{password}\""))),
            new LdapAttribute("userAccountControl", "512"),
            new LdapAttribute("givenName", user.FirstName),
            new LdapAttribute("sn", user.LastName),
            new LdapAttribute("mail", user.EmailAddress)
        };

        if (user.DisplayName != null)
        {
            attributeSet.Add(new LdapAttribute("displayName", user.DisplayName));
        }
        if (user.Description != null)
        {
            attributeSet.Add(new LdapAttribute("description", user.Description));
        }
        if (user.Phone != null)
        {
            attributeSet.Add(new LdapAttribute("telephoneNumber", user.Phone));
        }
        if (user.Address?.Street != null)
        {
            attributeSet.Add(new LdapAttribute("streetAddress", user.Address.Street));
        }
        if (user.Address?.City != null)
        {
            attributeSet.Add(new LdapAttribute("l", user.Address.City));
        }
        if (user.Address?.PostalCode != null)
        {
            attributeSet.Add(new LdapAttribute("postalCode", user.Address.PostalCode));
        }
        if (user.Address?.StateName != null)
        {
            attributeSet.Add(new LdapAttribute("st", user.Address.StateName));
        }
        if (user.Address?.CountryName != null)
        {
            attributeSet.Add(new LdapAttribute("co", user.Address.CountryName));
        }
        if (user.Address?.CountryCode != null)
        {
            attributeSet.Add(new LdapAttribute("c", user.Address.CountryCode));
        }
        var newEntry = new LdapEntry(dn, attributeSet);

        using var ldapConnection = await GetConnectionAsync();
        await ldapConnection.AddAsync(newEntry);
    }

    public async IAsyncEnumerable<LdapUser> GetAllUserAsync()
    {
        var filter = $"(&(objectCategory=person)(objectClass=user))";
        var users = GetFilterLdapEntryAsync(_ldapOptions.UserSearchBaseDn, filter);
        await foreach (var user in users)
        {
            yield return CreateUser(user.Dn, user.GetAttributeSet());
        }
    }

    public async Task<List<LdapEntry>> GetPagingUserAsync(int pageSize)
    {
        using var ldapConnection = await GetConnectionAsync();
        return await ldapConnection.SearchUsingSimplePagingAsync(new SearchOptions(
            _ldapOptions.UserSearchBaseDn,
            LdapConnection.ScopeSub,
            "(&(objectCategory=person)(objectClass=user))",
            _attributes),
            pageSize);
    }

    public async Task<LdapUser?> GetUserByUserNameAsync(string userName)
    {
        var filter = $"(&(objectClass=user)(sAMAccountName={userName}))";
        var user = await GetFilterLdapEntryAsync(_ldapOptions.UserSearchBaseDn, filter).FirstOrDefaultAsync();
        return user == null ? null : CreateUser(user.Dn, user.GetAttributeSet());
    }

    public async Task<LdapUser?> GetUsersByEmailAddressAsync(string emailAddress)
    {
        var filter = $"(&(objectClass=user)(mail={emailAddress}))";
        var user = await GetFilterLdapEntryAsync(_ldapOptions.UserSearchBaseDn, filter).FirstOrDefaultAsync();
        return user == null ? null : CreateUser(user.Dn, user.GetAttributeSet());
    }

    private async IAsyncEnumerable<LdapEntry> GetFilterLdapEntryAsync(string baseDn, string filter)
    {
        using var ldapConnection = await GetConnectionAsync();
        var searchResults = await ldapConnection.SearchAsync(
                baseDn,
                LdapConnection.ScopeSub,
                filter,
                _attributes,
                false);
        await foreach (var searchResult in searchResults)
        {
            yield return searchResult;
        }
    }

    public async IAsyncEnumerable<LdapUser> GetUsersInGroupAsync(string groupName)
    {
        var group = await GetGroupAsync(groupName);
        if (group == null)
        {
            yield break;
        }
        var filter = $"(&(objectCategory=person)(objectClass=user)(memberOf={group.Dn}))";
        var users = GetFilterLdapEntryAsync(_ldapOptions.UserSearchBaseDn, filter);

        await foreach (var user in users)
        {
            yield return CreateUser(user.Dn, user.GetAttributeSet());
        }
    }

    public async Task<LdapEntry?> GetGroupAsync(string groupName)
    {
        var filter = $"(&(objectCategory=group)(objectClass=group)(cn={groupName}))";
        return await GetFilterLdapEntryAsync(_ldapOptions.GroupSearchBaseDn, filter)
            .FirstOrDefaultAsync();
    }

    public void Dispose()
    {
        if (ldapConnection.Connected)
        {
            ldapConnection.Disconnect();
        }
        if (ldapConnection != null)
        {
            ldapConnection.Dispose();
        }
    }

    private LdapUser CreateUser(string distinguishedName, LdapAttributeSet attributeSet)
    {
        var ldapUser = new LdapUser();
        attributeSet.TryGetValue("objectSid", out var objectSid);
        ldapUser.ObjectSid = objectSid?.StringValue ?? "";
        attributeSet.TryGetValue("objectGUID", out var objectGUID);
        ldapUser.ObjectGuid = objectGUID?.StringValue ?? "";
        attributeSet.TryGetValue("objectCategory", out var objectCategory);
        ldapUser.ObjectCategory = objectCategory?.StringValue ?? "";
        attributeSet.TryGetValue("objectClass", out var objectClass);
        ldapUser.ObjectClass = objectClass?.StringValue ?? "";
        attributeSet.TryGetValue("memberOf", out var memberOf);
        ldapUser.MemberOf = memberOf?.StringValueArray ?? new string[] { };
        attributeSet.TryGetValue("cn", out var cn);
        ldapUser.CommonName = cn?.StringValue ?? "";
        attributeSet.TryGetValue("sAMAccountName", out var sAMAccountName);
        ldapUser.SamAccountName = sAMAccountName?.StringValue ?? "";
        attributeSet.TryGetValue("userPrincipalName", out var userPrincipalName);
        ldapUser.UserPrincipalName = userPrincipalName?.StringValue ?? "";
        attributeSet.TryGetValue("name", out var name);
        ldapUser.Name = name?.StringValue ?? "";
        attributeSet.TryGetValue("distinguishedName", out var _distinguishedName);
        ldapUser.DistinguishedName = _distinguishedName?.StringValue ?? distinguishedName;
        attributeSet.TryGetValue("displayName", out var displayName);
        ldapUser.DisplayName = displayName?.StringValue ?? "";
        attributeSet.TryGetValue("givenName", out var givenName);
        ldapUser.FirstName = givenName?.StringValue ?? "";
        attributeSet.TryGetValue("sn", out var sn);
        ldapUser.LastName = sn?.StringValue ?? "";
        attributeSet.TryGetValue("description", out var description);
        ldapUser.Description = description?.StringValue ?? "";
        attributeSet.TryGetValue("telephoneNumber", out var telephoneNumber);
        ldapUser.Phone = telephoneNumber?.StringValue ?? "";
        attributeSet.TryGetValue("mail", out var mail);
        ldapUser.EmailAddress = mail?.StringValue ?? "";
        attributeSet.TryGetValue("streetAddress", out var streetAddress);
        attributeSet.TryGetValue("l", out var city);
        attributeSet.TryGetValue("postalCode", out var postalCode);
        attributeSet.TryGetValue("st", out var stateName);
        attributeSet.TryGetValue("co", out var countryName);
        attributeSet.TryGetValue("c", out var countryCode);
        ldapUser.Address = new LdapAddress
        {
            Street = streetAddress?.StringValue ?? "",
            City = city?.StringValue ?? "",
            PostalCode = postalCode?.StringValue ?? "",
            StateName = stateName?.StringValue ?? "",
            CountryName = countryName?.StringValue ?? "",
            CountryCode = countryCode?.StringValue ?? ""
        };
        attributeSet.TryGetValue("sAMAccountType", out var sAMAccountType);
        ldapUser.SamAccountType = int.Parse(sAMAccountType?.StringValue ?? "0");
        ldapUser.IsDomainAdmin = ldapUser.MemberOf.Contains("CN=Domain Admins," + _ldapOptions.BaseDn);
        return ldapUser;
    }
}
