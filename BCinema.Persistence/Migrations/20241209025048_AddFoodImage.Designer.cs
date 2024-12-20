﻿// <auto-generated />
using System;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BCinema.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241209025048_AddFoodImage")]
    partial class AddFoodImage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BCinema.Domain.Entities.Food", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeleteAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Otp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Attempts")
                        .HasColumnType("integer");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Otps");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("uuid");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("double precision");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("VoucherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("UserId");

                    b.HasIndex("VoucherId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.PaymentDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("FoodId")
                        .HasColumnType("uuid");

                    b.Property<int?>("FoodQuantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uuid");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<Guid?>("SeatScheduleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FoodId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("SeatScheduleId");

                    b.ToTable("PaymentDetails");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Schedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MovieId")
                        .HasColumnType("integer");

                    b.Property<string>("MovieName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<int>("Runtime")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Seat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<string>("Row")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SeatTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("SeatTypeId");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.SeatSchedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SeatId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("SeatId");

                    b.ToTable("SeatSchedule");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.SeatType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeleteAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("SeatTypes");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Token", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RefreshTokenExpireAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeleteAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("IsActivated")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Point")
                        .HasColumnType("integer");

                    b.Property<string>("Provider")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.UserVoucher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("VoucherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VoucherId");

                    b.ToTable("UserVouchers");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Voucher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("Discount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Otp", b =>
                {
                    b.HasOne("BCinema.Domain.Entities.User", "User")
                        .WithMany("Otps")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Payment", b =>
                {
                    b.HasOne("BCinema.Domain.Entities.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BCinema.Domain.Entities.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BCinema.Domain.Entities.Voucher", "Voucher")
                        .WithMany("Payments")
                        .HasForeignKey("VoucherId");

                    b.Navigation("Schedule");

                    b.Navigation("User");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.PaymentDetail", b =>
                {
                    b.HasOne("BCinema.Domain.Entities.Food", "Food")
                        .WithMany("PaymentDetails")
                        .HasForeignKey("FoodId");

                    b.HasOne("BCinema.Domain.Entities.Payment", "Payment")
                        .WithMany("PaymentDetails")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BCinema.Domain.Entities.SeatSchedule", "SeatSchedule")
                        .WithMany("PaymentDetails")
                        .HasForeignKey("SeatScheduleId");

                    b.Navigation("Food");

                    b.Navigation("Payment");

                    b.Navigation("SeatSchedule");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Schedule", b =>
                {
                    b.HasOne("BCinema.Domain.Entities.Room", "Room")
                        .WithMany("Schedules")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Seat", b =>
                {
                    b.HasOne("BCinema.Domain.Entities.Room", "Room")
                        .WithMany("Seats")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BCinema.Domain.Entities.SeatType", "SeatType")
                        .WithMany("Seats")
                        .HasForeignKey("SeatTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("SeatType");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.SeatSchedule", b =>
                {
                    b.HasOne("BCinema.Domain.Entities.Schedule", "Schedule")
                        .WithMany("SeatSchedules")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BCinema.Domain.Entities.Seat", "Seat")
                        .WithMany("SeatSchedules")
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");

                    b.Navigation("Seat");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Token", b =>
                {
                    b.HasOne("BCinema.Domain.Entities.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.User", b =>
                {
                    b.HasOne("BCinema.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.UserVoucher", b =>
                {
                    b.HasOne("BCinema.Domain.Entities.User", "User")
                        .WithMany("UserVouchers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BCinema.Domain.Entities.Voucher", "Voucher")
                        .WithMany("UserVouchers")
                        .HasForeignKey("VoucherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Food", b =>
                {
                    b.Navigation("PaymentDetails");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Payment", b =>
                {
                    b.Navigation("PaymentDetails");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Room", b =>
                {
                    b.Navigation("Schedules");

                    b.Navigation("Seats");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Schedule", b =>
                {
                    b.Navigation("SeatSchedules");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Seat", b =>
                {
                    b.Navigation("SeatSchedules");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.SeatSchedule", b =>
                {
                    b.Navigation("PaymentDetails");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.SeatType", b =>
                {
                    b.Navigation("Seats");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.User", b =>
                {
                    b.Navigation("Otps");

                    b.Navigation("Payments");

                    b.Navigation("Tokens");

                    b.Navigation("UserVouchers");
                });

            modelBuilder.Entity("BCinema.Domain.Entities.Voucher", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("UserVouchers");
                });
#pragma warning restore 612, 618
        }
    }
}
