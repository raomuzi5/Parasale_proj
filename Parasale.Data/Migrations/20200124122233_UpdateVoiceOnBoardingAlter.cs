using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class UpdateVoiceOnBoardingAlter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "VoiceOnBoardings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsManager",
                table: "VoiceOnBoardings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNextExist",
                table: "VoiceOnBoardings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPreviousExist",
                table: "VoiceOnBoardings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUser",
                table: "VoiceOnBoardings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "VoiceOnBoardings");

            migrationBuilder.DropColumn(
                name: "IsManager",
                table: "VoiceOnBoardings");

            migrationBuilder.DropColumn(
                name: "IsNextExist",
                table: "VoiceOnBoardings");

            migrationBuilder.DropColumn(
                name: "IsPreviousExist",
                table: "VoiceOnBoardings");

            migrationBuilder.DropColumn(
                name: "IsUser",
                table: "VoiceOnBoardings");
        }
    }
}
