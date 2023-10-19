using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class Sessions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sessionObjections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SessionID = table.Column<int>(nullable: false),
                    objectionId = table.Column<int>(nullable: true),
                    objectionLogId = table.Column<int>(nullable: true),
                    cCSId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessionObjections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sessionObjections_CCS_cCSId",
                        column: x => x.cCSId,
                        principalTable: "CCS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sessionObjections_Objection_objectionId",
                        column: x => x.objectionId,
                        principalTable: "Objection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sessionObjections_ObjectionLogs_objectionLogId",
                        column: x => x.objectionLogId,
                        principalTable: "ObjectionLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "userSessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SessionID = table.Column<int>(nullable: false),
                    SessionName = table.Column<string>(nullable: true),
                    SessionStart = table.Column<DateTime>(nullable: true),
                    SessionEnd = table.Column<DateTime>(nullable: true),
                    appUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userSessions_AspNetUsers_appUserId",
                        column: x => x.appUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sessionObjections_cCSId",
                table: "sessionObjections",
                column: "cCSId");

            migrationBuilder.CreateIndex(
                name: "IX_sessionObjections_objectionId",
                table: "sessionObjections",
                column: "objectionId");

            migrationBuilder.CreateIndex(
                name: "IX_sessionObjections_objectionLogId",
                table: "sessionObjections",
                column: "objectionLogId");

            migrationBuilder.CreateIndex(
                name: "IX_userSessions_appUserId",
                table: "userSessions",
                column: "appUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sessionObjections");

            migrationBuilder.DropTable(
                name: "userSessions");
        }
    }
}
