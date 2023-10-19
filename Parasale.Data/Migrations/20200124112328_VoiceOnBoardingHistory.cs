using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class VoiceOnBoardingHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceOnBoardings_AspNetUsers_UserId",
                table: "VoiceOnBoardings");

            migrationBuilder.DropIndex(
                name: "IX_VoiceOnBoardings_UserId",
                table: "VoiceOnBoardings");

            migrationBuilder.DropColumn(
                name: "isSavedForLater",
                table: "VoiceOnBoardings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VoiceOnBoardings",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubStep",
                table: "VoiceOnBoardings",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VoiceOnBoardings",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubStep",
                table: "VoiceOnBoardings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSavedForLater",
                table: "VoiceOnBoardings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoiceOnBoardings_UserId",
                table: "VoiceOnBoardings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceOnBoardings_AspNetUsers_UserId",
                table: "VoiceOnBoardings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
