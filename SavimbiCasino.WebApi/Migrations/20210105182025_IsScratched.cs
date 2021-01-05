using Microsoft.EntityFrameworkCore.Migrations;

namespace SavimbiCasino.WebApi.Migrations
{
    public partial class IsScratched : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsScratched",
                table: "Players",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScratched",
                table: "Players");
        }
    }
}
