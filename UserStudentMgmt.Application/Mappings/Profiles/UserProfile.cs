using AutoMapper;
using UserStudentMgmt.Application.DTOs.Users;
using UserStudentMgmt.Domain.Entities;

namespace UserStudentMgmt.Application.Mappings.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocType.Name));
        
        CreateMap<UserRequestDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.DocType, opt => opt.Ignore());
    }
}