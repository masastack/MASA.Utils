namespace Masa.Utils.Security.Token.Model;

public class JwtConfigurationOptions
{
    public string Issuer { get; set; } = default!;

    public string Audience { get; set; } = default!;

    public string SecurityKey { get; set; } = default!;
}
