using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class isDummy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDummy",
                table: "qObjection",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDummy",
                table: "qCollections",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDummy",
                table: "ObjectionLogs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDummy",
                table: "Objection",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDummy",
                table: "Collections",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDummy",
                table: "CCS",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDummy",
                table: "qObjection");

            migrationBuilder.DropColumn(
                name: "isDummy",
                table: "qCollections");

            migrationBuilder.DropColumn(
                name: "isDummy",
                table: "ObjectionLogs");

            migrationBuilder.DropColumn(
                name: "isDummy",
                table: "Objection");

            migrationBuilder.DropColumn(
                name: "isDummy",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "isDummy",
                table: "CCS");
        }
    }
}
