using System.ComponentModel.DataAnnotations;

namespace Clicker.Security.Domain.DTO.User;

public record UserSignUpRequestDto(
    [property: Required(AllowEmptyStrings = false, ErrorMessage = "Telegram id is required!")]
   string TelegramId,
   Guid? ReferrerId )
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
    [PasswordPolicy]
    public string Password { get; set; } = null!;
}