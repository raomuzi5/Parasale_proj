using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class CCS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CCS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TotalScore = table.Column<double>(nullable: true),
                    WPMTarget = table.Column<double>(nullable: true),
                    WPMMeasure = table.Column<double>(nullable: true),
                    WPMScore = table.Column<double>(nullable: true),
                    WPRTarget = table.Column<double>(nullable: true),
                    WPRMeasure = table.Column<double>(nullable: true),
                    WPRScore = table.Column<double>(nullable: true),
                    RPATarget = table.Column<double>(nullable: true),
                    RPA = table.Column<double>(nullable: true),
                    RPAScore = table.Column<double>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: true),
                    appUserId = table.Column<string>(nullable: true),
                    objectionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CCS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CCS_AspNetUsers_appUserId",
                        column: x => x.appUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CCS_Objection_objectionId",
                        column: x => x.objectionId,
                        principalTable: "Objection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CCS_appUserId",
                table: "CCS",
                column: "appUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CCS_objectionId",
                table: "CCS",
                column: "objectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CCS");
        }
    }
}
