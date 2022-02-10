namespace MASA.Utils.Security.Authentication;

public class MasaUserClaims : IMasaUserClaims
{
    private HttpContext _httpContext;

    public MasaUserClaims(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
    }

    public ClaimsPrincipal Principal => _httpContext.User;

    public Guid UserId
    {
        get
        {
            _ = Guid.TryParse(_httpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value, out var userId);
            return userId;
        }
    }

    public string UserName => _httpContext.User.Identity?.Name ?? string.Empty;

    public string NickName => _httpContext.User.Claims.FirstOrDefault(c => c.Type == MasaClaimTypes.MASA_NICK_NAME)?.Value ?? string.Empty;

    public string DepartmentName => _httpContext.User.Claims.FirstOrDefault(c => c.Type == MasaClaimTypes.MASA_DEPARTMENT_NAME)?.Value ??
                                    string.Empty;

    public IEnumerable<Guid> DepartmentIdList => _httpContext.User.Claims.Where(c => c.Type == MasaClaimTypes.MASA_DEPARTMENT_ID)
        .Select(c => Guid.Parse(c.Value))
        .AsEnumerable();

    public bool IsAdministrator
    {
        get
        {
            if (!Principal.Claims.Any()) return false;
            var userPermissionClaims = Principal.Claims.Where(c => c.Type == MasaClaimTypes.MASA_PERMISSION)
                .Select(permission => permission.Value)
                .ToList();

            if ((userPermissionClaims.Count) <= 0) return false;

            return userPermissionClaims.Any(permission => permission.Equals(MasaPermissionCodes.ADMIN));
        }
    }

    public IEnumerable<Claim> Claims
        => _httpContext.User.Claims;
}
