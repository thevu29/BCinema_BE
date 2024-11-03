using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BCinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_Seats_SeatId",
                table: "PaymentDetails");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2c18a686-9fd4-4283-949a-1ea25bcdccc7"));

            migrationBuilder.DeleteData(
                table: "SeatTypes",
                keyColumn: "Id",
                keyValue: new Guid("a3c920af-5f1d-4c28-ac26-5b3c2f16816e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("71e2fc6f-5cfb-4e9a-9052-67d2c0a1c5e8"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a75923ce-8c94-40c2-9594-e7461f814afe"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "SeatId",
                table: "PaymentDetails",
                newName: "SeatScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentDetails_SeatId",
                table: "PaymentDetails",
                newName: "IX_PaymentDetails_SeatScheduleId");

            migrationBuilder.CreateTable(
                name: "SeatSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SeatId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatSchedule_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeatSchedule_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeatSchedule_ScheduleId",
                table: "SeatSchedule",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatSchedule_SeatId",
                table: "SeatSchedule",
                column: "SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_SeatSchedule_SeatScheduleId",
                table: "PaymentDetails",
                column: "SeatScheduleId",
                principalTable: "SeatSchedule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_SeatSchedule_SeatScheduleId",
                table: "PaymentDetails");

            migrationBuilder.DropTable(
                name: "SeatSchedule");

            migrationBuilder.RenameColumn(
                name: "SeatScheduleId",
                table: "PaymentDetails",
                newName: "SeatId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentDetails_SeatScheduleId",
                table: "PaymentDetails",
                newName: "IX_PaymentDetails_SeatId");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Seats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("2c18a686-9fd4-4283-949a-1ea25bcdccc7"), new DateTime(2024, 11, 2, 3, 41, 46, 903, DateTimeKind.Utc).AddTicks(8692), null, "User" },
                    { new Guid("a75923ce-8c94-40c2-9594-e7461f814afe"), new DateTime(2024, 11, 2, 3, 41, 46, 903, DateTimeKind.Utc).AddTicks(8686), null, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "SeatTypes",
                columns: new[] { "Id", "CreateAt", "Name", "Price" },
                values: new object[] { new Guid("a3c920af-5f1d-4c28-ac26-5b3c2f16816e"), new DateTime(2024, 11, 2, 3, 41, 46, 903, DateTimeKind.Utc).AddTicks(9574), "Regular", 50.0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreateAt", "DeleteAt", "Email", "Name", "Password", "Point", "Provider", "RoleId" },
                values: new object[] { new Guid("71e2fc6f-5cfb-4e9a-9052-67d2c0a1c5e8"), null, new DateTime(2024, 11, 2, 3, 41, 46, 903, DateTimeKind.Utc).AddTicks(8886), null, "admin@gmail.com", "Admin", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", 0, null, new Guid("a75923ce-8c94-40c2-9594-e7461f814afe") });

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_Seats_SeatId",
                table: "PaymentDetails",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id");
        }
    }
}
