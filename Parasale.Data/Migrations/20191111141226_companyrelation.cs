using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class companyrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "appUserId",
                table: "Companies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_appUserId",
                table: "Companies",
                column: "appUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_AspNetUsers_appUserId",
                table: "Companies",
                column: "appUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AspNetUsers_appUserId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_appUserId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "appUserId",
                table: "Companies");
        }
    }
}
