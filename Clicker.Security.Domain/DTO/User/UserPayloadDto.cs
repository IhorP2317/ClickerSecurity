namespace Clicker.Security.Domain.DTO.User;

public record UserPayloadDto(Guid Id, string TelegramId)
{
    public string Role { get; set; } = null!;
}