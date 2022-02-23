namespace Masa.Utils.Security.Authentication;

public class MasaUser
{
    private static IMasaUserClaims _masaUserClaims = new MasaUserClaims(new HttpContextAccessor()
    {
        HttpContext = HttpContextUtility.GetCurrentHttpContext()
    });

    public static ClaimsPrincipal? Principal => _masaUserClaims.Principal;

    public static Guid UserId => _masaUserClaims.UserId;

    public static string UserName => _masaUserClaims.DepartmentName;

    public static string NickName => _masaUserClaims.NickName;

    public static string DepartmentName => _masaUserClaims.DepartmentName;

    public static IEnumerable<Guid> DepartmentIdList => _masaUserClaims.DepartmentIdList;

    public static bool IsAdministrator => _masaUserClaims.IsAdministrator;

    public static IEnumerable<Claim> Claims => _masaUserClaims.Claims;
}
