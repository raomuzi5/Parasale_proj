using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class UpdateVoiceOnBoarding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Element",
                table: "VoiceOnBoardings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "VoiceOnBoardings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Element",
                table: "VoiceOnBoardings");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "VoiceOnBoardings");
        }
    }
}
