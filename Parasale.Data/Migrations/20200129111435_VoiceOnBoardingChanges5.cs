using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class VoiceOnBoardingChanges5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "VoiceOnBoardings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "VoiceOnBoardings");
        }
    }
}
