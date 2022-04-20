namespace Masa.Utils.Ldap.Novell;

public interface ILdapFactory
{
    ILdapProvider CreateProvider(LdapOptions ldapOptions);
}
