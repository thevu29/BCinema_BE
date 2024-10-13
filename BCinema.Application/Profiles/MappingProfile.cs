using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Doman.Entities;

namespace BCinema.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
            CreateMap<Role, RoleDto>();
        }
    }
}
