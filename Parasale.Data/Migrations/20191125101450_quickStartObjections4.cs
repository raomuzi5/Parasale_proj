using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class quickStartObjections4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "qObjection",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InitialObjection = table.Column<string>(nullable: true),
                    PitchKeyword = table.Column<string>(nullable: true),
                    ValidPitchResponse = table.Column<string>(nullable: true),
                    BadPitchResponse = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    AssignedAdmin = table.Column<string>(nullable: true),
                    collectionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qObjection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_qObjection_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_qObjection_qCollections_collectionId",
                        column: x => x.collectionId,
                        principalTable: "qCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qObjection_UserId",
                table: "qObjection",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_qObjection_collectionId",
                table: "qObjection",
                column: "collectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qObjection");

            migrationBuilder.AddColumn<bool>(
                name: "QuickStart",
                table: "Collections",
                nullable: true);
        }
    }
}
