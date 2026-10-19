using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZombieSurvival.Migrations
{
    /// <inheritdoc />
    public partial class PhaseAndCarCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsItDay",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "CarCondition",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DayPhase",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarCondition",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DayPhase",
                table: "Players");

            migrationBuilder.AddColumn<bool>(
                name: "IsItDay",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
