
using Clicker.Security.BL.Abstractions;
using Clicker.Security.Domain.DTO.Token;
using Microsoft.AspNetCore.Mvc;
using Carter;
using Clicker.Security.API.Filters;
using Clicker.Security.Domain.DTO.User;
namespace Clicker.Security.API.Extensions;




public class AuthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth");

        group.MapPost("/signUp", SignUpAsync)
            .WithName(nameof(SignUpAsync))
            .Produces<UserPayloadDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidateModelFilter>();
        
        group.MapPost("/login", LoginAsync)
            .WithName(nameof(LoginAsync))
            .Produces<UserAuthResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidateModelFilter>();

        group.MapPost("/refresh", RefreshAccessTokenAsync)
            .WithName(nameof(RefreshAccessTokenAsync))
            .Produces<TokenDto>(StatusCodes.Status200OK)
            .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidateModelFilter>();
    }

    private static async Task<IResult> SignUpAsync(HttpContext httpContext, UserSignUpRequestDto userDto)
    {
        var authService = httpContext.RequestServices.GetRequiredService<IAuthService>();
        var createdUser = await authService.RegisterAsync(userDto);
        return Results.Ok(createdUser);
    }
    
    private static async Task<IResult> LoginAsync(HttpContext httpContext, UserLoginRequestDto userDto)
    {
        var authService = httpContext.RequestServices.GetRequiredService<IAuthService>();
        var confirmedUser = await authService.LoginAsync(userDto);
        return Results.Ok(confirmedUser);
    }

    private static async Task<IResult> RefreshAccessTokenAsync(HttpContext httpContext, TokenDto tokenDto)
    {
        var authService = httpContext.RequestServices.GetRequiredService<IAuthService>();
        var token = await authService.RefreshAccessTokenAsync(tokenDto);
        return Results.Ok(token);
    }
}
