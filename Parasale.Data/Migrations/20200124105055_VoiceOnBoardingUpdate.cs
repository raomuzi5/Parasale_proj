using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class VoiceOnBoardingUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VoiceOnBoardingId",
                table: "VoiceOnBoardings",
                newName: "Id");

            migrationBuilder.AddColumn<bool>(
                name: "IsStartupPopUp",
                table: "VoiceOnBoardings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "VoiceOnBoardings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "VoiceOnBoardings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "VoiceOnBoardings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSavedForLater",
                table: "VoiceOnBoardings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "VoiceOnBoardings");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "VoiceOnBoardings");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "VoiceOnBoardings");

            migrationBuilder.DropColumn(
                name: "isSavedForLater",
                table: "VoiceOnBoardings");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "VoiceOnBoardings",
                newName: "VoiceOnBoardingId");

           
            migrationBuilder.DropColumn(
                name: "IsStartupPopUp",
                table: "VoiceOnBoardings");
        }
    }
}
