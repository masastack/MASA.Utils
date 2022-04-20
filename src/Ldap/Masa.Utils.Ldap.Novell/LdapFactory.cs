namespace Masa.Utils.Ldap.Novell;

public class LdapFactory : ILdapFactory
{
    public ILdapProvider CreateProvider(LdapOptions ldapOptions)
    {
        return new LdapProvider(ldapOptions);
    }
}
