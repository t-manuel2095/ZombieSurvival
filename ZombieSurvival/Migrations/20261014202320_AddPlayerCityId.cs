using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZombieSurvival.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerCityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_CityId",
                table: "Players",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Cities_CityId",
                table: "Players",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Cities_CityId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_CityId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Players");
        }
    }
}
