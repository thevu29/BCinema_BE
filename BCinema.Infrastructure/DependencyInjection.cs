﻿using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using BCinema.Infrastructure.Repositories;
using BCinema.Infrastructure.Services;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.DependencyInjection;

namespace BCinema.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<StorageClient>(provider => StorageClient.Create());
            services.AddScoped<IFileStorageService, FirebaseStorageService>();
            
            services.AddScoped<IMovieFetchService, MovieFetchService>();

            services.AddScoped<IOtpRepository, OtpRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISeatTypeRepository, SeatTypeRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IUserVoucherRepository, UserVoucherRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentDetailRepository, PaymentDetailRepository>();
            services.AddScoped<ISeatScheduleRepository, SeatScheduleRepository>();

            return services;
        }
    }
}
