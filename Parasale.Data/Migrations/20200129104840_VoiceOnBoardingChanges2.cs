using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class VoiceOnBoardingChanges2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceOnBoardings_VoiceOnBoardings_ParentVOBId",
                table: "VoiceOnBoardings");

            migrationBuilder.AlterColumn<int>(
                name: "ParentVOBId",
                table: "VoiceOnBoardings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceOnBoardings_VoiceOnBoardings_ParentVOBId",
                table: "VoiceOnBoardings",
                column: "ParentVOBId",
                principalTable: "VoiceOnBoardings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceOnBoardings_VoiceOnBoardings_ParentVOBId",
                table: "VoiceOnBoardings");

            migrationBuilder.AlterColumn<int>(
                name: "ParentVOBId",
                table: "VoiceOnBoardings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceOnBoardings_VoiceOnBoardings_ParentVOBId",
                table: "VoiceOnBoardings",
                column: "ParentVOBId",
                principalTable: "VoiceOnBoardings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
