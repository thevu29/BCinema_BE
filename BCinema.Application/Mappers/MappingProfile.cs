using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Features.Users.Commands;
using BCinema.Application.Features.Roles.Commands;
using BCinema.Application.Features.SeatTypes.Commands;
using BCinema.Application.Features.UserVouchers.Commands;
using BCinema.Application.Features.Vouchers.Commands;
using BCinema.Domain.Entities;

namespace BCinema.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<CreateUserCommand, User>();

            CreateMap<UpdateUserCommand, User>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.RoleId, opt => opt.Condition(src => src.RoleId != null))
                .ForMember(dest => dest.Avatar, opt => opt.Ignore()); ;

            // Role
            CreateMap<Role, RoleDto>();
            CreateMap<CreateRoleCommand, Role>();

            CreateMap<UpdateRoleCommand, Role>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null));

            // Voucher
            CreateMap<Voucher, VoucherDto>();

            CreateMap<CreateVoucherCommand, Voucher>()
                .ForMember(dest => dest.ExpireAt, opt => opt.MapFrom(src => src.ExpireAt.ToLocalTime()));

            // UserVoucher
            CreateMap<UserVoucher, UserVoucherDto>();
            CreateMap<CreateUserVoucherCommand, UserVoucher>();

            // Schedule
            CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie.Name))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name));

            // Room
            CreateMap<Room, RoomDto>();

            // Seat
            CreateMap<Seat, SeatDto>();

            // Seat Type
            CreateMap<SeatType, SeatTypeDto>();
            CreateMap<CreateSeatTypeCommand, SeatType>();

            CreateMap<UpdateSeatTypeCommand, SeatType>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price != 0));
        }
    }
}