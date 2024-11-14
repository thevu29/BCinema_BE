using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MovieName",
                table: "Schedules",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieName",
                table: "Schedules");
        }
    }
}
