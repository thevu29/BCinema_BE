using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Foods",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Foods");
        }
    }
}
