
using System.ComponentModel.DataAnnotations;


namespace Clicker.Security.Domain.DTO.User;

public record UserAuthRequestDto(
    [property: Required(AllowEmptyStrings = false, ErrorMessage = "Telegram id is required!")]
    [property: PositiveNumeric(ErrorMessage = "Telegram id must be a positive numeric value!")] string TelegramId)
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
    [PasswordPolicy]
    public string Password { get; set; } = null!;
}
