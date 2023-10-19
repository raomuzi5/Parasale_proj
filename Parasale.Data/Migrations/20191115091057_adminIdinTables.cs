using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class adminIdinTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedAdmin",
                table: "ObjectionLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedAdmin",
                table: "Invites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedAdmin",
                table: "Collections",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedAdmin",
                table: "AssignedCollections",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedAdmin",
                table: "ObjectionLogs");

            migrationBuilder.DropColumn(
                name: "AssignedAdmin",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "AssignedAdmin",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "AssignedAdmin",
                table: "AssignedCollections");
        }
    }
}
