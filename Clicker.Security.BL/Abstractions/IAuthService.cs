using Clicker.Security.Domain.DTO.Token;
using Clicker.Security.Domain.DTO.User;

namespace Clicker.Security.BL.Abstractions;

public interface IAuthService
{
    Task<UserPayloadDto> RegisterAsync(UserSignUpRequestDto userDto, string role = "User");
    Task<UserAuthResponseDto> LoginAsync(UserLoginRequestDto userDto);
    Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto);
}