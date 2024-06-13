using Clicker.Security.Domain.DTO.Token;

namespace Clicker.Security.Domain.DTO.User;

public record UserAuthResponseDto(TokenDto BearerToken, UserPayloadDto Payload);
