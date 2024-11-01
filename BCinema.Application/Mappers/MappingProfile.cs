﻿using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Features.Foods.Commands;
using BCinema.Application.Features.Users.Commands;
using BCinema.Application.Features.Roles.Commands;
using BCinema.Application.Features.Rooms.Commands;
using BCinema.Application.Features.Schedule.Commands;
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
                .ForMember(dest => dest.RoleId, opt => opt.Condition(src => src.RoleId != null))
                .ForMember(dest => dest.Avatar, opt => opt.Ignore());

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
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            
            CreateMap<Schedule, SchedulesDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)))
                .ForMember(dest => dest.Schedules, opt => opt.Ignore());
            
            CreateMap<CreateSchedulesCommand, Schedule>()
                .ForMember(dest => dest.Date, opt => opt.Ignore());

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
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            
            CreateMap<CreateSeatCommand, Seat>();
            
            CreateMap<UpdateSeatCommand, Seat>()
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status != null))
                .ForMember(dest => dest.SeatTypeId, opt => opt.Condition(src => src.SeatTypeId != null));
            
            // Seat Type
            CreateMap<SeatType, SeatTypeDto>();
            CreateMap<CreateSeatTypeCommand, SeatType>();
            
            CreateMap<UpdateSeatTypeCommand, SeatType>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price != 0));
            
            // Food
            CreateMap<Food, FoodDto>();
            CreateMap<CreateFoodCommand, Food>();

            CreateMap<UpdateFoodCommand, Food>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price != null))
                .ForMember(dest => dest.Quantity, opt => opt.Condition(src => src.Quantity != null));
        }
    }
}