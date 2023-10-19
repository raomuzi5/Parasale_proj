using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ObjectionId",
                table: "ObjectionNotification",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_ObjectionNotification_ObjectionId",
                table: "ObjectionNotification",
                column: "ObjectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectionNotification_Objection_ObjectionId",
                table: "ObjectionNotification",
                column: "ObjectionId",
                principalTable: "Objection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObjectionNotification_Objection_ObjectionId",
                table: "ObjectionNotification");

            migrationBuilder.DropIndex(
                name: "IX_ObjectionNotification_ObjectionId",
                table: "ObjectionNotification");

            migrationBuilder.AlterColumn<int>(
                name: "ObjectionId",
                table: "ObjectionNotification",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
