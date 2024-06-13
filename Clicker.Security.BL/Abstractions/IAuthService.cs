using Clicker.Security.Domain.DTO.Token;
using Clicker.Security.Domain.DTO.User;

namespace Clicker.Security.BL.Abstractions;

public interface IAuthService
{
    Task<UserPayloadDto> RegisterAsync(UserAuthRequestDto userDto, string role = "User");
    Task<UserAuthResponseDto> LoginAsync(UserAuthRequestDto userDto);
    Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto);
}