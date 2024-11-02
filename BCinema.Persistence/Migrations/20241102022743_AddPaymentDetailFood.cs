using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BCinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentDetailFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_Foods_FoodId",
                table: "PaymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Vouchers_VoucherId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDetails_FoodId",
                table: "PaymentDetails");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1e9c39aa-23ef-410a-a698-ad8692ea78a5"));

            migrationBuilder.DeleteData(
                table: "SeatTypes",
                keyColumn: "Id",
                keyValue: new Guid("bf1f0663-1ed3-430b-8778-be12f5b40e5c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("550cabfc-d238-4fb3-af05-b2e2ed308409"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("db32ee97-c967-4629-8267-0a4856935cef"));

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "FoodQuantity",
                table: "PaymentDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "VoucherId",
                table: "Payments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "PaymentDetailFood",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    FoodId = table.Column<Guid>(type: "uuid", nullable: false),
                    FoodQuantity = table.Column<int>(type: "integer", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetailFood", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDetailFood_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentDetailFood_PaymentDetails_PaymentDetailId",
                        column: x => x.PaymentDetailId,
                        principalTable: "PaymentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("6161ed15-091b-4b96-ac96-2d6c627c58dd"), new DateTime(2024, 11, 2, 2, 27, 42, 436, DateTimeKind.Utc).AddTicks(7487), null, "Admin" },
                    { new Guid("6d909286-0bc6-4947-ab78-5db24288e8ed"), new DateTime(2024, 11, 2, 2, 27, 42, 436, DateTimeKind.Utc).AddTicks(7492), null, "User" }
                });

            migrationBuilder.InsertData(
                table: "SeatTypes",
                columns: new[] { "Id", "CreateAt", "Name", "Price" },
                values: new object[] { new Guid("5f1b00bc-bcaa-415a-9e43-a8799c5b8df3"), new DateTime(2024, 11, 2, 2, 27, 42, 437, DateTimeKind.Utc).AddTicks(772), "Regular", 50.0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreateAt", "DeleteAt", "Email", "Name", "Password", "Point", "Provider", "RoleId" },
                values: new object[] { new Guid("407fa4cf-2ca7-4aa2-ac79-04a0e066bf75"), null, new DateTime(2024, 11, 2, 2, 27, 42, 436, DateTimeKind.Utc).AddTicks(8160), null, "admin@gmail.com", "Admin", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", 0, null, new Guid("6161ed15-091b-4b96-ac96-2d6c627c58dd") });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetailFood_FoodId",
                table: "PaymentDetailFood",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetailFood_PaymentDetailId",
                table: "PaymentDetailFood",
                column: "PaymentDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Vouchers_VoucherId",
                table: "Payments",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Vouchers_VoucherId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentDetailFood");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6d909286-0bc6-4947-ab78-5db24288e8ed"));

            migrationBuilder.DeleteData(
                table: "SeatTypes",
                keyColumn: "Id",
                keyValue: new Guid("5f1b00bc-bcaa-415a-9e43-a8799c5b8df3"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("407fa4cf-2ca7-4aa2-ac79-04a0e066bf75"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6161ed15-091b-4b96-ac96-2d6c627c58dd"));

            migrationBuilder.AlterColumn<Guid>(
                name: "VoucherId",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FoodId",
                table: "PaymentDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "FoodQuantity",
                table: "PaymentDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("1e9c39aa-23ef-410a-a698-ad8692ea78a5"), new DateTime(2024, 10, 29, 14, 40, 20, 305, DateTimeKind.Utc).AddTicks(5608), null, "User" },
                    { new Guid("db32ee97-c967-4629-8267-0a4856935cef"), new DateTime(2024, 10, 29, 14, 40, 20, 305, DateTimeKind.Utc).AddTicks(5566), null, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "SeatTypes",
                columns: new[] { "Id", "CreateAt", "Name", "Price" },
                values: new object[] { new Guid("bf1f0663-1ed3-430b-8778-be12f5b40e5c"), new DateTime(2024, 10, 29, 14, 40, 20, 305, DateTimeKind.Utc).AddTicks(8507), "Regular", 50.0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreateAt", "DeleteAt", "Email", "Name", "Password", "Point", "Provider", "RoleId" },
                values: new object[] { new Guid("550cabfc-d238-4fb3-af05-b2e2ed308409"), null, new DateTime(2024, 10, 29, 14, 40, 20, 305, DateTimeKind.Utc).AddTicks(6327), null, "admin@gmail.com", "Admin", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", 0, null, new Guid("db32ee97-c967-4629-8267-0a4856935cef") });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_FoodId",
                table: "PaymentDetails",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_Foods_FoodId",
                table: "PaymentDetails",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Vouchers_VoucherId",
                table: "Payments",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
