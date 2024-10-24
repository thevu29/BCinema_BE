using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Features.Foods.Commands;
using BCinema.Application.Features.UserVouchers.Commands;
using BCinema.Application.Features.Vouchers.Commands;
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

            // Food
            CreateMap<Food,  FoodDto>();
            CreateMap<CreateFoodCommand, Food>();
            CreateMap<UpdateFoodCommand, Food>();

            // Voucher
            CreateMap<Voucher, VoucherDto>();

            CreateMap<CreateVoucherCommand, Voucher>()
                .ForMember(dest => dest.ExpireAt, opt => opt.MapFrom(src => src.ExpireAt.ToLocalTime()));

            CreateMap<UpdateVoucherCommand, Voucher>()
                .ForMember(dest => dest.ExpireAt, opt => opt.MapFrom(src => src.ExpireAt.ToLocalTime()));

            // UserVoucher
            CreateMap<UserVoucher, UserVoucherDto>();
            CreateMap<CreateUserVoucherCommand, UserVoucher>();
        }
    }
}