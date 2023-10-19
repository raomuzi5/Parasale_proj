using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class objectionsupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Objection",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objection_UserId",
                table: "Objection",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objection_AspNetUsers_UserId",
                table: "Objection",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objection_AspNetUsers_UserId",
                table: "Objection");

            migrationBuilder.DropIndex(
                name: "IX_Objection_UserId",
                table: "Objection");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Objection");
        }
    }
}
