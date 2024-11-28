using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Features.Foods.Commands;
using BCinema.Application.Features.Payments.Commands;
using BCinema.Application.Features.Users.Commands;
using BCinema.Application.Features.Roles.Commands;
using BCinema.Application.Features.Rooms.Commands;
using BCinema.Application.Features.Schedules.Commands;
using BCinema.Application.Features.Seats.Commands;
using BCinema.Application.Features.SeatTypes.Commands;
using BCinema.Application.Features.UserVouchers.Commands;
using BCinema.Application.Features.Vouchers.Commands;
using BCinema.Domain.Entities;

namespace BCinema.Application.Mappers
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
                .ForMember(dest => dest.Avatar, opt => opt.Ignore());

            // Role
            CreateMap<Role, RoleDto>();
            CreateMap<CreateRoleCommand, Role>();

            CreateMap<UpdateRoleCommand, Role>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null));

            // Voucher
            CreateMap<Voucher, VoucherDto>();
            CreateMap<CreateVoucherCommand, Voucher>();

            // UserVoucher
            CreateMap<UserVoucher, UserVoucherDto>();
            CreateMap<CreateUserVoucherCommand, UserVoucher>();

            // Schedule
            CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name));
            
            CreateMap<Schedule, SchedulesDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)))
                .ForMember(dest => dest.Schedules, opt => opt.Ignore());
            
            CreateMap<Schedule, ScheduleDetailDto>()
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Date.TimeOfDay))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            
            CreateMap<UpdateScheduleCommand, Schedule>()
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.RoomId, opt => opt.Condition(src => src.RoomId != null))
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status != null));
            
            // Room
            CreateMap<Room, RoomDto>();
            CreateMap<CreateRoomCommand, Room>();
            
            CreateMap<UpdateRoomCommand, Room>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null));

            // Seat
            CreateMap<Seat, SeatDto>()
                .ForMember(dest => dest.SeatType, opt => opt.MapFrom(src => src.SeatType.Name))
                .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.SeatType.Price));
            
            CreateMap<CreateSeatCommand, Seat>();
            
            // Seat Type
            CreateMap<SeatType, SeatTypeDto>();
            CreateMap<CreateSeatTypeCommand, SeatType>();
            
            CreateMap<UpdateSeatTypeCommand, SeatType>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price != 0));
            
            // Seat Schedule
            CreateMap<SeatSchedule, SeatScheduleDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Seat.SeatType.Price))
                .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.Seat.Row))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Seat.Number))
                .ForMember(dest => dest.SeatType, opt => opt.MapFrom(src => src.Seat.SeatType.Name))
                .ForMember(dest => dest.SeatTypeId, opt => opt.MapFrom(src => src.Seat.SeatTypeId));
            
            // Food
            CreateMap<Food, FoodDto>();
            CreateMap<CreateFoodCommand, Food>();

            CreateMap<UpdateFoodCommand, Food>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price != null))
                .ForMember(dest => dest.Quantity, opt => opt.Condition(src => src.Quantity != null));
            
            // Payment
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));
            
            CreateMap<CreatePaymentCommand, Payment>();
            
            // Payment Detail
            CreateMap<PaymentDetail, PaymentDetailDto>().ReverseMap();
        }
    }
}