using Clicker.Security.DAL.Models;

namespace Clicker.Security.BL.Abstractions;

public interface ITokenGenerator
{
    public Task<string> GenerateAccessToken(ApplicationUser user);
    public string GenerateRefreshToken(ApplicationUser user);
}