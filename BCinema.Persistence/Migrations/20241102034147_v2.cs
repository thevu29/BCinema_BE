using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BCinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
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
                keyValue: new Guid("65964b94-d515-4f55-b41a-c813697238e2"));

            migrationBuilder.DeleteData(
                table: "SeatTypes",
                keyColumn: "Id",
                keyValue: new Guid("841ed9d4-8d52-45f3-86bd-d01542018ae6"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7ce44327-3617-40d0-a0c3-d9fdc6070850"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b83d83de-77a2-4f9b-8c1d-0bdbb8cf1384"));

            migrationBuilder.AlterColumn<Guid>(
                name: "SeatId",
                table: "PaymentDetails",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<Guid>(
                name: "SeatId",
                table: "PaymentDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("65964b94-d515-4f55-b41a-c813697238e2"), new DateTime(2024, 11, 2, 3, 40, 13, 174, DateTimeKind.Utc).AddTicks(9784), null, "User" },
                    { new Guid("b83d83de-77a2-4f9b-8c1d-0bdbb8cf1384"), new DateTime(2024, 11, 2, 3, 40, 13, 174, DateTimeKind.Utc).AddTicks(9779), null, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "SeatTypes",
                columns: new[] { "Id", "CreateAt", "Name", "Price" },
                values: new object[] { new Guid("841ed9d4-8d52-45f3-86bd-d01542018ae6"), new DateTime(2024, 11, 2, 3, 40, 13, 175, DateTimeKind.Utc).AddTicks(832), "Regular", 50.0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreateAt", "DeleteAt", "Email", "Name", "Password", "Point", "Provider", "RoleId" },
                values: new object[] { new Guid("7ce44327-3617-40d0-a0c3-d9fdc6070850"), null, new DateTime(2024, 11, 2, 3, 40, 13, 174, DateTimeKind.Utc).AddTicks(9973), null, "admin@gmail.com", "Admin", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", 0, null, new Guid("b83d83de-77a2-4f9b-8c1d-0bdbb8cf1384") });

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_Seats_SeatId",
                table: "PaymentDetails",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
