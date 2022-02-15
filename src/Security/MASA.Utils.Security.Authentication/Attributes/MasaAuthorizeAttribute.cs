namespace MASA.Utils.Security.Authentication.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class MasaAuthorizeAttribute : AuthorizeAttribute
{
    public string[] Permissions { get; set; }

    public MasaAuthorizeAttribute(params string[] permissions)
    {
        Permissions = permissions;
    }
}
