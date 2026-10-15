using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZombieSurvival.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Money",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Money",
                table: "Players");
        }
    }
}
