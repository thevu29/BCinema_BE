using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Features.Seats.Commands;
using BCinema.Application.Features.SeatTypes.Commands;
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

            // Voucher
            CreateMap<Voucher, VoucherDto>();

            CreateMap<CreateVoucherCommand, Voucher>()
                .ForMember(dest => dest.ExpireAt, opt => opt.MapFrom(src => src.ExpireAt.ToLocalTime()));

            CreateMap<UpdateVoucherCommand, Voucher>()
                .ForMember(dest => dest.ExpireAt, opt => opt.MapFrom(src => src.ExpireAt.ToLocalTime()));

            // UserVoucher
            CreateMap<UserVoucher, UserVoucherDto>();
            CreateMap<CreateUserVoucherCommand, UserVoucher>();

            // Seat
            CreateMap<Seat, SeatDto>();
            CreateMap<CreateSeatCommand, Seat>();
            CreateMap<UpdateSeatCommand, Seat>();

            // SeatType
            CreateMap<SeatType, SeatTypeDto>();
            CreateMap<CreateSeatTypeCommand, SeatType>();
            CreateMap<UpdateSeatTypeCommand, SeatType>();
        }
    }
}