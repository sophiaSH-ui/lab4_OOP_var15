using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lab4_oop.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCompletedToVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Vehicles");
        }
    }
}
