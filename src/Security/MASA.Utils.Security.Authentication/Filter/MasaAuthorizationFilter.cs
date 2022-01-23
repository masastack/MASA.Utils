namespace MASA.Utils.Security.Authentication.Filter;

public class MasaAuthorizationFilter : IAuthorizationFilter
{
    private readonly string _loginPermissionCode;
    private readonly MemoryCache<string, List<object>> _mcMasaAuthAtributes = new();

    public MasaAuthorizationFilter(IOptionsSnapshot<MasaAuthOptions> options)
    {
        _loginPermissionCode = $"{options.Value.SystemCode}00010001";
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        // Allow Anonymous skips all authorization
        if (HasAllowAnonymous(context)) return;

        if (!(context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
            return;

        /* !!!important when controllerActionDescriptor is null
         * controllerActionDescriptor != null -> result: true
         * controllerActionDescriptor.MethodInfo -> result: MethodInfo
         * controllerActionDescriptor.ControllerTypeInfo -> result: null
         * finally, get the full name by MethodInfo.DeclaringType.FullName
         */
        var key = controllerActionDescriptor.MethodInfo.DeclaringType!.FullName;

        var attributeList = _mcMasaAuthAtributes.GetOrAdd(key!, _ =>
        {
            var lstAttribute = new List<object>();
            lstAttribute.AddRange(controllerActionDescriptor.MethodInfo.GetCustomAttributes<MasaAuthorizeAttribute>(true));
            lstAttribute.AddRange(controllerActionDescriptor.MethodInfo.DeclaringType.GetCustomAttributes<MasaAuthorizeAttribute>(true));

            return lstAttribute.Distinct().ToList();
        });

        var authorizeAttributes = attributeList.OfType<MasaAuthorizeAttribute>().ToList();

        var claims = context.HttpContext.User.Claims;
        var userPermissionClaim = claims.Where(c => c.Type == MasaClaimTypes.MASA_PERMISSION).ToList();
        if (!userPermissionClaim.Any())
        {
            // todo Internationalization
            context.Result = new JsonResult(new {message = "No Permissions"})
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }

        // todo MasaPermissions update to array
        var userPermissions = userPermissionClaim.Select(c => c.Value).ToList();

        //Admin
        if (MasaUser.IsAdministrator) return;

        //No MasaAttribute
        if (authorizeAttributes.Count == 0) return;

        if (userPermissions.Count == 0)
        {
            // todo Internationalization
            context.Result = new JsonResult(new {message = "No Permissions"})
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
        //Has MasaAttribute But No ctor params
        else if (authorizeAttributes.All(masaAuthorize => masaAuthorize.Permissions.All(string.IsNullOrEmpty))
                 && userPermissions.Any(userPermission => userPermission.Equals(_loginPermissionCode)))
        {
            return;
        }
        else
        {
            if (authorizeAttributes.Any(masaAuthorize => masaAuthorize.Permissions.Intersect(userPermissions).Any()))
                return;

            // todo Internationalization
            context.Result = new JsonResult(new {message = "No Permissions"})
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }

    private static bool HasAllowAnonymous(AuthorizationFilterContext context)
    {
        if (context.Filters.Any(filter => filter is IAllowAnonymous))
            return true;

        // When doing endpoint routing, MVC does not add AllowAnonymousFilters for AllowAnonymousAttributes that
        // were discovered on controllers and actions. To maintain compat with 2.x,
        // we'll check for the presence of IAllowAnonymous in endpoint metadata.
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            return true;
        }

        return false;
    }
}
