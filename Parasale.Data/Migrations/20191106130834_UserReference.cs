using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class UserReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "appUserId",
                table: "Collections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collections_appUserId",
                table: "Collections",
                column: "appUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_AspNetUsers_appUserId",
                table: "Collections",
                column: "appUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collections_AspNetUsers_appUserId",
                table: "Collections");

            migrationBuilder.DropIndex(
                name: "IX_Collections_appUserId",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "appUserId",
                table: "Collections");
        }
    }
}
