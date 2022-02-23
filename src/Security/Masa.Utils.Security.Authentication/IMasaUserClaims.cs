namespace Masa.Utils.Security.Authentication;

public interface IMasaUserClaims
{
    public ClaimsPrincipal? Principal { get; }

    public Guid UserId { get; }

    public string UserName { get; }

    public string NickName { get; }

    public string DepartmentName { get; }

    public IEnumerable<Guid> DepartmentIdList { get; }

    public bool IsAdministrator { get; }

    public IEnumerable<Claim> Claims { get; }
}
