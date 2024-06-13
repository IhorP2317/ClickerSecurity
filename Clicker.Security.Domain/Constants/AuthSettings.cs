using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Clicker.Security.Domain.Constants;

public class AuthSettings
{
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; }
    public SecurityKey SymmetricSecurityKey {
        get =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
    }
    public int RefreshTokenSize { get; set; }
}