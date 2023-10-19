using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class Tbl_VoiceonboardingAndTbl_AppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StepTitle",
                table: "VoiceOnBoardings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StepTitle",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubStep",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StepTitle",
                table: "VoiceOnBoardings");

            migrationBuilder.DropColumn(
                name: "StepTitle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubStep",
                table: "AspNetUsers");
        }
    }
}
