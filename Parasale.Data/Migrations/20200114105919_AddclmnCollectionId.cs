using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class AddclmnCollectionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "UserHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserHistory_CollectionId",
                table: "UserHistory",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistory_Collections_CollectionId",
                table: "UserHistory",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistory_Collections_CollectionId",
                table: "UserHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserHistory_CollectionId",
                table: "UserHistory");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "UserHistory");
        }
    }
}
