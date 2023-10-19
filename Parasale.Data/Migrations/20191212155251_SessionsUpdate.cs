using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class SessionsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SessionID",
                table: "sessionObjections",
                newName: "sessionId");

            migrationBuilder.AlterColumn<int>(
                name: "sessionId",
                table: "sessionObjections",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_sessionObjections_sessionId",
                table: "sessionObjections",
                column: "sessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_sessionObjections_userSessions_sessionId",
                table: "sessionObjections",
                column: "sessionId",
                principalTable: "userSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sessionObjections_userSessions_sessionId",
                table: "sessionObjections");

            migrationBuilder.DropIndex(
                name: "IX_sessionObjections_sessionId",
                table: "sessionObjections");

            migrationBuilder.RenameColumn(
                name: "sessionId",
                table: "sessionObjections",
                newName: "SessionID");

            migrationBuilder.AlterColumn<int>(
                name: "SessionID",
                table: "sessionObjections",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
