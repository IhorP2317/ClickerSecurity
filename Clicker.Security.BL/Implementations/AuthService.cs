using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Clicker.Security.BL.Abstractions;
using Clicker.Security.DAL.Models;
using Clicker.Security.Domain.Constants;
using Clicker.Security.Domain.DTO.Token;
using Clicker.Security.Domain.DTO.User;
using Clicker.Security.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Clicker.Security.BL.Implementations;

public class AuthService:IAuthService
{
    private readonly AuthSettings _authSettings;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthService(IOptions<AuthSettings> authSettings, IMapper mapper, UserManager<ApplicationUser> userManager, ITokenGenerator tokenGenerator)
    {
        _authSettings = authSettings.Value;
        _mapper = mapper;
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<UserPayloadDto> RegisterAsync(UserAuthRequestDto userSignUpDto, string role = "User") {
        var user = _mapper.Map<ApplicationUser>(userSignUpDto);
        user.UserName = userSignUpDto.TelegramId;
        var result = await _userManager.CreateAsync(user, userSignUpDto.Password);

        if (result.Succeeded) {
            await _userManager.AddToRoleAsync(user, role);
        } else {
            throw new AuthException(result.Errors.First().Description, StatusCodes.Status409Conflict);
        }
        
        var createdUser = await _userManager.FindByNameAsync(user.UserName);
        var createdUserResponseDto = _mapper.Map<UserPayloadDto>(createdUser);

        createdUserResponseDto.Role = (await _userManager.GetRolesAsync(createdUser)).First();
        return createdUserResponseDto;
    }
 public async Task<UserAuthResponseDto> LoginAsync(UserAuthRequestDto userDto)
        {
            var user = await _userManager.FindByNameAsync(userDto.TelegramId);

            if (user == null)
                throw new AuthException($"User with telegram {userDto.TelegramId} not found!", StatusCodes.Status404NotFound);

            var validPassword = await _userManager.CheckPasswordAsync(user, userDto.Password);

            if (!validPassword)
                throw new AuthException($"User with telegram Id {userDto.TelegramId} unauthorized!", StatusCodes.Status401Unauthorized);


            user.RefreshToken = _tokenGenerator.GenerateRefreshToken(user);
            await _userManager.UpdateAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var rolesString = string.Join(", ", roles);

            var payload = new UserPayloadDto(user.Id, user.TelegramId)
            {
                Role = rolesString
            };

            var bearerToken = new TokenDto(
                AccessToken: await _tokenGenerator.GenerateAccessToken(user),
                RefreshToken: user.RefreshToken
            );

            return new UserAuthResponseDto(bearerToken, payload);
        }

        public async Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var userName = principal.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken)
                throw new AuthException("Invalid refresh token,", StatusCodes.Status401Unauthorized);
            user.RefreshToken = _tokenGenerator.GenerateRefreshToken(user);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new AuthException("Could not create refresh token!", StatusCodes.Status500InternalServerError);
            return new TokenDto(
                AccessToken: await _tokenGenerator.GenerateAccessToken(user),
                RefreshToken: user.RefreshToken
            );
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token) {
                var tokenValidationParameters = new TokenValidationParameters {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _authSettings.SymmetricSecurityKey,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero

                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

                return principal;
            }
}