using Microsoft.AspNetCore.Identity;

namespace Clicker.Security.DAL.Models;

public class ApplicationUser:IdentityUser<Guid>
{
    public string TelegramId { get; set; }
    public string? RefreshToken { get; set; }
    public Guid? ReferrerId  { get; set; }

}