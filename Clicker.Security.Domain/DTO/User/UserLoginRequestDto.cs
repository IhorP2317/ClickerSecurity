
using System.ComponentModel.DataAnnotations;


namespace Clicker.Security.Domain.DTO.User;

public record UserLoginRequestDto(
    [property: Required(AllowEmptyStrings = false, ErrorMessage = "Telegram id is required!")]
     string TelegramId)
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
    [PasswordPolicy]
    public string Password { get; set; } = null!;
}
