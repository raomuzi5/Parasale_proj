using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class QuickStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "qCollections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CollectionName = table.Column<string>(nullable: false),
                    appUserId = table.Column<string>(nullable: true),
                    AssignedAdmin = table.Column<string>(nullable: true),
                    QuickStart = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_qCollections_AspNetUsers_appUserId",
                        column: x => x.appUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qCollections_appUserId",
                table: "qCollections",
                column: "appUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qCollections");

            migrationBuilder.AddColumn<bool>(
                name: "QuickStart",
                table: "Collections",
                nullable: true);
        }
    }
}
