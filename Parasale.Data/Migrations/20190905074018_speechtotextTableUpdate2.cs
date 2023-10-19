using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class speechtotextTableUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objection_AspNetUsers_AppUserId",
                table: "Objection");

            migrationBuilder.DropIndex(
                name: "IX_Objection_AppUserId",
                table: "Objection");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Objection");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Objection",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objection_AppUserId",
                table: "Objection",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objection_AspNetUsers_AppUserId",
                table: "Objection",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
