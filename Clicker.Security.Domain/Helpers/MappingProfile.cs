using AutoMapper;
using Clicker.Security.DAL.Models;
using Clicker.Security.Domain.DTO.User;

namespace Clicker.Security.Domain.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<UserLoginRequestDto, ApplicationUser>();
        CreateMap<UserSignUpRequestDto, ApplicationUser>();
        CreateMap<ApplicationUser, UserPayloadDto>()
            .ForMember(dest => dest.Role, opt => opt.Ignore());
    }
}