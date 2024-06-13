using System.ComponentModel.DataAnnotations;

namespace Clicker.Security.Domain.DTO.Token;

public record TokenDto(
    [property: Required(AllowEmptyStrings = false, ErrorMessage = "Access Token is Required!")] string AccessToken,
    [property: Required(AllowEmptyStrings = false, ErrorMessage = "Access Token is Required!")] string RefreshToken
);
