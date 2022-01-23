namespace MASA.Utils.Security.Authentication;

public class MasaUser
{
    private static HttpContext _httpContext => HttpContextUtility.GetCurrentHttpContext();

    public static ClaimsPrincipal Principal => _httpContext?.User;

    public static Guid UserId
    {
        get
        {
            _ = Guid.TryParse(_httpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value, out var userId);
            return userId;
        }
    }

    public static string UserName
    {
        get { return _httpContext?.User?.Identity?.Name; }
    }

    public static string NickName
    {
        get { return _httpContext?.User?.Claims?.FirstOrDefault(c => c.Type == MasaClaimTypes.MASA_NICK_NAME)?.Value; }
    }

    public static string DepartmentName
    {
        get { return _httpContext?.User?.Claims?.FirstOrDefault(c => c.Type == MasaClaimTypes.MASA_DEPARTMENT_NAME)?.Value; }
    }

    public static IEnumerable<Guid> DepartmentIdList
    {
        get
        {
            return _httpContext?.User?.Claims?.Where(c => c.Type == MasaClaimTypes.MASA_DEPARTMENT_ID).Select(c => Guid.Parse(c.Value))
                .AsEnumerable();
        }
    }

    public static bool IsAdministrator
    {
        get
        {
            if (Principal == null || Principal.Claims == null || !Principal.Claims.Any()) return false;
            else
            {
                var userPermissionClaims = Principal.Claims.Where(c => c.Type == MasaClaimTypes.MASA_PERMISSION)
                    .Select(permission => permission.Value);

                if ((userPermissionClaims?.Count() ?? 0) <= 0) return false;

                return userPermissionClaims.Any(permission => permission.Equals(MasaPermissionCodes.ADMIN));
            }
        }
    }

    public static IEnumerable<Claim> Claims
    {
        get { return _httpContext?.User?.Claims; }
    }
}
