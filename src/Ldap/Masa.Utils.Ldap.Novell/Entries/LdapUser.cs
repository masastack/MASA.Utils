namespace Masa.Utils.Ldap.Novell.Entries;

public class LdapUser
{
    public string SecurityIdentifier { get; set; } = string.Empty;

    public string UniqueIdentifier { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Class { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string CommonName { get; set; } = string.Empty;

    public string DistinguishedName { get; set; } = string.Empty;

    public string SamAccountName { get; set; } = string.Empty;

    public int SamAccountType { get; set; }

    public string[] MemberOf { get; set; } = Array.Empty<string>();

    public bool IsDomainAdmin { get; set; }

    public string UserPrincipalName { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public string EmailAddress { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public LdapAddress Address { get; set; } = new();
}
